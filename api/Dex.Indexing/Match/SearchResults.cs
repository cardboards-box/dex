namespace Dex.Indexing;

public class SearchResults : MatchResult<MatchImage> { }

public class SearchResults<T> : MatchResult<MatchMeta<T>> { }
