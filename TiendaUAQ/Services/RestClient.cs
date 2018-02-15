using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace TiendaUAQ.Services
{
    public class RestClient
    {
        public async Task<T> GetDepartamentos<T>(string url)
        {
            try
            {
                HttpClient cliente = new HttpClient();
                var authData = string.Format("{0}:{1}", "tiendaUAQ", "t13nd4U4q");
                var authHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(authData));
                cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeaderValue);
                var respuesta = await cliente.GetAsync(url);
                Debug.Write(respuesta);
                if (respuesta.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var jsonRespuesta = await respuesta.Content.ReadAsStringAsync();
                    var jsonArmado = "{'listaDepartamentos':" + jsonRespuesta + "}";
                    Debug.WriteLine(jsonArmado);
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonArmado);
                }
                else
                {
                    var jsonArmado = "{'listaDepartamentos':[]}";
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonArmado);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\nOcurrio un error en la funcion Get del Task");
                Debug.WriteLine(ex);
            }
            return default(T);
        }

        public async Task<T> GetSubdepartamentos<T>(string url)
        {
            try
            {
                HttpClient cliente = new HttpClient();
                var authData = string.Format("{0}:{1}", "tiendaUAQ", "t13nd4U4q");
                var authHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(authData));
                cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeaderValue);
                var respuesta = await cliente.GetAsync(url);
                Debug.Write(respuesta);
                if (respuesta.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var jsonRespuesta = await respuesta.Content.ReadAsStringAsync();
                    var jsonArmado = "{'listaSubdepartamentos':" + jsonRespuesta + "}";
                    Debug.WriteLine(jsonArmado);
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonArmado);
                }
                else
                {
                    var jsonArmado = "{'listaSubdepartamentos':[]}";
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonArmado);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\nOcurrio un error en la funcion Get del Task");
                Debug.WriteLine(ex);
            }
            return default(T);
        }

        public async Task<T> GetProductos<T>(string url)
        {
            try
            {
                Debug.Write(url);
                HttpClient cliente = new HttpClient();
                var authData = string.Format("{0}:{1}", "tiendaUAQ", "t13nd4U4q");
                var authHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(authData));
                cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeaderValue);
                var respuesta = await cliente.GetAsync(url);
                Debug.Write(respuesta);
                if (respuesta.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var jsonRespuesta = await respuesta.Content.ReadAsStringAsync();
                    var jsonArmado = "{'listaProductos':" + jsonRespuesta + "}";
                    Debug.WriteLine(jsonArmado);
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonArmado);
                }
                else
                {
                    var jsonArmado = "{'listaProductos':[]}";
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonArmado);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\nOcurrio un error en la funcion Get del Task");
                Debug.WriteLine(ex);
            }
            return default(T);
        }

        public async Task<T> GetPedidos<T>(string url)
        {
            try
            {
                HttpClient cliente = new HttpClient();
                var authData = string.Format("{0}:{1}", "tiendaUAQ", "t13nd4U4q");
                var authHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(authData));
                cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeaderValue);
                var respuesta = await cliente.GetAsync(url);
                Debug.Write(respuesta);
                Debug.Write("*****");
                Debug.WriteLine(url);
                Debug.Write("*****");
                if (respuesta.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var jsonRespuesta = await respuesta.Content.ReadAsStringAsync();
                    if(jsonRespuesta.Contains("idProducto")){
                        return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonRespuesta);
                    } else{
                        var jsonArmado = "{'idPedido':'0'}";
                        return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonArmado);
                    }
                }
                else
                {
                    var jsonArmado = "{'idPedido':'0'}";
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonArmado);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\nOcurrio un error en la funcion Get del Task");
                Debug.WriteLine(ex);
            }
            return default(T);
        }

        public async Task<T> GetDetallePedido<T>(string url)
        {
            try
            {
                HttpClient cliente = new HttpClient();
                var authData = string.Format("{0}:{1}", "tiendaUAQ", "t13nd4U4q");
                var authHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(authData));
                cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeaderValue);
                var respuesta = await cliente.GetAsync(url);
                if (respuesta.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var jsonRespuesta = await respuesta.Content.ReadAsStringAsync();
                    if (jsonRespuesta.Contains("idDetallePedido"))
                    {
                        return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonRespuesta);
                    }
                    else
                    {
                        var jsonArmado = "{'idDetallePedido':'0'}";
                        return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonArmado);
                    }
                }
                else
                {
                    var jsonArmado = "{'idDetallePedido':'0'}";
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonArmado);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\nOcurrio un error en la funcion Get del Task");
                Debug.WriteLine(ex);
            }
            return default(T);
        }

        public async Task<T> convertirJson<T>(string json)
        {
            try
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\nOcurrio un error en la funcion Get del Task");
                Debug.WriteLine(ex);
            }
            return default(T);
        }

        public async Task<T> GetUsuarios<T>(string url)
        {
            try
            {
                HttpClient cliente = new HttpClient();
                var authData = string.Format("{0}:{1}", "tiendaUAQ", "t13nd4U4q");
                var authHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(authData));
                cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeaderValue);
                var respuesta = await cliente.GetAsync(url);
                if (respuesta.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var jsonRespuesta = await respuesta.Content.ReadAsStringAsync();
                    if (jsonRespuesta.Contains("idUsuario"))
                    {
                        return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonRespuesta);
                    }
                    else
                    {
                        var jsonArmado = "{'idUsuario':'0'}";
                        return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonArmado);
                    }
                }
                else
                {
                    var jsonArmado = "{'idUsuario':'0'}";
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonArmado);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\nOcurrio un error en la funcion Get del Task");
                Debug.WriteLine(ex);
            }
            return default(T);
        }

        public async Task<T> GetPrductosId<T>(string url)
        {
            try
            {
                HttpClient cliente = new HttpClient();
                var authData = string.Format("{0}:{1}", "tiendaUAQ", "t13nd4U4q");
                var authHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(authData));
                cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeaderValue);
                var respuesta = await cliente.GetAsync(url);
                if (respuesta.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var jsonRespuesta = await respuesta.Content.ReadAsStringAsync();
                    if (jsonRespuesta.Contains("idProducto"))
                    {
                        return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonRespuesta);
                    }
                    else
                    {
                        var jsonArmado = "{'idProducto':'0'}";
                        return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonArmado);
                    }
                }
                else
                {
                    var jsonArmado = "{'idProducto':'0'}";
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonArmado);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\nOcurrio un error en la funcion Get del Task");
                Debug.WriteLine(ex);
            }
            return default(T);
        }
    }
}
