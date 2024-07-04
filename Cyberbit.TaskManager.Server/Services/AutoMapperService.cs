using AutoMapper;
using Cyberbit.TaskManager.Server.Interfaces;
using Cyberbit.TaskManager.Server.Models;
using Cyberbit.TaskManager.Server.Models.Dto;
using System.Linq;

namespace Cyberbit.TaskManager.Server.Services
{
    public class AutoMapperService : IAutoMapperService
    {
        public IMapper Mapper { get; }

        public AutoMapperService()
        {
            var config = new MapperConfiguration(InitMapper);
            Mapper = new Mapper(config);
            config.AssertConfigurationIsValid();
        }

        private void InitMapper(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<User, UserDto>()
                .ForSourceMember(user => user.IsDeleted, opt => opt.DoNotValidate())
                .ForSourceMember(user => user.Password, opt => opt.DoNotValidate())
                .ReverseMap()
                .ForMember(user => user.IsDeleted, opt => opt.Ignore())
                .ForMember(user => user.Password, opt => opt.Ignore());

            cfg.CreateMap<Models.Task, TaskDto>()
                .ForMember(dest => dest.CategoryIds, opt => opt.MapFrom(src => src.TaskCategories.Select(tc => tc.CategoryId)))
                .ReverseMap()
                .ForMember(t => t.CreatedByUserId, opt => opt.MapFrom(src => src.CreatedByUserId))
                .ForMember(t => t.CreatedByUser, opt => opt.Ignore())
                .ForMember(t => t.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(t => t.User, opt => opt.Ignore())
                .ForMember(t => t.TaskCategories, opt => opt.Ignore());
                ;

            cfg.CreateMap<Category, CategoryDto>()
                .ForSourceMember(user => user.IsDeleted, opt => opt.DoNotValidate())
                .ReverseMap()
                .ForMember(user => user.IsDeleted, opt => opt.Ignore());
        }
    }
}
