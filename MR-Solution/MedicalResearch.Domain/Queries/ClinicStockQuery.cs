using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalResearch.Domain.Queries
{
    public class ClinicStockQuery: Query
    {
        public int ClinicId { get; set; }
    }
}
