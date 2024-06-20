namespace OrangeBranchTaskManager.Exception.ExceptionsBase;

public class ErrorOnExecutionException : OrangeBranchTaskManagerException
{
    public List<string> Errors { get; set; }
    public ErrorOnExecutionException(List<string> errors)
    {
        Errors = errors;
    }
}
