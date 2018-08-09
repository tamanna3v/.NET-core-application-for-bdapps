using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using HT.CheckerApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace HT.CheckerApp.API.Repositories
{
    public class PBDrawResultRepository : IPBDrawResultRepository
    {
        private readonly DataBaseContext _ctx;

        public PBDrawResultRepository(DataBaseContext ctx)
        {
            _ctx = ctx;
        }

        public IEnumerable<PBDrawResult> GetAll()
        {
            return _ctx.PBDrawResults;
        }

        public PBDrawResult GetById(int id)
        {
            return _ctx.PBDrawResults.FirstOrDefault(x => x.Id == id);
        }

        public PBDrawResult Add(PBDrawResult toAdd)
        {
            _ctx.PBDrawResults.Add(toAdd);
            return toAdd;
        }

        public PBDrawResult Update(PBDrawResult toUpdate)
        {
            _ctx.PBDrawResults.Update(toUpdate);
            return toUpdate;
        }

        public void Delete(PBDrawResult toDelete)
        {
            _ctx.PBDrawResults.Remove(toDelete);
        }

        public int Save()
        {
            return _ctx.SaveChanges();
        }
        public List<PBDrawResultViewModel> Match(string pbNumbers)
        {
            try
            {
                List<PBDrawResultViewModel> totalMatchedNumbers = new List<PBDrawResultViewModel>();

                if (pbNumbers == null)
                {
                    return null;
                }

                List<string> pbNoList = pbNumbers.Split(',').Distinct().ToList();

                foreach (string pbNo in pbNoList)
                {
                    if (pbNo.Contains('-'))
                    {
                         var seriesNumbers = pbNo.Split('-').ToArray();

                        string queryString = string.Format("Select * from PBDrawResults where PBNo between '{0}' and '{1}'", seriesNumbers[0].Trim(), seriesNumbers[1].Trim());
                        List<PBDrawResult> matchedNumbers= _ctx.PBDrawResults.FromSql(queryString).ToList();

                        totalMatchedNumbers.AddRange(Mapper.Map<List<PBDrawResultViewModel>>(matchedNumbers));
                    }
                    else
                    {
                        PBDrawResult number = this.GetAll().FirstOrDefault(oItem => oItem.PBNo.Equals(pbNo.Trim()));

                        if (number != null)
                            totalMatchedNumbers.Add(Mapper.Map<PBDrawResultViewModel>(number));
                    }

                }

                return totalMatchedNumbers;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private static IEnumerable<int> GetSeriesNumbers(string pbNo)
        {
            var seriesNumbers = pbNo.Split('-').ToArray();
            int startingNumber = Convert.ToInt32(seriesNumbers[0]);
            int endingNumber = Convert.ToInt32(seriesNumbers[1]);
            int range = (endingNumber - startingNumber) + 1;
            IEnumerable<int> numbers = Enumerable.Range(startingNumber, range);
            return numbers;
        }

    }
}
