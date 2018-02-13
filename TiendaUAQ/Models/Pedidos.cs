using System;
using System.Collections.Generic;

namespace TiendaUAQ.Models
{
    public class Pedidos
    {
        public Pedidos(){}
        public int idPedido { get; set; }
        public int idUsuario { get; set; }
        public int cveEstatus { get; set; }
        public string direccion { get; set; }
        public string referencia { get; set; }
        public double importe { get; set; }
        public List<Productos> detalle { get; set; }
    }

    public class ListaPedidos { public List<Pedidos> listaPedidos { get; set; } }
}
