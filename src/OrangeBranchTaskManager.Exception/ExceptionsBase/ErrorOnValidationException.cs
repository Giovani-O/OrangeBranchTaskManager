namespace OrangeBranchTaskManager.Exception.ExceptionsBase;

public class ErrorOnValidationException : OrangeBranchTaskManagerException
{
    public Dictionary<string, List<string>> Errors { get; set; }
    public ErrorOnValidationException(Dictionary<string, List<string>> errors)
    {
        Errors = errors;
    }
}
