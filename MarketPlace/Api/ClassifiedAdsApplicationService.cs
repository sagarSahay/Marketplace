namespace MarketPlace.Api
{
    using System;
    using System.Threading.Tasks;
    using Contracts;
    using Marketplace.Domain;
    using Marketplace.Framework;

    public class ClassifiedAdsApplicationService : IApplicationService
    {
        private readonly IEntityStore _repository;
        private readonly ICurrencyLookup _currencyLookup;

        public ClassifiedAdsApplicationService(IEntityStore repository,
            ICurrencyLookup currencyLookup)
        {
            _repository = repository;
            _currencyLookup = currencyLookup;
        }

        public async Task Handle(object command)
        {
            switch (command)
            {
                case ClassifiedAds.V1.Create cmd:
                    await HandleCreate(cmd);
                    break;
                case ClassifiedAds.V1.SetTitle cmd:
                    await HandleUpdate(cmd.Id, c => c.SetTitle(ClassifiedAdTitle.FromString(cmd.Title)));
                    break;
                case ClassifiedAds.V1.UpdateText cmd:
                    await HandleUpdate(cmd.Id, c => c.UpdateText(ClassifiedAdText.FromString(cmd.Text)));
                    break;
                case ClassifiedAds.V1.UpdatePrice cmd:
                    await HandleUpdate(cmd.Id,
                        c => c.UpdatePrice(Price.FromDecimal(cmd.Price, cmd.Currency, _currencyLookup)));
                    break;
                case ClassifiedAds.V1.RequestToPublish cmd:
                    await HandleUpdate(cmd.Id, c => c.RequestToPublish());
                    break;
                default:
                    throw new InvalidOperationException($"Command type {command.GetType().FullName} is unknown.");
            }
        }

        private async Task HandleCreate(ClassifiedAds.V1.Create cmd)
        {
            if (await _repository.Exists<ClassifiedAd>(cmd.Id.ToString()))
            {
                throw new InvalidOperationException($"Entity with id {cmd.Id} already exists");
            }

            var classifiedAd = new ClassifiedAd(new ClassifiedAdId(cmd.Id), new UserId(cmd.OwnerId));
            await _repository.Save(classifiedAd);
        }

        private async Task HandleUpdate(Guid id, Action<ClassifiedAd> operation)
        {
            var classifiedAd = await _repository.Load<ClassifiedAd>(id.ToString());
            if (classifiedAd == null)
            {
                throw new InvalidOperationException($"Entity with id {id} cannot be found");
            }

            operation(classifiedAd);
            await _repository.Save(classifiedAd);
        }
    }
}