using System;
using System.Collections.Generic;
using System.Diagnostics;
using TiendaUAQ.Models;
using TiendaUAQ.Services;
using Xamarin.Forms;

namespace TiendaUAQ.Views
{
    public partial class ProductosView : ContentPage
    {
        private Grid gridProductos = new Grid();
        public ProductosView(int cveSubdepartamento)
        {
            InitializeComponent();
            cargaProductos(cveSubdepartamento);
        }

        public void cargaProductos(int cveSubdepartamento){
            etiquetaCargando.Text = "Cargando productos, por favor espere...";
            vistaProductos.Content = etiquetaCargando;
            Device.BeginInvokeOnMainThread(async () =>
            {
                RestClient cliente = new RestClient();
                var productos = await cliente.GetProductos<ListaProductos>("http://189.211.201.181:88/TiendaUAQWebservice/api/tblproductos/subdepartamento/"+cveSubdepartamento);
                Debug.Write(productos);
                if (productos != null)
                {
                    //int totalRegistros = categorias.listaCategorias.Count;
                    int totalRegistros = productos.listaProductos.Count;
                    int maximoColumnas = 2;
                    int auxColumnas = 0;
                    int renglones = 0;
                    if (totalRegistros > 0)
                    {
                        for (int i = 0; i < maximoColumnas; i++)
                        {
                            gridProductos.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(0.5, GridUnitType.Star) });
                        }
                        for (int columnas = 0; columnas < totalRegistros; columnas++)
                        {
                            if (columnas == 0)
                            {
                                gridProductos.RowDefinitions.Add(new RowDefinition() { Height = 160 });
                                auxColumnas = 0;
                            }
                            else if (auxColumnas / maximoColumnas == 1)
                            {//Si todavia faltan elementos 
                             //Crear renglon    
                                gridProductos.RowDefinitions.Add(new RowDefinition() { Height = 150 });
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
                                //int cveCategoria = categorias.listaCategorias[columnas].cveCategoria;
                                int idProducto = productos.listaProductos[columnas].idProducto;
                                string nombre = productos.listaProductos[columnas].nombre;
                                //string descCategoria = categorias.listaCategorias[columnas].descCategoria;
                                string url_portada = "http://189.211.201.181:88/" + productos.listaProductos[columnas].url_imagen;
                                //string url_portada = "https://www.adidas.mx/dis/dw/image/v2/aaqx_prd/on/demandware.static/-/Sites-adidas-products/default/dw5ef5c4e2/zoom/S97604_01_standard.jpg?sh=840&strip=false&sw=840";
                                //string url_portada = "https://pbs.twimg.com/profile_images/3673725732/da6f8684f131d039ee285cbf2bc52529.png";
                                Debug.Write(url_portada);
                                var imagen = new Image()
                                {
                                    Source = url_portada,
                                    //WidthRequest = 100,
                                    HeightRequest = 70,
                                    VerticalOptions = LayoutOptions.Center,
                                    HorizontalOptions = LayoutOptions.Center,
                                    Opacity = 0.8
                                };
                                string estatusProducto = "# Existencias: " + productos.listaProductos[columnas].existencias + "."; 
                                Productos producto = new Productos{idProducto = idProducto, nombre = nombre,
                                    descripcion = productos.listaProductos[columnas].descripcion, precio = productos.listaProductos[columnas].precio,  precioUnitario = productos.listaProductos[columnas].precio, url_imagen = url_portada, estatusProducto = estatusProducto};

                                //Se crea el evento del clic de la imagen
                                var tapGestureRecognizer = new TapGestureRecognizer();
                                tapGestureRecognizer.Tapped += (s, e) =>
                                {
                                    //imagen.Opacity = .5;
                                    //cargaSubdepartamentos(cveDepartamento, descDepartamento);
                                    cargaProducto(producto);
                                };
                                imagen.GestureRecognizers.Add(tapGestureRecognizer);
                                //gridCategorias.Children.Add(imagen, auxColumnas, renglones);

                                //Diseño nuevo
                                var stacklayout1 = new StackLayout
                                {
                                    Orientation = StackOrientation.Horizontal,
                                    HorizontalOptions = LayoutOptions.Center,
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

                                var label2 = new Label
                                {
                                    FontSize = 10,
                                    Text = "Label 2",
                                    TextColor = Color.Gray,
                                    HorizontalOptions = LayoutOptions.FillAndExpand,
                                };

                                var label3 = new Label
                                {
                                    FontSize = 14,
                                    Text = "$"+productos.listaProductos[columnas].precio,
                                    TextColor = Color.Maroon,
                                    HorizontalOptions = LayoutOptions.Center,
                                    HorizontalTextAlignment = TextAlignment.Center,
                                    VerticalOptions = LayoutOptions.Center,
                                    VerticalTextAlignment = TextAlignment.Center
                                };
                                var label4 = new Label
                                {
                                    FontSize = 12,
                                    Text = nombre,
                                    TextColor = Color.Black,
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
                                            //label1,
                                            //label2,
                                            label3,
                                        }
                                };

                                var stacklayout3 = new StackLayout
                                {
                                    Orientation = StackOrientation.Horizontal,
                                    HorizontalOptions = LayoutOptions.Center,
                                    Children = {
                                            label4,
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
                    } else{
                        etiquetaCargando.Text = "No existen productos en este subdepartamento.";
                        vistaProductos.Content = etiquetaCargando;
                    }
                } else{
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
