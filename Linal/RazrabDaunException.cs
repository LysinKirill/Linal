namespace Linal;

public class RazrabDaunException : Exception
{
    public RazrabDaunException() : base("The creator of this program is a moron!") {}

    public RazrabDaunException(string message) : base(message + "\nKeep in mind: the one who wrote this piece of junk is a moron!") {}
}