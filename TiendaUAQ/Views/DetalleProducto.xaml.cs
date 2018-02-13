using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using TiendaUAQ.Models;
using TiendaUAQ.Services;
using Xamarin.Forms;

namespace TiendaUAQ.Views
{
    public partial class DetalleProducto : ContentPage
    {
        int idProductoGlobal = 0;
        int cantidadArticulos = 0;
        Double precio = 0;
        public DetalleProducto(Productos producto)
        {
            InitializeComponent();
            cargaDetalleProducto(producto);
        }

        void cargaDetalleProducto(Productos producto)
        {
            List<Productos> productos = new List<Productos>{
                new Productos { idProducto =producto.idProducto,
                    nombre = producto.nombre,
                    descripcion = producto.descripcion,
                    precio = producto.precio,
                    url_imagen = producto.url_imagen
                }
            };
            idProductoGlobal = producto.idProducto;
            precio = producto.precio;
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
            if (Application.Current.Properties.ContainsKey("idUsuarioTienda"))
            {
                if (cantidadArticulos == 0)
                {
                    await DisplayAlert("Información", "Seleccione la cantidad de artículos que desea agregar al carrito", "Aceptar");
                    return;
                }
                Button boton = (Button)sender;
                var idProducto = boton.CommandParameter;
                HttpClient cliente = new HttpClient();
                var precioProds = precio * cantidadArticulos;
                FormUrlEncodedContent formContent = null;
                if (Application.Current.Properties.ContainsKey("idPedido"))
                {
                    formContent = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("idPedido", Application.Current.Properties["idPedido"].ToString()),
                        new KeyValuePair<string, string>("idProducto",idProducto.ToString()),
                        new KeyValuePair<string, string>("cantidad",cantidadArticulos.ToString()),
                        new KeyValuePair<string, string>("precio",precioProds.ToString())
                    });
                } else{
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
                var response = await myHttpClient.PostAsync("http://189.211.201.181:88/TiendaUAQWebservice/api/tbldetallespedidos/carrito/guardar/",formContent);
                var json = await response.Content.ReadAsStringAsync();
                RestClient c = new RestClient();
                var pedidoX = await c.convertirJson<Pedidos>(json);
                if (response.IsSuccessStatusCode)
                {
                    var idPedido = pedidoX.idPedido;
                    Application.Current.Properties["idPedido"] = idPedido;
                    await DisplayAlert("Correcto", "Se agregó el producto al carrito correctamente.", "Aceptar");
                }
                else
                {
                    await DisplayAlert("Error", "No se pudo agregar el producto al carrito. Intente nuevamente.", "Aceptar");
                }
            } else{
                await DisplayAlert("Información", "Inicie sesión.", "Aceptar");    
            }
        }

        async void verCarrito(object sender, System.EventArgs e){
            await Navigation.PushAsync(new Carrito());        
        }

        async void cargaCombo_BindingContextChanged(object sender, System.EventArgs e)
        {
            var picker = (Picker)sender;
            int valor = 3;
            var posicion = valor - 1;
            picker.SelectedIndex = posicion;
            await DisplayAlert("Información", ""+idProductoGlobal, "Aceptar");   
        }

        void btnAgregarAlCarrito_BindingContextChanged(object sender, System.EventArgs e)
        {
            var button = (Button)sender;
            button.IsVisible = false;
        }

        void btnModificar_BindingContextChanged(object sender, System.EventArgs e)
        {
            var button = (Button)sender;
            button.IsVisible = false;
        }
    }
}
