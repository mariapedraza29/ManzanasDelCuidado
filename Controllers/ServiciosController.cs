using ManzanasDelCuidado.Modelos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace ManzanasDelCuidado.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiciosController : ControllerBase
    {
        private readonly string cadenaSql;
        public ServiciosController(IConfiguration config)
        {
            cadenaSql = config.GetConnectionString("CadenaSQL");
        }

        [HttpGet]
        [Route("Listar")]
        public IActionResult listar()
        {
            List<Servicios> lista = new List<Servicios>();
            try
            {
                using (var conexion = new SqlConnection(cadenaSql))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_ListarServicios", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            lista.Add(new Servicios
                            {
                                codigoServ = Convert.ToInt32(rd["codigoServ"]),
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
        [Route("Obtener/{DocMujeres:int}")]
        public IActionResult obtener(int codigoServ)
        {
            List<Servicios> lista = new List<Servicios>();
            Servicios mujeres = new Servicios();
            try
            {
                using (var conexion = new SqlConnection(cadenaSql))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_ListarServicios", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            lista.Add(new Servicios
                            {
                                codigoServ = Convert.ToInt32(rd["codigoServ"]),
                                nombre = rd["nombre"].ToString(),
                                localidad = rd["localidad"].ToString(),
                                direccion = rd["direccion"].ToString(),
                                fkCodigoMunc = Convert.ToInt32(rd["fkCodigoMunc"])

                            });

                        }
                    }
                }
                mujeres = lista.Where(item => item.codigoServ == codigoServ).FirstOrDefault();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", Response = mujeres });

            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message, Response = mujeres });
            }
        }
        [HttpPost]
        [Route("registrar")]
        public IActionResult Registrar([FromBody] Servicios objeto)
        {
            try
            {
                using (var conexion = new SqlConnection(cadenaSql))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_registrarMujeres", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("codigoMunc", objeto.codigoServ);
                    cmd.Parameters.AddWithValue("nombre", objeto.nombre);
                    cmd.Parameters.AddWithValue("localidad", objeto.localidad);
                    cmd.Parameters.AddWithValue("direccion", objeto.direccion);
                    cmd.Parameters.AddWithValue("fkCodigoMunc", objeto.fkCodigoMunc);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "El Servicio ha sido registrado" });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });
            }
        }
        [HttpPut]
        [Route("Editar")]
        public IActionResult Editar([FromBody] Servicios objeto)
        {
            try
            {
                using (var conexion = new SqlConnection(cadenaSql))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_actualizarServicios", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("codigoMunc", objeto.codigoServ == 0 ? DBNull.Value : objeto.codigoServ);
                    cmd.Parameters.AddWithValue("nombre", objeto.nombre is null ? DBNull.Value : objeto.nombre);
                    cmd.Parameters.AddWithValue("localidad", objeto.localidad is null ? DBNull.Value : objeto.localidad);
                    cmd.Parameters.AddWithValue("direccion", objeto.direccion is null ? DBNull.Value : objeto.direccion);
                    cmd.Parameters.AddWithValue("fkCodigoMunc", objeto.fkCodigoMunc == 0 ? DBNull.Value : objeto.fkCodigoMunc);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "El Servicio ha sido actualizado" });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });
            }
        }
        [HttpPut]
        [Route("eliminar")]
        public IActionResult Editar(int codigoServ)
        {
            try
            {
                using (var conexion = new SqlConnection(cadenaSql))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_eliminarServicios", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("codigoServ", codigoServ);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "El servicio ha sido actualizado" });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });
            }
        }
    }
}
