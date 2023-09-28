using ManzanasDelCuidado.Modelos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace ManzanasDelCuidado.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //creamos el controlador ManzanaController donde estarán alamacenados el crud de cada uno de las clases
    public class ManzanasController : ControllerBase
    {
        private readonly string cadenaSql;
        public ManzanasController(IConfiguration config)
        {
            cadenaSql = config.GetConnectionString("CadenaSQL");
        }

        [HttpGet]
        [Route("Listar")]
        public IActionResult listar()
        {
            List<Manzanas> lista = new List<Manzanas>();
            try
            {
                using (var conexion = new SqlConnection(cadenaSql))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_ListarManzanas", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            lista.Add(new Manzanas
                            {
                                codigoManz = Convert.ToInt32(rd["codigoManz"]),
                                nombre = rd["nombre"].ToString(),
                                localidad = rd["localidad"].ToString(),
                                direccion = rd["direccion"].ToString(),
                                fkCodigoMunc = Convert.ToInt32(rd["fkCodigoMunc"])

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
        [Route("Obtener/{codigoManz:int}")]
        public IActionResult obtener(int codigoManz)
        {
            List<Manzanas> lista = new List<Manzanas>();
            Manzanas manzanas = new Manzanas();
            try
            {
                using (var conexion = new SqlConnection(cadenaSql))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_ListarManzanas", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            lista.Add(new Manzanas
                            {
                                codigoManz = Convert.ToInt32(rd["codigoManz"]),
                                nombre = rd["nombre"].ToString(),
                                localidad = rd["localidad"].ToString(),
                                direccion = rd["direccion"].ToString(),
                                fkCodigoMunc = Convert.ToInt32(rd["fkCodigoMunc"])

                            });

                        }
                    }
                }
                manzanas = lista.Where(item => item.codigoManz == codigoManz).FirstOrDefault();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", Response = manzanas });

            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message, Response = manzanas });
            }
        }
        [HttpPost]
        [Route("registrar")]
        public IActionResult Registrar([FromBody] Manzanas objeto)
        {
            try
            {
                using (var conexion = new SqlConnection(cadenaSql))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_registrarManzanas", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("codigoManz", objeto.codigoManz);
                    cmd.Parameters.AddWithValue("nombre", objeto.nombre);
                    cmd.Parameters.AddWithValue("localidad", objeto.localidad);
                    cmd.Parameters.AddWithValue("direccion", objeto.direccion);
                    cmd.Parameters.AddWithValue("fkCodigoMunc", objeto.fkCodigoMunc);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "La manzana ha sido registrada" });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });
            }
        }
        [HttpPut]
        [Route("Editar")]
        public IActionResult Editar([FromBody] Manzanas objeto)
        {
            try
            {
                using (var conexion = new SqlConnection(cadenaSql))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_actualizarManzanas", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("codigoManz", objeto.codigoManz == 0 ? DBNull.Value : objeto.codigoManz);
                    cmd.Parameters.AddWithValue("nombre", objeto.nombre is null ? DBNull.Value : objeto.nombre);
                    cmd.Parameters.AddWithValue("localidad", objeto.localidad is null ? DBNull.Value : objeto.localidad);
                    cmd.Parameters.AddWithValue("direccion", objeto.direccion is null ? DBNull.Value : objeto.direccion);
                    cmd.Parameters.AddWithValue("fkCodigoMunc", objeto.fkCodigoMunc == 0 ? DBNull.Value : objeto.fkCodigoMunc);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "la manzana ha sido actualizado" });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });
            }
        }
        [HttpPut]
        [Route("eliminar")]
        public IActionResult Eliminar(int codigoManz)
        {
            try
            {
                using (var conexion = new SqlConnection(cadenaSql))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_eliminarManzanas", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("codigoManz", codigoManz);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "la manzana ha sido eliminada" });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });
            }
        }
    }
}
