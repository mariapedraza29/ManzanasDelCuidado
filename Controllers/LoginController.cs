using ManzanasDelCuidado.Modelos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace ManzanasDelCuidado.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly string cadenaSql;
        public LoginController(IConfiguration config)
        {
            cadenaSql = config.GetConnectionString("CadenaSQL");
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login([FromBody] Mujeres mujer)
        {

            var conexion = new SqlConnection(cadenaSql);
            conexion.Open();
            try
            {

                var cmd = new SqlCommand("EXEC sp_Login @correo, @contraseña", conexion);
                cmd.Parameters.AddWithValue("@correo", mujer.correo);
                cmd.Parameters.AddWithValue("@contraseña", mujer.contrasena);
                DataTable dt = new();
                new SqlDataAdapter(cmd.ToString(), conexion).Fill(dt);
                return Ok(new Mujeres
                {
                    correo = dt.Rows[0]["correo"].ToString(),
                    DocMujeres = (int)dt.Rows[0]["DocMujeres"],
                    nombres = dt.Rows[0]["nombres"].ToString(),
                    apellidos = dt.Rows[0]["apellidos"].ToString(),
                    ocupacion = dt.Rows[0]["ocupacion"].ToString(),
                    telefono = dt.Rows[0]["telefono"].ToString(),
                    direccion = dt.Rows[0]["direccion"].ToString(),
                    foto = (byte)dt.Rows[0]["foto"]
                });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });
            }
            finally
            {
                conexion.Close();
            }
        }
    }
}

