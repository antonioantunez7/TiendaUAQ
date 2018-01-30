using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace TiendaUAQ.Services
{
    public class RestClient
    {
        public async Task<T> Get<T>(string url)
        {
            try
            {
                HttpClient cliente = new HttpClient();
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

        public async Task<T> GetProductos<T>(string url)
        {
            try
            {
                HttpClient cliente = new HttpClient();
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
    }
}
