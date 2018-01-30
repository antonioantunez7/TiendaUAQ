using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace TiendaUAQ.Views
{
    public partial class Registrarse : ContentPage
    {
        public Registrarse()
        {
            InitializeComponent();
        }

        async void GuardarRegistro_Clicked(object sender, System.EventArgs e)
        {
            if (string.IsNullOrEmpty(txtNombre.Text))
            {
                await DisplayAlert("Información", "Ingrese su nombre", "Aceptar");
                txtNombre.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtPaterno.Text))
            {
                await DisplayAlert("Información", "Ingrese su apellido paterno", "Aceptar");
                txtPaterno.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtMaterno.Text))
            {
                await DisplayAlert("Información", "Ingrese su apellido materno", "Aceptar");
                txtMaterno.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtCorreo.Text))
            {
                await DisplayAlert("Información", "Ingrese su correo electrónico", "Aceptar");
                txtCorreo.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtPassword.Text))
            {
                await DisplayAlert("Información", "Ingrese su contraseña", "Aceptar");
                txtPassword.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtConfirmaPassword.Text))
            {
                await DisplayAlert("Información", "Ingrese la confirmación de su contraseña", "Aceptar");
                txtConfirmaPassword.Focus();
                return;
            }
            waitActivityIndicador.IsRunning = true;//Pone el de cargando
            btnGuardar.IsEnabled = false;//Deshabilita el boton
            //Application.Current.MainPage = new NavigationPage(new MenuPrincipal());//Reemplaza la pagina        
        }

        void Cancelar_Clicked(object sender, System.EventArgs e)
        {
            Application.Current.MainPage = new NavigationPage(new Inicio());
        }
    }
}
