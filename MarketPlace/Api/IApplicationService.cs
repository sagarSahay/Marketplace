namespace MarketPlace.Api
{
    using System.Threading.Tasks;

    public interface IApplicationService
    {
        Task Handle(object command);
    }
}