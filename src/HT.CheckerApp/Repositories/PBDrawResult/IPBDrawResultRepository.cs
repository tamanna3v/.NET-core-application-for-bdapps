using System.Collections.Generic;
using HT.CheckerApp.API.Models;

namespace HT.CheckerApp.API.Repositories
{
    public interface IPBDrawResultRepository
    {
        IEnumerable<PBDrawResult> GetAll();
        PBDrawResult GetById(int id);
        PBDrawResult Add(PBDrawResult toAdd);
        PBDrawResult Update(PBDrawResult toUpdate);
        void Delete(PBDrawResult toDelete);
        int Save();
        List<PBDrawResultViewModel> Match(string pbNumbers);
    }
}