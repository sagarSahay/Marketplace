namespace Marketplace.Domain
{
    using System;
    using Framework;

    public class ClassifiedAdId : Value<ClassifiedAdId>
    {
        private readonly Guid _value;

        public ClassifiedAdId(Guid value)
        {
            if (value == default(Guid))
            {
                throw new ArgumentNullException(nameof(value), "Classified Ad id cannot be empty");
            }

            _value = value;
        }

        public static implicit operator Guid(ClassifiedAdId self) => self._value;
    }
}