using System;
using System.Collections.Generic;
using System.Diagnostics;
using PayPal.Forms;
using PayPal.Forms.Abstractions;
using TiendaUAQ.Models;
using TiendaUAQ.Services;
using Xamarin.Forms;

namespace TiendaUAQ.Views
{
    public partial class DireccionEnvio : ContentPage
    {
        
        public DireccionEnvio()
        {
            InitializeComponent();
        }

        async void realizarPago_Clicked(object sender, System.EventArgs e)
        {
            
            Device.BeginInvokeOnMainThread(async () =>
            {
                if (string.IsNullOrEmpty(txtCalle.Text))
                {
                    await DisplayAlert("Información", "Ingrese su calle", "Aceptar");
                    txtCalle.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(txtColonia.Text))
                {
                    await DisplayAlert("Información", "Ingrese su colonia", "Aceptar");
                    txtColonia.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(txtCiudad.Text))
                {
                    await DisplayAlert("Información", "Ingrese su ciudad", "Aceptar");
                    txtCiudad.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(txtEstado.Text))
                {
                    await DisplayAlert("Información", "Ingrese su estado", "Aceptar");
                    txtEstado.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(txtCP.Text))
                {
                    await DisplayAlert("Información", "Ingrese su código postal", "Aceptar");
                    txtCP.Focus();
                    return;
                }
                waitActivityIndicador.IsRunning = true;//Pone el de cargando
                btnContinuarPago.IsEnabled = false;//Deshabilita el boton
                RestClient cliente = new RestClient();
                var pedidos = await cliente.GetPedidos<Pedidos>("http://189.211.201.181:88/TiendaUAQWebservice/api/tbldetallespedidos/pedido/usuario/" + Application.Current.Properties["idUsuarioTienda"].ToString());
                if (pedidos != null)
                {
                    if (pedidos.idPedido != 0)
                    {
                        Debug.WriteLine("\n\n ---- si entro a hacer el cobro");
                        int totalItems = pedidos.detalle.Count;
                        Debug.WriteLine("\n\n" + totalItems);
                        int costoEnvio = 0;
                        int iva = 0;
                        var i = 0;
                        var procedePago = true;
                        string mensajeError = "";
                        PayPalItem[] items = new PayPalItem[totalItems];
                        foreach (var producto in pedidos.detalle)
                        {
                            if (producto.existencias == 0)
                            {//Si ya no hay productos
                                procedePago = false;
                                mensajeError = "Existen productos agotados, verifique el carrito e intente nuevamente.";
                            }
                            else if (producto.cantidad > producto.existencias)
                            {//Si la cantidad de productos en el carrito es mayor al de existencias
                                procedePago = false;
                                mensajeError = "Existen productos que no se cubre el total de pedidos al de existencias, verifique el carrito e intente nuevamente.";
                            }
                            else
                            {
                                items[i] = new PayPalItem(producto.nombre, (uint)producto.cantidad, new Decimal(0.01), "MXN", producto.idProducto.ToString());
                                //items[i] = new PayPalItem(producto.nombre, (uint)producto.cantidad, new Decimal(producto.precioUnitario), "MXN", producto.idProducto.ToString());
                            }
                            i++;

                        }
                        if (procedePago)//si el pago procede
                        {
                            var result = await CrossPayPalManager.Current.Buy(
                                items,
                                new Decimal(costoEnvio),//costo del envio
                                new Decimal(iva),//impuesto o iva
                                new ShippingAddress("Dirección de envío", txtCalle.Text+", "+txtColonia.Text , "", txtCiudad.Text, txtEstado.Text, txtCP.Text, "MX")
                                                //Nombre, direccion 1, direccion 2, ciudad, estado, codigo postal, codigo del pais
                            );
                            if (result.Status == PayPalStatus.Cancelled)
                            {
                                Debug.WriteLine("Cancelled");
                                await Navigation.PushAsync(new Carrito()); 
                            }
                            else if (result.Status == PayPalStatus.Error)
                            {
                                Debug.WriteLine(result.ErrorMessage);
                                btnContinuarPago.IsEnabled = true;//Habilita el boton
                                waitActivityIndicador.IsRunning = false;//Quita el de cargando
                                await DisplayAlert("Error", "Ocurrió un error al realizar el pago. " + result.ErrorMessage, "Aceptar");
                            }
                            else if (result.Status == PayPalStatus.Successful)
                            {
                                //var result2 = await Shipping
                                Debug.WriteLine("\n\n\n ---------------- ");
                                Debug.WriteLine(result.ServerResponse.Response.Id);
                                string direccionCompletaEnvio = txtCalle.Text + ", " + txtColonia.Text + ", " + txtCiudad.Text + ", " + txtEstado.Text + ", " + txtCP.Text;
                                await DisplayAlert("Correcto", "Se realizó el pago correctamente. " + direccionCompletaEnvio, "Aceptar");
                                //Se debe de actualizar el pedido y cada uno de los artículos se debe disminuir el número de existencias
                                await Navigation.PushAsync(new Carrito());
                            }
                        }
                        else
                        {
                            btnContinuarPago.IsEnabled = true;//Habilita el boton
                            waitActivityIndicador.IsRunning = false;//Quita el de cargando
                            await DisplayAlert("Error", mensajeError, "Aceptar");
                        }
                    }
                    else
                    {
                        btnContinuarPago.IsEnabled = true;//Habilita el boton
                        waitActivityIndicador.IsRunning = false;//Quita el de cargando
                        await DisplayAlert("Error", "El carrito esta vacío.", "Aceptar");
                    }
                }
                else
                {
                    await DisplayAlert("Error", "Error de conexión.", "Aceptar");
                }
            });
        }

        async void Cancelar_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
