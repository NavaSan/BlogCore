using BlogCore.AccesoDatos.Data.Repositories.IRepository;
using BlogCore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.AccesoDatos.Data.Repositories
{
    public class ContenedorTrabajo : IContenedorTrabajo
    {
        private readonly ApplicationDbContext _db;

        public ContenedorTrabajo(ApplicationDbContext db)
        {
            _db = db;
            CategoriaRepository = new CategoriaRepository(_db);
            ArticuloRepository = new ArticuloRepository(_db);
            SliderRepository = new SliderRepository(_db);
        }

        public ICategoriaRepository CategoriaRepository { get; set; }
        public IArticuloRepository ArticuloRepository { get; set; }
        public ISliderRepository SliderRepository { get; set; }

        public void Dispose()
        {
            _db.Dispose();
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
