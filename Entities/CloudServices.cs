﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class CloudServices
    {
        [Key]
        public int Id { get; set; }
        public string? ServiceName { get; set; }
        public string? SSDStorage { get; set; }
        public int? EmailAccounts { get; set; }
        public int? MSDatabase { get; set; }
        public int? WebsitesAllowed { get; set; }
        public string? SSLCertificate { get; set; }
        public decimal Price { get; set; }
    }
}
