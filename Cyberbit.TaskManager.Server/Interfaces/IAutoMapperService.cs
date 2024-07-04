using AutoMapper;

namespace Cyberbit.TaskManager.Server.Interfaces
{
    public interface IAutoMapperService
    {
        IMapper Mapper { get; }
    }
}
