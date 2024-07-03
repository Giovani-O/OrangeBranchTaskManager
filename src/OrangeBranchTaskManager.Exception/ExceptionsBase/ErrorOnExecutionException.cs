using System.Net;

namespace OrangeBranchTaskManager.Exception.ExceptionsBase;

public class ErrorOnExecutionException : OrangeBranchTaskManagerException
{
    private Dictionary<string, List<string>> Errors { get; set; }
    
    public override int StatusCode => (int)HttpStatusCode.BadRequest;
    
    public ErrorOnExecutionException(Dictionary<string, List<string>> errors) : base(string.Empty)
    {
        Errors = errors;
    }
    
    public override Dictionary<string, List<string>> GetErrors()
    {
        return Errors;
    }
}
