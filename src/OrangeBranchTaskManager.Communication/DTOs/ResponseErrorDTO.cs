namespace OrangeBranchTaskManager.Communication.DTOs;

public class ResponseErrorDTO
{
    public Dictionary<string, List<string>> ErrorMessages { get; set; }

    public ResponseErrorDTO(string errorMessage)
    {
        ErrorMessages = new Dictionary<string, List<string>>() 
        {
            { "Error", new List<string> { errorMessage } }
        };
    }

    public ResponseErrorDTO(Dictionary<string, List<string>> errorMessages)
    {
        ErrorMessages = errorMessages;
    }
}
