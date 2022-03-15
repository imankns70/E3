using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.AsyncDataServices;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;
using PlatformService.SyncDataServivces.Http;

namespace PlatformService.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepo _repsitory;
        private readonly IMapper _mapper;
        private readonly ICommandDataClient _commanDataClient;
        private readonly IMessageBusClient _messageBusClient;

        public PlatformsController(IPlatformRepo repository, IMapper mapper,
            ICommandDataClient commanDataClient,
            IMessageBusClient messageBusClient)
        {
            _repsitory = repository;
            _mapper = mapper;
            _commanDataClient = commanDataClient;
            _messageBusClient = messageBusClient;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
        {
            Console.WriteLine("..> Getting Platform");
            var platformItesm = _repsitory.GetAllPlatforms();

            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platformItesm));

        }

        [HttpGet("{id}", Name = "GetPlatformById")]
        public ActionResult<PlatformReadDto> GetPlatformById(int id)
        {
            var platform = _repsitory.GetPlatformById(id);
            if (platform != null)
            {
                return Ok(_mapper.Map<PlatformReadDto>(platform));

            }

            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<PlatformReadDto>> CreatePlatform(PlatformCreateDto viewModel)
        {

            var platformModel = _mapper.Map<Platform>(viewModel);
            _repsitory.CreatePlatform(platformModel);
            _repsitory.SaveChanges();
            var platformReadDto = _mapper.Map<PlatformReadDto>(platformModel);

            // Send Sync Message
            try
            {

                await _commanDataClient.SendPlatformToCommand(platformReadDto);
                Console.WriteLine("sending was ok ");
            }
            catch (System.Exception ex)
            {

                Console.WriteLine($"--> Could not send synchronously:{ex.Message}");
            }

            // Send Async Message
            try
            {
                var platformPublishedDto = _mapper.Map<PlatformPublishedDto>(platformReadDto);
                platformPublishedDto.Event = "Platform_Published";
                _messageBusClient.PublishNewPlatform(platformPublishedDto);
            }
            catch (Exception ex)
            {

                Console.WriteLine($"--> Could not send Asynchronously:{ex.Message}");

            }
            return CreatedAtRoute(nameof(GetPlatformById), new { id = platformReadDto.Id }, platformReadDto);
        }

    }
}