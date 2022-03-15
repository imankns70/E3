using System.Threading.Tasks;
using PlatformService.Dtos;

namespace PlatformService.SyncDataServivces.Http
{

    public interface ICommandDataClient
    {
        Task SendPlatformToCommand(PlatformReadDto plat);
    }
}