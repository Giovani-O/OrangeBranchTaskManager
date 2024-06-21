namespace OrangeBranchTaskManager.Exception.ExceptionsBase;
public class ErrorOnSendMessageException : OrangeBranchTaskManagerException
{
    public Dictionary<string, List<string>> Errors { get; set; }
    public ErrorOnSendMessageException(Dictionary<string, List<string>> errors)
    {
        Errors = errors;
    }
}
