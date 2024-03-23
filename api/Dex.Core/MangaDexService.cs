namespace Dex.Core;

public interface IMangaDexService
{
    IAsyncEnumerable<Chapter> Latest(string[]? languages = null, bool includeExternal = false);

    Task<ChapterList> Chapters(ChaptersFilter? filter = null, string[]? languages = null, bool includeExternal = false);

    Task<MangaList> AllManga(params string[] ids);
}

internal class MangaDexService(IMangaDex _api): IMangaDexService
{
    public Task<ChapterList> Chapters(ChaptersFilter? filter = null, string[]? languages = null, bool includeExternal = false)
    {
        filter ??= new();
        filter.Limit = 100;
        filter.Order = new() { [ChaptersFilter.OrderKey.updatedAt] = OrderValue.desc };
        filter.Includes = [MangaIncludes.manga];
        filter.TranslatedLanguage = languages ?? [ "en" ];
        filter.IncludeExternalUrl = includeExternal;
        return _api.Chapter.List(filter);
    }

    public Task<MangaList> AllManga(params string[] ids)
    {
        var filter = new MangaFilter
        {
            Ids = ids,
            Includes =
            [
                MangaIncludes.cover_art,
                MangaIncludes.author, 
                MangaIncludes.artist, 
                MangaIncludes.tag, 
                MangaIncludes.user, 
                MangaIncludes.scanlation_group
            ]
        };
        return _api.Manga.List(filter);
    }

    public async IAsyncEnumerable<Chapter> Latest(string[]? languages = null, bool includeExternal = false)
    {
        var chapters = await Chapters(null, languages, includeExternal);

        var ids = chapters.Data
            .Select(GetManga)
            .Where(m => m != null)
            .Select(m => m!.Id)
            .Distinct()
            .ToArray();

        if (ids.Length == 0) yield break;

        var allManga = await AllManga(ids);

        foreach (var chapter in chapters.Data)
        {
            chapter.Relationships = Relationships(chapter, allManga).ToArray();
            yield return chapter;
        }
    }

    public static Manga? GetManga(IRelationshipModel model)
    {
        return model.Relationships.FirstOrDefault(r => r is Manga) as Manga;
    }

    public static RelatedDataRelationship From(Manga manga)
    {
        return new RelatedDataRelationship
        {
            Id = manga.Id,
            Type = "manga",
            Attributes = manga.Attributes,
            Related = MangaDexSharp.Relationships.based_on
        };
    }

    public static IEnumerable<IRelationship> Relationships(Chapter chapter, MangaList allManga)
    {
        var mid = GetManga(chapter)?.Id ?? string.Empty;
        var manga = allManga.Data.FirstOrDefault(m => m.Id == mid);

        var ids = new List<string> { mid };

        foreach(var rel in chapter.Relationships)
        {
            if (ids.Contains(rel.Id)) continue;

            ids.Add(rel.Id);
            yield return rel;
        }

        if (manga is null) yield break;

        foreach(var rel in manga.Relationships)
        {
            if (ids.Contains(rel.Id)) continue;
            ids.Add(rel.Id);
            yield return rel;
        }

        yield return From(manga);
    }
}
