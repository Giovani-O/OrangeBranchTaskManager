using AutoMapper;
using OrangeBranchTaskManager.Communication.DTOs;
using OrangeBranchTaskManager.Domain.Entities;


namespace OrangeBranchTaskManager.Application.Mappings;

public class TaskDTOMappingProfile : Profile
{
    public TaskDTOMappingProfile()
    {
        CreateMap<TaskModel, TaskDTO>().ReverseMap();
    }
}
