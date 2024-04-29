// CalculatorWeb.cs
using System;
using System.Collections.Generic;
using System.Linq;


/* This is the original backend that I had created for the web version of the calculator. We ended up deciding to use a common back end for web, mobile, and desktop.
 * `Brek Cranney */

public class CalculatorWeb
{
    private Stack<decimal> stack;

    public CalculatorWeb()
    {
        stack = new Stack<decimal>();
    }

    // Method to push a number onto the stack
    // Brek Cranney
    public void Push(decimal number)
    {
        stack.Push(number);
    }

    // Method to pop a number from the stack
    // Brek Cranney
    public decimal Pop()
    {
        if (stack.Count == 0)
        {
            throw new InvalidOperationException("Stack is empty");
        }
        return stack.Pop();
    }

    // Method to perform addition
    // Brek Cranney
    public void Add()
    {
        decimal num2 = Pop();
        decimal num1 = Pop();
        decimal result = num1 + num2;
        Push(result);
    }

    // Method to perform subtraction
    // Brek Cranney
    public void Subtract()
    {
        decimal num2 = Pop();
        decimal num1 = Pop();
        decimal result = num1 - num2;
        Push(result);
    }

    // Method to perform multiplication
    // Brek Cranney
    public void Multiply()
    {
        decimal num2 = Pop();
        decimal num1 = Pop();
        decimal result = num1 * num2;
        Push(result);
    }

    // Method to perform division
    // Brek Cranney
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
    // Brek Cranney
    public void Clear()
    {
        stack.Clear();
    }

    // Method to get the current stack content
    // Brek Cranney
    public string GetStackContent()
    {
        return string.Join(" ", stack.Reverse());
    }
}


