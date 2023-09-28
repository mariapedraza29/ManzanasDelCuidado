using ManzanasDelCuidado.Modelos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace ManzanasDelCuidado.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MunicipiosController : ControllerBase
    {
        private readonly string cadenaSql;
        public MunicipiosController(IConfiguration config)
        {
            cadenaSql = config.GetConnectionString("CadenaSQL");
        }

        [HttpGet]
        [Route("Listar")]
        public IActionResult listar()
        {
            List<Municipios> lista = new List<Municipios>();
            try
            {
                using (var conexion = new SqlConnection(cadenaSql))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_ListarMunicipios", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            lista.Add(new Municipios
                            {
                                codigoMunc = Convert.ToInt32(rd["CodigoMunc"]),
                                Nombre = rd["Nombre"].ToString()
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
        [Route("Obtener/{codigoMunc:int}")]
        public IActionResult obtener(int codigoMunc)
        {
            List<Municipios> lista = new List<Municipios>();
            Municipios municipios = new Municipios();
            try
            {
                using (var conexion = new SqlConnection(cadenaSql))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_ListarMunicipios", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            lista.Add(new Municipios
                            {
                                codigoMunc = Convert.ToInt32(rd["CodigoMunc"]),
                                Nombre = rd["Nombre"].ToString()

                            });

                        }
                    }
                }
                municipios = lista.Where(item => item.codigoMunc == codigoMunc).FirstOrDefault();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", Response = municipios });

            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message, Response = municipios });
            }
        }
        [HttpPost]
        [Route("registrar")]
        public IActionResult Registrar([FromBody] Municipios objeto )
        {
            try
            {
                using (var conexion = new SqlConnection(cadenaSql))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_registrarMunicipios", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("codigoMunc", objeto.codigoMunc);
                    cmd.Parameters.AddWithValue("nombre", objeto.Nombre);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "El municipio ha sido registrado" });
            }catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message});
            }
        }
        [HttpPut]
        [Route("Editar")]
        public IActionResult Editar([FromBody] Municipios objeto)
        {
            try
            {
                using (var conexion = new SqlConnection(cadenaSql))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_actualizarMunicipios", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("codigoMunc", objeto.codigoMunc == 0 ? DBNull.Value : objeto.codigoMunc);
                    cmd.Parameters.AddWithValue("nombre", objeto.Nombre is null ? DBNull.Value : objeto.Nombre);
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
        [HttpPut]
        [Route("eliminar")]
        public IActionResult Editar(int codigoMunc)
        {
            try
            {
                using (var conexion = new SqlConnection(cadenaSql))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_eliminarMunicipios", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("codigoMunc", codigoMunc);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "El municipio ha sido eliminado" });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });
            }
        }

    }
}
