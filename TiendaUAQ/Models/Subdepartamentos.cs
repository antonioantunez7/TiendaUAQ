using System;
using System.Collections.Generic;

namespace TiendaUAQ.Models
{
    public class Subdepartamentos
    {
        public Subdepartamentos(){}
        public int cveSubdepartamento { get; set; }
        public string descSubdepartamento { get; set; }
        public int cveDepartamento { get; set; }
        public string activo { get; set; }
        public string fechaRegistro { get; set; }
        public string fechaActualizacion { get; set; }

    }

    public class ListaSubdepartamentos{ public List<Subdepartamentos> listaSubdepartamentos { get; set; }}
}
