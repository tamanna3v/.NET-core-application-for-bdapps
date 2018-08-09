using System.Collections.Generic;
using HT.CheckerApp.API.Models;

namespace HT.CheckerApp.API.Repositories
{
    public interface IPBOnDemandRepository
    {
        IEnumerable<PBOnDemand> GetAll();
        PBOnDemand GetById(int id);
        PBOnDemand Add(PBOnDemand toAdd);
        PBOnDemand Update(PBOnDemand toUpdate);
        void Delete(PBOnDemand toDelete);
        int Save();
    }
}