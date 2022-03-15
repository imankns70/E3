using System;
using System.Collections.Generic;
using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using Microsoft.AspNetCore.Mvc;
namespace CommandService.Controllers
{

    [ApiController]
    [Route("api/c/[controller]")]
    public class PlatformsController : ControllerBase
    {

        private readonly ICommandRepo _repository;
        private readonly IMapper _mapper;
        public PlatformsController(ICommandRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
        {
            Console.WriteLine("--> Getting Platforms from commandsService");
            var platformItems = _repository.GetAllPlatforms();

            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platformItems));

        }

        [HttpPost]
        public ActionResult TestInboundConnection()
        {
            Console.WriteLine("--> Inbound Post # Command Service");

            return Ok("Inbound Test of from Platforms Controller");
        }
    }
}