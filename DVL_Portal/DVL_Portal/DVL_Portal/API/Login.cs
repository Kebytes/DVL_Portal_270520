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
    class Login
    {
        static HttpClient cliente;

        static string BASEURL = "http://localhost:49911/api";

        public static async Task<List<Pedido>> Pedidos()
        {
            cliente = new HttpClient();

            var uri = new Uri("/pedidos"); ///pedido

            var request = new HttpRequestMessage(HttpMethod.Get, BASEURL + uri);


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


        public static async Task<string> IniciarSesion(string email, string contrasena)
        {
            cliente = new HttpClient();
            //var BaseUrl = "http://localhost:49911/api/cliente";
            var BaseUrl = "http://apidvlsystem.developmxhost.com/api/cliente/getInfo";

            var request = new HttpRequestMessage(HttpMethod.Get, BaseUrl);
            request.Headers.Add("Correo_Cliente", email);
            request.Headers.Add("Contrasena", contrasena);

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

        public static async Task<string> IniciarSesion(int estacion, string contrasena)
        {
            cliente = new HttpClient();
            //var BaseUrl = "http://localhost:49911/api/cliente";
            var BaseUrl = "http://apidvlsystem.developmxhost.com/api/cliente/Estacion";

            var request = new HttpRequestMessage(HttpMethod.Get, BaseUrl);
            request.Headers.Add("Estacion", estacion.ToString());
            request.Headers.Add("Contrasena", contrasena);

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

        public static async Task<bool> Comprobar_Estado()
        {
            cliente = new HttpClient();
            var BASEURL = "http://apidvlsystem.developmxhost.com/api/cliente/Registrar";
            var request = new HttpRequestMessage(HttpMethod.Get, BASEURL);
            request.Headers.Add("Estado", "");

            var response = await cliente.SendAsync(request);

            try
            {
                if (response.IsSuccessStatusCode)
                {
                    var json = response.Content.ReadAsStringAsync().Result;
                    bool bandera = Convert.ToBoolean(json);
                    return bandera;
                }
                else
                    return false;
            }
            catch(Exception e) { return false; }
        }

        public static async Task<Clientes> InsertarCliente(Clientes cli)
        {
            try
            {
                //Cliente
                HttpClient cliente = new HttpClient();

                //Crear la petición
                var request = new HttpRequestMessage(HttpMethod.Post, new Uri("http://apidvlsystem.developmxhost.com/api/cliente/InsertarCliente"));

                //Serializar objeto
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

                //Deserializar el json de la respuesta
                Clientes realizado = new Clientes();
                realizado = JsonConvert.DeserializeObject<Clientes>(respuesta);
                return realizado;

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error al insertar al cliente: " + ex.ToString());
                return null;
            }
        }
    }
}
