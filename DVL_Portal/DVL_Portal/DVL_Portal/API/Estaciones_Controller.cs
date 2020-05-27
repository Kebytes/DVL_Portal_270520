using DVL_Portal.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DVL_Portal.API
{
    class Estaciones_Controller
    {
        #region VARIABLES
        //http://apidvlsystem.developmxhost.com/
        //http://localhost:49911
        private static readonly string BaseUrl = "http://apidvlsystem.developmxhost.com/api/Estaciones";
        private static HttpClient cliente;
        #endregion

        public static async Task<string> GetEstacionesPorId(string id)
        {
            cliente = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, new Uri(BaseUrl + "/PorId"));
            request.Headers.Add("Id_Usuario", id);

            var response = await cliente.SendAsync(request);

            try
            {
                if (response.IsSuccessStatusCode)
                {
                    var json = response.Content.ReadAsStringAsync().Result;
                    //List<Estacion> estaciones = JsonConvert.DeserializeObject<List<Estacion>>(json);
                    //return estaciones;
                    return json;
                }
                return null;
            }
            catch (Exception e) { e.ToString(); return null; }

        }

        public static async Task<string> GetEstacion(string idEstacion, string numeroEstacion)
        {
            cliente = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, new Uri(BaseUrl + "/PorEstacion"));
            request.Headers.Add("Numero_Estacion", numeroEstacion);
            request.Headers.Add("id_Estaciones", idEstacion);

            var response = await cliente.SendAsync(request);

            try
            {
                if (response.IsSuccessStatusCode)
                {
                    var json = response.Content.ReadAsStringAsync().Result;
                    //List<Estacion> estaciones = JsonConvert.DeserializeObject<List<Estacion>>(json);
                    //return estaciones;
                    return json;
                }
                return null;
            }
            catch (Exception e) { e.ToString(); return null; }

        }

        public static async Task<bool> InsertarPedido(Models.Estacion estacion)
        {
            string Base = "http://apidvlsystem.developmxhost.com/api/Estaciones";
            try
            {
                //http://localhost:49911/api/pedidos/InsertarPedido
                //Cliente
                HttpClient cliente = new HttpClient();

                //Crear la petición
                var request = new HttpRequestMessage(HttpMethod.Post, new Uri(Base + "/Insertar_Estacion"));

                //Serializar objeto
                string json = JsonConvert.SerializeObject(estacion);

                //Agregar el json a la petición
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");

                //Enviar la petición
                var response = await cliente.SendAsync(request);

                //Obtener la respuesta
                string respuesta = response.Content.ReadAsStringAsync().Result;

                //Validar que no haya error
                if (!response.IsSuccessStatusCode)
                    return false;

                //Deserializar el entero de la respuesta
                Estacion realizado = new Estacion();
                realizado = JsonConvert.DeserializeObject<Estacion>(respuesta);
                return true;

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error al insertar el pedido: " + ex.ToString());
                return false;
            }
        }
    }
}
