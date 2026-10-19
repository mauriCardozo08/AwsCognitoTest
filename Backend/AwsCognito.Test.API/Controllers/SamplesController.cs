using AwsCognito.Test.API.Models;
using AwsCognito.Test.Application.Abstractions.Services;
using AwsCognito.Test.Application.DTOs.Samples;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AwsCognito.Test.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class SamplesController : ControllerBase
    {
        private readonly ISampleService _sampleService;

        public SamplesController(ISampleService sampleService)
        {
            _sampleService = sampleService;
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(SampleHttpResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<SampleHttpResponse>> GetById(Guid id, CancellationToken cancellationToken)
        {
            var result = await _sampleService.GetById(id, cancellationToken);
            return Ok(ToResponse(result));
        }

        [HttpPost]
        [ProducesResponseType(typeof(SampleHttpResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<SampleHttpResponse>> Create(
            [FromBody] CreateSampleHttpRequest request,
            CancellationToken cancellationToken)
        {
            var result = await _sampleService.Create(new CreateSampleRequestDTO(request.Name), cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, ToResponse(result));
        }

        private static SampleHttpResponse ToResponse(SampleDTO dto) => new()
        {
            Id = dto.Id,
            Name = dto.Name,
            CreatedAtUtc = dto.CreatedAtUtc
        };
    }
}
