
namespace PixelArtGenerator.Core
{
    public class PromptBuilder
    {
        private string _description = string.Empty;
        private string _imageSize = string.Empty;
        private string _palette = string.Empty;
        private string _detail = string.Empty;
        private string _direction = string.Empty;
        private string _orientation = string.Empty;

        public static PromptBuilder Create() => new PromptBuilder();

        public PromptBuilder WithDescription(string description)
        {
            _description = description?.Trim() ?? string.Empty;
            return this;
        }

        public PromptBuilder WithImageSize(string imageSize)
        {
            _imageSize = imageSize?.Trim() ?? string.Empty;
            return this;
        }

        public PromptBuilder WithColorPalette(string palette)
        {
            _palette = palette?.Trim() ?? string.Empty;
            return this;
        }

        public PromptBuilder WithDetail(string detail)
        {
            _detail = detail?.Trim() ?? string.Empty;
            return this;
        }

        public PromptBuilder WithSubjectDirection(string direction)
        {
            _direction = direction?.Trim() ?? string.Empty;
            return this;
        }

        public PromptBuilder WithOrientation(string orientation)
        {
            _orientation = orientation?.Trim() ?? string.Empty;
            return this;
        }

        public string Build()
        {
            var parts = new System.Collections.Generic.List<string>();

            AddField(parts, "Text description", _description);
            AddField(parts, "Image size", _imageSize);
            AddField(parts, "Colors", _palette);
            AddField(parts, "Level of detail", _detail);
            AddField(parts, "Subject direction", _direction);
            AddField(parts, "Orientation", _orientation);

            return string.Join(", ", parts);
        }

        private static void AddField(System.Collections.Generic.List<string> parts, string name, string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                parts.Add($"{name} = {value}");
            }
        }
    }
}
