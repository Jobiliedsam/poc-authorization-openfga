namespace Authorization.API.Utils;

public static class HeaderHelper
{
    public static string GetStoreIdFromHeader(this HttpRequest request) => request
        .Headers["X-OpenFGA-Store-ID"].FirstOrDefault() ?? string.Empty;
}