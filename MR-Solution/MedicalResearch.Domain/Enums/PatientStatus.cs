using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalResearch.Domain.Enums
{
    public enum PatientStatus
    {
        Screened = 1,
        Randomized =2,
        Finished = 3,
        FinishedEarly = 4
    }
}
