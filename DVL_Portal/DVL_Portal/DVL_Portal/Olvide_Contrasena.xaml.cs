using DVL_Portal.API;
using DVL_Portal.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DVL_Portal
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Olvide_Contrasena : ContentPage
    {
        public Olvide_Contrasena()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Usuario.Text))
            {
                await DisplayAlert("Un momento...", "Llenar todos los campos.", "Entendido.");
            }

            else
            {
                string fecha = DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.CreateSpecificCulture("es-MX"));

                Clientes cli = new Clientes
                {
                    Correo_Cliente = Usuario.Text
                };

                string token = GetMD5(fecha + GetMD5(cli.Correo_Cliente)).Substring(0, 7);
                cli.Token = token;
                Application.Current.Properties["Token_Password"] = token;
                Application.Current.Properties["Usuario"] = JsonConvert.SerializeObject(cli);
                bool respuesta = await Password_Controller.Registrar_Token(cli);
                if (respuesta)
                {
                    await Application.Current.SavePropertiesAsync();
                    await ((NavigationPage)this.Parent).PushAsync(new Confirmar_Token());
                    await DisplayAlert("Éxito.", "Se ha enviado un código al correo proporcionado.", "Entendido.");
                }

                else
                {
                    await DisplayAlert("Error.", "Ocurrió un problema con la solicitud.", "Entendido.");
                }
            }
        }

        public static string GetMD5(string str)
        {
            MD5 md5 = MD5CryptoServiceProvider.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = null;
            StringBuilder sb = new StringBuilder();
            stream = md5.ComputeHash(encoding.GetBytes(str));
            for (int i = 0; i < stream.Length; i++)
                sb.AppendFormat("{0:x2}", stream[i]);
            return sb.ToString();
        }
    }
}