using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalResearch.Domain.Queries;

public class Query
{
    public int Take { get; set; }
    public int Skip { get; set; }
    public string? SortColumn { get; set; }
    public string? SearchTerm { get; set; }
    public bool IsAscending { get; set; } = true;
}
