using System;
using System.Collections.Generic;
using TiendaUAQ.Models;
using Xamarin.Forms;

namespace TiendaUAQ.Views
{
    public partial class DetalleProducto : ContentPage
    {
        int cantidadArticulos = 0;
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
                //await DisplayAlert("Información", "valor: "+selectedIndex + "- texto: " + textIndex+"- articulos: "+cantidadArticulos, "Aceptar");
            }
        }

        async void agregarAlCarrito(object sender, System.EventArgs e)
        {
            if(cantidadArticulos == 0){
                await DisplayAlert("Información", "Seleccione la cantidad de artículos que desea agregar al carrito", "Aceptar");
                return;
            } 
            await DisplayAlert("Información", "Ústed agregó "+cantidadArticulos+" artículos al carrito.", "Aceptar");
        }

        async void verCarrito(object sender, System.EventArgs e){
            await Navigation.PushAsync(new Carrito());        
        }
    }
}
