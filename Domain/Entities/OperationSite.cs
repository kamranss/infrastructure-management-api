﻿using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class OperationSite:BaseEntity
    {
        public string? Name { get; set; }
        public int Code { get; set; }
        public int? DepartmentId { get; set; }
        public Department Department { get; set; }
    }
}
