using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace MongoTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var ctx = new MongoContext();
            // var iphone = new Order
            // {
            //     Product = "Iphone"
            // };
            // var motoG = new Order
            // {
            //     Product = "Moto G"
            // };
            //
            // await ctx.Orders.InsertOneAsync(iphone);
            // await ctx.Orders.InsertOneAsync(motoG);
            //
            // var user = new User
            // {
            //     Name = "Kevin",
            //     MainProduct = iphone.Id,
            //     Products = new List<ObjectId>()
            //     {
            //         motoG.Id,
            //         iphone.Id
            //     }
            // };
            // await ctx.Users.InsertOneAsync(user);
            //


            ComplexQueryWithList(ctx);
            // ComplexQuery(ctx);
        }

        private static void ComplexQueryWithList(MongoContext ctx)
        {
            var queryGroup = ctx.Users.AsQueryable()
                .SelectMany(x => x.Products, (item, product) => new
                {
                    Name = item.Name,
                    Product = product
                })
                .GroupJoin(
                    ctx.Orders.AsQueryable(),
                    userProductId => userProductId.Product,
                    order => order.Id,
                    (userProduct, products) => new
                    {
                        Name = userProduct.Name,
                        Products = products
                    })
                .GroupBy(x => x.Name)
                .Select(x => new {name = x.Key, products = x.Select(e => e.Products) })
                .ToList();


            var query = queryGroup.Select(x => new {x.name, products = x.products.SelectMany(d => d)}).ToList();
            
            Console.WriteLine();
            Console.WriteLine("Printing all users from complex query");
            Console.WriteLine("Find {0} users.", query.Count);
            query.ForEach(q =>
            {
                // Console.WriteLine($"User {q.UserName}");
                Console.WriteLine("printing user orders");
                // q.products.ToList().ForEach(op => Console.WriteLine(op));
            });
        }

        private static void ComplexQuery(MongoContext ctx)
        {
            var query = ctx.Users.AsQueryable()
                .GroupJoin(ctx.Orders.AsQueryable(),
                    user => user.MainProduct,
                    order => order.Id,
                    (user, orders) => new
                    {
                        user_name = user.Name,
                        order_list = orders
                    }).ToList();

            Console.WriteLine();
            Console.WriteLine("Printing all users from complex query");
            Console.WriteLine("Find {0} users.", query.Count);
            query.ForEach(q =>
            {
                // Console.WriteLine($"User {q.UserName}");
                Console.WriteLine("printing user orders");
                q.order_list.ToList().ForEach(op => Console.WriteLine(op.Product));
            });
        }
    }
}