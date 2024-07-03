namespace OrangeBranchTaskManager.Exception.ExceptionsBase;

public abstract class OrangeBranchTaskManagerException : SystemException
{
    public OrangeBranchTaskManagerException(string message) : base(message)
    {   
        
    }
    
    public abstract int StatusCode { get;  }
    public abstract Dictionary<string, List<string>> GetErrors();
}
