using CommandsService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommandsService.Data
{
    public interface ICommandRepo
    {
        bool SaveChanges();

        //Platforms
        IEnumerable<Platform> GetAllPlatforms();
        void CreatePlatform(Platform platform);
        bool PlatfromExists(int platformId);
        bool ExternalPlatfromExists(int externalPlatformId);

        //Commands
        IEnumerable<Command> GetCommandsFormPlatform(int platformId);
        Command GetCommand(int platformId, int commandId);
        void CreateCommand(int platformId, Command command); 


    }
}
