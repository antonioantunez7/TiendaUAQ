using System;
using System.Collections.Generic;

namespace TiendaUAQ.Models
{
    public class DetallePedido
    {
        public DetallePedido() { }
        public int idDetallePedido { get; set; }
        public int idPedido { get; set; }
        public int idProducto { get; set; }
        public int cantidad { get; set; }
        public double precio { get; set; }
    }

    public class ListaDetallePedido { public List<DetallePedido> listaDetallePedido { get; set; } }
}
