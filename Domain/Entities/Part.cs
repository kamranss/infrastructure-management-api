﻿using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Part:BaseEntity
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
        public List<Equipment>? Equipments { get; set; }
        //public int Quantity { get; set; }
        //public int ? ReorderLimit { get; set; }
    }
}
