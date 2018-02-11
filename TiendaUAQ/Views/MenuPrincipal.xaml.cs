using System;
using System.Collections.Generic;

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
                    var resp = await this.DisplayAlert("Confirmación", "¿Desea cerrar sesión?", "SI", "NO");
                    if (resp)
                    {
                        //Destruye las variables de sesión (persistencia de los datos)
                        if (Application.Current.Properties.ContainsKey("idUsuario")
                            && Application.Current.Properties.ContainsKey("nombre")
                            && Application.Current.Properties.ContainsKey("paterno")
                            && Application.Current.Properties.ContainsKey("materno"))
                        {
                            Application.Current.Properties.Remove("idUsuario");
                            Application.Current.Properties.Remove("nombre");
                            Application.Current.Properties.Remove("paterno");
                            Application.Current.Properties.Remove("materno");
                        }
                        Application.Current.MainPage = new NavigationPage(new Inicio());
                    }
                }
                if (menu.id == 6)//Ingresar o registrarse
                {
                    IsPresented = false;//Para que el menu desaparesca cuando se le haga click
                    Detail = new NavigationPage(new Inicio());
                }
                ListaMenu.SelectedItem = null;//Para que automaticamente se deseleccione el elemento
            }
        }
    }
}
