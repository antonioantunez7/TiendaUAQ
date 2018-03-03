using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using TiendaUAQ.Models;
using TiendaUAQ.Services;
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
            } else{
                var patron = "^([\\w\\.\\-]+)@([\\w\\-]+)((\\.(\\w){2,3})+)$";
                if(!Regex.IsMatch(txtCorreo.Text, patron)){
                    await DisplayAlert("Información", "Correo electrónico inválido", "Aceptar");
                    txtCorreo.Focus();
                    return;    
                }
            }
            if (string.IsNullOrEmpty(txtPassword.Text))
            {
                await DisplayAlert("Información", "Ingrese su contraseña", "Aceptar");
                txtPassword.Focus();
                return;
            }
            else
            {
                if (txtPassword.Text != txtConfirmaPassword.Text)
                {
                    await DisplayAlert("Información", "Las contraseñas no coinciden", "Aceptar");
                    txtConfirmaPassword.Focus();
                    return;
                }
            }
            if (string.IsNullOrEmpty(txtConfirmaPassword.Text))
            {
                await DisplayAlert("Información", "Ingrese la confirmación de su contraseña", "Aceptar");
                txtConfirmaPassword.Focus();
                return;
            }
            waitActivityIndicador.IsRunning = true;//Pone el de cargando
            btnGuardar.IsEnabled = false;//Deshabilita el boton
            //valida si el usuario esta disponible
            FormUrlEncodedContent formContent1 = null;
            formContent1 = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("usuario",txtCorreo.Text)
            });

            var myHttpClientValida = new HttpClient();
            var authData1 = string.Format("{0}:{1}", "tiendaUAQ", "t13nd4U4q");
            var authHeaderValue1 = Convert.ToBase64String(Encoding.UTF8.GetBytes(authData1));
            myHttpClientValida.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeaderValue1);
            var responseValida = await myHttpClientValida.PostAsync("http://148.240.202.160:88/TiendaUAQWebservice/api/tblusuarios/valida/", formContent1);
            var json1 = await responseValida.Content.ReadAsStringAsync();
            RestClient c1 = new RestClient();
            var usuarioV = await c1.convertirJson<Usuarios>(json1);
            if (responseValida.IsSuccessStatusCode)
            {
                if(usuarioV == null){
                    HttpClient cliente = new HttpClient();
                    FormUrlEncodedContent formContent = null;
                    formContent = new FormUrlEncodedContent(new[]
                    {
                            new KeyValuePair<string, string>("nombre", txtNombre.Text),
                            new KeyValuePair<string, string>("paterno",txtPaterno.Text),
                            new KeyValuePair<string, string>("materno",txtMaterno.Text),
                            new KeyValuePair<string, string>("usuario",txtCorreo.Text),
                            new KeyValuePair<string, string>("password",txtPassword.Text),
                            new KeyValuePair<string, string>("cveTipoUsuario","1")//1:Cliente
                        });

                    var myHttpClient = new HttpClient();
                    var authData = string.Format("{0}:{1}", "tiendaUAQ", "t13nd4U4q");
                    var authHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(authData));
                    myHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeaderValue);
                    var response = await myHttpClient.PostAsync("http://148.240.202.160:88/TiendaUAQWebservice/api/tblusuarios/guardar/", formContent);
                    var json = await response.Content.ReadAsStringAsync();
                    RestClient c = new RestClient();
                    var usuarioX = await c.convertirJson<Usuarios>(json);
                    if (response.IsSuccessStatusCode)
                    {
                        var usuario = usuarioX.idUsuario;
                        waitActivityIndicador.IsRunning = true;//Pone el de cargando
                        Application.Current.Properties["idUsuarioTienda"] = usuario;
                        Application.Current.Properties["nombre"] = usuarioX.nombre;
                        Application.Current.Properties["paterno"] = usuarioX.paterno;
                        Application.Current.Properties["materno"] = usuarioX.materno;
                        Application.Current.Properties["usuario"] = usuarioX.usuario;
                        var nombreUsuario = txtNombre.Text + " " + txtPaterno.Text + " " + txtMaterno.Text;
                        string mensajeEnvioCorreo = enviarCorreo(txtCorreo.Text, nombreUsuario, txtCorreo.Text, txtPassword.Text);
                        await DisplayAlert("Correcto", "Se registró correctamente. " + mensajeEnvioCorreo, "Aceptar");
                        Application.Current.MainPage = new MenuPrincipal();//Reemplaza la pagina 
                    }
                    else
                    {
                        btnGuardar.IsEnabled = true;//Habilita el boton
                        waitActivityIndicador.IsRunning = false;//Pone el de cargando
                        await DisplayAlert("Error", "No se pudo registrar sus datos en la aplicación. Intente nuevamente.", "Aceptar");
                    }
                } else { 
                    await DisplayAlert("Información", "El usuario ya existe. Intente con otro.", "Aceptar");
                    txtCorreo.Focus();
                    btnGuardar.IsEnabled = true;//Habilita el boton
                    waitActivityIndicador.IsRunning = false;//quita el de cargando
                }
            } else{
                await DisplayAlert("Información", "Error en la petición.", "Aceptar");
                txtCorreo.Focus();
                btnGuardar.IsEnabled = true;//Habilita el boton
                waitActivityIndicador.IsRunning = false;//quita el de cargando
            }
        }

        async void Cancelar_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PopAsync();
        }

        string enviarCorreo(string correoDestino, string nombreUsuario, string usuario, string password){
            string body = "<html><body style='font-family: Arial, Helvetica, sans-serif;'><h2 style='color:#EC7063;'>Tienda UAQ</h2><p>Se registr&oacute; con &eacute;xito en la aplicaci&oacute;n Tienda UAQ, sus datos para iniciar sesi&oacute;n son: </p><table border='1' bordercolor='gray' style='border-collapse: collapse;' cellpadding='5'><tr><td><b style='color:#EC7063;'>Usuario: </b></td><td>" + usuario + "</td></tr><tr><td><b style='color:#EC7063;'>Contrase&ntilde;a: </b></td><td>" + password + "</td></tr></table><br><p style='color:gray;font-size:11px;'>*Este es un correo autom&aacute;tico, no es necesario responder.</p></body></html>";
            string mensaje = "";
            try
            {
                new SmtpClient
                {
                    Host = "Smtp.Gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    Timeout = 10000,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential("tiendauaq@gmail.com", "t13nd4u4q")
                }.Send(new MailMessage { From = new MailAddress(correoDestino, nombreUsuario), To = { correoDestino }, Subject = "Registro en la Tienda UAQ", Body = body,IsBodyHtml = true, BodyEncoding = Encoding.UTF8 });
                mensaje = "Se envió un mensaje a su correo electrónico proporcionado.";
            }
            catch (Exception ex)
            {
                mensaje = "Falló al enviar el mensaje a su correo electrónico. "+ex.Message;
            }
            return mensaje;
        }
    }
}
