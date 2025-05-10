using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalResearch.Domain.Models
{
    public class Role: TEntiny
    {
        public string Name { get; set; } = string.Empty;
        public List<User> Users { get; set; } = [];
    }
}
