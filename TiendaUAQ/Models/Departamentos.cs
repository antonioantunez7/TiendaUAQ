using System;
using System.Collections.Generic;

namespace TiendaUAQ.Models
{
    public class Departamentos
    {
        public Departamentos(){}
        public int cveDepartamento { get; set; }
        public string descDepartamento { get; set; }
        public string url_portada { get; set; }
        public string activo { get; set; }
        public string fechaRegistro { get; set; }
        public string fechaActualizacion { get; set; }
    }

    public class ListaDepartamentos{ public List<Departamentos> listaDepartamentos { get; set; }}
}
