using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alura.Loja.Testes.ConsoleApp
{
    internal class ProductDAOEntity : IProductDAO, IDisposable
    {
        private StoreContext _context;

        public ProductDAOEntity()
        {
            _context = new StoreContext();
        }

        public void Add(Produto product)
        {
            _context.Produtos.Add(product);
            _context.SaveChanges();
        }

        public void Update(Produto product)
        {
            _context.Produtos.Update(product);
            _context.SaveChanges();
        }

        public IList<Produto> Products()
        {
            return _context.Produtos.ToList();
        }

        public void Remove(Produto product)
        {
            _context.Produtos.Remove(product);
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
