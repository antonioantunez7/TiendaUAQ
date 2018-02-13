using System;
using System.Collections.Generic;

namespace TiendaUAQ.Models
{
    public class Productos
    {
        public Productos(){}
        public int idProducto { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public double precio { get; set; }
        public string url_imagen { get; set; }
        public int cveSubdepartamento { get; set; }
        public string activo { get; set; }
        public string fechaRegistro { get; set; }
        public string fechaActualizacion { get; set; }

        public int cantidad { get; set; }
        public string nombreCantidad { get; set; }
        public int idDetallePedido { get; set; }
    }

    public class ListaProductos{ public List<Productos> listaProductos { get; set; }}
}
