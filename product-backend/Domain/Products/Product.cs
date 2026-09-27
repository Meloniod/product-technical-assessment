using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Products
{
    public sealed class Product
    {
        public int Id { get; }
        public string Description { get; }
        public decimal SalePrice { get; }
        public string Category { get; }
        public string Image { get; }

        public Product(
            int id,
            string description,
            decimal salePrice,
            string category,
            string image)
        {
            Id = id;
            Description = description;
            SalePrice = salePrice;
            Category = category;
            Image = image;
        }
    }
}
