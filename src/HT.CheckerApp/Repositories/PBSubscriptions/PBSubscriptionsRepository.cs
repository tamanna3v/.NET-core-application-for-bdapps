using System.Collections.Generic;
using System.Linq;
using HT.CheckerApp.API.Models;

namespace HT.CheckerApp.API.Repositories
{
    public class PBSubscriptionsRepository : IPBSubscriptionsRepository
    {
        private readonly DataBaseContext _ctx;

        public PBSubscriptionsRepository(DataBaseContext ctx)
        {
            _ctx = ctx;
        }

        public IEnumerable<PBSubscriptions> GetAll()
        {
            return _ctx.PBSubscriptions;
        }

        public PBSubscriptions GetById(int id)
        {
            return _ctx.PBSubscriptions.FirstOrDefault(x => x.Id == id);
        }

        public PBSubscriptions Add(PBSubscriptions toAdd)
        {
            _ctx.PBSubscriptions.Add(toAdd);
            return toAdd;
        }

        public PBSubscriptions Update(PBSubscriptions toUpdate)
        {
            _ctx.PBSubscriptions.Update(toUpdate);
            return toUpdate;
        }

        public void Delete(PBSubscriptions toDelete)
        {
            _ctx.PBSubscriptions.Remove(toDelete);
        }

        public int Save()
        {
            return _ctx.SaveChanges();
        }
    }
}
