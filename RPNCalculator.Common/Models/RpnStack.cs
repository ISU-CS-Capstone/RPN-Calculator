namespace RPNCalculator.Common.Models;

public class RpnStack
{
    private Stack<double> _stack;
    public RpnStack()
    {
        _stack = new Stack<double>();
    }

    public void Push(double num)
    {
        _stack.Push(num);
    }

    public double Pop()
    {
        return _stack.Pop();
    }

    public int Count()
    {
        return _stack.Count;
    }
}