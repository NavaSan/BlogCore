using BlogCore.AccesoDatos.Data.Repositories.IRepository;
using BlogCore.Data;
using BlogCore.Models;
using BlogCore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;


namespace BlogCore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ArticulosController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ArticulosController(IContenedorTrabajo contenedorTrabajo, IWebHostEnvironment webHostEnvironment)
        {
            _contenedorTrabajo = contenedorTrabajo;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            ArticuloVM ariculoVm = new ArticuloVM()
            {
                Articulo = new BlogCore.Models.Articulo(),
                ListaCategoria = _contenedorTrabajo.CategoriaRepository.GetListCategorias()
            };

            return View(ariculoVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ArticuloVM articuloVM)
        {
           if(ModelState.IsValid)
            {
                string rutaPrincipal = _webHostEnvironment.WebRootPath;
                var archivos = HttpContext.Request.Form.Files;

                if(articuloVM.Articulo.Id == 0)
                { 
                    string nombreArchivo = Guid.NewGuid().ToString();
                    var subidas = Path.Combine(rutaPrincipal, @"img/articulos");
                    var extencion = Path.GetExtension(archivos[0].FileName);

                    using(var fileStreams = new FileStream(Path.Combine(subidas, nombreArchivo + extencion), FileMode.Create))
                    {
                        archivos[0].CopyTo(fileStreams);
                    }

                    articuloVM.Articulo.UrlImagen = @"\img\articulos\" + nombreArchivo + extencion;
                    articuloVM.Articulo.FechaCreacion = DateTime.Now.ToString();

                    _contenedorTrabajo.ArticuloRepository.Add(articuloVM.Articulo);
                    _contenedorTrabajo.Save();

                    return RedirectToAction(nameof(Index));
                }
            }
            articuloVM.ListaCategoria = _contenedorTrabajo.CategoriaRepository.GetListCategorias();
            return View(articuloVM);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            ArticuloVM articuloVM = new ArticuloVM()
            {
                Articulo = new BlogCore.Models.Articulo(),
                ListaCategoria = _contenedorTrabajo.CategoriaRepository.GetListCategorias()
            };

            if(id != null)
            {
                articuloVM.Articulo = _contenedorTrabajo.ArticuloRepository.Get(id.GetValueOrDefault());
            }

            return View(articuloVM);
        }

        [HttpPost]
        public IActionResult Edit(ArticuloVM articuloVM)
        {
            if (ModelState.IsValid)
            {
                string rutaPrincipal = _webHostEnvironment.WebRootPath;
                var archivos = HttpContext.Request.Form.Files;
                var articulosDesdeDb = _contenedorTrabajo.ArticuloRepository.Get(articuloVM.Articulo.Id);

                if (archivos.Count() > 0)
                {
                    string nombreArchivo = Guid.NewGuid().ToString();
                    var subidas = Path.Combine(rutaPrincipal, @"img/articulos");
                    var extencion = Path.GetExtension(archivos[0].FileName);
                    var nuevaExtncion = Path.GetExtension(archivos[0].FileName);

                    var rutaImagen = Path.Combine(rutaPrincipal, articulosDesdeDb.UrlImagen.TrimStart('\\'));

                    if (System.IO.File.Exists(rutaImagen))
                    {
                        System.IO.File.Delete(rutaImagen);
                    }

                    using (var fileStreams = new FileStream(Path.Combine(subidas, nombreArchivo + extencion), FileMode.Create))
                    {
                        archivos[0].CopyTo(fileStreams);
                    }

                    articuloVM.Articulo.UrlImagen = @"\img\articulos\" + nombreArchivo + extencion;
                    articuloVM.Articulo.FechaCreacion = DateTime.Now.ToString();

                    _contenedorTrabajo.ArticuloRepository.Update(articuloVM.Articulo);
                    _contenedorTrabajo.Save();

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    articuloVM.Articulo.UrlImagen = articulosDesdeDb.UrlImagen;
                }
                articuloVM.Articulo.FechaCreacion = DateTime.Now.ToString();
                _contenedorTrabajo.ArticuloRepository.Update(articuloVM.Articulo);
                _contenedorTrabajo.Save();

                return RedirectToAction(nameof(Index));
            }
            return View(articuloVM);
        }

        #region
        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = _contenedorTrabajo.ArticuloRepository.GetAll(includeProperties: "Categoria") });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var articulosDesdeDb = _contenedorTrabajo.ArticuloRepository.Get(id);
            string rutaDirectorioPrincipal = _webHostEnvironment.WebRootPath;

            var rutaImagen = Path.Combine(rutaDirectorioPrincipal, articulosDesdeDb.UrlImagen.TrimStart('\\'));

            if (System.IO.File.Exists(rutaImagen))
            {
                System.IO.File.Delete(rutaImagen);
            }

            if(articulosDesdeDb == null)
            {
                return Json(new { success = false, message = "Error borrando articulo" });
            }

            _contenedorTrabajo.ArticuloRepository.Remove(articulosDesdeDb);
            _contenedorTrabajo.Save();
            return Json(new { success = true, message = "Articulo eliminada con exito" });
        }

        #endregion
    }
}
