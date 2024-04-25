// CalculatorWeb.cs
using System;
using System.Collections.Generic;
using System.Linq;

public class CalculatorWeb
{
    private Stack<decimal> stack;

    public CalculatorWeb()
    {
        stack = new Stack<decimal>();
    }

    // Method to push a number onto the stack
    public void Push(decimal number)
    {
        stack.Push(number);
    }

    // Method to pop a number from the stack
    public decimal Pop()
    {
        if (stack.Count == 0)
        {
            throw new InvalidOperationException("Stack is empty");
        }
        return stack.Pop();
    }

    // Method to perform addition
    public void Add()
    {
        decimal num2 = Pop();
        decimal num1 = Pop();
        decimal result = num1 + num2;
        Push(result);
    }

    // Method to perform subtraction
    public void Subtract()
    {
        decimal num2 = Pop();
        decimal num1 = Pop();
        decimal result = num1 - num2;
        Push(result);
    }

    // Method to perform multiplication
    public void Multiply()
    {
        decimal num2 = Pop();
        decimal num1 = Pop();
        decimal result = num1 * num2;
        Push(result);
    }

    // Method to perform division
    public void Divide()
    {
        decimal num2 = Pop();
        decimal num1 = Pop();
        if (num2 == 0)
        {
            throw new InvalidOperationException("Cannot divide by zero");
        }
        decimal result = num1 / num2;
        Push(result);
    }

    // Method to clear the stack
    public void Clear()
    {
        stack.Clear();
    }

    // Method to get the current stack content
    public string GetStackContent()
    {
        return string.Join(" ", stack.Reverse());
    }
}


