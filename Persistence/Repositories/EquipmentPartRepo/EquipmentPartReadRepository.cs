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
    public class EquipmentPartReadRepository : ReadRepository<EquipmentPart>, IEquipmentPartReadRepository
    {
        public EquipmentPartReadRepository(MaintenanceDbContext context) : base(context)
        {
        }
    }
}
