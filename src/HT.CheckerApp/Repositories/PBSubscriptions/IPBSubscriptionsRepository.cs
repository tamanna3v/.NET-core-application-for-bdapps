using System.Collections.Generic;
using HT.CheckerApp.API.Models;

namespace HT.CheckerApp.API.Repositories
{
    public interface IPBSubscriptionsRepository
    {
        IEnumerable<PBSubscriptions> GetAll();
        PBSubscriptions GetById(int id);
        PBSubscriptions Add(PBSubscriptions toAdd);
        PBSubscriptions Update(PBSubscriptions toUpdate);
        void Delete(PBSubscriptions toDelete);
        int Save();
    }
}