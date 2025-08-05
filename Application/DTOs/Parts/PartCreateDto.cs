using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Parts
{
   public class PartCreateDto
    {
        public string Code { get; set; }           // e.g., "PART-001"
        public string Name { get; set; }           // e.g., "Oil Filter"
        public int Quantity { get; set; }          // Starting quantity
        public int ReorderLimit { get; set; }      // Minimum threshold
    }
}
