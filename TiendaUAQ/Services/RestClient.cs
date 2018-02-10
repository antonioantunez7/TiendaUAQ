﻿using System;
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
    }
}
