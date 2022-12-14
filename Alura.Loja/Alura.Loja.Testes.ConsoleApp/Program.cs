using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
            using (var context = new StoreContext())
            {
                var serviceProvider = context.GetInfrastructure();
                var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
                loggerFactory.AddProvider(SqlLoggerProvider.Create());

                var customer = context
                    .Clientes
                    .Include(c => c.EnderecoDeEntrega)
                    .FirstOrDefault();

                Console.WriteLine($"Endereço de entrega: {customer.EnderecoDeEntrega.Logradouro}");

                var product = context
                    .Produtos
                    .Where(p => p.Id == 1)
                    .FirstOrDefault();

                context.Entry(product)
                    .Collection(p => p.Compras)
                    .Query()
                    .Where(c => c.Preco > 10)
                    .Load();


                Console.WriteLine($"Mostrando as compras do produto {product.Nome}");
                foreach (var item in product.Compras)
                {
                    Console.WriteLine(item);
                }
            }
        }

        private static void ShowProductsFromSale()
        {
            using (var context = new StoreContext())
            {
                var serviceProvider = context.GetInfrastructure();
                var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
                loggerFactory.AddProvider(SqlLoggerProvider.Create());

                var sale = context
                    .Promocoes
                    .Include(p => p.Produtos)
                    .ThenInclude(pp => pp.Produto)
                    .FirstOrDefault();
                Console.WriteLine("\nMostrando os produtos da promoção...");
                foreach (var item in sale.Produtos)
                {
                    Console.WriteLine(item.Produto);
                }
            }
        }

        private static void IncludeNewSale()
        {
            using (var context = new StoreContext())
            {
                var serviceProvider = context.GetInfrastructure();
                var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
                loggerFactory.AddProvider(SqlLoggerProvider.Create());

                var sale = new Promocao();
                sale.Descricao = "Queima Total Janeiro 2023";
                sale.DataInicio = new DateTime(2023, 1, 1);
                sale.DataInicio = new DateTime(2023, 1, 31);

                var product = context
                    .Produtos
                    .Where(p => p.Categoria == "Bebidas")
                    .ToList();

                foreach (var item in product)
                {
                    sale.IncludeProduct(item);
                }

                context.Promocoes.Add(sale);

                ShowEntries(context.ChangeTracker.Entries());

                context.SaveChanges();
            }
        }

        private static void OneToOne()
        {
            var fulano = new Cliente();
            fulano.Nome = "Fulaninho de Tal";
            fulano.EnderecoDeEntrega = new Endereco()
            {
                Numero = 12,
                Logradouro = "Rua dos Inválidos",
                Complemento = "sobrado",
                Bairro = "Centro",
                Cidade = "Cidade"
            };

            using (var context = new StoreContext())
            {
                var serviceProvider = context.GetInfrastructure();
                var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
                loggerFactory.AddProvider(SqlLoggerProvider.Create());

                context.Clientes.Add(fulano);
                context.SaveChanges();
            }
        }

        private static void ManyToMany()
        {
            var p1 = new Produto() { Nome = "Suco de Laranja", Categoria = "Bebidas", PrecoUnitario = 8.79, Unidade = "Litros" };
            var p2 = new Produto() { Nome = "Café", Categoria = "Bebidas", PrecoUnitario = 12.45, Unidade = "Gramas" };
            var p3 = new Produto() { Nome = "Macarrão", Categoria = "Alimentos", PrecoUnitario = 4.23, Unidade = "Gramas " }; ;

            var promocaoDePascoa = new Promocao();
            promocaoDePascoa.Descricao = "Páscoa Feliz";
            promocaoDePascoa.DataInicio = DateTime.Now;
            promocaoDePascoa.DataTermino = DateTime.Now.AddMonths(3);

            promocaoDePascoa.IncludeProduct(p1);
            promocaoDePascoa.IncludeProduct(p2);
            promocaoDePascoa.IncludeProduct(p3);

            using (var context = new StoreContext())
            {
                var serviceProvider = context.GetInfrastructure();
                var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
                loggerFactory.AddProvider(SqlLoggerProvider.Create());

                //context.Promocoes.Add(promocaoDePascoa);
                var promocao = context.Promocoes.Find(1);
                context.Promocoes.Remove(promocao);
                context.SaveChanges();
            }
        }

        private static void ShowEntries(IEnumerable<EntityEntry> entries)
        {
            foreach (var e in entries)
            {
                Console.WriteLine(e.Entity.ToString() + " - " + e.State);
            }
        }
    }
}
