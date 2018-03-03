using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using TiendaUAQ.Models;
using TiendaUAQ.Services;
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

        async void recuperarContrasena(object sender, System.EventArgs e)
        {
            if (Application.Current.Properties.ContainsKey("idUsuarioTienda"))
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    RestClient cliente = new RestClient();
                    var usuarios = await cliente.GetUsuarios<Usuarios>("http://148.240.202.160:88/TiendaUAQWebservice/api/tblusuarios/idUsuario/" + Application.Current.Properties["idUsuarioTienda"].ToString());
                    if (usuarios != null)
                    {
                        if (usuarios.idUsuario != 0)
                        {
                            string usuario = usuarios.usuario;
                            string password = usuarios.password;
                            string correoDestino = Application.Current.Properties["usuario"].ToString();
                            string nombreUsuario = Application.Current.Properties["nombre"] + " " + Application.Current.Properties["paterno"] + " " + Application.Current.Properties["materno"];
                            string body = "<html><body style='font-family: Arial, Helvetica, sans-serif;'><h2 style='color:#EC7063;'>Tienda UAQ</h2><p>Se recuperó la contraseña con &eacute;xito en la aplicaci&oacute;n Tienda UAQ, sus datos para iniciar sesi&oacute;n son: </p><table border='1' bordercolor='gray' style='border-collapse: collapse;' cellpadding='5'><tr><td><b style='color:#EC7063;'>Usuario: </b></td><td>" + usuario + "</td></tr><tr><td><b style='color:#EC7063;'>Contrase&ntilde;a: </b></td><td>" + password + "</td></tr></table><br><p style='color:gray;font-size:11px;'>*Este es un correo autom&aacute;tico, no es necesario responder.</p></body></html>";
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
                                }.Send(new MailMessage { From = new MailAddress(correoDestino, nombreUsuario), To = { correoDestino }, Subject = "Recuperación de contraseña de la app de la Tienda UAQ", Body = body, IsBodyHtml = true, BodyEncoding = Encoding.UTF8 });
                                mensaje = "Se envió su usuario y contraseña a su correo electrónico: "+correoDestino+" .";
                            }
                            catch (Exception ex)
                            {
                                mensaje = "Falló al enviar el mensaje a su correo electrónico. " + ex.Message;
                            }
                            await DisplayAlert("Información", mensaje, "Aceptar");
                        } else{
                            await DisplayAlert("Información", "No encontró su registro en el sistema.", "Aceptar");
                        }
                    } else{
                        await DisplayAlert("Información", "No encontró su registro en el sistema.", "Aceptar");    
                    }
                });
            } else{
                await DisplayAlert("Información", "Acción inválida. No se encuentra su sesión iniciada.", "Aceptar");
            }
        }
    }
}
