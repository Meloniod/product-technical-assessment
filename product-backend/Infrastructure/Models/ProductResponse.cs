using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models
{
    public sealed class ProductResponse
    {
        public int Id { get; init; }

        public string Description { get; init; } = string.Empty;

        public decimal SalePrice { get; init; }

        public string Category { get; init; } = string.Empty;

        public string Image { get; init; } = string.Empty;
    }
}
