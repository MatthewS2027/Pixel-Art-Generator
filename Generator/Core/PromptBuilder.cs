
namespace PixelArtGenerator.Core
{
    public class PromptBuilder
    {
        private string _subject = string.Empty;
        private string _style = string.Empty;
        private string _detail = string.Empty;
        private string _mood = string.Empty;
        private string _composition = string.Empty;
        private string _palette = string.Empty;
        private string _extra = string.Empty;

        public static PromptBuilder Create() => new PromptBuilder();

        public PromptBuilder WithSubject(string subject)
        {
            _subject = subject?.Trim() ?? string.Empty;
            return this;
        }

        public PromptBuilder WithStyle(string style)
        {
            _style = style?.Trim() ?? string.Empty;
            return this;
        }

        public PromptBuilder WithDetail(string detail)
        {
            _detail = detail?.Trim() ?? string.Empty;
            return this;
        }

        public PromptBuilder WithMood(string mood)
        {
            _mood = mood?.Trim() ?? string.Empty;
            return this;
        }

        public PromptBuilder WithComposition(string composition)
        {
            _composition = composition?.Trim() ?? string.Empty;
            return this;
        }

        public PromptBuilder WithColorPalette(string palette)
        {
            _palette = palette?.Trim() ?? string.Empty;
            return this;
        }

        public PromptBuilder WithExtra(string extra)
        {
            _extra = extra?.Trim() ?? string.Empty;
            return this;
        }

        public string Build()
        {
            var parts = new System.Collections.Generic.List<string>();

            if (!string.IsNullOrWhiteSpace(_subject))
            {
                parts.Add(_subject);
            }

            if (!string.IsNullOrWhiteSpace(_style))
            {
                parts.Add(_style);
            }

            if (!string.IsNullOrWhiteSpace(_detail))
            {
                parts.Add(_detail);
            }

            if (!string.IsNullOrWhiteSpace(_mood))
            {
                parts.Add(_mood);
            }

            if (!string.IsNullOrWhiteSpace(_composition))
            {
                parts.Add(_composition);
            }

            if (!string.IsNullOrWhiteSpace(_palette))
            {
                parts.Add(_palette);
            }

            if (!string.IsNullOrWhiteSpace(_extra))
            {
                parts.Add(_extra);
            }

            return string.Join(", ", parts);
        }
    }
}
