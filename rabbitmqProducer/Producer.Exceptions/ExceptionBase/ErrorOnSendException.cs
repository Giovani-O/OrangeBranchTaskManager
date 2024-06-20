namespace Producer.Exceptions.ExceptionBase;
public class ErrorOnSendException : ProducerException
{
    public Dictionary<string, List<string>> Errors { get; set; }
    public ErrorOnSendException(Dictionary<string, List<string>> errors)
    {
        Errors = errors;
    }
}
