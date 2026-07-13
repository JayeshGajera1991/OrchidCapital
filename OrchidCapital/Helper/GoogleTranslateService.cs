using System.Globalization;
using Google.Cloud.Translation.V2;

public static class GoogleTranslateService
{
    private static readonly TranslationClient _client =
        TranslationClient.CreateFromApiKey("AIzaSyB8mKmEg0WEI8aCqPh6t0W83SKmYFSNCuQdotnet");

    public static string Translate(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return text;

        var language = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;

        var result = _client.TranslateText(text, language);

        return result.TranslatedText;
    }

    public static string Translate(string text, string languageCode)
    {
        if (string.IsNullOrWhiteSpace(text))
            return text;

        var result = _client.TranslateText(text, languageCode);

        return result.TranslatedText;
    }
}