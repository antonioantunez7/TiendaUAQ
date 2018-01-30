using System;
using System.Collections.Generic;
using TiendaUAQ.Models;
using Xamarin.Forms;

namespace TiendaUAQ.Views
{
    public partial class DetalleProducto : ContentPage
    {
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
            DetalleDelProducto.ItemsSource = productos;
        }

        async void agregarAlCarrito(object sender, System.EventArgs e)
        {
        }
    }
}
