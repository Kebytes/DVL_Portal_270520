using DVL_Portal.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DVL_Portal.API
{
    class Password_Controller
    {
        private static readonly string BASEURL = "http://apidvlsystem.developmxhost.com/";

        public static async Task<bool> Registrar_Token(Clientes clie)
        {
            try
            {
                HttpClient cliente = new HttpClient();

                //Crear la petición
                var request = new HttpRequestMessage(HttpMethod.Post, new Uri(BASEURL+ "/Password/SendToken"));

                string json = JsonConvert.SerializeObject(clie);

                //Agregar el json a la petición
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");

                //Enviar la petición
                var response = await cliente.SendAsync(request);

                //Obtener la respuesta
                string respuesta = response.Content.ReadAsStringAsync().Result;

                //Validar que no haya error
                if (!response.IsSuccessStatusCode)
                    return false;

                return true;

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error al realizar la solicitud." + ex.ToString());
                return false;
            }
        }

        public static async Task<Clientes> Cambiar_Contrasena(Clientes cli)
        {
            try
            {
                HttpClient cliente = new HttpClient();

                //Crear la petición
                var request = new HttpRequestMessage(HttpMethod.Post, new Uri(BASEURL + "/Password/Cambiar"));

                string json = JsonConvert.SerializeObject(cli);

                //Agregar el json a la petición
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");

                //Enviar la petición
                var response = await cliente.SendAsync(request);

                //Obtener la respuesta
                string respuesta = response.Content.ReadAsStringAsync().Result;

                //Validar que no haya error
                if (!response.IsSuccessStatusCode)
                    return null;

                return JsonConvert.DeserializeObject<Clientes>(respuesta);

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error al realizar la solicitud." + ex.ToString());
                return null;
            }
        }
    }
}
