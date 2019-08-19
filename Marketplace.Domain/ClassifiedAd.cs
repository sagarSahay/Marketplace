namespace Marketplace.Domain
{
    using System;
    using Framework;

    public class ClassifiedAd : Entity
    {
        public ClassifiedAdId Id { get; private set; }

        public UserId OwnerId { get; private set; }
        public ClassifiedAdTitle Title { get; private set; }
        public ClassifiedAdText Text { get; private set; }
        public Price Price { get; private set; }
        public ClassifiedAdState State { get; private set; }
        public UserId ApprovedBy { get; private set; }

        public ClassifiedAd(ClassifiedAdId id, UserId ownerId)
        {
            Id = id;
            OwnerId = ownerId;
            State = ClassifiedAdState.Inactive;
            EnsureValidState();

            Apply(new Events.ClassifiedAdCreated
            {
                Id = id,
                OwnerId = ownerId
            });
        }

        public void SetTitle(ClassifiedAdTitle title)
        {
            Title = title;
            EnsureValidState();

            Apply(new Events.ClassifiedAdTitleChanged
            {
                Id = Id,
                Title = title
            });
        }

        public void UpdateText(ClassifiedAdText text)
        {
            Text = text;
            EnsureValidState();

            Apply(new Events.ClassifiedAdTextUpdated
            {
                AdText = text,
                Id = Id
            });
        }

        public void UpdatePrice(Price price)
        {
            Price = price;
            EnsureValidState();

            Apply(new Events.ClassifiedAdPriceUpdated
            {
                Id = Id,
                CurrencyCode = price.CurrencyCode,
                Price = price.Amount
            });
        }

        public void RequestToPublish()
        {
            State = ClassifiedAdState.PendingReview;
            EnsureValidState();

            Apply(new Events.ClassifiedAdSentForReview
            {
                Id = Id,
            });
        }

        protected override void EnsureValidState()
        {
            var valid = Id != null &&
                        OwnerId != null;

            switch (State)
            {
                case ClassifiedAdState.PendingReview:
                    valid = valid && Title != null && Text != null && Price?.Amount > 0;
                    break;
                case ClassifiedAdState.Active:
                    valid = valid && Title != null && Text != null && Price?.Amount > 0 && ApprovedBy != null;
                    break;
                default:
                    valid = valid && true;
                    break;
            }

            if (!valid)
            {
                throw new InvalidEntityStateException(this, $"Post checks have failed in {State}");
            }
        }

        protected override void When(object @event)
        {
            switch (@event)
            {
                case Events.ClassifiedAdCreated e:
                    Id = new ClassifiedAdId(e.Id);
                    OwnerId = new UserId(e.OwnerId);
                    State = ClassifiedAdState.Inactive;
                    break;
                case Events.ClassifiedAdPriceUpdated e:
                    Price = new Price(e.Price, e.CurrencyCode);
                    break;
                case Events.ClassifiedAdTextUpdated e:
                    Text = new ClassifiedAdText(e.AdText);
                    break;
                case Events.ClassifiedAdTitleChanged e:
                    Title = new ClassifiedAdTitle(e.Title);
                    break;
                case Events.ClassifiedAdSentForReview e:
                    State = ClassifiedAdState.PendingReview;
                    break;
            }
        }

        public enum ClassifiedAdState
        {
            PendingReview,
            Active,
            Inactive,
            MarkAsSold
        }
    }
}