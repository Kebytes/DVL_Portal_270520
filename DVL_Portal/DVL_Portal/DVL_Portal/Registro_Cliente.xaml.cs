using DVL_Portal.API;
using DVL_Portal.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DVL_Portal
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Registro_Cliente : ContentPage
    {
        public Registro_Cliente()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            if (Validar_Datos())
            {
                Clientes cli = new Clientes()
                {
                    Razon_Social = Razon.Text.ToString(),
                    Telefono = Telefono.Text.ToString(),
                    Correo_Cliente = Email.Text.ToString(),
                    Contrasena = MD5(Password.Text.ToString()),
                    RFC = RFC.Text.ToString(),
                    Direccion_Factura = Direccion.Text.ToString(),
                    Correo_Factura = Email_Factura.Text.ToString(),
                    Nombre_Contacto = Contacto.Text.ToString(),
                    Reparto = Reparto.Text.ToString(),
                    Telefono_Contacto = Telefono_Contacto.Text.ToString()
                };

                Clientes agregado = await API.Login.InsertarCliente(cli);
                if(agregado != null)
                {
                    var json = JsonConvert.SerializeObject(agregado);
                    Application.Current.Properties["Usuario"] = json;
                    string pedidos = await Pedidos_Controller.GetPedidosOnly(agregado.id_Clientes);
                    Application.Current.Properties["Pedidos"] = pedidos;
                    await Application.Current.SavePropertiesAsync();
                    await ((NavigationPage)this.Parent).PushAsync(new MainPage());
                }
                else
                {
                    await DisplayAlert("Un momento...", "Cliente no agregado.", "Entendido.");
                }
            }
            else
            {
                await DisplayAlert("Un momento...", "Es necesario llenar todos los campos.","Entendido.");
            }
        }

        public bool Validar_Datos()
        {
            bool bandera = false;

            if (string.IsNullOrEmpty(Razon.Text) || string.IsNullOrEmpty(Telefono.Text) || string.IsNullOrEmpty(Email.Text)
                || string.IsNullOrEmpty(Password.Text) || string.IsNullOrEmpty(RFC.Text) || string.IsNullOrEmpty(Direccion.Text)
                || string.IsNullOrEmpty(Email_Factura.Text) || string.IsNullOrEmpty(Contacto.Text) || string.IsNullOrEmpty(Reparto.Text)
                || string.IsNullOrEmpty(Telefono_Contacto.Text))
                bandera = false;


            else
            {
                bandera = true;
            }

            return bandera;
        }

        public static string MD5(string password)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            //Calcular el valor hash
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(password));
            //Obtener los bytes del hash
            byte[] resultado = md5.Hash;

            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < resultado.Length; i++)
                stringBuilder.Append(resultado[i].ToString("x2"));

            return stringBuilder.ToString();
        }
    }
}