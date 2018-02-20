using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TiendaUAQ.Models;
using TiendaUAQ.Services;
using Xamarin.Forms;

namespace TiendaUAQ.Views
{
    public partial class DetalleProducto : ContentPage
    {
        int idProductoGlobal = 0;
        int cantidadArticulos = 0;
        Double precioUnitario = 0;
        int idDetallePedido = 0;
        int articulosAgregados = 0;
        Productos productoGlobal;
        public DetalleProducto(Productos producto)
        {
            InitializeComponent();
            productoGlobal = producto;            
            cargaDetalleProducto(producto);
        }

        void cargaDetalleProducto(Productos producto)
        {
            Double precioReal = producto.precio;
            if(precioReal == 0){
                precioReal = producto.precioUnitario;     
            }
            List<Productos> productos = new List<Productos>{
                new Productos { idProducto =producto.idProducto,
                    nombre = producto.nombre,
                    descripcion = producto.descripcion,
                    precio = precioReal,
                    precioUnitario = precioReal,
                    url_imagen = producto.url_imagen,
                    estatusProducto = producto.estatusProducto
                }
            };
            idProductoGlobal = producto.idProducto;
            precioUnitario = producto.precioUnitario;
            DetalleDelProducto.ItemsSource = productos;
        }

        void seleccionaImagenDE(object sender, EventArgs args)
        {
        }

        async void seleccionaCA_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;
            string textIndex = picker.Items[picker.SelectedIndex];
            if (selectedIndex != -1)
            {
                cantidadArticulos = Convert.ToInt32(textIndex);
            }
        }

        async void agregarAlCarrito(object sender, System.EventArgs e)
        {
            Button boton = (Button)sender;
            if (Application.Current.Properties.ContainsKey("idUsuarioTienda"))
            {
                if (cantidadArticulos == 0)
                {
                    await DisplayAlert("Información", "Seleccione la cantidad de artículos que desea agregar al carrito", "Aceptar");
                    return;
                }
                boton.IsEnabled = false;
                var idProducto = boton.CommandParameter;
                //Valida las existencias del producto
                RestClient cliente1 = new RestClient();
                var productos = await cliente1.GetPrductosId<Productos>("http://189.211.201.181:88/TiendaUAQWebservice/api/tblproductos/idProducto/" + idProductoGlobal);
                Debug.Write(productos);
                if (productos != null)
                {
                    if (productos.idProducto > 0)
                    {
                        int existencias = productos.existencias;
                        if (existencias == 0)
                        {
                            await DisplayAlert("Información", "Artículo agotado.", "Aceptar");
                        } else if (cantidadArticulos <= existencias)
                        {
                            HttpClient cliente = new HttpClient();
                            Double precioProds = precioUnitario * cantidadArticulos;
                            FormUrlEncodedContent formContent = null;
                            if (Application.Current.Properties.ContainsKey("idPedido"))
                            {
                                formContent = new FormUrlEncodedContent(new[]
                                {
                                    new KeyValuePair<string, string>("idPedido", Application.Current.Properties["idPedido"].ToString()),
                                    new KeyValuePair<string, string>("idProducto",idProducto.ToString()),
                                    new KeyValuePair<string, string>("cantidad",cantidadArticulos.ToString()),
                                    new KeyValuePair<string, string>("precio",precioProds.ToString()),
                                    new KeyValuePair<string, string>("idusuario",Application.Current.Properties["idUsuarioTienda"].ToString())
                                });
                            }
                            else
                            {
                                formContent = new FormUrlEncodedContent(new[]
                                        {
                                    new KeyValuePair<string, string>("idProducto",idProducto.ToString()),
                                    new KeyValuePair<string, string>("cantidad",cantidadArticulos.ToString()),
                                    new KeyValuePair<string, string>("precio",precioProds.ToString()),
                                    new KeyValuePair<string, string>("idusuario",Application.Current.Properties["idUsuarioTienda"].ToString())
                                });
                            }
                            var myHttpClient = new HttpClient();
                            var authData = string.Format("{0}:{1}", "tiendaUAQ", "t13nd4U4q");
                            var authHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(authData));
                            myHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeaderValue);
                            var response = await myHttpClient.PostAsync("http://189.211.201.181:88/TiendaUAQWebservice/api/tbldetallespedidos/carrito/guardar/", formContent);
                            var json = await response.Content.ReadAsStringAsync();
                            RestClient c = new RestClient();
                            var pedidoX = await c.convertirJson<Pedidos>(json);
                            if (response.IsSuccessStatusCode)
                            {
                                var idPedido = pedidoX.idPedido;
                                Application.Current.Properties["idPedido"] = idPedido;
                                await DisplayAlert("Correcto", "Se agregó el producto al carrito correctamente.", "Aceptar");
                                await Navigation.PushAsync(new Carrito());
                            }
                            else
                            {
                                await DisplayAlert("Información", "No se pudo agregar el producto al carrito. Intente nuevamente.", "Aceptar");
                            }
                        } else{
                            await DisplayAlert("Información", "No se cuenta con "+cantidadArticulos+" artículos disponibles. Puede agregar máximo "+existencias+" artículos.", "Aceptar");    
                        }
                    } else{
                        await DisplayAlert("Error", "No encontró el producto. Intente nuevamente.", "Aceptar");
                    }
                } else{
                    await DisplayAlert("Error", "Error en la consulta del producto. Intente nuevamente.", "Aceptar");
                }
            } else{
                await Navigation.PushAsync(new Inicio());   
            }
            boton.IsEnabled = true;

        }

        async void modificarDelCarrito(object sender, System.EventArgs e)
        {
            Button boton = (Button)sender;
            if (Application.Current.Properties.ContainsKey("idUsuarioTienda"))
            {
                if (cantidadArticulos == 0)
                {
                    await DisplayAlert("Información", "Seleccione la cantidad de artículos que desea agregar al carrito", "Aceptar");
                    return;
                }
                boton.IsEnabled = false;
                var idProducto = boton.CommandParameter;
                //Valida las existencias del producto
                RestClient cliente1 = new RestClient();
                var productos = await cliente1.GetPrductosId<Productos>("http://189.211.201.181:88/TiendaUAQWebservice/api/tblproductos/idProducto/" + idProductoGlobal);
                Debug.Write(productos);
                if (productos != null)
                {
                    if (productos.idProducto > 0)
                    {
                        int existencias = productos.existencias;
                        if (cantidadArticulos <= existencias)
                        {
                            HttpClient cliente = new HttpClient();
                            Double precioProds = precioUnitario * cantidadArticulos;
                            FormUrlEncodedContent formContent = null;
                            if (idDetallePedido != 0)
                            {
                                formContent = new FormUrlEncodedContent(new[]
                                {
                                    new KeyValuePair<string, string>("idDetallePedido", idDetallePedido.ToString()),
                                    new KeyValuePair<string, string>("precio",precioProds.ToString()),
                                    new KeyValuePair<string, string>("cantidad",cantidadArticulos.ToString()),
                                    new KeyValuePair<string, string>("activo","S")
                                });
                            }
                            else
                            {
                                await DisplayAlert("Correcto", "El producto no existe en un pedido aún.", "Aceptar");
                                return;
                            }
                            var myHttpClient = new HttpClient();
                            var authData = string.Format("{0}:{1}", "tiendaUAQ", "t13nd4U4q");
                            var authHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(authData));
                            myHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeaderValue);
                            var response = await myHttpClient.PostAsync("http://189.211.201.181:88/TiendaUAQWebservice/api/tbldetallespedidos/guardar/", formContent);
                            var json = await response.Content.ReadAsStringAsync();
                            RestClient c = new RestClient();
                            var detallepedidoX = await c.convertirJson<DetallePedido>(json);
                            if (response.IsSuccessStatusCode)
                            {
                                idDetallePedido = detallepedidoX.idDetallePedido;
                                await DisplayAlert("Correcto", "Se modificó la cantidad de productos en el carrito correctamente.", "Aceptar");
                                await Navigation.PushAsync(new Carrito());
                            }
                            else
                            {
                                await DisplayAlert("Error", "No se pudo modificar la cantidad de productos del carrito. Intente nuevamente.", "Aceptar");
                            }
                        }
                        else
                        {
                            await DisplayAlert("Error", "No se cuenta con " + cantidadArticulos + " artículos disponibles. Puede agregar máximo " + existencias + " artículos.", "Aceptar");
                        }
                    }
                    else
                    {
                        await DisplayAlert("Error", "No encontró el producto. Intente nuevamente.", "Aceptar");
                    }
                }
                else
                {
                    await DisplayAlert("Error", "Error en la consulta del producto. Intente nuevamente.", "Aceptar");
                }
            }
            else
            {
                await Navigation.PushAsync(new Inicio());
            }
            boton.IsEnabled = true;
        }

        async void verCarrito(object sender, System.EventArgs e){
            await Navigation.PushAsync(new Carrito());        
        }

        async void cargaCombo_BindingContextChanged(object sender, System.EventArgs e)
        {
            //await DisplayAlert("Información", "cargaCombo_BindingContextChanged.", "Aceptar");
            if (idDetallePedido != 0)
            {
                var picker = (Picker)sender;
                int valor = articulosAgregados;
                var posicion = valor - 1;
                picker.SelectedIndex = posicion;
            }
        }

        async void btnAgregarAlCarrito_BindingContextChanged(object sender, System.EventArgs e)
        {
            //await DisplayAlert("Información", "btnAgregarAlCarrito_BindingContextChanged.", "Aceptar");
            if (idDetallePedido != 0)
            {
                var button = (Button)sender;
                button.IsVisible = false;
            }
        }

        async void btnModificar_BindingContextChanged(object sender, System.EventArgs e)
        {
            //await DisplayAlert("Información", "btnModificar_BindingContextChanged.", "Aceptar");
            if (idDetallePedido == 0)
            {
                var button = (Button)sender;
                button.IsVisible = false;
            }
        }

        async protected override void OnAppearing()
        {
            base.OnAppearing();
            //idDetallePedido = 0;
            //articulosAgregados = 0;
            //cantidadArticulos = 0;
            if (Application.Current.Properties.ContainsKey("idPedido"))
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    RestClient cliente = new RestClient();
                    Console.WriteLine("http://189.211.201.181:88/TiendaUAQWebservice/api/tbldetallespedidos/existe/producto/" + idProductoGlobal + "/pedido/" + Application.Current.Properties["idPedido"].ToString());
                    var detallepedido = await cliente.GetDetallePedido<DetallePedido>("http://189.211.201.181:88/TiendaUAQWebservice/api/tbldetallespedidos/existe/producto/" + idProductoGlobal + "/pedido/" + Application.Current.Properties["idPedido"].ToString());
                    if (detallepedido != null)
                    {
                        if (detallepedido.idDetallePedido != 0)
                        {
                            idDetallePedido = detallepedido.idDetallePedido;
                            articulosAgregados = detallepedido.cantidad;
                            cantidadArticulos = articulosAgregados;
                            cargaDetalleProducto(productoGlobal);
                        }
                    }
                });
            }
        }

    }
}
