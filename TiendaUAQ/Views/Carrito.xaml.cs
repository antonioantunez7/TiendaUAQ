using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using PayPal.Forms;
using PayPal.Forms.Abstractions;
using TiendaUAQ.Models;
using TiendaUAQ.Services;
using Xamarin.Forms;

namespace TiendaUAQ.Views
{
    public partial class Carrito : ContentPage
    {
        public ObservableCollection<Productos> lproductos { get; set; }
        int cantidad = 0;
        double total = 0;
        public Carrito()
        {
            InitializeComponent();
        }

        async void cargaCarrito()
        {
            if (Application.Current.Properties.ContainsKey("idUsuarioTienda"))
            {
                etiquetaCargando.Text = "Cargando carrito, por favor espere...";
                svProductos.Content = etiquetaCargando;

                Device.BeginInvokeOnMainThread(async () =>
                {
                    RestClient cliente = new RestClient();

                    var pedidos = await cliente.GetPedidos<Pedidos>("http://148.240.202.160:88/TiendaUAQWebservice/api/tbldetallespedidos/pedido/usuario/"+Application.Current.Properties["idUsuarioTienda"].ToString());
                    if (pedidos != null)
                    {
                        if (pedidos.idPedido != 0)
                        {
                            if (Application.Current.Properties.ContainsKey("idPedido"))
                            {
                                Application.Current.Properties.Remove("idPedido");//Primero lo debe eliminar en caso de que existe para que en la validacion si existe pedido lo agregue    
                            }
                            Application.Current.Properties["idPedido"] = pedidos.idPedido;
                            lproductos = new ObservableCollection<Productos>();

                            foreach (var producto in pedidos.detalle)
                            {
                                string url_portada = "http://148.240.202.160:88/" + producto.url_imagen;
                                cantidad = producto.cantidad;
                                string estatusProducto = "# Existencias: " + producto.existencias;
                                string descripcionPrecio = "" + producto.precio;
                                if(producto.existencias == 0){//Si ya no hay productos
                                    estatusProducto = "[Producto agotado]";
                                    descripcionPrecio = "? [No contemplado]";
                                } else if(cantidad > producto.existencias){//Si la cantidad de productos en el carrito es mayor al de existencias
                                    estatusProducto = "# Existencias: "+producto.existencias+". Seleccione y cambie la cantidad de productos.";       
                                    descripcionPrecio = "? [No contemplado]";
                                }
                                lproductos.Add(new Productos
                                {
                                    idProducto = producto.idProducto,
                                    idDetallePedido = producto.idDetallePedido,
                                    nombre = producto.nombre,
                                    descripcion = producto.descripcion,
                                    precio = producto.precio,
                                    descripcionPrecio = descripcionPrecio,
                                    precioUnitario = producto.precioUnitario,
                                    url_imagen = url_portada,
                                    cantidad = cantidad,
                                    nombreCantidad = producto.nombre + " (Cant." + cantidad + ")",
                                    estatusProducto = estatusProducto

                                });
                                if (producto.existencias == 0)
                                {
                                    
                                } else if (cantidad > producto.existencias){
                                    
                                } else{
                                    total = total + producto.precio;
                                }

                            }
                            listaProductos.ItemsSource = lproductos;

                            var label1 = new Label
                            {
                                FontSize = 20,
                                Text = "Total a pagar $ " + total,
                                TextColor = Color.FromHex("EC7063"),
                                HorizontalOptions = LayoutOptions.Center,
                                HorizontalTextAlignment = TextAlignment.Center,
                            };
                            var button = new Button
                            {
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
                            if (Application.Current.Properties.ContainsKey("idPedido"))
                            {
                                Application.Current.Properties.Remove("idPedido");//Primero lo debe eliminar en caso de que existe para que en la validacion si existe pedido lo agregue    
                            }
                            etiquetaCargando.Text = "El carrito esta vacío.";
                            svProductos.Content = etiquetaCargando;
                        }
                    }
                    else
                    {
                        if (Application.Current.Properties.ContainsKey("idPedido"))
                        {
                            Application.Current.Properties.Remove("idPedido");//Primero lo debe eliminar en caso de que existe para que en la validacion si existe pedido lo agregue    
                        }
                        etiquetaCargando.Text = "Error de conexión.";
                        svProductos.Content = etiquetaCargando;
                    }
                });
            } else{
                etiquetaCargando.Text = "Inicie sesión desde el menú principal 'Iniciar sesión'. Si no cuenta con un usuario y contraseña seleccione 'Registrarse'.";
                svProductos.Content = svIS;    
                //await Navigation.PushAsync(new Inicio());
            }
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
            if (Application.Current.Properties.ContainsKey("idUsuarioTienda"))
            {
                if (total > 0)
                {
                    if (Application.Current.Properties.ContainsKey("idPedido"))
                    {
                        await Navigation.PushAsync(new DireccionEnvio());
                    } else{
                        await DisplayAlert("Error", "No existe el idPedido. Verifique o comuniquese con el administrador.", "Aceptar");    
                    }
                } else{
                    await DisplayAlert("Error", "No hay artículo disponibles del listado del carrito. O su total a pagar es $0 MXN.", "Aceptar");
                }
            } else{
                await Navigation.PushAsync(new Inicio());    
            }
            ///await DisplayAlert("Información", "Ústed va a comprar todos los artículos al carrito.", "Aceptar");
        }

        async void eliminarDelCarrito(object sender, System.EventArgs e)
        {
            var resp = await this.DisplayAlert("Confirmación", "¿Desea eliminar este producto del carrito?", "SI", "NO");
            if (resp)
            {
                Button boton = (Button)sender;
                var idDetallePedido = boton.CommandParameter;
                HttpClient cliente = new HttpClient();
                FormUrlEncodedContent formContent = null;
                if (idDetallePedido != "")
                {
                    formContent = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("idDetallePedido", idDetallePedido.ToString()),
                        new KeyValuePair<string, string>("activo","N")
                    });
                }
                else
                {
                    await DisplayAlert("Información", "Seleccione el producto que desee elimininar del carrito.", "Aceptar");
                    return;
                }
                var myHttpClient = new HttpClient();
                var authData = string.Format("{0}:{1}", "tiendaUAQ", "t13nd4U4q");
                var authHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(authData));
                myHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeaderValue);
                var response = await myHttpClient.PostAsync("http://148.240.202.160:88/TiendaUAQWebservice/api/tbldetallespedidos/guardar/", formContent);
                var json = await response.Content.ReadAsStringAsync();
                Debug.WriteLine(json);
                if (response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Correcto", "Se eliminó el producto del carrito correctamente.", "Aceptar");
                    cargaCarrito();
                }
                else
                {
                    await DisplayAlert("Error", "No se pudo eliminar el producto del carrito. Intente nuevamente.", "Aceptar");
                }
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            cargaCarrito();
        }

        async void IniciarSesion_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new Inicio());
        }
    }
}
