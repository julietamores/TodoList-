using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace project_web.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly ILogger<UsuarioController> _logger;

        public UsuarioController(ILogger<UsuarioController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            LoginModel loginModel = HttpContext.Session.Get<LoginModel>(
                                "UsuarioLogueado");

            if(loginModel == null)
            {
                return Redirect("~/Home/Index");
            }

            return View();
        }

        public IActionResult Nuevo()
        {
            LoginModel loginModel = HttpContext.Session.Get<LoginModel>("UsuarioLogueado");

            if(loginModel == null)
            {
                return Redirect("~/Home/Index");
            }

            UsuarioViewModel usuarioViewModel = new UsuarioViewModel
            {
                apellidoPersona = "",
                id = 0,
                nombrePersona = "",
                nombreUsuario = "",
                accion = CodigosAccion.Nuevo
            };

            return View("~/Views/Usuario/Usuario.cshtml", usuarioViewModel);
        }

        public IActionResult Editar(long idUsuario)
        {
            transversal_library.IUserService userService = new UserService();
            userService.GetUser("", "");

            LoginModel loginModel = HttpContext.Session.Get<LoginModel>("UsuarioLogueado");

            if(loginModel == null)
            {
                return Redirect("~/Home/Index");
            }

            List<UsuarioModel> list = HttpContext.Session.Get<List<UsuarioModel>>("ListaUsuarios");

            if(list == null)
            {
                list = new List<UsuarioModel>();
            }

            UsuarioModel usuarioModel = list.Find(x => x.id == idUsuario);
            
            UsuarioViewModel usuarioViewModel = new UsuarioViewModel 
            {
                accion = CodigosAccion.Editar,
                apellidoPersona = usuarioModel.apellidoPersona,
                id = usuarioModel.id,
                nombrePersona = usuarioModel.nombrePersona,
                nombreUsuario = usuarioModel.nombreUsuario
            };

            return View("~/Views/Usuario/Usuario.cshtml", usuarioViewModel);
        }

        public IActionResult Ver(long idUsuario)
        {
            LoginModel loginModel = HttpContext.Session.Get<LoginModel>("UsuarioLogueado");

            if(loginModel == null)
            {
                return Redirect("~/Home/Index");
            }

            List<UsuarioModel> list = HttpContext.Session.Get<List<UsuarioModel>>("ListaUsuarios");

            if(list == null)
            {
                list = new List<UsuarioModel>();
            }

            UsuarioModel usuarioModel = list.Find(x => x.id == idUsuario);

            UsuarioViewModel usuarioViewModel = new UsuarioViewModel 
            {
                accion = CodigosAccion.Ver,
                apellidoPersona = usuarioModel.apellidoPersona,
                id = usuarioModel.id,
                nombrePersona = usuarioModel.nombrePersona,
                nombreUsuario = usuarioModel.nombreUsuario
            };

            return View("~/Views/Usuario/Usuario.cshtml", usuarioViewModel);
        }

        [HttpPost]
        public JsonResult Listar(QueryGridModel queryGridModel)
        {
            LoginModel loginModel = HttpContext.Session.Get<LoginModel>("UsuarioLogueado");

            if(loginModel == null)
            {
                return Json(Models.Common.JsonReturn.Redirect("Home/Index"));
            }

            try
            {
                long cantidadTotal = 0;
                List<UsuarioModel> listaUsarios = new List<UsuarioModel>();

                using (DAOFactory df = new DAOFactory())
                {
                    Ordenamiento ordenamiento = obtenerOrdenamientoUsuario(queryGridModel);
                    List<Asociacion> asociaciones = obtenerAsociacionesUsuario();
                    List<AtributoBusqueda> atributosBusqueda = obtenerAtributosBusquedaUsuario();

                    Paginado paginado = new Paginado
                    {
                        Comienzo = queryGridModel.start,
                        Cantidad = queryGridModel.length
                    };

                    IList<entity_library.Sistema.Usuario> usuarios = df.DAOUsuario.ObtenerListaUsuario(
                        queryGridModel.search != null ? queryGridModel.search.value : "",
                        atributosBusqueda,
                        paginado,
                        ordenamiento,
                        asociaciones,
                        out cantidadTotal);

                    foreach (entity_library.Sistema.Usuario usuario in usuarios)
                    {
                        listaUsarios.Add(new UsuarioModel
                        {
                            id = usuario.Id,
                            apellidoPersona = usuario.NombreCompleto,
                            nombrePersona = usuario.NombreCompleto,
                            nombreUsuario = usuario.NombreUsuario,
                            password = usuario.Password
                        });
                    }

                    return Json(JsonReturn.SuccessWithInnerObject(new
                    {
                        draw = queryGridModel.draw,
                        recordsFiltered = cantidadTotal,
                        recordsTotal = cantidadTotal,
                        data = listaUsarios
                    }));
                }
            }
            catch (System.Exception ex)
            {
                return Json(JsonReturn.ErrorWithSimpleMessage("Hubo un error"));
            }
        }

        private static List<AtributoBusqueda> obtenerAtributosBusquedaUsuario()
        {
            List<AtributoBusqueda> atributosBusqueda = new List<AtributoBusqueda>();

            atributosBusqueda.Add(new AtributoBusqueda
            {
                NombreAtributo = "Usuario.NombreUsuario",
                TipoDato = TipoDato.String
            });

            atributosBusqueda.Add(new AtributoBusqueda
            {
                NombreAtributo = "Usuario.NombreCompleto",
                TipoDato = TipoDato.String
            });

            return atributosBusqueda;
        }

        private static List<Asociacion> obtenerAsociacionesUsuario()
        {
            List<Asociacion> asociaciones = new List<Asociacion>();

            return asociaciones;
        }

        private static Ordenamiento obtenerOrdenamientoUsuario(
            QueryGridModel modeloConsulta)
        {
            Ordenamiento ordenamiento = new Ordenamiento
            {
                Atributo = "Usuario.NombreCompleto",
                Direccion = "asc"
            };

            if (modeloConsulta.order != null &&
                modeloConsulta.order.Count > 0)
            {
                int columnIndex = modeloConsulta.order[0].column;
                string col = modeloConsulta.columns[columnIndex].data;

                if (col == "nombrePersona") col = "Usuario.NombreCompleto";
                else if(col == "nombreUsuario") col = "Usuario.NombreUsuario";
                else col = "Usuario.NombreCompleto";

                ordenamiento.Atributo = col;
                ordenamiento.Direccion =
                    modeloConsulta.order[0].dir == DirectionModel.desc ? "desc" : "asc";
            }

            return ordenamiento;
        }


        [HttpPost]
        public JsonResult Guardar(UsuarioModel usuarioModel)
        {
            LoginModel loginModel = HttpContext.Session.Get<LoginModel>("UsuarioLogueado");

            if(loginModel == null)
            {
                return Json(Models.Common.JsonReturn.Redirect("Home/Index"));
            }

            try
            {
                using (DAOFactory daoFactory = new DAOFactory())
                {
                    entity_library.Sistema.Usuario usuario = daoFactory.DAOUsuario.ObtenerUsuario(usuarioModel.id);

                    if(usuario == null)
                    {
                        usuario = new entity_library.Sistema.Usuario();
                    }

                    usuario.NombreCompleto = usuarioModel.nombrePersona;
                    usuario.NombreUsuario = usuarioModel.nombreUsuario;
                    usuario.Password = usuarioModel.password;

                    daoFactory.BeginTrans();
                    daoFactory.DAOUsuario.Guardar(usuario);
                    daoFactory.Commit();

                    return Json(JsonReturn.SuccessWithoutInnerObject());
                }
            }
            catch (Exception ex)
            {
                return Json(JsonReturn.ErrorWithSimpleMessage("Hubo un error"));
            }
        }

        [HttpPost]
        public JsonResult Eliminar(long id)
        {
            LoginModel loginModel = HttpContext.Session.Get<LoginModel>("UsuarioLogueado");

            if(loginModel == null)
            {
                return Json(Models.Common.JsonReturn.Redirect("Home/Index"));
            }

            List<UsuarioModel> list = HttpContext.Session.Get<List<UsuarioModel>>("ListaUsuarios");

            if(list == null)
            {
                list = new List<UsuarioModel>();
            }

            UsuarioModel usuario = list.Find(x => x.id == id);
            
            if(usuario == null)
            {
                return Json(Models.Common.JsonReturn.ErrorWithSimpleMessage("El usuario que desea eliminar no existe más"));
            }
            
            list.Remove(usuario);
            
            HttpContext.Session.Set<List<UsuarioModel>>("ListaUsuarios", list);

            return Json(JsonReturn.SuccessWithoutInnerObject());
        }
    }
}
