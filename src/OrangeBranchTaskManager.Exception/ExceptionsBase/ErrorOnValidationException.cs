namespace OrangeBranchTaskManager.Exception.ExceptionsBase;

public class ErrorOnValidationException : OrangeBranchTaskManagerException
{
    public List<string> Errors { get; set; }
    public ErrorOnValidationException(List<string> errors)
    {
        Errors = errors;
    }
}
