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
    class Pedidos_Controller
    {
        static HttpClient cliente;

        static string BASEURL = "http://localhost:49911/api/pedidos";

        public static int Insertar_Pedido(Pedido pedido)
        {
            var uri = new Uri("/pedidos/Insertar_Pedido");
            var client = new RestClient(BASEURL + uri);
            var request = new RestRequest();
            //var request = new RestRequest();

            //var json = Newtonsoft.Json.JsonConvert.SerializeObject(pedido);

            request.Method = Method.POST;
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("Accept", "application/json");
            request.Parameters.Clear();
            //request.AddJsonBody(pedido);
            request.AddParameter("application/json", JsonConvert.SerializeObject(pedido), ParameterType.RequestBody);

            var response = client.Execute(request);

            if (response.IsSuccessful)
            {
                var id = response.Content;
                return Int32.Parse(id);
            }
            else
                return 0;




            //cliente = new HttpClient();

            //var request = new HttpRequestMessage(HttpMethod.Post, BASEURL + uri);

            //var json = Newtonsoft.Json.JsonConvert.SerializeObject(pedido);


            //request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            //var response = await cliente.SendAsync(request);

            //if (response.IsSuccessStatusCode)
            //{

            //}


            //using (cliente = new HttpClient())
            //{
            //    var response = cliente.PostAsync(BASEURL + uri, new StringContent(json, Encoding.UTF8, "application/json"));
            //    response.Wait();
            //    if (response.Result.IsSuccessStatusCode)
            //    {
            //        var id = response.Result.Content.ReadAsStringAsync();
            //        return id;
            //    }
            //}

            //return 0;
            //request.Content =
            //Mandar objeto por body
        }

        public static async Task<List<Pedido>> GetPedidosById(int id)
        {
            cliente = new HttpClient();
            var BaseUrl = "http://localhost:49911/api/pedidos/PedidosPorId";
            //var BaseUrl = "http://apidvlsystem.developmxhost.com/api/cliente/getInfo";

            //var uri = new Uri("http://apidvlsystem.developmxhost.com/api/cliente/getInfo");
            //var uri = new Uri("/getInfo");

            var request = new HttpRequestMessage(HttpMethod.Get, BaseUrl);
            request.Headers.Add("id_Clientes", id.ToString());

            var response = await cliente.SendAsync(request);

            try
            {
                if (response.IsSuccessStatusCode)
                {
                    var json = response.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<List<Pedido>>(json);
                }

                else
                    return null;
            }

            catch (Exception e) { e.ToString(); return null; }
        }

        public static async Task<Pedido> InsertarPedido(Pedido pedido)
        {
            try
            {
                //http://localhost:49911/api/pedidos/InsertarPedido
                //Cliente
                HttpClient cliente = new HttpClient();

                //Crear la petición
                var request = new HttpRequestMessage(HttpMethod.Post, new Uri("http://apidvlsystem.developmxhost.com/api/pedidos/InsertarPedido"));

                //Serializar objeto
                string json = JsonConvert.SerializeObject(pedido);

                //Agregar el json a la petición
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");

                //Enviar la petición
                var response = await cliente.SendAsync(request);

                //Obtener la respuesta
                string respuesta = response.Content.ReadAsStringAsync().Result;

                //Validar que no haya error
                if (!response.IsSuccessStatusCode)
                    return null;

                //Deserializar el json de la respuesta
                Pedido realizado = new Pedido();
                realizado = JsonConvert.DeserializeObject<Pedido>(respuesta);
                return realizado;

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error al insertar el pedido: " + ex.ToString());
                return null;
            }
        }

        public static async Task<bool> Cancelar_Pedido(Pedido pedido)
        {
            try
            {
                HttpClient cliente = new HttpClient();

                var request = new HttpRequestMessage(HttpMethod.Post, new Uri("http://apidvlsystem.developmxhost.com/api/pedidos/CancelarPedido"));

                string json = JsonConvert.SerializeObject(pedido);

                request.Content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await cliente.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                    return false;
                else
                    return true;
            }

            catch (Exception ex)
            {
                Debug.WriteLine("Error al cancelar el pedido: " + ex.ToString());
                return false;
            }
        }

        public static async Task<bool> Actualizar_Pedido(Pedido pedido)
        {
            try
            {
                HttpClient cliente = new HttpClient();

                var request = new HttpRequestMessage(HttpMethod.Post, new Uri("http://apidvlsystem.developmxhost.com/api/pedidos/ActualizarPedido"));

                string json = JsonConvert.SerializeObject(pedido);

                request.Content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await cliente.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                    return false;
                else
                    return true;
            }

            catch (Exception ex)
            {
                Debug.WriteLine("Error al actualizar el pedido: " + ex.ToString());
                return false;
            }
        }

        public static async Task<string> GetPedidosOnly(int id)
        {
            cliente = new HttpClient();
            //var BaseUrl = "http://localhost:49911/api/cliente";
            var BaseUrl = "http://apidvlsystem.developmxhost.com/api/pedidos/PedidosPorIdOnly";

            //var uri = new Uri("http://apidvlsystem.developmxhost.com/api/cliente/getInfo");
            //var uri = new Uri("/getInfo");

            var request = new HttpRequestMessage(HttpMethod.Get, BaseUrl);
            request.Headers.Add("id_Clientes", id.ToString());

            var response = await cliente.SendAsync(request);

            try
            {
                if (response.IsSuccessStatusCode)
                {
                    var json = response.Content.ReadAsStringAsync().Result;
                    //return JsonConvert.DeserializeObject<Cliente>(json);
                    return json;
                }

                else
                    return null;
            }

            catch (Exception e) { e.ToString(); return null; }
        }

        public static async Task<string> GetPedidosClienteEstacion(int id)
        {
            cliente = new HttpClient();
            //var BaseUrl = "http://localhost:49911/api/cliente";
            var BaseUrl = "http://apidvlsystem.developmxhost.com/api/pedidos/Pedidos_Estacion";

            var request = new HttpRequestMessage(HttpMethod.Get, BaseUrl);
            request.Headers.Add("id_Estacion", id.ToString());

            var response = await cliente.SendAsync(request);

            try
            {
                if (response.IsSuccessStatusCode)
                {
                    var json = response.Content.ReadAsStringAsync().Result;
                    //return JsonConvert.DeserializeObject<Cliente>(json);
                    return json;
                }

                else
                    return null;
            }

            catch (Exception e) { e.ToString(); return null; }
        }

        public static async Task<string> GetPedidosEstacion(int id, int estacion)
        {
            cliente = new HttpClient();
            //var BaseUrl = "http://localhost:49911/api/cliente";
            var BaseUrl = "http://apidvlsystem.developmxhost.com/api/pedidos/PedidosPorEstacion";

            var request = new HttpRequestMessage(HttpMethod.Get, BaseUrl);
            request.Headers.Add("id_Clientes", id.ToString());
            request.Headers.Add("Estacion", estacion.ToString());

            var response = await cliente.SendAsync(request);

            try
            {
                if (response.IsSuccessStatusCode)
                {
                    var json = response.Content.ReadAsStringAsync().Result;
                    //return JsonConvert.DeserializeObject<Cliente>(json);
                    return json;
                }

                else
                    return null;
            }

            catch (Exception e) { e.ToString(); return null; }
        }
    }
}
