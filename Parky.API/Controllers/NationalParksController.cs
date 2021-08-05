using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Parky.API.Models;
using Parky.API.Models.DTOs;
using Parky.API.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parky.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NationalParksController : Controller
    {
        private INationalParkRepository _npRepository;
        private readonly IMapper _mapper;

        public NationalParksController(INationalParkRepository npRepository, IMapper mapper)
        {
            _npRepository = npRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetNationalParks()
        {
            var list = _npRepository.GetNationalParks();
            var listDTO = new List<NationalParkDTO>();

            foreach (var item in list) {
                listDTO.Add(_mapper.Map<NationalParkDTO>(item));
            }

            return Ok(listDTO);
        }

        [HttpGet("{parkId}", Name = "GetNationalPark")]
        public IActionResult GetNationalPark(int parkId)
        {
            var park = _npRepository.GetNationalPark(parkId);
            if(park == null) {
                return NotFound();
            }

            return Ok(_mapper.Map<NationalParkDTO>(park));
        }

        [HttpPost]
        public IActionResult CreateNationalPark([FromBody] NationalParkDTO nationalParkDTO) {
            if(nationalParkDTO == null) {
                return BadRequest(ModelState);
            }

            if (_npRepository.NationalParkExists(nationalParkDTO.Name))
            {
                ModelState.AddModelError("", "National Park Exists!");
                return StatusCode(404, ModelState);
            }

            var nationalParkObj = _mapper.Map<NationalPark>(nationalParkDTO);
            if (!_npRepository.CreateNationalPark(nationalParkObj))
            {
                ModelState.AddModelError("", $"Something went wrong when adding {nationalParkObj.Name}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetNationalPark", new { parkId = nationalParkObj.Id}, nationalParkObj);
        }

        [HttpPatch("parkId", Name = "UpdateNationPark")]
        public IActionResult UpdateNationPark(int parkId, [FromBody] NationalParkDTO nationalParkDTO) {
            if (nationalParkDTO == null  || parkId != nationalParkDTO.Id)
            {
                return BadRequest(ModelState);
            }

            var nationalParkObj = _mapper.Map<NationalPark>(nationalParkDTO);
            if (!_npRepository.UpdateNationalPark(nationalParkObj))
            {
                ModelState.AddModelError("", $"Something went wrong when updating {nationalParkObj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("parkId", Name = "DeleteNationPark")]
        public IActionResult DeleteNationPark(int parkId)
        {
            if (_npRepository.NationalParkExists(parkId))
            {
                return NotFound();
            }

            var nationalParkObj = _npRepository.GetNationalPark(parkId);
            if (!_npRepository.DeleteNationalPark(nationalParkObj))
            {
                ModelState.AddModelError("", $"Something went wrong when deleting {nationalParkObj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
