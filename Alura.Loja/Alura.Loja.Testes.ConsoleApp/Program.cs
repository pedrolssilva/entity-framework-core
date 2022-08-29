using Microsoft.EntityFrameworkCore;
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

                var products = context.Produtos.ToList();
                foreach (var p in products)
                {
                    Console.WriteLine(p);
                }
                
                Console.WriteLine("===================================");
                foreach (var e in context.ChangeTracker.Entries())
                {
                    Console.WriteLine(e.State);
                }

                var p1 = products.Last();
                p1.Nome = "007 - The spy who loved me";
                //context.SaveChanges();

                Console.WriteLine("===================================");
                foreach (var e in context.ChangeTracker.Entries())
                {
                    Console.WriteLine(e.State);
                }

                //Console.WriteLine("===================================");
                //products = context.Produtos.ToList();
                //foreach (var p in products)
                //{
                //    Console.WriteLine(p);
                //}
            }
        }
    }
}
