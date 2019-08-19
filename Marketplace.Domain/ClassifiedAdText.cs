namespace Marketplace.Domain
{
    using Framework;

    public class ClassifiedAdText : Value<ClassifiedAdText>
    {
        private readonly string _value;

        internal ClassifiedAdText(string value)
        {
            _value = value;
        }
        
        public static ClassifiedAdText FromString(string text) => new ClassifiedAdText(text);
        
        public static implicit operator string(ClassifiedAdText self) => self._value;
    }
}