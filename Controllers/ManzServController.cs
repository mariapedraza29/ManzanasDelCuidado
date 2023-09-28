using ManzanasDelCuidado.Modelos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace ManzanasDelCuidado.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManzServController : ControllerBase
    {
            private readonly string cadenaSql;
            public ManzServController(IConfiguration config)
            {
                cadenaSql = config.GetConnectionString("CadenaSQL");
            }

            [HttpGet]
            [Route("Listar")]
            public IActionResult listar()
            {
                List<ManzServ> lista = new List<ManzServ>();
                try
                {
                    using (var conexion = new SqlConnection(cadenaSql))
                    {
                        conexion.Open();
                        var cmd = new SqlCommand("sp_ListarManzServ", conexion);
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (var rd = cmd.ExecuteReader())
                        {
                            while (rd.Read())
                            {
                                lista.Add(new ManzServ
                                {
                                    fkCodigoServ = Convert.ToInt32(rd["fkCodigoServ"]),
                                    fkCodigoManz = Convert.ToInt32(rd["fkCodigoManz"])
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
            public IActionResult obtener(int fkCodigoServ, int fkCodigoManz)
            {
                List<ManzServ> lista = new List<ManzServ>();
                ManzServ municipios = new ManzServ();
                try
                {
                    using (var conexion = new SqlConnection(cadenaSql))
                    {
                        conexion.Open();
                        var cmd = new SqlCommand("sp_ListarManzServ", conexion);
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (var rd = cmd.ExecuteReader())
                        {
                            while (rd.Read())
                            {
                                lista.Add(new ManzServ
                                {
                                    fkCodigoManz = Convert.ToInt32(rd["fkCodigoManz"]),
                                    fkCodigoServ = Convert.ToInt32(rd["fkCodigoServ"])

                                });

                            }
                        }
                    }
                    municipios = lista.Where(item => item.fkCodigoServ== fkCodigoServ && item.fkCodigoManz == fkCodigoManz).FirstOrDefault();
                    return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", Response = municipios });

                }
                catch (Exception error)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message, Response = municipios });
                }
            }
            [HttpPost]
            [Route("registrar")]
            public IActionResult Registrar([FromBody] ManzServ objeto)
            {
                try
                {
                    using (var conexion = new SqlConnection(cadenaSql))
                    {
                        conexion.Open();
                        var cmd = new SqlCommand("sp_registrarManzServ", conexion);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("fkCodigoManz", objeto.fkCodigoManz);
                        cmd.Parameters.AddWithValue("fkCodigoServ", objeto.fkCodigoServ);
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
                [Route("eliminar")]
                public IActionResult Editar(int fkCodigoManz, int fkCodigoServ)
                {
                    try
                    {
                        using (var conexion = new SqlConnection(cadenaSql))
                        {
                            conexion.Open();
                            var cmd = new SqlCommand("sp_eliminarManzServ", conexion);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("fkCodigoManz", fkCodigoManz);
                            cmd.Parameters.AddWithValue("fkCodigoServ", fkCodigoServ);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.ExecuteNonQuery();
                        }
                        return StatusCode(StatusCodes.Status200OK, new { mensaje = "eliminado" });
                    }
                    catch (Exception error)
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });
                    }
                }
            }
    }
