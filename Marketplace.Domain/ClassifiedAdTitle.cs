namespace Marketplace.Domain
{
    using System;
    using System.Text.RegularExpressions;
    using Framework;

    public class ClassifiedAdTitle : Value<ClassifiedAdTitle>
    {
        private readonly string _value;

        public static ClassifiedAdTitle FromString(string title) => new ClassifiedAdTitle(title);

        public static ClassifiedAdTitle FromHtml(string htmlTitle)
        {
            var supportedTagsReplaced = htmlTitle
                .Replace("<i>", "*")
                .Replace("</i>", "*")
                .Replace("<b>", "**")
                .Replace("</b>", "**");

            return new ClassifiedAdTitle(Regex.Replace(supportedTagsReplaced, "<.*?>", string.Empty));
        }

        internal ClassifiedAdTitle(string value)
        {
            if (value.Length > 100)
                throw new ArgumentException("Title cannot be greater than 100 characters", nameof(value));

            _value = value;
        }
        
        public static implicit operator string(ClassifiedAdTitle self) => self._value;
    }
}