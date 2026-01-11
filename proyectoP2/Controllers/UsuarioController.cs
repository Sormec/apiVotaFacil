using proyectoP2.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Xml.Linq;
using System.Data;
using Newtonsoft.Json;
using ClasesVotafacil;

namespace proyectoP2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : Controller
    {
        private readonly IConfiguration Configuration;
        
        public UsuarioController(IConfiguration configuration) { Configuration = configuration; }

        [Route("[action]")]
        [HttpPost]
        //CONSULTA_USUARIO_LOGIN
        public async Task<ActionResult<Usuario>> GetLogin([FromBody] Usuario usuarios)
        {
            try
            {
                //1. SE CONECTA A LA BD
                var cadenaConexion = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build()
                .GetSection("ConnectionStrings")["Conexion"];
                //2. TRANSFORMAR LO QUE ME LLEGA DESDE EL BODY A UN FORMATO XML 
                XDocument xmlParam = DBXmlMethods.GetXml(usuarios); 
                //SE CONECTA AL PROCEDIMIENTO ALMACENADO Y LA BD LE DEVUELVE EL RESULTADO DEL PROCEDIMIENTO ALMACENADO
                DataSet resultado = await DBXmlMethods.EjecutaBase(ProcedimientosBD.consultaUsuario, cadenaConexion, usuarios.Transaccion,
                    xmlParam.ToString());
                List<Usuario> listData = new List<Usuario>();
                if(resultado.Tables.Count > 0)
                {
                    try
                    {
                        if (resultado.Tables[0].Rows.Count > 0)//si encuentra el usuario devuelve la tabla
                        {
                            Usuario userterm = new Usuario();
                            userterm.Id = Convert.ToInt32(resultado.Tables[0].Rows[0]["id"]);
                            userterm.Cedula = resultado.Tables[0].Rows[0]["cedula"].ToString();
                            //el token se transforma en un json para ser enviado al html
                            return Ok(JsonConvert.SerializeObject(CrearToken(userterm)));
                        }
                        else
                        {
                            MensajeResultado objresponse = new MensajeResultado();
                            objresponse.Respuesta = "ERROR!";
                            objresponse.Leyenda = "Error en las credenciales de acceso";
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write("error" + ex.Message);
                    }
                }
                return Ok();
            }
            catch (Exception ex)
            {
                Console.Write("error" + ex.Message);
                return StatusCode(500); //retorno status 500 si se produce una excepcion
            }
        }
        //esta funcion realiza el proceso de crear un token cada vez que un usuario ingrese al sistema
        private string CrearToken(Usuario usuario)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.Cedula),
            };
            //la clave privada
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(Configuration.GetSection("AppSettings:Token").Value));
            //genera la credencial de acceso con la clave privada y se firma con el Sha de 512 bits
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),//un dia durara el token
                SigningCredentials = creds
            };
            var tokenHandlet = new JwtSecurityTokenHandler();
            var token = tokenHandlet.CreateToken(tokenDescriptor);//el Jwt crea el token con la info del argumento

            return tokenHandlet.WriteToken(token);
        }
    }
}
