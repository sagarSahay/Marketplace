using Microsoft.AspNetCore.Mvc;

namespace MarketPlace.Api
{
    using System.Threading.Tasks;
    using Contracts;

    [Route("/ad")]
    public class ClassifiedAdsCommandsApi : Controller
    {
        private readonly ClassifiedAdsApplicationService _applicationService;

        public ClassifiedAdsCommandsApi(ClassifiedAdsApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        [HttpPost]
        public async Task<IActionResult> Post(ClassifiedAds.V1.Create request)
        {
            _applicationService.Handle(request);
            return Ok();
        }
    }
}