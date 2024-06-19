namespace OrangeBranchTaskManager.Communication.DTOs;

public class ResponseErrorDTO
{
    public List<string> ErrorMessages { get; set; }

    public ResponseErrorDTO(string errorMessage)
    {
        ErrorMessages = [errorMessage];
    }

    public ResponseErrorDTO(List<string> errorMessages)
    {
        ErrorMessages = errorMessages;
    }
}
