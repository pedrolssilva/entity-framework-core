using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alura.Loja.Testes.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //GravarUsandoAdoNet();
            //RecordUsingEntity();
            RestoreProducts();
            DeleteProducts();
            RestoreProducts();
        }

        private static void UpdateProduct()
        {
            //include a product
            RecordUsingEntity();
            RestoreProducts();

            // update the product
            using (var repo = new ProductDAOEntity())
            {
                Produto first = repo.Products().First();
                first.Nome = "Cassino Royale - Editado";
                repo.Update(first);
            }
            RestoreProducts();
        }

        private static void DeleteProducts()
        {
            using (var repo = new ProductDAOEntity())
            {
                IList<Produto> products = repo.Products();
                foreach (var item in products)
                {
                    repo.Remove(item);
                }
            }
        }

        private static void RestoreProducts()
        {
            using (var repo = new ProductDAOEntity())
            {
                IList<Produto> products = repo.Products();
                Console.WriteLine($"Products found {products.Count}");
                foreach (var item in products)
                {
                    Console.WriteLine(item.Nome);
                }
            }
        }

        private static void RecordUsingEntity()
        {
            Produto p = new Produto();
            p.Nome = "Harry Potter e a Ordem da Fênix";
            p.Categoria = "Livros";
            p.Preco = 19.89;

            using (var repo  = new ProductDAOEntity())
            {
                repo.Add(p);
            }
        }

        private static void GravarUsandoAdoNet()
        {
            Produto p = new Produto();
            p.Nome = "Harry Potter e a Ordem da Fênix";
            p.Categoria = "Livros";
            p.Preco = 19.89;

            using (var repo = new ProdutoDAO())
            {
                repo.Add(p);
            }
        }
    }
}
