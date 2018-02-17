using System;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using System.Text;
using PayPal.Forms;
using PayPal.Forms.Abstractions;
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
        Button BuyManythingsCustomAddressButton;
        public App()
        {
            //Application.Current.Properties["idUsuarioTienda"] = 6;
            InitializeComponent();
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
            /*BuyManythingsCustomAddressButton = new Button()
            {
                Text = "Buy Many Things Button with Custom Address"
            };
            BuyManythingsCustomAddressButton.Clicked += BuyManythingsCustomAddressButton_Clicked;
            MainPage = new ContentPage
            {
                Content = new StackLayout
                {
                    VerticalOptions = LayoutOptions.Center,
                    Children = {
                        BuyManythingsCustomAddressButton
                    }
                }
            };*/
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
                mensaje = "Se envió un mensaje a su correo electrónico proporcionado.";
            }
            catch (Exception ex)
            {
                mensaje = "Falló al enviar el mensaje a su correo electrónico. " + ex.Message;
            }
            return mensaje;
        }

        async void BuyManythingsCustomAddressButton_Clicked(object sender, EventArgs e)
        {
            var result = await CrossPayPalManager.Current.Buy(
                /*new PayPalItem[] {
                    //Nombre del producto, total de productos, precio, moneda (USD o MXN), codigo del producto (codigo de barras)
                    new PayPalItem ("Gorra negra", 2, new Decimal (1.50), "MXN", "1"),
                    new PayPalItem ("Mochila azul", 1, new Decimal (2.00), "MXN", "2"),
                    new PayPalItem ("Uniforme de los gatos salvajes de UAQ", 1, new Decimal (3.90), "MXN", "3")
                },*/
                new PayPalItem[] {
                    //Nombre del producto, total de productos, precio, moneda (USD o MXN), codigo del producto (codigo de barras)
                    //new PayPalItem ("Taza UAQ", 2, new Decimal (0.50), "MXN", "1"),
                    new PayPalItem ("Zapatos", 1, new Decimal (0.01), "MXN", "2"),
                    //new PayPalItem ("Kit de natación Googles y Gorro", 1, new Decimal (0.90), "MXN", "3")
                },
                new Decimal(0),//costo del envio
                new Decimal(0)//impuesto o iva
                              //new ShippingAddress("Domicilio de prueba", "Lago San Ignacio #102, Col. Seminario 4ta Sección ", "", "Toluca de Lerdo", "Estado de México", "50170", "MX")
                              //Nombre, direccion 1, direccion 2, ciudad, estado, codigo postal, codigo del pais
            );
            if (result.Status == PayPalStatus.Cancelled)
            {
                Debug.WriteLine("Cancelled");
            }
            else if (result.Status == PayPalStatus.Error)
            {
                Debug.WriteLine(result.ErrorMessage);
            }
            else if (result.Status == PayPalStatus.Successful)
            {
                Debug.WriteLine("si termino");
                Console.WriteLine("si termino");
                string m = enviarCorreo("scorpion_malboro@hotmail.com", "tono antun", "scorpion_malboro@hotmail.com", "12");
                Console.WriteLine("\n\n"+m);
                Debug.WriteLine(result.ServerResponse.Response.Id);
            }
        }
    }
}
