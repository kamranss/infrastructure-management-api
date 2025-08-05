using Application.Repositories.EquipmentPartRepo;
using Domain.Entities;
using Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories.EquipmentPartRepo
{
    public class EquipmentPartWriteRepository : WriteRepository<EquipmentPart>, IEquipmentPartWriteRepository
    {
        public EquipmentPartWriteRepository(MaintenanceDbContext context) : base(context)
        {
        }
    }
}
