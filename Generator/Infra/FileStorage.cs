using System;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PixelArtGenerator.Infrastructure
{
    public record FileStorageResult(string JsonPath, string? PngPath);

    public static class FileStorage
    {
        public static FileStorageResult SaveResponse(string responseJson, string outputFolder, string fileNameBase)
        {
            Directory.CreateDirectory(outputFolder);

            string safeBaseName = SanitizeFileName(fileNameBase);
            string jsonPath = Path.Combine(outputFolder, safeBaseName + ".json");
            File.WriteAllText(jsonPath, responseJson, Encoding.UTF8);

            string? pngPath = null;
            if (TryExtractImageBytes(responseJson, out var imageBytes))
            {
                pngPath = Path.Combine(outputFolder, safeBaseName + ".png");
                File.WriteAllBytes(pngPath, imageBytes);
            }

            return new FileStorageResult(jsonPath, pngPath);
        }

        private static bool TryExtractImageBytes(string responseJson, out byte[] imageBytes)
        {
            imageBytes = Array.Empty<byte>();
            if (string.IsNullOrWhiteSpace(responseJson))
            {
                return false;
            }

            JToken parsed;
            try
            {
                parsed = JToken.Parse(responseJson);
            }
            catch (JsonException)
            {
                return false;
            }

            var candidate = FindBase64ImageToken(parsed);
            if (candidate == null)
            {
                return false;
            }

            var rawValue = candidate.Value<string>()?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(rawValue))
            {
                return false;
            }

            rawValue = StripDataUriPrefix(rawValue);
            if (!TryDecodeBase64(rawValue, out imageBytes))
            {
                return false;
            }

            return imageBytes.Length > 0;
        }

        private static JToken? FindBase64ImageToken(JToken token)
        {
            if (token.Type == JTokenType.Object)
            {
                var obj = (JObject)token;
                foreach (var property in obj.Properties())
                {
                    if (IsImageField(property.Name) && property.Value.Type == JTokenType.String)
                    {
                        var candidate = property.Value.Value<string>() ?? string.Empty;
                        if (!string.IsNullOrWhiteSpace(candidate) && LooksLikeBase64(candidate))
                        {
                            return property.Value;
                        }
                    }

                    var childMatch = FindBase64ImageToken(property.Value);
                    if (childMatch != null)
                    {
                        return childMatch;
                    }
                }
            }
            else if (token.Type == JTokenType.Array)
            {
                foreach (var child in token.Children())
                {
                    var childMatch = FindBase64ImageToken(child);
                    if (childMatch != null)
                    {
                        return childMatch;
                    }
                }
            }

            return null;
        }

        private static bool IsImageField(string fieldName)
        {
            return new[]
            {
                "image",
                "image_base64",
                "b64_json",
                "base64",
                "imageData",
                "image_data",
                "encoded_image",
                "data"
            }
            .Contains(fieldName, StringComparer.OrdinalIgnoreCase);
        }

        private static string StripDataUriPrefix(string value)
        {
            const string prefix = "data:image/png;base64,";
            if (value.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
            {
                return value.Substring(prefix.Length);
            }

            var commaIndex = value.IndexOf(',', StringComparison.Ordinal);
            if (commaIndex > 0 && value.Substring(0, commaIndex).Contains("base64", StringComparison.OrdinalIgnoreCase))
            {
                return value.Substring(commaIndex + 1);
            }

            return value;
        }

        private static bool LooksLikeBase64(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            value = value.Trim();
            if (value.Length < 20)
            {
                return false;
            }

            if (value.Contains(' ') || value.Contains('\n') || value.Contains('\r'))
            {
                value = value.Replace("\r", string.Empty).Replace("\n", string.Empty).Replace(" ", string.Empty);
            }

            return value.All(c => char.IsLetterOrDigit(c) || c == '+' || c == '/' || c == '=' || c == '_');
        }

        private static bool TryDecodeBase64(string value, out byte[] bytes)
        {
            bytes = Array.Empty<byte>();
            try
            {
                bytes = Convert.FromBase64String(value);
                return bytes.Length > 0;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        private static string SanitizeFileName(string fileName)
        {
            var invalidChars = Path.GetInvalidFileNameChars();
            var sanitized = string.Concat(fileName.Select(ch => invalidChars.Contains(ch) ? '_' : ch));
            return string.IsNullOrWhiteSpace(sanitized) ? "output" : sanitized;
        }
    }
}
