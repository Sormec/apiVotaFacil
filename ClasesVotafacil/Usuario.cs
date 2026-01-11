using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClasesVotafacil
{
    public class Usuario
    {
        public int Id { get; set; }
        public string? Cedula { get; set; }
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public string? Password { get; set; }
        public string Transaccion { get; set; }
    }
}
