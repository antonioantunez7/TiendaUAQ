using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text;
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
            if (Application.Current.Properties.ContainsKey("idPedido"))
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
                            int totalItems = pedidos.detalle.Count;
                            int costoEnvio = 0;
                            int iva = 0;
                            var i = 0;
                            double total = 0;
                            var procedePago = true;
                            string mensajeError = "";
                            PayPalItem[] items = new PayPalItem[totalItems];
                            string cuerpoCorreo = "<html><body style='font-family: Arial, Helvetica, sans-serif;'><h2 style='color:#EC7063;'>Tienda UAQ</h2><p>Detalles de la compra: </p><table border='1' bordercolor='gray' style='border-collapse: collapse;' cellpadding='5'><thead><th style='color:#EC7063;'>Producto</th><th style='color:#EC7063;'>Precio</th></thead><tbody>";
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
                                    total = total + producto.precio;
                                    //items[i] = new PayPalItem(producto.nombre, (uint)producto.cantidad, new Decimal(0.01), "MXN", producto.idProducto.ToString());
                                    items[i] = new PayPalItem(producto.nombre, (uint)producto.cantidad, new Decimal(producto.precioUnitario), "MXN", producto.idProducto.ToString());
                                cuerpoCorreo += "<tr><td>" + producto.nombre + " (Cant. " + producto.cantidad + ")</td><td style='text-align:right'>$" + producto.precio + "</td></tr>";
                                }
                                i++;

                            }

                            if (procedePago)//si el pago procede
                        {
                                var result = await CrossPayPalManager.Current.Buy(
                                    items,
                                    new Decimal(costoEnvio),//costo del envio
                                    new Decimal(iva),//impuesto o iva
                                    new ShippingAddress("Dirección de envío", txtCalle.Text + ", " + txtColonia.Text, "", txtCiudad.Text, txtEstado.Text, txtCP.Text, "MX")
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
                                    string direccionCompletaEnvio = txtCalle.Text + ", " + txtColonia.Text + ", " + txtCiudad.Text + ", " + txtEstado.Text + ", C.P.: " + txtCP.Text;
                                    string referencia = result.ServerResponse.Response.Id;
                                    var formContent = new FormUrlEncodedContent(new[]
                                    {
                                        new KeyValuePair<string, string>("idPedido", Application.Current.Properties["idPedido"].ToString()),
                                        new KeyValuePair<string, string>("direccion",direccionCompletaEnvio),
                                        new KeyValuePair<string, string>("referencia",referencia),
                                        new KeyValuePair<string, string>("importe",total.ToString())
                                    });
                                    var myHttpClient = new HttpClient();
                                    var authData = string.Format("{0}:{1}", "tiendaUAQ", "t13nd4U4q");
                                    var authHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(authData));
                                    myHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeaderValue);
                                    var response = await myHttpClient.PostAsync("http://189.211.201.181:88/TiendaUAQWebService/api/tblpedidos/compra", formContent);
                                    var json = await response.Content.ReadAsStringAsync();
                                    Debug.WriteLine(json);
                                    if (response.IsSuccessStatusCode)
                                    { 
                                        //var result2 = await Shipping
                                        Debug.WriteLine("\n\n\n ---------------- ");
                                        Debug.WriteLine(result.ServerResponse.Response.Id);
                                        cuerpoCorreo += "</tbody><tr><td style='color:#EC7063;'>Importe</td><td style='text-align:right'><b>$" + total + " MXN</b></td></tr></table><br><p>Dirección de envío:</p><p style='color:#EC7063;'>" + direccionCompletaEnvio + "</p><br><p style='color:gray;font-size:11px;'> *Este es un correo autom&aacute;tico, no es necesario responder.</p></html>";
                                        Debug.WriteLine(cuerpoCorreo);
                                        string nombreUsuario = Application.Current.Properties["nombre"] + " " + Application.Current.Properties["paterno"] + " " + Application.Current.Properties["materno"];
                                        string correoUsuarioX = Application.Current.Properties["usuario"].ToString();
                                        string respuestaCorreo = enviarCorreoCompra(correoUsuarioX, nombreUsuario, cuerpoCorreo);
                                        await DisplayAlert("Correcto", "Se realizó el pago correctamente. " + respuestaCorreo, "Aceptar");
                                        if (Application.Current.Properties.ContainsKey("idPedido"))
                                        {
                                            Application.Current.Properties.Remove("idPedido");//Primero lo debe eliminar en caso de que existe para que en la validacion si existe pedido lo agregue    
                                        }
                                        //Se debe de actualizar el pedido y cada uno de los artículos se debe disminuir el número de existencias
                                        await Navigation.PushAsync(new Carrito());
                                    } else{
                                        await DisplayAlert("Error", "No se realizó el pago correctamente el pago, sin embargo se realizó el cobro. Verifique con el administrador.s", "Aceptar");
                                    }
                                
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
            } else{
                await DisplayAlert("Error", "No existe pedido en la BD.", "Aceptar");   
            }
        }

        async void Cancelar_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PopAsync();
        }

        string enviarCorreoCompra(string correoDestino, string nombreUsuario, string cuerpoCorreo)
        {
            string mensaje = "";
            try
            {
                new SmtpClient
                {
                    Host = "Smtp.Gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    Timeout = 10000,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential("tiendauaq@gmail.com", "t13nd4u4q")
                }.Send(new MailMessage { From = new MailAddress(correoDestino, nombreUsuario), To = { correoDestino }, Subject = "Compra realizada en la Tienda UAQ", Body = cuerpoCorreo, IsBodyHtml = true, BodyEncoding = Encoding.UTF8 });
                mensaje = "Se envió el detalle de su compra a su correo electrónico.";
            }
            catch (Exception ex)
            {
                mensaje = "Falló al enviar el mensaje a su correo electrónico. " + ex.Message;
            }
            return mensaje;
        }
    }
}
