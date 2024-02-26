using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using ContosoCrafts.WebSite.Models;
using Microsoft.AspNetCore.Hosting;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ContosoCrafts.WebSite.Services
{
    public class JsonFileProductService
    {
        public JsonFileProductService(IWebHostEnvironment webHostEnvironment)
        {
            WebHostEnvironment = webHostEnvironment;
        }

        public IWebHostEnvironment WebHostEnvironment { get; }

        private string JsonFileName
        {
            get { return Path.Combine(WebHostEnvironment.WebRootPath, "data", "products.json"); }
        }

        public IEnumerable<Product> GetProducts()
        {
            using (var jsonFileReader = File.OpenText(JsonFileName))
            {
                return JsonSerializer.Deserialize<Product[]>(jsonFileReader.ReadToEnd(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
            }
        }

        public void AddProduct(Product product)
        {
            var products = GetProducts().ToList();
            products.Add(product);

            using (var outputStream = File.OpenWrite(JsonFileName))
            {
                JsonSerializer.Serialize<IEnumerable<Product>>(
                   new Utf8JsonWriter(outputStream, new JsonWriterOptions
                   {
                       SkipValidation = true,
                       Indented = true
                   }),
                   products
                 );
            }

        }

        public void AddRating(string productId, int rating)
        {
            IEnumerable<Product> products = GetProducts();

            //LINQ
            var query = products.First(x => x.Id == productId);

            if(query.Ratings == null)
            {
                query.Ratings = new int[] { rating };
            }
            else
            {
                var ratings = query.Ratings.ToList();
                ratings.Add(rating);
                query.Ratings = ratings.ToArray();
            }

            using(var outputStream = File.OpenWrite(JsonFileName))
            {
                JsonSerializer.Serialize(
                   new Utf8JsonWriter(outputStream, new JsonWriterOptions
                   {
                       SkipValidation = true,
                       Indented = true
                   }),
                   products
                 );
            }
        }

        //public void RemoveProduct(string productId)
        //{
        //    var products = GetProducts().ToList();
        //    IEnumerable<Product> productsEmpty = [];

        //    var query = products.First(x => x.Id == productId);

        //    var removed = products.Remove(query);

        //    //System.Diagnostics.Debug.WriteLine();

        //    using (var outputStream = File.OpenWrite(JsonFileName))
        //    {
        //        JsonSerializer.Serialize(
        //           new Utf8JsonWriter(outputStream, new JsonWriterOptions
        //           {
        //               SkipValidation = true,
        //               Indented = true
        //           }),
        //           productsEmpty
        //         );
        //        JsonSerializer.Serialize<IEnumerable<Product>>(
        //           new Utf8JsonWriter(outputStream, new JsonWriterOptions
        //           {
        //               SkipValidation = true,
        //               Indented = true
        //           }),
        //           products
        //         );
        //    }
        //}
    }
}
