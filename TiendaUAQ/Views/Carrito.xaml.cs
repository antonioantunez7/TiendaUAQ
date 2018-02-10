using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TiendaUAQ.Models;
using TiendaUAQ.Services;
using Xamarin.Forms;

namespace TiendaUAQ.Views
{
    public partial class Carrito : ContentPage
    {
        public ObservableCollection<Productos> lproductos { get; set; }
        public Carrito()
        {
            InitializeComponent();
            cargaCarrito();
        }

        async void cargaCarrito()
        {
            etiquetaCargando.Text = "Cargando carrito, por favor espere...";
            svProductos.Content = etiquetaCargando;

            Device.BeginInvokeOnMainThread(async () =>
            {
                RestClient cliente = new RestClient();

                var productos = await cliente.GetProductos<ListaProductos>("http://189.211.201.181:88/TiendaUAQWebservice/api/tblproductos");
                if (productos != null)
                {
                    if (productos.listaProductos.Count > 0)
                    {
                        lproductos = new ObservableCollection<Productos>();
                        double total = 0;
                        foreach (var producto in productos.listaProductos)
                        {
                            string url_portada = "http://189.211.201.181:88/" + producto.url_imagen;
                            //string url_portada = "https://www.adidas.mx/dis/dw/image/v2/aaqx_prd/on/demandware.static/-/Sites-adidas-products/default/dw5ef5c4e2/zoom/S97604_01_standard.jpg?sh=840&strip=false&sw=840";
                            lproductos.Add(new Productos
                            {
                                idProducto = producto.idProducto,
                                nombre = producto.nombre,
                                descripcion = producto.descripcion,
                                precio = producto.precio,
                                url_imagen = url_portada    
                            });
                            total = total + producto.precio;
                        }
                        listaProductos.ItemsSource = lproductos;

                        var label1 = new Label
                        {
                            FontSize = 20,
                            Text = "Total a pagar $ "+total,
                            TextColor = Color.FromHex("EC7063"),
                            HorizontalOptions = LayoutOptions.Center,
                            HorizontalTextAlignment = TextAlignment.Center,
                            //VerticalOptions = LayoutOptions.Center,
                            //VerticalTextAlignment = TextAlignment.Center
                        };
                        var button = new Button { 
                            Text = "Comprar",
                            TextColor = Color.White,
                            VerticalOptions = LayoutOptions.Center,
                            FontFamily = "Futura-Medium",
                            HorizontalOptions = LayoutOptions.FillAndExpand
                        };
                        button.Clicked += comprar;
                        var stacklayout1 = new StackLayout
                        {
                            Orientation = StackOrientation.Horizontal,
                            HorizontalOptions = LayoutOptions.Center,
                            Children = {
                                label1
                                }
                        };
                        var stacklayout2 = new StackLayout
                        {
                            Orientation = StackOrientation.Horizontal,
                            HorizontalOptions = LayoutOptions.Fill,
                            Children = {
                                button
                                }
                        };
                        var stacklayoutPrincipal = new StackLayout()
                        {
                            Orientation = StackOrientation.Vertical,
                            Children = {
                                listaProductos,stacklayout1,stacklayout2
                                        }
                        };
                        svProductos.Content = stacklayoutPrincipal;
                    }
                    else
                    {
                        etiquetaCargando.Text = "El carrito esta vacío.";
                        svProductos.Content = etiquetaCargando;
                    }
                }
                else
                {
                    etiquetaCargando.Text = "Error de conexión.";
                    svProductos.Content = etiquetaCargando;
                }
            });
        }

        async void detalleProducto_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            var producto = e.SelectedItem as Productos;
            if (producto != null)
            {
                await Navigation.PushAsync(new DetalleProducto(producto));
                listaProductos.SelectedItem = null;//Para que automaticamente se deseleccione el elemento
            }  
        }

        async void comprar(object sender, EventArgs e)
        {
            await DisplayAlert("Información", "Ústed va a comprar todos los artículos al carrito.", "Aceptar");
        }
    }
}
