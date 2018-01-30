using System;
using System.Collections.Generic;

namespace TiendaUAQ.Models
{
    public class Usuarios
    {
        public Usuarios(){}
        public int idUsuario { get; set; }
        public string nombre { get; set; }
        public string paterno { get; set; }
        public string materno { get; set; }
        public string usuario { get; set; }
        public string password { get; set; }
    }
    public class ListaUsuarios{public List<Usuarios> listaUsuarios { get; set; }}
}
