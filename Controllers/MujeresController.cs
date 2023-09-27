using ManzanasDelCuidado.Modelos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace ManzanasDelCuidado.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MujeresController : ControllerBase
    {
        private readonly string cadenaSql;
        public MujeresController(IConfiguration config)
        {
            cadenaSql = config.GetConnectionString("CadenaSQL");
        }

        [HttpGet]
        [Route("Listar")]
        public IActionResult listar()
        {
            List<Mujeres> lista = new List<Mujeres>();
            try
            {
                using (var conexion = new SqlConnection(cadenaSql))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_ListarMujeres", conexion);
                    cmd.CommandType =CommandType.StoredProcedure;
                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            lista.Add(new Mujeres
                            {
                                DocMujeres = Convert.ToInt32(rd["CodigoMunc"]),
                                nombres = rd["Nombre"].ToString(),
                                apellidos = rd["apellidos"].ToString(),
                                correo = rd["correo"].ToString(),
                                ocupacion = rd["ocupacion"].ToString(),
                                telefono = rd["telefono"].ToString(),
                                direccion = rd["direccion"].ToString(),
                                foto = Convert.ToByte(rd["foto"])

                            });

                        }
                    }
                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", Response = lista });

            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message, Response = lista });
            }
        }
        [HttpGet]
        [Route("Obtener/{DocMujeres:int}")]
        public IActionResult obtener(int DocMujeres)
        {
            List<Mujeres> lista = new List<Mujeres>();
            Mujeres mujeres = new Mujeres();
            try
            {
                using (var conexion = new SqlConnection(cadenaSql))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_ListarMujeres", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            lista.Add(new Mujeres
                            {
                                DocMujeres = Convert.ToInt32(rd["DocMujeres"]),
                                nombres = rd["Nombre"].ToString(),
                                apellidos = rd["apellidos"].ToString(),
                                correo = rd["correo"].ToString(),
                                ocupacion = rd["ocupacion"].ToString(),
                                telefono = rd["telefono"].ToString(),
                                direccion = rd["direccion"].ToString(),
                                foto = Convert.ToByte(rd["foto"])

                            });

                        }
                    }
                }
                mujeres = lista.Where(item => item.DocMujeres == DocMujeres).FirstOrDefault();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", Response = mujeres });

            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message, Response = mujeres });
            }
        }
        [HttpPost]
        [Route("registrar")]
        public IActionResult Registrar([FromBody] Mujeres objeto)
        {
            try
            {
                using (var conexion = new SqlConnection(cadenaSql))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_registrarMujeres", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("codigoMunc", objeto.DocMujeres);
                    cmd.Parameters.AddWithValue("nombres", objeto.nombres);
                    cmd.Parameters.AddWithValue("apellidos", objeto.apellidos);
                    cmd.Parameters.AddWithValue("correo", objeto.correo);
                    cmd.Parameters.AddWithValue("ocupacion", objeto.ocupacion);
                    cmd.Parameters.AddWithValue("telefono", objeto.telefono);
                    cmd.Parameters.AddWithValue("direccion", objeto.direccion);
                    cmd.Parameters.AddWithValue("foto", objeto.foto);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "El usuario ha sido registrado" });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });
            }
        }
        [HttpPut]
        [Route("Editar")]
        public IActionResult Editar([FromBody] Mujeres objeto)
        {
            try
            {
                using (var conexion = new SqlConnection(cadenaSql))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_actualizarMujeres", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("codigoMunc", objeto.DocMujeres == 0 ? DBNull.Value : objeto.DocMujeres);
                    cmd.Parameters.AddWithValue("nombres", objeto.nombres is null ? DBNull.Value : objeto.nombres);
                    cmd.Parameters.AddWithValue("apellidos", objeto.apellidos is null ? DBNull.Value : objeto.apellidos);
                    cmd.Parameters.AddWithValue("correo", objeto.correo is null ? DBNull.Value : objeto.correo);
                    cmd.Parameters.AddWithValue("ocupacion", objeto.ocupacion is null ? DBNull.Value : objeto.ocupacion);
                    cmd.Parameters.AddWithValue("telefono", objeto.telefono is null ? DBNull.Value : objeto.telefono);
                    cmd.Parameters.AddWithValue("direccion", objeto.direccion is null ? DBNull.Value : objeto.direccion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "El usuario ha sido actualizado" });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });
            }
        }
        [HttpPut]
        [Route("eliminar")]
        public IActionResult Editar(int DocMujeres)
        {
            try
            {
                using (var conexion = new SqlConnection(cadenaSql))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_eliminarMujeres", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("codigoMunc", DocMujeres);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "El municipio ha sido actualizado" });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });
            }
        }

    
    }
}
