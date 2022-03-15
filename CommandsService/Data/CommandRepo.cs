using CommandsService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommandsService.Data
{
    public class CommandRepo : ICommandRepo
    {
        private readonly AppDbContext _context;
        public CommandRepo(AppDbContext context)
        {
            _context = context;
        }
        public void CreateCommand(int platformId, Command command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }
            command.PlatformId = platformId;
            _context.Commands.Add(command);
        }

        public void CreatePlatform(Platform platform)
        {
            if (platform== null)
            {
                throw new ArgumentNullException(nameof(platform));
            }
            _context.Platforms.Add(platform);
        }

        public bool ExternalPlatfromExists(int externalPlatformId)
        {
            return _context.Platforms.Any(a => a.ExternalId == externalPlatformId);
        }

        public IEnumerable<Platform> GetAllPlatforms()
        {
            return _context.Platforms.ToList();
        }

        public Command GetCommand(int platformId, int commandId)
        {
            return _context.Commands.Where(a => a.PlatformId == platformId && a.Id == commandId).FirstOrDefault();

        }

        public IEnumerable<Command> GetCommandsFormPlatform(int platformId)
        {
            return _context.Commands.Where(a => a.PlatformId == platformId).OrderBy(a => a.Platform.Name);
        }

        public bool PlatfromExists(int platformId)
        {
            return _context.Platforms.Any(a => a.Id == platformId);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}
