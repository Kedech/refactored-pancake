using Newtonsoft.Json;
using PruebaIngreso.Models;
using Quote.Contracts;
using Quote.Models;
using System.Threading.Tasks;

namespace PruebaIngreso.Services
{
    public class MarginProviderDecorator : IMarginProvider
    {
        public readonly TourQuote _tourQuote;
        private readonly IApiService _apiService;
        private decimal _margin;

        public MarginProviderDecorator(TourQuote quote, IApiService apiService)
        {
            this._tourQuote = quote;
            this._apiService = apiService;
            this._margin = GetMargin(_tourQuote.ContractService.ServiceCode);
        }

        public decimal margin => _margin;

        public decimal GetMargin(string code)
        {
            var marginJson = "";
            Task.Run(async () =>
            {
                marginJson = await _apiService.GetMarginAsync(code);
            }).GetAwaiter().GetResult();

            var marginData = JsonConvert.DeserializeObject<MarginResponse>(marginJson);
            return marginData?.Margin ?? 0.0m;
        }
    }
}