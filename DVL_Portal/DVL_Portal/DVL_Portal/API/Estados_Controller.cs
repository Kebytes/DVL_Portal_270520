using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DVL_Portal.API
{
    public class Estados_Controller
    {
        private static readonly string BaseUrl = "http://apidvlsystem.developmxhost.com/api/";
        private static HttpClient cliente;

        public static async Task<string> GetEstados()
        {
            cliente = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, new Uri(BaseUrl + "/Estados/getEstados"));
            request.Headers.Add("Obtener", "");

            var response = await cliente.SendAsync(request);

            try
            {
                if (response.IsSuccessStatusCode)
                {
                    var json = response.Content.ReadAsStringAsync().Result;
                    return json;
                }
                return null;
            }
            catch (Exception e) { e.ToString(); return null; }
        }

        public static async Task<string> GetCiudades()
        {
            cliente = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, new Uri(BaseUrl + "/Ciudades/getCiudades"));
            request.Headers.Add("Obtener", "");

            var response = await cliente.SendAsync(request);

            try
            {
                if (response.IsSuccessStatusCode)
                {
                    var json = response.Content.ReadAsStringAsync().Result;
                    return json;
                }
                return null;
            }
            catch (Exception e) { e.ToString(); return null; }
        }
    }
}
