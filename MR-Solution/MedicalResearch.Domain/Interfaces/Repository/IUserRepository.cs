using MedicalResearch.Domain.Extensions;
using MedicalResearch.Domain.Models;
using MedicalResearch.Domain.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalResearch.Domain.Interfaces.Repository
{
    public interface IUserRepository: IBaseRepository<User>
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task<PagedList<User>> SearchByTermAsync(Query query);
    }
}
