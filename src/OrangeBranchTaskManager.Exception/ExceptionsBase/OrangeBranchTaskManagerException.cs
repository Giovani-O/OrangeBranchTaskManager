namespace OrangeBranchTaskManager.Exception.ExceptionsBase;

public class OrangeBranchTaskManagerException : SystemException
{
    public OrangeBranchTaskManagerException()
    {
        
    }

    public OrangeBranchTaskManagerException(string ErrorMessage)
    {
        ErrorMessage = ErrorMessage;
    }
}
