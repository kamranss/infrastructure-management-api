using Application.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Parts
{
    public class PartDto:BaseDto
    {
        public int? Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public int Quantity { get; set; } = 0;
        public int? ReorderLimit { get; set; } = null;

    }
}
