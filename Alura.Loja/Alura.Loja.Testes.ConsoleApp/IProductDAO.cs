using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alura.Loja.Testes.ConsoleApp
{
    interface IProductDAO
    {
        void Add(Produto product);
        void Update(Produto product);
        void Remove(Produto product);
        IList<Produto> Products();
    }
}
