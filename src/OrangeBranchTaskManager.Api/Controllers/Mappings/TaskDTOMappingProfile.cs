using AutoMapper;
using OrangeBranchTaskManager.Communication.DTOs;
using OrangeBranchTaskManager.Domain.Entities;


namespace OrangeBranchTaskManager.Api.Controllers.Mappings;

public class TaskDTOMappingProfile : Profile
{
    public TaskDTOMappingProfile()
    {
        CreateMap<TaskModel, TaskDTO>().ReverseMap();
    }
}
