using System.Net.Http;
using System.Threading.Tasks;

namespace PruebaIngreso.Services
{
    public interface IApiService
    {
        Task<string> GetMarginAsync(string code);
    }

    public class ApiService : IApiService
    {
        private static readonly HttpClient client = new HttpClient();

        public async Task<string> GetMarginAsync(string code)
        {
            string apiUrl = $"https://refactored-pancake.free.beeceptor.com/margin/{code}";
            var response = await client.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                return "{\"margin\": 0.0}";
            }
        }
    }
}