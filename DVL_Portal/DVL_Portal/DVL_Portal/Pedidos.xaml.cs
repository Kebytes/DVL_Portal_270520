using DVL_Portal.API;
using DVL_Portal.Models;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace DVL_Portal
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Pedidos : ContentPage
	{
		public Pedidos ()
		{
			InitializeComponent ();

            if (Application.Current.Properties.ContainsKey("Usuario"))
            {
                Clientes cli = JsonConvert.DeserializeObject<Clientes>(Application.Current.Properties["Usuario"].ToString());
                bool cambiar = CambiarFecha(cli.Horas_Pedido);
                if (cambiar)
                    FechaDeseada.MinimumDate = DateTime.Now.AddDays(1);
                else
                    FechaDeseada.MinimumDate = DateTime.Now;
            }

            if (Application.Current.Properties.ContainsKey("Usuario_Estacion"))
            {
                FechaDeseada.MinimumDate = DateTime.Now;
            }
        }

        protected override bool OnBackButtonPressed()
        {
            Navigation.PopAsync();
            return base.OnBackButtonPressed();
        }

        protected async override void OnAppearing()
        {
            int x = Navigation.NavigationStack.IndexOf(this) - 1;
            if (x >= 0)
            {
                var previousPage = Navigation.NavigationStack[Navigation.NavigationStack.IndexOf(this) - 1];
                Navigation.RemovePage(previousPage);
            }

            if (Application.Current.Properties.ContainsKey("Usuario"))
            {
                Clientes cli = JsonConvert.DeserializeObject<Clientes>(Application.Current.Properties["Usuario"].ToString());
                string estac = await Estaciones_Controller.GetEstacionesPorId(cli.id_Clientes.ToString());
                Application.Current.Properties["Estaciones"] = estac;
                await Application.Current.SavePropertiesAsync();
            }

            //if (Application.Current.Properties.ContainsKey("Usuario_Estacion"))
            //{
            //    Estacion estacion = JsonConvert.DeserializeObject<Estacion>(Application.Current.Properties["Usuario_Estacion"].ToString());
            //    string estac = JsonConvert.SerializeObject(estacion);
            //    Application.Current.Properties["Estaciones"] = estac;
            //    await Application.Current.SavePropertiesAsync();
            //}

            base.OnAppearing();

            if (Application.Current.Properties.ContainsKey("Usuario_Estacion"))
            {
                ListaElementos estacion = new ListaElementos();
                Estacion.ItemsSource = estacion.elementos;
            }
            else
            {
                if (Application.Current.Properties.ContainsKey("Usuario"))
                {
                    ListaElementos estaciones = new ListaElementos();
                    Estacion.ItemsSource = estaciones.elementos;
                }
            }

            Autotanque Opciones = new Autotanque();
            AutotanqueOpcion.ItemsSource = Opciones.Opciones;

            Magna.Text = IncrementoMagna.Value.ToString();
            Premium.Text = IncrementoPremium.Value.ToString();
            Diesel.Text = IncrementoDiesel.Value.ToString();
            AutotanqueOpcion.SelectedIndex = 0;
            Estacion.SelectedIndex = 0;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            if (validarDatos())
            {
                Pedido pedido = new Pedido
                {
                    Fecha_Programada = FechaDeseada.Date,
                    id_Estacion = ListaElementos.getIdEstacion(Estacion.Items[Estacion.SelectedIndex]),
                    Estatus = "P",
                    Autotanque = Autotanque.getValor(AutotanqueOpcion.Items[AutotanqueOpcion.SelectedIndex]),
                    Litros_Magna = Int32.Parse(Magna.Text.ToString()),
                    Litros_Premium = Int32.Parse(Premium.Text.ToString()),
                    Litros_Diesel = Int32.Parse(Diesel.Text.ToString()),
                    Fecha_Entregada = DateTime.MinValue
                };
                if (Application.Current.Properties.ContainsKey("Usuario"))
                {
                    pedido.cliente = JsonConvert.DeserializeObject<Clientes>(Application.Current.Properties["Usuario"].ToString());
                }

                if (Application.Current.Properties.ContainsKey("Usuario_Estacion"))
                {
                    List<Estacion> estacion = JsonConvert.DeserializeObject<List<Estacion>>(Application.Current.Properties["Usuario_Estacion"].ToString());
                    pedido.cliente = new Clientes();
                    pedido.cliente.Nombre_Contacto = estacion[0].Nombre_ContactoEstacion;
                }

                var display = await DisplayAlert("Pedido.", "¿Confirmar pedido?", "Sí", "No");

                if (display)
                {
                    Pedido temporal = await Pedidos_Controller.InsertarPedido(pedido);

                    if (temporal != null)
                    {
                        await ((NavigationPage)this.Parent).PushAsync(new Historial_Pedidos());
                        await DisplayAlert("Pedido.", "Pedido realizado.", "Ok");

                        //if (Application.Current.Properties.ContainsKey("Usuario"))
                        //{
                        //    Application.Current.Properties["Pedidos"] = await Pedidos_Controller.GetPedidosOnly(temporal.cliente.id_Clientes); ;
                        //    await Application.Current.SavePropertiesAsync();
                        //    await ((NavigationPage)this.Parent).PushAsync(new Historial_Pedidos());
                        //    await DisplayAlert("Pedido.", "Pedido realizado.", "Ok");
                        //}

                        //if (Application.Current.Properties.ContainsKey("Usuario_Estacion"))
                        //{
                        //    Application.Current.Properties["Pedidos"] = await Pedidos_Controller.GetPedidosClienteEstacion(temporal.id_Estacion);
                        //    await Application.Current.SavePropertiesAsync();
                        //    await ((NavigationPage)this.Parent).PushAsync(new Historial_Pedidos());
                        //    await DisplayAlert("Pedido.", "Pedido realizado.", "Ok");
                        //}
                    }

                    else
                        await DisplayAlert("Pedido.", "Pedido no realizado", "Aceptar");
                }
                else
                    await DisplayAlert("Pedido.", "Pedido cancelado", "Ok");
            }

            else
            {
                await DisplayAlert("Pedido.", "Es necesario llenar todos los campos requeridos.", "Ok");
            }
        }

        private void IncrementoDiesel_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            Diesel.Text = IncrementoDiesel.Value.ToString();
        }

        private void Diesel_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!Diesel.Text.Equals(""))
                IncrementoDiesel.Value = Double.Parse(Diesel.Text);
        }

        private void IncrementoPremium_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            Premium.Text = IncrementoPremium.Value.ToString();
        }

        private void Premium_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!Premium.Text.Equals(""))
                IncrementoPremium.Value = Double.Parse(Premium.Text);
        }

        private void IncrementoMagna_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            Magna.Text = IncrementoMagna.Value.ToString();
        }

        private void Magna_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!Magna.Text.Equals(""))
                IncrementoMagna.Value = Double.Parse(Magna.Text);
        }

        public bool validarDatos()
        {
            bool bandera;

            if (string.IsNullOrEmpty(FechaDeseada.Date.ToString()) || string.IsNullOrEmpty(Magna.Text) || string.IsNullOrEmpty(Premium.Text)
                || string.IsNullOrEmpty(Diesel.Text) || string.IsNullOrEmpty(AutotanqueOpcion.SelectedItem.ToString()) || string.IsNullOrEmpty(Estacion.SelectedItem.ToString()))
            {
                bandera = false;
            }
            else
                bandera = true;

            return bandera;
        }

        public class ListaElementos
        {
            public List<Models.Estacion> elementos { get; set; }
            //public Estacion elemento { get; set; }

            public ListaElementos()
            {
                elementos = new List<Models.Estacion>();
                //elemento = new Estacion();
                loadElementos();
            }

            public void loadElementos()
            {
                if (Application.Current.Properties.ContainsKey("Usuario_Estacion"))
                {
                    elementos = JsonConvert.DeserializeObject<List<Estacion>>(Application.Current.Properties["Usuario_Estacion"].ToString());
                }

                else if (Application.Current.Properties.ContainsKey("Estaciones"))
                {
                    elementos = JsonConvert.DeserializeObject<List<Estacion>>(Application.Current.Properties["Estaciones"].ToString());
                }
            }

            public static int getIdEstacion(string nombre)
            {
                int id = 0;
                if (Application.Current.Properties.ContainsKey("Usuario_Estacion"))
                {
                    ListaElementos elemento = new ListaElementos();
                    List<Estacion> temp = elemento.elementos;
                    id = temp[0].id_Estaciones;
                }
                else
                {
                    ListaElementos elemento = new ListaElementos();
                    List<Models.Estacion> estaciones = elemento.elementos;
                    for (int i = 0; i < estaciones.Count; i++)
                    {
                        Models.Estacion xp = estaciones[i];
                        if (xp.Nombre_Estacion.Equals(nombre))
                        {
                            id = xp.id_Estaciones;
                            break;
                        }
                    }
                }

                return id;
            }
        }

        public class Autotanque
        {
            public List<Opcion> Opciones { get; set; }

            public Autotanque()
            {
                Opciones = new List<Opcion>();
                getNombres();
            }

            public void getNombres()
            {
                Opciones.Add(new Opcion { Nombre = "Propio" });
                Opciones.Add(new Opcion { Nombre = "Foráneo" });
                Opciones.Add(new Opcion { Nombre = "Pemex" });
            }

            public static string getValor(string valor)
            {
                string nombre = "";
                Autotanque elemento = new Autotanque();
                List<Opcion> estaciones = elemento.Opciones;
                for (int i = 0; i < estaciones.Count; i++)
                {
                    Opcion xp = estaciones[i];
                    if (xp.Nombre.Equals(valor))
                    {
                        nombre = xp.Nombre;
                        break;
                    }
                }
                return nombre;
            }

            public class Opcion
            {
                public string Nombre { get; set; }
            }
        }

        public bool CambiarFecha(int horas)
        {
            bool cambiar = false;
            //int val = 24 - horas;
            TimeSpan hora = DateTime.Now.TimeOfDay;
            TimeSpan result = TimeSpan.FromHours(horas);

            if (TimeSpan.Compare(hora, result) == -1)
            { //Hora del cliente mayor
                cambiar = false;
            }

            if (TimeSpan.Compare(hora, result) == 1 || TimeSpan.Compare(hora, result) == 0)
            { //Hora del cliente menor
                cambiar = true;
            }

            return cambiar;
        }
    }
}