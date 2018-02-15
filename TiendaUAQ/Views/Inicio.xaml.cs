using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using TiendaUAQ.Models;
using TiendaUAQ.Services;
using Xamarin.Forms;

namespace TiendaUAQ.Views
{
    public partial class Inicio : ContentPage
    {
        public Inicio()
        {
            InitializeComponent();
        }

        async void Ingresar_Clicked(object sender, System.EventArgs e)
        {
            if (string.IsNullOrEmpty(txtUsuario.Text))
            {
                await DisplayAlert("Información", "Ingrese su usuario", "Aceptar");
                txtUsuario.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtPassword.Text))
            {
                await DisplayAlert("Información", "Ingrese su contraseña", "Aceptar");
                txtPassword.Focus();
                return;
            }
            waitActivityIndicador.IsRunning = true;//Pone el de cargando
            btnIngresar.IsEnabled = false;//Deshabilita el boton
            Device.BeginInvokeOnMainThread(async () =>
            {
                HttpClient cliente = new HttpClient();
                var formContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("usuario",txtUsuario.Text),
                    new KeyValuePair<string, string>("password",txtPassword.Text)
                });
                var myHttpClient = new HttpClient();
                var authData = string.Format("{0}:{1}", "tiendaUAQ", "t13nd4U4q");
                var authHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(authData));
                myHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeaderValue);
                var response = await myHttpClient.PostAsync("http://189.211.201.181:88/TiendaUAQWebservice/api/tblusuarios/login", formContent);
                var json = await response.Content.ReadAsStringAsync();
                RestClient c = new RestClient();
                var usuarioX = await c.convertirJson<Usuarios>(json);
                btnIngresar.IsEnabled = true;//Habilita el boton
                waitActivityIndicador.IsRunning = false;//Quita el de cargando
                if (response.IsSuccessStatusCode)
                {
                    if (usuarioX != null)
                    {
                        int idUsuario = usuarioX.idUsuario;
                        string nombre = usuarioX.nombre;
                        string paterno = usuarioX.paterno;
                        string materno = usuarioX.materno;
                        //Guarda las variables en la app, persistencia de los datos
                        Application.Current.Properties["idUsuarioTienda"] = idUsuario;
                        Application.Current.Properties["nombre"] = nombre;
                        Application.Current.Properties["paterno"] = paterno;
                        Application.Current.Properties["materno"] = materno;
                        Usuarios usuario = new Usuarios { idUsuario = idUsuario, nombre = nombre, paterno = paterno, materno = materno };
                        await DisplayAlert("Correcto", "Inició sesión correctamente.", "Aceptar");
                        Application.Current.MainPage = new MenuPrincipal();//Reemplaza la pagina
                    } else{
                        await DisplayAlert("Información", "Usuario o contraseña no validos", "Aceptar");
                        txtPassword.Text = string.Empty;
                        txtPassword.Focus();
                        return;    
                    }
                }
                else
                {
                    await DisplayAlert("Información", "Usuario o contraseña no validos", "Aceptar");
                    txtPassword.Text = string.Empty;
                    txtPassword.Focus();
                    return;
                }
                txtPassword.Text = string.Empty;
                txtPassword.Focus();
            });
        }

        async void Registrarse_Clicked(object sender, System.EventArgs e)
        {
            //Application.Current.MainPage = new NavigationPage(new Registrarse());//Reemplaza la pagina  
            await Navigation.PushAsync(new Registrarse());
        }
    }
}
