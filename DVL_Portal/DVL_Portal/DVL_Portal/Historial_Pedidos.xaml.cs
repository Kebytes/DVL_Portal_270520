using DVL_Portal.API;
using DVL_Portal.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace DVL_Portal
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Historial_Pedidos : ContentPage
	{
		public Historial_Pedidos ()
		{
			InitializeComponent ();
            this.BindingContext = this;
            detallesPedido.ItemSelected += DetallesPedido_ItemSelected;
        }

        private async void DetallesPedido_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null) return;

            if (sender is ListView lv) lv.SelectedItem = null;

            Pedido x = (Pedido)detallesPedido.SelectedItem;
            if (Application.Current.Properties.ContainsKey("Usuario"))
            {
                Clientes cli = JsonConvert.DeserializeObject<Clientes>(Application.Current.Properties["Usuario"].ToString());
                x.cliente = cli;
            }

            string json = JsonConvert.SerializeObject(x);
            await ((NavigationPage)this.Parent).PushAsync(new Detalle_Pedido(json));
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            int x = Navigation.NavigationStack.IndexOf(this) - 1;
            if (x >= 0)
            {
                var previousPage = Navigation.NavigationStack[Navigation.NavigationStack.IndexOf(this) - 1];
                Navigation.RemovePage(previousPage);
            }
            //var previousPage = Navigation.NavigationStack[Navigation.NavigationStack.IndexOf(this) - 1];
            //Navigation.RemovePage(previousPage);

            if (Application.Current.Properties.ContainsKey("Usuario"))
            {
                Clientes cliente = JsonConvert.DeserializeObject<Clientes>(Application.Current.Properties["Usuario"].ToString());
                string pedidos = await Pedidos_Controller.GetPedidosOnly(cliente.id_Clientes);
                Application.Current.Properties["Pedidos"] = pedidos;
                await Application.Current.SavePropertiesAsync();
            }

            if (Application.Current.Properties.ContainsKey("Usuario_Estacion"))
            {
                List<Estacion> estacion = JsonConvert.DeserializeObject<List<Estacion>>(Application.Current.Properties["Usuario_Estacion"].ToString());
                string pedidos = await Pedidos_Controller.GetPedidosClienteEstacion(estacion[0].id_Estaciones);
                Application.Current.Properties["Pedidos"] = pedidos;
                await Application.Current.SavePropertiesAsync();
            }

            ListaElementos elementos = new ListaElementos();
            detallesPedido.ItemsSource = elementos.elementos;
        }

        public class ListaElementos
        {
            public List<Models.Pedido> elementos { get; set; }

            public ListaElementos()
            {
                elementos = new List<Models.Pedido>();
                loadElementos();
            }

            public void loadElementos()
            {
                if (Application.Current.Properties.ContainsKey("Pedidos"))
                {
                    elementos = JsonConvert.DeserializeObject<List<Models.Pedido>>(Application.Current.Properties["Pedidos"].ToString());

                    for (int i = 0; i < elementos.Count; i++)
                    {
                        elementos[i].OracionFecha = elementos[i].Fecha_Programada.ToString("dd/MMMM/yyyy", CultureInfo.CreateSpecificCulture("es-ES")).Replace("/", " ");
                        elementos[i].OracionRegistro = elementos[i].Fecha_Registro.ToString("dd/MMMM/yyyy", CultureInfo.CreateSpecificCulture("es-ES")).Replace("/", " ");

                        elementos[i].OracionMagna = elementos[i].Litros_Magna.ToString("#,##0.###") + " L";
                        elementos[i].totalLitros += elementos[i].Litros_Magna;

                        elementos[i].OracionPremium = elementos[i].Litros_Premium.ToString("#,##0.###") + " L";
                        elementos[i].totalLitros += elementos[i].Litros_Premium;

                        elementos[i].OracionDiesel = elementos[i].Litros_Diesel.ToString("#,##0.###") + " L";
                        elementos[i].totalLitros += elementos[i].Litros_Diesel;

                        elementos[i].TotalLitros = elementos[i].totalLitros.ToString("#,##0.###") + " L";

                        if (elementos[i].Estatus.Equals("A"))
                        {
                            elementos[i].OracionEstatus = "Agendado";
                            elementos[i].OracionImagen = "confirmado.png";
                        }

                        if (elementos[i].Estatus.Equals("C"))
                        {
                            elementos[i].OracionEstatus = "Cancelado";
                            elementos[i].OracionImagen = "cancel.png";
                        }

                        if (elementos[i].Estatus.Equals("P"))
                        {
                            elementos[i].OracionEstatus = "Pendiente";
                            elementos[i].OracionImagen = "question.png";
                        }

                        if (elementos[i].Estatus.Equals("F"))
                        {
                            elementos[i].OracionEstatus = "Facturado";
                            elementos[i].OracionImagen = "facturado.png";
                        }

                        if (elementos[i].Estatus.Equals("E"))
                        {
                            elementos[i].OracionEstatus = "Entregado";
                            elementos[i].OracionImagen = "entregado.png";
                        }
                    }
                }
            }
        }

        private void BtnNuevoPedido_Clicked(object sender, EventArgs e)
        {
            ((NavigationPage)this.Parent).PushAsync(new Pedidos());
        }

        private async void BtnRefrescar_Clicked(object sender, EventArgs e)
        {
            if (Application.Current.Properties.ContainsKey("Usuario"))
            {
                Clientes cliente = JsonConvert.DeserializeObject<Clientes>(Application.Current.Properties["Usuario"].ToString());
                string pedidos = await Pedidos_Controller.GetPedidosOnly(cliente.id_Clientes);
                Application.Current.Properties["Pedidos"] = pedidos;
                await Application.Current.SavePropertiesAsync();
            }

            if (Application.Current.Properties.ContainsKey("Usuario_Estacion"))
            {
                List<Estacion> estacion = JsonConvert.DeserializeObject<List<Estacion>>(Application.Current.Properties["Usuario_Estacion"].ToString());
                string pedidos = await Pedidos_Controller.GetPedidosClienteEstacion(estacion[0].id_Estaciones);
                Application.Current.Properties["Pedidos"] = pedidos;
                await Application.Current.SavePropertiesAsync();
            }


            ListaElementos elementos = new ListaElementos();
            detallesPedido.ItemsSource = elementos.elementos;

            await DisplayAlert("Pedidos.", "Pedidos actualizados.", "Ok.");
        }
    }
}