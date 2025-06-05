using MedicalResearch.Domain.Models;
using MedicalResearch.Domain.Queries;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalResearch.Domain.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<ClinicStockMedicine> SearchByTermAndClinic(this IQueryable<ClinicStockMedicine> query, string? term, int clinicId)
        {
            if (string.IsNullOrEmpty(term))
            {
                return query;
            }

            term = term.Trim().ToLower();
            return query.Where(x =>
                EF.Functions.Like(x.Medicine.Description.ToLower(), $"%{term}%") && x.Amount > 0 && x.ClinicId.Equals(clinicId));
        }

        public static IQueryable<ClinicStockMedicine> SearchByTerm(this IQueryable<ClinicStockMedicine> query, string? term )
        {
            if (string.IsNullOrEmpty(term))
            {
                return query;
            }

            term = term.Trim().ToLower();
            return query.Where(x =>
                EF.Functions.Like(x.Medicine.Description.ToLower(), $"%{term}%") && x.Amount > 0 );
        }

        public static IQueryable<Clinic> SearchByTerm(this IQueryable<Clinic> query, string? term)
        {
            if (string.IsNullOrEmpty(term))
            {
                return query;
            }

            term = term.Trim().ToLower();
            return query.Where(x => EF.Functions.Like(x.Name.ToLower(), $"%{term}%"));
        }

        public static IQueryable<DosageForm> SearchByTerm(this IQueryable<DosageForm> query, string? term)
        {
            if (string.IsNullOrEmpty(term))
            {
                return query;
            }

            term = term.Trim().ToLower();
            return query.Where(x => EF.Functions.Like(x.Name.ToLower(), $"%{term}%"));
        }


        public static IQueryable<MedicineType> SearchByTerm(this IQueryable<MedicineType> query, string? term)
        {
            if (string.IsNullOrEmpty(term))
            {
                return query;
            }

            term = term.Trim().ToLower();
            return query.Where(x => EF.Functions.Like(x.Name.ToLower(), $"%{term}%"));
        }

        public static IQueryable<MedicineContainer> SearchByTerm(this IQueryable<MedicineContainer> query, string? term)
        {
            if (string.IsNullOrEmpty(term))
            {
                return query;
            }

            term = term.Trim().ToLower();
            return query.Where(x => EF.Functions.Like(x.Name.ToLower(), $"%{term}%"));
        }


        public static IQueryable<Medicine> SearchByTerm(this IQueryable<Medicine> query, string? term)
        {
            if (string.IsNullOrEmpty(term))
            {
                return query;
            }

            term = term.Trim().ToLower();
            return query.Where(x => EF.Functions.Like(x.Description.ToLower(), $"%{term}%"));
        }


        public static IQueryable<Role> SearchByTerm(this IQueryable<Role> query, string? term)
        {
            if (string.IsNullOrEmpty(term))
            {
                return query;
            }

            term = term.Trim().ToLower();
            return query.Where(x => EF.Functions.Like(x.Name.ToLower(), $"%{term}%"));
        }


        public static IQueryable<User> SearchByTerm(this IQueryable<User> query, string? term)
        {
            if (string.IsNullOrEmpty(term))
            {
                return query;
            }

            term = term.Trim().ToLower();
            return query.Where(x => EF.Functions.Like(x.FirstName.ToLower(), $"%{term}%") ||
                                    EF.Functions.Like(x.LastName.ToLower(), $"%{term}%") ||
                                    EF.Functions.Like(x.Email.ToLower(), $"%{term}%") );
        }

        public static IQueryable<Visit> SearchByTerm(this IQueryable<Visit> query, string? term)
        {
            if (string.IsNullOrEmpty(term))
            {
                return query;
            }

            term = term.Trim().ToLower();
            return query.Where(x => EF.Functions.Like(x.Patient.Number.ToLower(), $"%{term}%"));
        }


        public static IQueryable<Supply> SearchByTerm(this IQueryable<Supply> query, string? term)
        {
            if (string.IsNullOrEmpty(term))
            {
                return query;
            }

            term = term.Trim().ToLower();
            return query.Where(x =>
                EF.Functions.Like(x.Medicine.Description.ToLower(), $"%{term}%") );
        }


        public static IQueryable<Patient> SearchByTerm(this IQueryable<Patient> query, string? term)
        {
            if (string.IsNullOrEmpty(term))
            {
                return query;
            }

            term = term.Trim().ToLower();
            return query.Where(x => EF.Functions.Like(x.Number.ToLower(), $"%{term}%"));
        }



        public static  async Task<PagedList<T>> SortSkipTakeAsync<T>(this IQueryable<T> t, Query query) where T : class
        {
            if (string.IsNullOrEmpty(query.SortColumn))
            {
                query.SortColumn = "Id";
            }
            var prop = typeof(T).GetProperty(query.SortColumn);
            if (prop != null)
            {
                if (query.IsAscending)
                {
                    t = t.OrderBy(t => EF.Property<object>(t, query.SortColumn));
                }
                else
                {
                    t = t.OrderByDescending(t => EF.Property<object>(t, query.SortColumn));
                }
            }
            var result = await PagedList<T>.ToPagedList(t, query.Skip, query.Take);
                  
            return result;
        }
    }
}
