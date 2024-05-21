using AutoMapper;
using OrangeBranchTaskManager.Api.DTOs;
using OrangeBranchTaskManager.Api.Models;

namespace OrangeBranchTaskManager.Api.Controllers.Mappings;

public class TaskDTOMappingProfile : Profile
{
    public TaskDTOMappingProfile()
    {
        CreateMap<TaskModel, TaskDTO>().ReverseMap();
    }
}
