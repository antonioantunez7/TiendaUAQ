using System;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using System.Text;
using TiendaUAQ.Models;
using TiendaUAQ.Services;
using TiendaUAQ.Views;
using Xamarin.Forms;

namespace TiendaUAQ
{
    public partial class App : Application
    {
        public static bool UseMockDataStore = true;
        public static string BackendUrl = "https://localhost:5000";

        public App()
        {
           // Application.Current.Properties["idUsuarioTienda"] = 6;
            InitializeComponent();
            consultaCarrito();
            //string m = enviarCorreo("scorpion_malboro@hotmail.com","tono antun","scorpion_malboro@hotmail.com","12");
            MainPage = new MenuPrincipal();//Se reemplaza por las lineas siguientes porque el menu se duplicaba
            /*if (Application.Current.Properties.ContainsKey("idUsuario")
                && Application.Current.Properties.ContainsKey("nombre")
                && Application.Current.Properties.ContainsKey("paterno")
                && Application.Current.Properties.ContainsKey("materno"))
            {
                var idUsuario = Convert.ToInt32(Application.Current.Properties["idUsuario"]);
                var nombre = (string)Application.Current.Properties["nombre"];
                var paterno = (string)Application.Current.Properties["paterno"];
                var materno = (string)Application.Current.Properties["paterno"];
                Usuarios usuario = new Usuarios { idUsuario = idUsuario, nombre = nombre, paterno = paterno, materno = materno };
                Application.Current.MainPage = new NavigationPage(new MenuPrincipal());//Reemplaza la pagina
            }
            else
            {
                MainPage = new Inicio();
            }*/
            /*if (UseMockDataStore)
                DependencyService.Register<MockDataStore>();
            else
                DependencyService.Register<CloudDataStore>();

            if (Device.RuntimePlatform == Device.iOS)
                MainPage = new MainPage();
            else
                MainPage = new NavigationPage(new MainPage());*/
        }

        void consultaCarrito(){
            if (Application.Current.Properties.ContainsKey("idUsuarioTienda")){
                Device.BeginInvokeOnMainThread(async () =>
                {
                    RestClient cliente = new RestClient();
                    var pedidos = await cliente.GetPedidos<Pedidos>("http://189.211.201.181:88/TiendaUAQWebservice/api/tbldetallespedidos/pedido/usuario/"+Application.Current.Properties["idUsuarioTienda"].ToString());
                    Debug.WriteLine(pedidos);
                    if (pedidos != null)
                    {
                        if (pedidos.idPedido != 0)
                        {
                            Application.Current.Properties["idPedido"] = pedidos.idPedido;
                        }
                    }
                });

            }    
        }

        string enviarCorreo(string correoDestino, string nombreUsuario, string usuario, string password)
        {
            string body = "<html><body><h2 style='color:#EC7063;'>Tienda UAQ</h2><p>Se registr&oacute; con &eacute;xito en la aplicaci&oacute;n Tienda UAQ, sus datos para iniciar sesi&oacute;n son: </p><table><tr><td><b style='color:#EC7063;'>Usuario: </b></td><td>"+correoDestino+"</td></tr><tr><td><b style='color:#EC7063;'>Contrase&ntilde;a: </b></td><td>"+password+"</td></tr></table><br><p style='color:gray;font-size:11px;'>*Este es un correo autom&aacute;tico, no es necesario responder.</p></body></html>";
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
                }.Send(new MailMessage { From = new MailAddress(correoDestino, nombreUsuario), To = { correoDestino }, Subject = "Registro en la Tienda UAQ", Body = body/*"Ústed se registró correctamente en la aplicación móvil de la Tienda UAQ (Universidad Autonóma de Querétaro), sus datos son-> usuario: " + usuario + ", contraseña: " + password*/,IsBodyHtml = true, BodyEncoding = Encoding.UTF8});
                //erroremail.Text = "Email has been sent successfully.";
                mensaje = "Se envió un mensaje a su correo electrónico proporcionado.";
            }
            catch (Exception ex)
            {
                //erroremail.Text = "ERROR: " + ex.Message;
                mensaje = "Falló al enviar el mensaje a su correo electrónico. " + ex.Message;
            }
            return mensaje;
        }
    }
}
