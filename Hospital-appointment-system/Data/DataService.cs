using Hospital_appointment_system.Models;
using System.Text.Json;
using System.Xml.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Hospital_appointment_system.Data
{
    public class DataService
    {
        private readonly HttpClient _httpClient;

        public DataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Doctor>> GetDataAsync()
        {
            var response = await _httpClient.GetAsync("https://localhost:7188/api/DoctorApi");
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<IEnumerable<Doctor>>(jsonString) ?? new List<Doctor>();
        }
    }

}
