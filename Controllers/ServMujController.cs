using ManzanasDelCuidado.Modelos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace ManzanasDelCuidado.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServMujController : ControllerBase
    {
        private readonly string cadenaSql;
        public ServMujController(IConfiguration config)
        {
            cadenaSql = config.GetConnectionString("CadenaSQL");
        }

        [HttpGet]
        [Route("Listar")]
        public IActionResult listar()
        {
            List<ServMuj> lista = new List<ServMuj>();
            try
            {
                using (var conexion = new SqlConnection(cadenaSql))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_ListarServMuj", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            lista.Add(new ServMuj
                            {
                                fechaInicio = Convert.ToDateTime(rd["fechaInicio"]),
                                fechaFinal = rd["fechaFinal"].ToString(),
                                documentoPdf = Convert.ToByte(rd["documentoPdf"]),
                                fkDocMujeres = Convert.ToInt32(rd["fkDocMujeres"]),
                                fkCodigoServ1 = Convert.ToInt32(rd["fkCodigoMunc"])
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
        public IActionResult obtener(int fkCodigoServ1, int fkDocMujeres)
        {
            List<ServMuj> lista = new List<ServMuj>();
            ServMuj servMuj = new ServMuj();
            try
            {
                using (var conexion = new SqlConnection(cadenaSql))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_ListarServMuj", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            lista.Add(new ServMuj
                            {
                                fechaInicio = Convert.ToDateTime(rd["fechaInicio"]),
                                fechaFinal = rd["fechaFinal"].ToString(),
                                documentoPdf = Convert.ToByte(rd["documentoPdf"]),
                                fkDocMujeres = Convert.ToInt32(rd["fkDocMujeres"]),
                                fkCodigoServ1 = Convert.ToInt32(rd["fkCodigoServ1"])

                            });

                        }
                    }
                }
                servMuj = lista.Where(item => item.fkCodigoServ1 == fkCodigoServ1 && item.fkDocMujeres == fkDocMujeres).FirstOrDefault();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", Response = servMuj });

            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message, Response = servMuj });
            }
        }
        [HttpPost]
        [Route("registrar")]
        public IActionResult Registrar([FromBody] ServMuj objeto)
        {
            try
            {
                using (var conexion = new SqlConnection(cadenaSql))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_registrarServMuj", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("fechaInicio", objeto.fechaInicio);
                    cmd.Parameters.AddWithValue("fechaFinal", objeto.fechaFinal);
                    cmd.Parameters.AddWithValue("documentoPdf", objeto.documentoPdf);
                    cmd.Parameters.AddWithValue("fkDocMujeres", objeto.fkDocMujeres);
                    cmd.Parameters.AddWithValue("fkCodigoServ1", objeto.fkCodigoServ1);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "registrado" });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });
            }
        }
        [HttpPut]
        [Route("Editar")]
        public IActionResult Editar([FromBody] ServMuj objeto)
        {
            try
            {
                using (var conexion = new SqlConnection(cadenaSql))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_actualizarServMuj", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("fechaInicio", objeto.fechaInicio == default ? DBNull.Value : objeto.fechaInicio);
                    cmd.Parameters.AddWithValue("nombre", objeto.fechaFinal is null ? DBNull.Value : objeto.fechaFinal);
                    cmd.Parameters.AddWithValue("localidad", objeto.documentoPdf == default ? DBNull.Value : objeto.documentoPdf);
                    cmd.Parameters.AddWithValue("direccion", objeto.fkDocMujeres == 0 ? DBNull.Value : objeto.fkDocMujeres);
                    cmd.Parameters.AddWithValue("fkCodigoMunc", objeto.fkCodigoServ1 == 0 ? DBNull.Value : objeto.fkCodigoServ1);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "actualizado" });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });
            }
        }
        [HttpPut]
        [Route("eliminar")]
        public IActionResult Editar(int fkDocMujeres, int fkCodigoServ1)
        {
            try
            {
                using (var conexion = new SqlConnection(cadenaSql))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_eliminarServMuj", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("codigoServ", fkDocMujeres);
                    cmd.Parameters.AddWithValue("codigoServ", fkCodigoServ1);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Eliminado" });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });
            }
        }
    }
}
