namespace Linal;

public class UnluckyException : Exception
{
    public UnluckyException() : base("Unluckyyyy...")
    {
    }

    public UnluckyException(string message) : base(message)
    {
    }
}