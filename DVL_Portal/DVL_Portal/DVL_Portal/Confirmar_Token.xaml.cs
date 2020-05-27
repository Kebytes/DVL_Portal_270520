using DVL_Portal.API;
using DVL_Portal.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DVL_Portal
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Confirmar_Token : ContentPage
	{
		public Confirmar_Token ()
		{
			InitializeComponent ();
		}

        private async void Token_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TokenCod.Text))
            {
                await DisplayAlert("Un momento...", "Llenar el campo requerido.", "Entendido.");
            }
            else
            {
                string token = TokenCod.Text.ToString();
                if (Application.Current.Properties.ContainsKey("Token_Password"))
                {
                    if (token.Equals(Application.Current.Properties["Token_Password"].ToString()))
                    {
                        //if (Application.Current.Properties.ContainsKey("Usuario"))
                        //{
                            //Clientes cli = JsonConvert.DeserializeObject<Clientes>(Application.Current.Properties.ContainsKey("Usuario").ToString());
                            //Clientes nuevo = await Password_Controller.Cambiar_Contrasena(cli);
                            //if(nuevo != null)
                            //{
                                //Application.Current.Properties["Usuario"] = JsonConvert.SerializeObject(cli);
                                //await Application.Current.SavePropertiesAsync();
                        await ((NavigationPage)this.Parent).PushAsync(new Contrasena_Nueva());
                        await DisplayAlert("Éxito.", "Ingresa la nueva contraseña.", "Entendido.");
                        //}

                        //else
                        //{
                        //    await DisplayAlert("Momento...", "Ocurrió algún error.", "Entendido.");
                        //    //}
                        //}

                    }
                    else
                    {
                        await DisplayAlert("Momento...", "Código no válido.", "Entendido.");
                    }
                }
            }
        }

    }
}