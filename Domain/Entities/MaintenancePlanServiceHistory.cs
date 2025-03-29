﻿using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    internal class MaintenancePlanServiceHistory:BaseEntity
    {
        public int? ServiceId { get; set; }
        public Service Service { get; set; }
        public bool IsImplemented { get; set; }
        public int EquipmentValue { get; set; }
    }
}
