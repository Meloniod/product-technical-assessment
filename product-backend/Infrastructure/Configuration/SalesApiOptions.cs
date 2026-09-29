using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Configuration
{
    public sealed class SalesApiOptions
    {
        public const string SectionName = "SalesApi";

        public string BaseUrl { get; init; } = string.Empty;

        public int TimeoutSeconds { get; init; } = 300;
    }
}
