using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using ASP.NetMVC.Models;
using Dapper;

namespace ASP.NetMVC
{
    public class ProductRepository : IProductRepository
    {
        private readonly IDbConnection _conn;

        public ProductRepository(IDbConnection conn)
        {
            _conn = conn;
        }
        public IEnumerable<Product> GetAllProducts()
        {
            return _conn.Query<Product>("Select * FROM PRODUCTS;");
        }

        public Product GetProduct(int id)
        {
            return _conn.QuerySingle<Product>("SELECT * FROM PRODUCTS WHERE PRODUCTID = @id",
                    new { id = id });
        }

        void IProductRepository.UpdateProduct(Product product)
        {
            _conn.Execute("UPDATE products SET Name = @Name, Price = @Price WHERE ProductID = @id",
                new { name = product.Name, price = product.Price, id = product.ProductID });
        }

        public void InsertProduct(Product productToInsert)
        {
            _conn.Execute("INSERT INTO products (NAME, PRICE, CATEGORYID, ONSALE) VALUES (@name, @price, @categoryID, 1);",
                new { name = productToInsert.Name, price = productToInsert.Price, categoryID = productToInsert.CategoryID });

        }

        public IEnumerable<Category> GetCategories()
        {
            return _conn.Query<Category>("SELECT * FROM categories;");
        }

        public Product AssignCategory()
        {
            var categoryList = GetCategories();
            var product = new Product();
            product.Categories = categoryList;

            return product;
        }

        public void DeleteProduct(Product product)
        {
            
                _conn.Execute("DELETE FROM REVIEWS WHERE ProductID = @id;",
                                           new { id = product.ProductID });
                _conn.Execute("DELETE FROM Sales WHERE ProductID = @id;",
                                           new { id = product.ProductID });
                _conn.Execute("DELETE FROM Products WHERE ProductID = @id;",
                                           new { id = product.ProductID });
            

        }
    }

}
