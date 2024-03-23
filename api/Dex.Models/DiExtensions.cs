namespace Dex.Models;

public static class DiExtensions
{
    public static IDependencyResolver AddModels(this IDependencyResolver resolver)
    {
        return resolver
            .Model<Language>()
            .Model<DexSource>()
            .Model<DexManga>()
            .Model<DexChapter>()

            .Type<Localization>("localization");
    }
}
