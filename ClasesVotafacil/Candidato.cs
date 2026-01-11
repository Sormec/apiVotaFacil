using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClasesVotafacil
{
    public class Candidato
    {
        public int Id { get; set; }
        public string? Presidente { get; set; }
        public string? Vicepresidente { get; set; }
        public string? Partido_Politico { get; set; }
        public int N_Votos { get; set; }
        public string? Transaccion { get; set; }
    }
}
