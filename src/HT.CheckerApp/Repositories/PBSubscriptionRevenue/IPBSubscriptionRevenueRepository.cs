using System.Collections.Generic;
using HT.CheckerApp.API.Models;

namespace HT.CheckerApp.API.Repositories
{
    public interface IPBSubscriptionRevenueRepository
    {
        IEnumerable<PBSubscriptionRevenue> GetAll();
        PBSubscriptionRevenue GetById(int id);
        PBSubscriptionRevenue Add(PBSubscriptionRevenue toAdd);
        PBSubscriptionRevenue Update(PBSubscriptionRevenue toUpdate);
        void Delete(PBSubscriptionRevenue toDelete);
        int Save();
    }
}