using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalResearch.Domain.Models
{
    public class DosageForm: TEntiny
    {
        public string Name { get; set; } = string.Empty;
        public List<Medicine> Medicines { get; set; } = [];
    }
}
