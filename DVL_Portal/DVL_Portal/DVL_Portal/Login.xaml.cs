using DVL_Portal.API;
using DVL_Portal.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DVL_Portal
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Login : ContentPage
	{
        private bool estado;
        

        public Login ()
		{
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent ();
		}

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (Application.Current.Properties.ContainsKey("Usuario"))
            {
                Clientes cli = JsonConvert.DeserializeObject<Clientes>(Application.Current.Properties["Usuario"].ToString());
                if(cli.id_Clientes != 0)
                {
                    await ((NavigationPage)this.Parent).PushAsync(new MainPage());
                }
            }

            if (Application.Current.Properties.ContainsKey("Usuario_Estacion"))
            {
                List<Estacion> estacion = JsonConvert.DeserializeObject<List<Estacion>>(Application.Current.Properties["Usuario_Estacion"].ToString());
                if (estacion[0].id_Cliente != 0)
                {
                    await ((NavigationPage)this.Parent).PushAsync(new MainPage());
                }
            }

            estado = await API.Login.Comprobar_Estado();
            //await DisplayAlert("Respuesta de la api:", "La respuesta es: "+estado, "Reintentar.");
            if (estado == true)
            {
                Registro.IsVisible = true;
            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            int numEstacion = 0;
            List<Estacion> estacion = null;
            Clientes cliente = null;
            string email = "";
            string json = "";

            if (string.IsNullOrEmpty(Usuario.Text) || string.IsNullOrEmpty(Password.Text))
            {
                await DisplayAlert("Un momento...", "Llenar todos los campos.", "Ok");
            }

            else
            {
                string password = Password.Text.ToString().ToLower();
                if (Regex.IsMatch(Usuario.Text.ToString(), @"^\d+$"))
                {
                    numEstacion = Int32.Parse(Usuario.Text.ToString());
                    json = await API.Login.IniciarSesion(numEstacion, password);
                    estacion = JsonConvert.DeserializeObject<List<Estacion>>(json);
                }

                else
                {
                    email = Usuario.Text.ToString().ToLower();
                    json = await API.Login.IniciarSesion(email, password);
                    cliente = JsonConvert.DeserializeObject<Clientes>(json);
                }
                
                
                //List<Pedido> listado = await API.Login.Pedidos();
                //Cliente cliente = await API.Login.IniciarSesion(email, password);
                

                if (cliente != null)
                {
                    //Información del usuario logeado
                    Application.Current.Properties["Usuario"] = json;
                    //Información de las estaciones del usuario logeado
                    //string estaciones = await Estaciones_Controller.GetEstacionesPorId(cliente.id_Clientes.ToString());
                    string pedidos = await Pedidos_Controller.GetPedidosOnly(cliente.id_Clientes);
                    Application.Current.Properties["Pedidos"] = pedidos;
                    //Application.Current.Properties["Estaciones"] = estaciones;
                    await Application.Current.SavePropertiesAsync();
                    await ((NavigationPage)this.Parent).PushAsync(new MainPage()); //MainPage
                    //await Navigation.PopAsync();
                }

                else if (estacion != null)
                {
                    //Información del usuario logeado
                    Application.Current.Properties["Usuario_Estacion"] = json;
                    string pedidos = await Pedidos_Controller.GetPedidosEstacion(estacion[0].id_Cliente, estacion[0].Numero_Estacion);
                    Application.Current.Properties["Pedidos"] = pedidos;
                    //Application.Current.Properties["Estaciones"] = estaciones;
                    await Application.Current.SavePropertiesAsync();
                    await ((NavigationPage)this.Parent).PushAsync(new MainPage()); //MainPage
                }

                else
                    await DisplayAlert("Algún dato erroneo.", "Cliente no encontrado.", "Reintentar.");
            }
        }

        private async void Contrasena_Clicked(object sender, EventArgs e)
        {
            await((NavigationPage)this.Parent).PushAsync(new Olvide_Contrasena());
        }

        private async void Registro_Clicked(object sender, EventArgs e)
        {
            await((NavigationPage)this.Parent).PushAsync(new Registro_Cliente());
        }
    }
}