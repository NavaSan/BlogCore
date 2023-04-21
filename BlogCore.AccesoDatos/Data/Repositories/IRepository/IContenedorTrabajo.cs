using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.AccesoDatos.Data.Repositories.IRepository
{
    public interface IContenedorTrabajo : IDisposable
    {
        ICategoriaRepository CategoriaRepository { get; }
        IArticuloRepository ArticuloRepository { get; }
        ISliderRepository SliderRepository { get; }

        void Save();
    }
}
