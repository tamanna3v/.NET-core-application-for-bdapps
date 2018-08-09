using System.Collections.Generic;
using System.Linq;
using HT.CheckerApp.API.Models;

namespace HT.CheckerApp.API.Repositories
{
    public class PBSubscriptionRevenueRepository : IPBSubscriptionRevenueRepository
    {
        private readonly DataBaseContext _ctx;

        public PBSubscriptionRevenueRepository(DataBaseContext ctx)
        {
            _ctx = ctx;
        }

        public IEnumerable<PBSubscriptionRevenue> GetAll()
        {
            return _ctx.PBSubscriptionRevenues;
        }

        public PBSubscriptionRevenue GetById(int id)
        {
            return _ctx.PBSubscriptionRevenues.FirstOrDefault(x => x.Id == id);
        }

        public PBSubscriptionRevenue Add(PBSubscriptionRevenue toAdd)
        {
            _ctx.PBSubscriptionRevenues.Add(toAdd);
            return toAdd;
        }

        public PBSubscriptionRevenue Update(PBSubscriptionRevenue toUpdate)
        {
            _ctx.PBSubscriptionRevenues.Update(toUpdate);
            return toUpdate;
        }

        public void Delete(PBSubscriptionRevenue toDelete)
        {
            _ctx.PBSubscriptionRevenues.Remove(toDelete);
        }

        public int Save()
        {
            return _ctx.SaveChanges();
        }
    }
}
