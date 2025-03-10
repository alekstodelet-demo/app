using Microsoft.AspNetCore.Mvc;
using WebApi.Dtos;
using Application.UseCases;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ServiceController : ControllerBase
    {
        private readonly ServiceUseCases _serviceUseCases;

        public ServiceController(ServiceUseCases serviceUseCases)
        {
            _serviceUseCases = serviceUseCases;
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> Get()
        {
            var response = await _serviceUseCases.GetAll();
            return Ok(response);
        }

        [HttpGet]
        [Route("GetOneById")]
        public async Task<IActionResult> GetOneById(int id)
        {
            var response = await _serviceUseCases.GetOneByID(id);
            return Ok(response);
        }

        [HttpGet]
        [Route("GetPaginated")]
        public async Task<IActionResult> GetPaginated(int pageSize, int pageNumber)
        {
            var response = await _serviceUseCases.GetPagniated(pageSize, pageNumber);
            return Ok(response);
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create(CreateServiceRequest requestDto)
        {
            var request = new Domain.Entities.Service
            {
                name = requestDto.name,
                short_name = requestDto.short_name,
                code = requestDto.code,
                description = requestDto.description,
                day_count = requestDto.day_count,
                workflow_id = requestDto.workflow_id,
                price = requestDto.price,
            };
            var response = await _serviceUseCases.Create(request);
            return Ok(response);
        }

        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> Update(UpdateServiceRequest requestDto)
        {
            var request = new Domain.Entities.Service
            {
                id = requestDto.id,
                name = requestDto.name,
                short_name = requestDto.short_name,
                code = requestDto.code,
                description = requestDto.description,
                day_count = requestDto.day_count,
                workflow_id = requestDto.workflow_id,
                price = requestDto.price,
            };
            var response = await _serviceUseCases.Update(request);
            return Ok(response);
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            await _serviceUseCases.Delete(id);
            return Ok();
        }
    }
}
