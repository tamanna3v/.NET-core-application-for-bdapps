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
    [Route("api/PBOnDemand")]
    public class PBOnDemandController : Controller
    {
        private readonly IPBOnDemandRepository _pbOnDemandRepository;
        public PBOnDemandController(IPBOnDemandRepository pbOnDemandRepository)
        {
            _pbOnDemandRepository = pbOnDemandRepository;
        }
        // GET: api/PBOnDemand
        [HttpGet("", Name = "GetAllPBOnDemand")]
        public IActionResult Get()
        {
            try
            {
                return Ok(_pbOnDemandRepository.GetAll().Select(x => Mapper.Map<PBOnDemandViewModel>(x)));
            }
            catch (Exception exception)
            {
                //logg exception or do anything with it
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        // GET: api/PBOnDemand/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                PBOnDemand pbOnDemand = _pbOnDemandRepository.GetById(id);

                if (pbOnDemand == null)
                {
                    return NotFound();
                }

                return Ok(Mapper.Map<PBOnDemandViewModel>(pbOnDemand));
            }
            catch (Exception exception)
            {
                //Do something with the exception
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        // POST: api/PBOnDemand
        [HttpPost]
        public IActionResult Post([FromBody]PBOnDemandViewModel viewModel)
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

                PBOnDemand item = Mapper.Map<PBOnDemand>(viewModel);
                item.CreatedDate = DateTime.Now;
                item.LastUpdatedDate = DateTime.Now;
                item.IsActive = true;

                _pbOnDemandRepository.Add(item);
                int save = _pbOnDemandRepository.Save();

                if (save > 0)
                {
                    return CreatedAtRoute("GetById", new { controller = "PBOnDemand", id = item.Id }, item);
                }

                return BadRequest();
            }
            catch (Exception exception)
            {
                //Do something with the exception
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
        // PUT: api/PBOnDemand/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]PBOnDemandViewModel viewModel)
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

                PBOnDemand singleById = _pbOnDemandRepository.GetById(id);

                if (singleById == null)
                {
                    return NotFound();
                }

                singleById.PBNo = viewModel.PBNo;
                singleById.MSISDN= viewModel.MSISDN;
                singleById.PrizeDate = viewModel.PrizeDate;
                singleById.LastUpdatedBy = viewModel.LastUpdatedBy;
                singleById.LastUpdatedDate = DateTime.Now;

                _pbOnDemandRepository.Update(singleById);
                int save = _pbOnDemandRepository.Save();

                if (save > 0)
                {
                    return Ok(Mapper.Map<PBOnDemandViewModel>(singleById));
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
                PBOnDemand singleById =_pbOnDemandRepository.GetById(id);

                if (singleById == null)
                {
                    return NotFound();
                }

                _pbOnDemandRepository.Delete(singleById);
                int save = _pbOnDemandRepository.Save();

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
