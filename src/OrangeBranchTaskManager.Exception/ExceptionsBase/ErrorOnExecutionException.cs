namespace OrangeBranchTaskManager.Exception.ExceptionsBase;

public class ErrorOnExecutionException : OrangeBranchTaskManagerException
{
    public Dictionary<string, List<string>> Errors { get; set; }
    public ErrorOnExecutionException(Dictionary<string, List<string>> errors)
    {
        Errors = errors;
    }
}
