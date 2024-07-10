namespace OrangeBranchTaskManager.Application.UseCases.CurrentUser;
public interface ICurrentUserService
{
    string? GetUsername();
    string? GetEmail();
}
