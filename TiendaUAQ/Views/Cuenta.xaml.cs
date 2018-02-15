using System;
using System.Collections.Generic;
using TiendaUAQ.Models;
using Xamarin.Forms;

namespace TiendaUAQ.Views
{
    public partial class Cuenta : ContentPage
    {
        public Cuenta()
        {
            InitializeComponent();
            cargaDetalleCuenta();
        }

        void cargaDetalleCuenta()
        {
            string nombre = "";
            string paterno = "";
            string materno = "";
            if (Application.Current.Properties.ContainsKey("idUsuarioTienda")
                && Application.Current.Properties.ContainsKey("nombre")
                && Application.Current.Properties.ContainsKey("paterno")
                && Application.Current.Properties.ContainsKey("materno"))
            {
                nombre = Application.Current.Properties["nombre"].ToString();
                paterno = Application.Current.Properties["paterno"].ToString();
                materno = Application.Current.Properties["materno"].ToString();
            }
            List<Usuarios> usuarios = new List<Usuarios>{
                new Usuarios { nombre = nombre+" "+paterno+" "+materno
                }
            };
            DetalleCuenta.ItemsSource = usuarios;
        }
    }
}
