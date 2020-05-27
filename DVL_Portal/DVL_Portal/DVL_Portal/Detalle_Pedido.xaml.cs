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
	public partial class Detalle_Pedido : ContentPage
	{
        private Pedido elegido;
        public Detalle_Pedido (string json)
		{
			InitializeComponent ();
            elegido = JsonConvert.DeserializeObject<Pedido>(json);

            if (elegido.Estatus.Equals("A"))
            {
                elegido.OracionEstatus = "Agendado";
                elegido.OracionImagen = "confirmado.png";
            }

            if (elegido.Estatus.Equals("C"))
            {
                elegido.OracionEstatus = "Cancelado";
                elegido.OracionImagen = "cancel.png";
            }

            elegido.TotalLitros += " Totales";
            elegido.OracionMagna += " Magna";
            elegido.OracionPremium += " Premium";
            elegido.OracionDiesel += " Diesel";

            ImagenEstado.SetBinding(Image.SourceProperty, "OracionImagen");
            ImagenEstado.BindingContext = elegido;

            Confirmado.SetBinding(Label.TextProperty, "OracionEstatus");
            Confirmado.BindingContext = elegido;

            Folio.SetBinding(Label.TextProperty, "Folio_Pedido");
            Folio.BindingContext = elegido;

            ValorEstacion.SetBinding(Label.TextProperty, "Nombre_Estacion");
            ValorEstacion.BindingContext = elegido;

            FechaAgendada.SetBinding(Label.TextProperty, "OracionFecha");
            FechaAgendada.BindingContext = elegido;

            Total.SetBinding(Label.TextProperty, "TotalLitros");
            Total.BindingContext = elegido;

            Diesel.SetBinding(Label.TextProperty, "OracionDiesel");
            Diesel.BindingContext = elegido;

            Premium.SetBinding(Label.TextProperty, "OracionPremium");
            Premium.BindingContext = elegido;

            Magna.SetBinding(Label.TextProperty, "OracionMagna");
            Magna.BindingContext = elegido;


            ValorAutanque.SetBinding(Label.TextProperty, "Autotanque");
            ValorAutanque.BindingContext = elegido;

            FechaRegistro.SetBinding(Label.TextProperty, "OracionRegistro");
            FechaRegistro.BindingContext = elegido;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            if (elegido.Estatus != "C" && elegido.Estatus == "P")
            {
                var alert = await DisplayAlert("Cancelar pedido.", "¿Cancelar el pedido?", "Sí", "No");
                if (alert)
                {
                    if (Application.Current.Properties.ContainsKey("Usuario_Estacion"))
                    {
                        List<Estacion> estacion = JsonConvert.DeserializeObject<List<Estacion>>(Application.Current.Properties["Usuario_Estacion"].ToString());
                        elegido.cliente = new Clientes();
                        elegido.cliente.Nombre_Contacto = estacion[0].Nombre_ContactoEstacion;
                    }

                    bool cancelado = await Pedidos_Controller.Cancelar_Pedido(elegido);
                    if (cancelado)
                    {
                        await DisplayAlert("Pedido.", "Pedido cancelado.", "Ok");
                        await ((NavigationPage)this.Parent).PushAsync(new Historial_Pedidos());
                    }
                    else
                    {
                        await DisplayAlert("Error.", "Pedido no cancelado.", "Ok");
                    }
                }
            }

            else
            {
                await DisplayAlert("Pedido.", "El pedido no puede ser cancelado.", "Ok");
            }
        }
    }
}