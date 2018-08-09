using System;
using System.Linq;
using System.Net;
using HT.CheckerApp.API.Models;
using HT.CheckerApp.API.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace HT.CheckerApp.API.Controllers
{

    [Produces("application/json")]
    [Route("api/PBSubscriptionRevenue")]
    public class PBSubscriptionRevenueController : Controller
    {
        private readonly IPBSubscriptionRevenueRepository _pbSubscriptionRevenueRepository;
        public PBSubscriptionRevenueController(IPBSubscriptionRevenueRepository pbSubscriptionRevenueRepository)
        {
            _pbSubscriptionRevenueRepository = pbSubscriptionRevenueRepository;
        }
        // GET: api/PBSubscriptionRevenue
        [HttpGet("", Name = "GetAllPBSubscriptionRevenue")]
        public IActionResult Get()
        {
            try
            {
                return Ok(_pbSubscriptionRevenueRepository.GetAll().Select(x => Mapper.Map<PBSubscriptionRevenueViewModel>(x)));
            }
            catch (Exception exception)
            {
                //logg exception or do anything with it
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        // GET: api/PBSubscriptionRevenue/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                PBSubscriptionRevenue pbSubscriptionRevenue = _pbSubscriptionRevenueRepository.GetById(id);

                if (pbSubscriptionRevenue == null)
                {
                    return NotFound();
                }

                return Ok(Mapper.Map<PBSubscriptionRevenueViewModel>(pbSubscriptionRevenue));
            }
            catch (Exception exception)
            {
                //Do something with the exception
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        // POST: api/PBSubscriptionRevenue
        [HttpPost]
        public IActionResult Post([FromBody]PBSubscriptionRevenueViewModel viewModel)
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

                PBSubscriptionRevenue item = Mapper.Map<PBSubscriptionRevenue>(viewModel);
                item.CreatedDate = DateTime.Now;
                item.LastUpdatedDate = DateTime.Now;
                item.IsActive = true;

                _pbSubscriptionRevenueRepository.Add(item);
                int save = _pbSubscriptionRevenueRepository.Save();

                if (save > 0)
                {
                    return CreatedAtRoute("GetById", new { controller = "PBSubscriptionRevenue", id = item.Id }, item);
                }

                return BadRequest();
            }
            catch (Exception exception)
            {
                //Do something with the exception
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
        // PUT: api/PBSubscriptionRevenue/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]PBSubscriptionRevenueViewModel viewModel)
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

                PBSubscriptionRevenue singleById = _pbSubscriptionRevenueRepository.GetById(id);

                if (singleById == null)
                {
                    return NotFound();
                }

                singleById.MSISDN= viewModel.MSISDN;
                singleById.Cycle = viewModel.Cycle;
                singleById.SubscriptionDate = viewModel.SubscriptionDate;
                singleById.SubscriptionType = viewModel.SubscriptionType;
                singleById.LastUpdatedBy = viewModel.LastUpdatedBy;
                singleById.LastUpdatedDate = DateTime.Now;

                _pbSubscriptionRevenueRepository.Update(singleById);
                int save = _pbSubscriptionRevenueRepository.Save();

                if (save > 0)
                {
                    return Ok(Mapper.Map<PBSubscriptionRevenueViewModel>(singleById));
                }

                return BadRequest();
            }
            catch (Exception exception)
            {
                //Do something with the exception
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                PBSubscriptionRevenue singleById =_pbSubscriptionRevenueRepository.GetById(id);

                if (singleById == null)
                {
                    return NotFound();
                }

                _pbSubscriptionRevenueRepository.Delete(singleById);
                int save = _pbSubscriptionRevenueRepository.Save();

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
