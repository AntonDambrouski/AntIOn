using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Workout.Api.ApiModels.SetDTOs;
using Workout.Core.Interfaces.Repositories;
using Workout.Core.Models;

namespace Workout.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SetsController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;

        public SetsController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _uof = unitOfWork;
        }

        [HttpGet]
        public async Task<IEnumerable<SetDisplayDTO>> Get()
        {
            var sets = await _uof.SetRepository.GetAllAsync();
            var setDTOs = _mapper.Map<IEnumerable<SetDisplayDTO>>(sets);
            return setDTOs;
        }

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<SetDetailedDTO>> Get(string id)
        {
            var set = await _uof.SetRepository.GetByIdAsync(id);
            if (set is null)
            {
                return NotFound();
            }

            var setDTO = _mapper.Map<SetDetailedDTO>(set);
            return setDTO;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SetCreateDTO item)
        {
            var set = _mapper.Map<Set>(item);
            await _uof.SetRepository.CreateAsync(set);
            return CreatedAtAction(nameof(Post), new { set.Id }, set);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] SetUpdateDTO item)
        {
            if (id != item.Id)
            {
                return BadRequest();
            }

            var set = await _uof.SetRepository.GetByIdAsync(id);
            if (set is null)
            {
                return NotFound();
            }

            set.Name = item.Name;
            set.Rest = item.Rest;
            set.Exercise = item.Exercise;
            set.Max = item.Max;
            set.Value = item.Value;
            set.ValueUnit = item.ValueUnit;

            await _uof.SetRepository.UpdateAsync(id, set);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var set = await _uof.SetRepository.GetByIdAsync(id);
            if (set is null)
            {
                return NotFound();
            }

            await _uof.SetRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
