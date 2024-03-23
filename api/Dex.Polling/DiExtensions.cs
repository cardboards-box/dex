namespace Dex.Polling;

public static class DiExtensions
{
    public static IDependencyResolver AddPolling(this IDependencyResolver bob)
    {
        return bob;
    }
}
