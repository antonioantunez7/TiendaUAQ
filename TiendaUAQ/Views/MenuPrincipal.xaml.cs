using System;
using System.Collections.Generic;
using System.Diagnostics;
using TiendaUAQ.Models;
using TiendaUAQ.Services;
using Xamarin.Forms;

namespace TiendaUAQ.Views
{
    public partial class MenuPrincipal : MasterDetailPage
    {
        public MenuPrincipal()
        {
            InitializeComponent();
            inicio();
        }

        void inicio()
        {
            List<Models.Menu> menu = new List<Models.Menu>{//Le cambie Menu a Models.Menu porque al ejecutarlo en iOS manda error de ambiguo
                new Models.Menu { id= 1, titulo = "Inicio"/*, detalle = "Regresa a la página de inicio."*/, icono = "inicio.png"},
                new Models.Menu { id= 2, titulo = "Carrito de compras"/*, detalle = "Regresa a la página de super."*/, icono = "icono.png"},
                new Models.Menu { id= 3, titulo = "Departamentos"/*, detalle = "Regresa a la página de departamentos."*/, icono = "icono.png"},
                new Models.Menu { id= 4, titulo = "Acerca de"/*, detalle = "Regresa a la página de acerca de."*/, icono = "acerca.png"},
                new Models.Menu { id= 5, titulo = "Salir"/*, detalle = "Cerrar la aplicación."*/, icono = "salir.png"},
                //new Models.Menu { id= 6, titulo = "Ingresar/Registrarse"/*, detalle = "Cerrar la aplicación."*/, icono = "acerca.png"}
            };
            if(Application.Current.Properties.ContainsKey("idUsuarioTienda")){
                menu.Add(new Models.Menu { id = 6, titulo = "Cuenta", icono = "acerca.png"});
            }
            ListaMenu.ItemsSource = menu;

            Detail = new NavigationPage(new Buscador());//Se cambia para que sea la cartelera la primera en cargar
        }

        public async void ListaMenu_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            var menu = e.SelectedItem as Models.Menu;
            if (menu != null)
            {
                if (menu.id == 1)//Inicio (Cartelera o categorias)
                {
                    Detail = new NavigationPage(new Buscador());
                    IsPresented = false;//Para que el menu desaparesca cuando se le haga click
                }
                if (menu.id == 2)//Carrito de compras
                {
                    IsPresented = false;//Para que el menu desaparesca cuando se le haga click
                    Detail = new NavigationPage(new Carrito());
                }
                if (menu.id == 3)//Departamentos
                {
                    IsPresented = false;//Para que el menu desaparesca cuando se le haga click
                    Detail = new NavigationPage(new DepartamentosView());
                }
                if (menu.id == 4)//Acerca de
                {
                    IsPresented = false;//Para que el menu desaparesca cuando se le haga click
                    Detail = new NavigationPage(new AcercaDe());
                }
                if (menu.id == 5)//Salir
                {
                    string mensaje = "¿Deseas cerrar la aplicación?";
                    if (Application.Current.Properties.ContainsKey("idUsuarioTienda")){
                        mensaje = "¿Desea cerrar sesión?";     
                    }
                    var resp = await this.DisplayAlert("Confirmación", mensaje , "SI", "NO");
                    if (resp)
                    {
                        //Destruye las variables de sesión (persistencia de los datos)
                        if (Application.Current.Properties.ContainsKey("idUsuarioTienda")
                            && Application.Current.Properties.ContainsKey("nombre")
                            && Application.Current.Properties.ContainsKey("paterno")
                            && Application.Current.Properties.ContainsKey("materno")
                            && Application.Current.Properties.ContainsKey("usuario"))
                        {
                            Application.Current.Properties.Remove("idUsuarioTienda");
                            Application.Current.Properties.Remove("nombre");
                            Application.Current.Properties.Remove("paterno");
                            Application.Current.Properties.Remove("materno");
                            Application.Current.Properties.Remove("usuario");
                            Application.Current.MainPage = new MenuPrincipal();
                        } else{
                            System.Environment.Exit(0); 
                        }
                    }
                }
                if (menu.id == 6)//Ingresar o registrarse
                {
                    IsPresented = false;//Para que el menu desaparesca cuando se le haga click
                    Detail = new NavigationPage(new Cuenta());
                }
                ListaMenu.SelectedItem = null;//Para que automaticamente se deseleccione el elemento
            }
        }

        void consultaCarrito()
        {
            if (Application.Current.Properties.ContainsKey("idUsuarioTienda"))
            {
                if (Application.Current.Properties.ContainsKey("idPedido")){
                    Application.Current.Properties.Remove("idPedido");//Primero lo debe eliminar en caso de que existe para que en la validacion si existe pedido lo agregue    
                }
                Device.BeginInvokeOnMainThread(async () =>
                {
                    RestClient cliente = new RestClient();
                    var pedidos = await cliente.GetPedidos<Pedidos>("http://189.211.201.181:88/TiendaUAQWebservice/api/tbldetallespedidos/pedido/usuario/" + Application.Current.Properties["idUsuarioTienda"].ToString());
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

        protected override void OnAppearing()
        {
            base.OnAppearing();
            consultaCarrito();
        }
    }
}
