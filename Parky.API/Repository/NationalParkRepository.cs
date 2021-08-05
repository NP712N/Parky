using Parky.API.Data;
using Parky.API.Models;
using Parky.API.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parky.API.Repository
{
    public class NationalParkRepository : INationalParkRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public NationalParkRepository(ApplicationDbContext applicationDbContext) {
            _applicationDbContext = applicationDbContext;
        }

        public bool CreateNationalPark(NationalPark nationalPark)
        {
            _applicationDbContext.NationalParks.Add(nationalPark);
            return Save();
        }

        public bool DeleteNationalPark(NationalPark nationalPark)
        {
            _applicationDbContext.NationalParks.Remove(nationalPark);
            return Save();
        }

        public NationalPark GetNationalPark(int nationalParkId)
        {
           return _applicationDbContext.NationalParks.FirstOrDefault(n => n.Id == nationalParkId);
        }

        public ICollection<NationalPark> GetNationalParks()
        {
            return _applicationDbContext.NationalParks.OrderBy(n => n.Name).ToList();
        }

        public bool NationalParkExists(string name)
        {
            return _applicationDbContext.NationalParks.Any(n => n.Name.ToLower().Trim() == name.ToLower().Trim());
        }

        public bool NationalParkExists(int id)
        {
            return _applicationDbContext.NationalParks.Any(n => n.Id == id);
        }

        public bool Save()
        {
            return _applicationDbContext.SaveChanges() < 0 ? false : true;
        }

        public bool UpdateNationalPark(NationalPark nationalPark)
        {
            _applicationDbContext.NationalParks.Update(nationalPark);
            return Save();
        }
    }
}
