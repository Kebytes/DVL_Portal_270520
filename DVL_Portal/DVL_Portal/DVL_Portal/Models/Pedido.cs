using System;
using System.Collections.Generic;
using System.Text;

namespace DVL_Portal.Models
{
    public class Pedido
    {
        public Pedido()
        {

        }

        public string OracionImagen { get; set; }
        public string OracionEstatus { get; set; }

        public int id_Pedido { get; set; }
        public string Folio_Pedido { get; set; }
        public DateTime Fecha_Registro { get; set; }
        public DateTime Fecha_Programada { get; set; }
        public string OracionFecha { get; set; }
        public string OracionRegistro { get; set; }

        public string Estatus { get; set; }
        public string Autotanque { get; set; }
        public int Litros_Magna { get; set; }
        public int Litros_Premium { get; set; }
        public int Litros_Diesel { get; set; }
        public DateTime Fecha_Entregada { get; set; }
        public int id_Estacion { get; set; }
        public string Nombre_Estacion { get; set; }
        //public List<Detalle> Detalles { get; set; }
        public Clientes cliente
        { get; set; }
        public string OracionMagna { get; set; }
        public string OracionPremium { get; set; }
        public string OracionDiesel { get; set; }
        public int totalLitros { get; set; }
        public string TotalLitros { get; set; }
        public String Color { get; set; }
    }


    public class Clientes
    {
        public int id_Clientes { get; set; }
        public string Razon_Social { get; set; }
        public string Telefono { get; set; }
        public string Correo_Cliente { get; set; }
        public string Contrasena { get; set; }
        public string RFC { get; set; }
        public string Direccion_Factura { get; set; }
        public string Correo_Factura { get; set; }
        public string Nombre_Contacto { get; set; }
        public string Reparto { get; set; }
        public string Telefono_Contacto { get; set; }
        public int Horas_Pedido { get; set; }
        public Estacion Estacion { get; set; }
        public string Oracion { get; set; }
        public string Token { get; set; }

        public Clientes()
        {
        }


    }


    public class Estacion
    {
        public int id_Estaciones { get; set; }
        public int id_Cliente { get; set; }
        public string Calle { get; set; }
        public string Colonia { get; set; }
        public string Numero { get; set; }
        public string Codigo_Postal { get; set; }
        public int id_Estado { get; set; }
        public int id_Ciudad { get; set; }
        public string Permiso_CRE { get; set; }
        public string Nombre_ContactoEstacion { get; set; }
        public string Telefono_ContactoEstacion { get; set; }
        public DateTime Fecha_Insercion { get; set; }
        public string Nombre_Estacion { get; set; }
        public string Nombre_Estado { get; set; }
        public string Nombre_Ciudad { get; set; }
        public string Destino { get; set; }
        public string Numero_Cuenta { get; set; }
        public string Clabe_Interbancaria { get; set; }
        public string Contrasena { get; set; }
        public int Numero_Estacion { get; set; }


        public string Direccion { get; set; }
        public string DireccionLocal { get; set; }
        public string Oracion { get; set; }

        public Estacion()
        {

        }
    }
}
