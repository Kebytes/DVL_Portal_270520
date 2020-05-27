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
	public partial class Contrasena_Nueva : ContentPage
	{
		public Contrasena_Nueva ()
		{
			InitializeComponent ();
		}

        private async void Button_Clicked(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(Password.Text) || string.IsNullOrEmpty(Password2.Text))
            {
                await DisplayAlert("Un momento...", "Llenar todos los campos.", "Entendido.");
            }

            else
            {
                if(Password.Text.ToString().Equals(Password2.Text.ToString()))
                {
                    Clientes cli = JsonConvert.DeserializeObject<Clientes>(Application.Current.Properties["Usuario"].ToString());
                    cli.Contrasena = Password.Text.ToString();
                    Clientes nuevo = await Password_Controller.Cambiar_Contrasena(cli);
                    if (nuevo != null)
                    {
                        Application.Current.Properties["Usuario"] = JsonConvert.SerializeObject(nuevo);
                        await Application.Current.SavePropertiesAsync();
                        await DisplayAlert("Éxito.", "Contraseña actualizada.", "Entendido.");
                        await ((NavigationPage)this.Parent).PushAsync(new MainPage());
                    }

                    else
                    {
                        await DisplayAlert("Momento...", "Ocurrió algún error.", "Entendido.");
                    }
                }
                else
                {
                    await DisplayAlert("Error.", "Las contraseñas no coinciden.", "Entendido.");
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