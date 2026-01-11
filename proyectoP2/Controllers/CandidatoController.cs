using ClasesVotafacil;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Xml.Linq;

namespace proyectoP2.Controllers
{
    //CONSULTA_CANDIDATO_ALL
    //CONSULTA_CANDIDATO_GRAFICA
    //CONSULTA_CANDIDATO_ID
    //CONTAR_VOTO
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CandidatoController : Controller
    {
        //esto indica que el método del controlador se asociará a una ruta que coincide con el nombre del método
        [Route("[action]")]
        [HttpPost]
        public async Task<ActionResult<Candidato>> GetCandidatoAll(Candidato can)
        {
            //1. SE CONECTA A LA BD
            var cadenaConexion = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build()
                .GetSection("ConnectionStrings")["Conexion"];
            //2. TRANSFORMAR LO QUE ME LLEGA DESDE EL BODY A UN FORMATO XML
            //en 'pr' esta la consulta que se obtiene desde el front
            XDocument xmlParam = Shared.DBXmlMethods.GetXml(can);
            //3. SE CONECTA AL PROCEDIMIENTO ALMACENADO Y LA BD LE DEVUELVE EL RESULTADO DEL PROCEDIMIENTO ALMACENADO
            //PARA QUE SEA DEVUELTO AL FRONT-END
            DataSet dsResultado = await Shared.DBXmlMethods.EjecutaBase(Shared.ProcedimientosBD.consultaCandidato, cadenaConexion,
                can.Transaccion, xmlParam.ToString());
            //RECORRER EL DATASET PARA LLEVARLO A FORMATO ADECUADO Y ENVIARLO AL FRONT-END
            List<Candidato> listCandidato = new List<Candidato>();
            if (dsResultado.Tables.Count > 0)
            {
                try
                {   //bucle para ubicar las filas de la primera tabla del procedimiento almacenado
                    foreach (DataRow row in dsResultado.Tables[0].Rows)
                    {
                        Candidato objResponse = new Candidato
                        {
                            Id = Convert.ToInt32(row["id"]),
                            Presidente = row["presidente"].ToString(),
                            Vicepresidente = row["vicepresidente"].ToString(),
                            Partido_Politico = row["partido_politico"].ToString(),
                            N_Votos = Convert.ToInt32(row["n_votos"])
                        };
                        listCandidato.Add(objResponse);
                    }
                }
                catch (Exception ex)
                {
                    Console.Write(ex.ToString());
                }
            }
            Console.WriteLine("Proceso Terminado");
            return Ok(listCandidato);
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ActionResult<Candidato>> GetDataGrafica(Candidato can)
        {
            //1. SE CONECTA A LA BD
            var cadenaConexion = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build()
                .GetSection("ConnectionStrings")["Conexion"];
            //2. TRANSFORMAR LO QUE ME LLEGA DESDE EL BODY A UN FORMATO XML
            //en 'pr' esta la consulta que se obtiene desde el front
            XDocument xmlParam = Shared.DBXmlMethods.GetXml(can);
            //3. SE CONECTA AL PROCEDIMIENTO ALMACENADO Y LA BD LE DEVUELVE EL RESULTADO DEL PROCEDIMIENTO ALMACENADO
            //PARA QUE SEA DEVUELTO AL FRONT-END
            DataSet dsResultado = await Shared.DBXmlMethods.EjecutaBase(Shared.ProcedimientosBD.consultaCandidato, cadenaConexion,
                can.Transaccion, xmlParam.ToString());
            //RECORRER EL DATASET PARA LLEVARLO A FORMATO ADECUADO Y ENVIARLO AL FRONT-END
            List<CandidatoGrafica> listCandidato = new List<CandidatoGrafica>();
            if (dsResultado.Tables.Count > 0)
            {
                try
                {   //bucle para ubicar las filas de la primera tabla del procedimiento almacenado
                    foreach (DataRow row in dsResultado.Tables[0].Rows)
                    {
                        CandidatoGrafica objResponse = new CandidatoGrafica
                        {
                            name = row["presidente"].ToString(),
                            value = Convert.ToInt32(row["n_votos"])
                        };
                        listCandidato.Add(objResponse);
                    }
                }
                catch (Exception ex)
                {
                    Console.Write(ex.ToString());
                }
            }
            Console.WriteLine("Proceso Terminado");
            return Ok(listCandidato);
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ActionResult<MensajeResultado>> SetCandidato(Candidato can)
        {
            //1. SE CONECTA A LA BD
            var cadenaConexion = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build()
                .GetSection("ConnectionStrings")["Conexion"];

            //2. TRANSFORMAR LO QUE ME LLEGA DESDE EL BODY A UN FORMATO XML
            XDocument xmlParam = Shared.DBXmlMethods.GetXml(can);
            //3. SE CONECTA AL PROCEDIMIENTO ALMACENADO Y LA BD LE DEVUELVE EL RESULTADO DEL PROCEDIMIENTO ALMACENADO
            //PARA QUE SEA DEVUELTO AL FRONT-END
            DataSet dsResultado = await Shared.DBXmlMethods.EjecutaBase(Shared.ProcedimientosBD.setCandidato, cadenaConexion,
                can.Transaccion, xmlParam.ToString());
            List<MensajeResultado> listMensajeResultado = new List<MensajeResultado>();
            if (dsResultado.Tables.Count > 0)
            {
                try
                {
                    foreach (DataRow row in dsResultado.Tables[0].Rows)
                    {
                        MensajeResultado objResponse = new MensajeResultado
                        {
                            Respuesta = row["respuesta"].ToString(),
                            Leyenda = row["leyenda"].ToString()
                        };
                        listMensajeResultado.Add(objResponse);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            return Ok(listMensajeResultado);
        }

    }
}
