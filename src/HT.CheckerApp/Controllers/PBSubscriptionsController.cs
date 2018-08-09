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
    [Route("api/PBSubscriptions")]
    public class PBSubscriptionsController : Controller
    {
        private readonly IPBSubscriptionsRepository _pbSubscriptionsRepository;
        public PBSubscriptionsController(IPBSubscriptionsRepository pbSubscriptionsRepository)
        {
            _pbSubscriptionsRepository = pbSubscriptionsRepository;
        }
        // GET: api/PBSubscriptions
        [HttpGet("", Name = "GetAllSubscriptions")]
        public IActionResult Get()
        {
            try
            {
                return Ok(_pbSubscriptionsRepository.GetAll().Select(x => Mapper.Map<PBSubscriptionsViewModel>(x)));
            }
            catch (Exception exception)
            {
                //logg exception or do anything with it
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        // GET: api/PBSubscriptions/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                PBSubscriptions pbSubscriptions = _pbSubscriptionsRepository.GetById(id);

                if (pbSubscriptions == null)
                {
                    return NotFound();
                }

                return Ok(Mapper.Map<PBSubscriptionsViewModel>(pbSubscriptions));
            }
            catch (Exception exception)
            {
                //Do something with the exception
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        // POST: api/PBSubscriptions
        [HttpPost]
        public IActionResult Post([FromBody]PBSubscriptionsViewModel viewModel)
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

                PBSubscriptions item = Mapper.Map<PBSubscriptions>(viewModel);
                item.CreatedDate = DateTime.Now;
                item.LastUpdatedDate = DateTime.Now;
                item.IsActive = true;

                _pbSubscriptionsRepository.Add(item);
                int save = _pbSubscriptionsRepository.Save();

                if (save > 0)
                {
                    return CreatedAtRoute("GetById", new { controller = "PBSubscriptions", id = item.Id }, item);
                }

                return BadRequest();
            }
            catch (Exception exception)
            {
                //Do something with the exception
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
        // PUT: api/PBSubscriptions/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]PBSubscriptionsViewModel viewModel)
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

                PBSubscriptions singleById = _pbSubscriptionsRepository.GetById(id);

                if (singleById == null)
                {
                    return NotFound();
                }

                singleById.PBNo= viewModel.PBNo;
                singleById.MSISDN = viewModel.MSISDN;
                singleById.Keyword = viewModel.Keyword;
                singleById.SubStartedDate = viewModel.SubStartedDate;
                singleById.PrizeDate = viewModel.PrizeDate;
                singleById.LastUpdatedBy = viewModel.LastUpdatedBy;
                singleById.LastUpdatedDate = DateTime.Now;

                _pbSubscriptionsRepository.Update(singleById);
                int save = _pbSubscriptionsRepository.Save();

                if (save > 0)
                {
                    return Ok(Mapper.Map<PBSubscriptionsViewModel>(singleById));
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
                PBSubscriptions singleById =_pbSubscriptionsRepository.GetById(id);

                if (singleById == null)
                {
                    return NotFound();
                }

                _pbSubscriptionsRepository.Delete(singleById);
                int save = _pbSubscriptionsRepository.Save();

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
