using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using PruebaIngreso.Services;
using Quote.Contracts;
using Quote.Models;

namespace PruebaIngreso.Controllers
{
    public class HomeController : Controller
    {
        private readonly IQuoteEngine quote;
        private readonly IApiService _apiService;

        public HomeController(IQuoteEngine quote, IApiService apiService)
        {
            this.quote = quote;
            this._apiService = apiService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Test()
        {
            var request = new TourQuoteRequest
            {
                adults = 1,
                ArrivalDate = DateTime.Now.AddDays(1),
                DepartingDate = DateTime.Now.AddDays(2),
                getAllRates = true,
                GetQuotes = true,
                RetrieveOptions = new TourQuoteRequestOptions
                {
                    GetContracts = true,
                    GetCalculatedQuote = true,
                },
                TourCode = "E-U10-PRVPARKTRF",
                Language = Language.Spanish
            };

            var result = this.quote.Quote(request);
            var tour = result.Tours.FirstOrDefault();
            ViewBag.Message = "Test 1 Correcto";
            return View(tour);
        }

        [ActionName("prueba2")]
        public ActionResult Test2()
        {
            ViewBag.Message = "Test 2 Correcto";
            ViewBag.Title = "Test";
            return View("Test2");
        }

        public async Task<ActionResult> Test3()
        {
            try
            {
                //Codigos para probar los status: E-U10-UNILATIN 204, E-U10-DSCVCOVE 404 y E-E10-PF2SHOW 500
                string marginResponse = await _apiService.GetMarginAsync("E-E10-PF2SHOW");
                ViewBag.Margin = marginResponse;
            }
            catch (Exception)
            {
                ViewBag.Margin = "{\"margin\": 0.0}";
            }

            return View();
        }

        public ActionResult Test4()
        {
            var request = new TourQuoteRequest
            {
                adults = 1,
                ArrivalDate = DateTime.Now.AddDays(1),
                DepartingDate = DateTime.Now.AddDays(2),
                getAllRates = true,
                GetQuotes = true,
                RetrieveOptions = new TourQuoteRequestOptions
                {
                    GetContracts = true,
                    GetCalculatedQuote = true,
                },
                Language = Quote.Models.Language.Spanish
            };
            var apiService = new ApiService();
            var result = this.quote.Quote(request);
            var marginDecorator = new List<MarginProviderDecorator>();
            foreach (var quote in result.TourQuotes)
            {
                var decorator = new MarginProviderDecorator(quote, apiService);
                marginDecorator.Add(decorator);
            }
            return View(marginDecorator);
        }
    }
}