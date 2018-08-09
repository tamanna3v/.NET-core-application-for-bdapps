using System;
using System.Linq;
using System.Net;
using HT.CheckerApp.API.Models;
using HT.CheckerApp.API.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace HT.CheckerApp.API.Controllers
{

    [Produces("application/json")]
    [Route("api/PBDrawResult")]
    public class PBDrawResultController : Controller
    {
        private readonly IPBDrawResultRepository _pbDrawResultRepository;
        public PBDrawResultController(IPBDrawResultRepository pbDrawResultRepository)
        {
            _pbDrawResultRepository = pbDrawResultRepository;
        }
        // GET: api/PBDrawResult
        [HttpGet("", Name = "GetAll")]
        public IActionResult Get()
        {
            try
            {
                return Ok(_pbDrawResultRepository.GetAll().Select(x => Mapper.Map<PBDrawResultViewModel>(x)));
            }
            catch (Exception exception)
            {
                //logg exception or do anything with it
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        // GET: api/PBDrawResult/5
        [HttpGet("{id}", Name = "Get")]
        public IActionResult Get(int id)
        {
            try
            {
                PBDrawResult pbDrawResult = _pbDrawResultRepository.GetById(id);

                if (pbDrawResult == null)
                {
                    return NotFound();
                }

                return Ok(Mapper.Map<PBDrawResultViewModel>(pbDrawResult));
            }
            catch (Exception exception)
            {
                //Do something with the exception
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        // POST: api/PBDrawResult
        [HttpPost]
        public IActionResult Post([FromBody]PBDrawResultViewModel viewModel)
        {
            try
            {
                if (viewModel == null)
                {
                    return BadRequest();
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                List<string> pbNoList = viewModel.PBNo.Split(' ').ToList();
                foreach (string pbNo in pbNoList)
                {
                    PBDrawResult item = Mapper.Map<PBDrawResult>(viewModel);
                    item.PBNo = pbNo.Trim();
                    item.CreationDate = DateTime.Now;
                    item.LastUpdatedDate = DateTime.Now;
                    item.IsActive = true;

                    _pbDrawResultRepository.Add(item);
                    int save = _pbDrawResultRepository.Save();

                    if (save > 0)
                    {
                       // return CreatedAtRoute("GetById", new { controller = "PBDrawResult", id = item.Id }, item);
                    }
                }

                return StatusCode((int)HttpStatusCode.OK);
            }
            catch (Exception exception)
            {
                //Do something with the exception
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
        // PUT: api/PBDrawResult/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]PBDrawResultViewModel viewModel)
        {
            try
            {
                if (viewModel == null)
                {
                    return BadRequest();
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                PBDrawResult singleById = _pbDrawResultRepository.GetById(id);

                if (singleById == null)
                {
                    return NotFound();
                }

                singleById.PBNo = viewModel.PBNo;
                singleById.Draw = viewModel.Draw;
                singleById.Validity = viewModel.Validity;
                singleById.LastUpdatedBy = viewModel.LastUpdatedBy;
                singleById.LastUpdatedDate = DateTime.Now;

                _pbDrawResultRepository.Update(singleById);
                int save = _pbDrawResultRepository.Save();

                if (save > 0)
                {
                    return Ok(Mapper.Map<PBDrawResultViewModel>(singleById));
                }

                return BadRequest();
            }
            catch (Exception exception)
            {
                //Do something with the exception
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
        //[HttpGet]
        //public IActionResult Match(string pbNumbers)
        //{
        //    try
        //    {
        //        List<PBDrawResultViewModel> totalMatchedNumbers = new List<PBDrawResultViewModel>();

        //        if (pbNumbers == null)
        //        {
        //            return BadRequest();
        //        }

        //        totalMatchedNumbers = _pbDrawResultRepository.Match(pbNumbers);

        //        return Ok(totalMatchedNumbers);
        //    }
        //    catch (Exception exception)
        //    {
        //        //Do something with the exception
        //        return StatusCode((int)HttpStatusCode.InternalServerError);
        //    }
        //}

        private static IEnumerable<int> GetSeriesNumbers(string pbNo)
        {
            var seriesNumbers = pbNo.Split('-').ToArray();
            int startingNumber = Convert.ToInt32(seriesNumbers[0]);
            int endingNumber = Convert.ToInt32(seriesNumbers[1]);
            int range = startingNumber - endingNumber;
            IEnumerable<int> numbers = Enumerable.Range(startingNumber, range);
            return numbers;
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                PBDrawResult singleById =_pbDrawResultRepository.GetById(id);

                if (singleById == null)
                {
                    return NotFound();
                }

                _pbDrawResultRepository.Delete(singleById);
                int save = _pbDrawResultRepository.Save();

                if (save > 0)
                {
                    return NoContent();
                }

                return BadRequest();
            }
            catch (Exception exception)
            {
                //Do something with the exception
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
       
    }
}
