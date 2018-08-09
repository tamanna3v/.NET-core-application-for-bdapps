using System.Collections.Generic;
using System.Linq;
using HT.CheckerApp.API.Models;

namespace HT.CheckerApp.API.Repositories
{
    public class PBOnDemandRepository : IPBOnDemandRepository
    {
        private readonly DataBaseContext _ctx;

        public PBOnDemandRepository(DataBaseContext ctx)
        {
            _ctx = ctx;
        }

        public IEnumerable<PBOnDemand> GetAll()
        {
            return _ctx.PBOnDemand;
        }

        public PBOnDemand GetById(int id)
        {
            return _ctx.PBOnDemand.FirstOrDefault(x => x.Id == id);
        }

        public PBOnDemand Add(PBOnDemand toAdd)
        {
            _ctx.PBOnDemand.Add(toAdd);
            return toAdd;
        }

        public PBOnDemand Update(PBOnDemand toUpdate)
        {
            _ctx.PBOnDemand.Update(toUpdate);
            return toUpdate;
        }

        public void Delete(PBOnDemand toDelete)
        {
            _ctx.PBOnDemand.Remove(toDelete);
        }

        public int Save()
        {
            return _ctx.SaveChanges();
        }
    }
}
