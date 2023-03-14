using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.StoredProcedures
{
    public class Payments
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public string? CustomerName { get; set; }
        public string? ContactNumber { get; set; }
        public string? ServiceName { get; set; }
        public decimal Price { get; set; }
    }
}
