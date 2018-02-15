using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TiendaUAQ.Models;
using TiendaUAQ.Services;
using Xamarin.Forms;

namespace TiendaUAQ.Views
{
    public partial class Buscador : ContentPage
    {
        public Buscador()
        {
            InitializeComponent();
            etiquetaCargando.Text = "Ingrese una palabra para buscar productos.";
            vistaProductos.Content = etiquetaCargando;
            buscaProductos("");
        }

        void buscarArticulos(object sender, System.EventArgs e)
        {
            var palabra = MainSearchBar.Text.ToLower();
            if (palabra == "")
            {
                etiquetaCargando.Text = "Ingrese una palabra para buscar productos.";
                vistaProductos.Content = etiquetaCargando;
            }
            else
            {
                buscaProductos(palabra);
            }
        }

        public void buscaProductos(string palabra)
        {
            etiquetaCargando.Text = "Buscando artículos, por favor espere...";
            vistaProductos.Content = etiquetaCargando;
            Device.BeginInvokeOnMainThread(async () =>
            {
                RestClient cliente = new RestClient();
                var productos = await cliente.GetProductos<ListaProductos>("http://189.211.201.181:88/TiendaUAQWebservice/api/tblproductos/"+palabra);
                Debug.Write(productos);
                if (productos != null)
                {
                    int totalRegistros = productos.listaProductos.Count;
                    if (totalRegistros > 0)
                    {
                        Grid gridProductos = new Grid();
                        int maximoColumnas = 2;
                        int auxColumnas = 0;
                        int renglones = 0;
                        for (int i = 0; i < maximoColumnas; i++)
                        {
                            gridProductos.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(0.5, GridUnitType.Star) });
                        }
                        for (int columnas = 0; columnas < totalRegistros; columnas++)
                        {
                            if (columnas == 0)
                            {
                                gridProductos.RowDefinitions.Add(new RowDefinition() { Height = 180 });
                                auxColumnas = 0;
                            }
                            else if (auxColumnas / maximoColumnas == 1)
                            {//Si todavia faltan elementos 
                             //Crear renglon    
                                gridProductos.RowDefinitions.Add(new RowDefinition() { Height = 180 });
                                renglones++;
                                auxColumnas = 0;
                            }
                            if (auxColumnas == maximoColumnas)
                            {
                                auxColumnas = 0;
                            }
                            else
                            {
                                //Crear el objeto a insertar
                                int idProducto = productos.listaProductos[columnas].idProducto;
                                string nombre = productos.listaProductos[columnas].nombre;
                                string url_portada = "http://189.211.201.181:88/" + productos.listaProductos[columnas].url_imagen;
                                Double precio = productos.listaProductos[columnas].precio;
                                Debug.Write(url_portada);
                                var imagen = new Image()
                                {
                                    Source = url_portada,
                                    HeightRequest = 80,
                                    VerticalOptions = LayoutOptions.StartAndExpand,
                                    HorizontalOptions = LayoutOptions.Center,
                                    Opacity = 0.8
                                };
                                //Se crea el evento del clic de la imagen
                                var tapGestureRecognizer = new TapGestureRecognizer();
                                string estatusProducto = "# Existencias: " + productos.listaProductos[columnas].existencias + "."; 
                                Productos productoX = new Productos
                                {
                                    idProducto = idProducto,
                                    nombre = nombre,
                                    url_imagen = url_portada,
                                    descripcion = productos.listaProductos[columnas].descripcion,
                                    precio = precio,
                                    precioUnitario = precio,
                                    estatusProducto = estatusProducto
                                };
                                tapGestureRecognizer.Tapped += (s, e) =>
                                {
                                    cargaProducto(productoX);
                                };
                                imagen.GestureRecognizers.Add(tapGestureRecognizer);

                                //Diseño nuevo
                                var stacklayout1 = new StackLayout
                                {
                                    Children = {
                                            imagen
                                        }
                                };

                                var label1 = new Label
                                {
                                    FontSize = 10,
                                    Text = "Label 1",
                                    TextColor = Color.Black,
                                    FontAttributes = FontAttributes.Bold,
                                    HorizontalOptions = LayoutOptions.Start,
                                    VerticalOptions = LayoutOptions.Center,
                                    WidthRequest = 150
                                };

                                /*var label2 = new Label
                                {
                                    FontSize = 12,
                                    Text = "Lable 2",
                                    TextColor = Color.FromHex("FBFBFB"),
                                    HorizontalOptions = LayoutOptions.FillAndExpand,
                                };*/
                                var label2 = new Label
                                {
                                    FontSize = 12,
                                    Text = "$ "+precio,
                                    TextColor = Color.FromHex("EC7063"),
                                    HorizontalOptions = LayoutOptions.Center,
                                    HorizontalTextAlignment = TextAlignment.Center,
                                    VerticalOptions = LayoutOptions.Center,
                                    VerticalTextAlignment = TextAlignment.Center
                                };
                                var label3 = new Label
                                {
                                    FontSize = 12,
                                    Text = nombre,
                                    TextColor = Color.Gray,
                                    HorizontalOptions = LayoutOptions.Center,
                                    HorizontalTextAlignment = TextAlignment.Center,
                                    VerticalOptions = LayoutOptions.Center,
                                    VerticalTextAlignment = TextAlignment.Center
                                };
                                var stacklayout2 = new StackLayout
                                {
                                    Orientation = StackOrientation.Horizontal,
                                    HorizontalOptions = LayoutOptions.Center,
                                    Children = {
                                            label2
                                        }
                                };
                                var stacklayout3 = new StackLayout
                                {
                                    Orientation = StackOrientation.Horizontal,
                                    HorizontalOptions = LayoutOptions.Center,
                                    Children = {
                                            label3
                                        }
                                };

                                var stacklayoutPrincipal = new StackLayout()
                                {
                                    Orientation = StackOrientation.Vertical,
                                    Children = {
                                        stacklayout1,
                                        stacklayout2,
                                        stacklayout3
                                    }
                                };
                                var frame = new Frame()
                                {
                                    BackgroundColor = Color.FromHex("FBFBFB")
                                };
                                frame.Content = stacklayoutPrincipal;

                                gridProductos.Children.Add(frame, auxColumnas, renglones);

                            }
                            auxColumnas++;
                        }
                        vistaProductos.Content = gridProductos;
                    }
                    else
                    {
                        etiquetaCargando.Text = "No existen productos coincidentes.";
                        vistaProductos.Content = etiquetaCargando;
                    }
                }
                else
                {
                    etiquetaCargando.Text = "Error de conexión.";
                    vistaProductos.Content = etiquetaCargando;
                }
            });
        }

        private async void cargaProducto(Productos producto)
        {
            await Navigation.PushAsync(new DetalleProducto(producto));
        }
    }
}
