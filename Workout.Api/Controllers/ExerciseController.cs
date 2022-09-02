using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Workout.Api.ApiModels.ExerciseDTOs;
using Workout.Core.Interfaces.Repositories;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Workout.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExerciseController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uof;

        public ExerciseController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _uof = unitOfWork;
        }

        [HttpGet]
        public async Task<IEnumerable<ExerciseDTO>> Get()
        {
            var exercises = await _uof.ExerciseRepository.GetAllAsync();
            var exerciseDTOs = _mapper.Map<IEnumerable<ExerciseDTO>>(exercises);
            return exerciseDTOs;
        }

        // GET api/<ExerciseController>/5
        [HttpGet("{id}")]
        public async Task<ExerciseDTO> Get(int id)
        {
            var exercise = await _uof.ExerciseRepository.GetByIdAsync(id);
            var exerciseDTO = _mapper.Map<ExerciseDTO>(exercise);
            return exerciseDTO;
        }

        // POST api/<ExerciseController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ExerciseController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ExerciseController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
