using System;

public static class functions
{
    public static double func1(double x)
    {
        return x * x + 8 * x - 5;
    }
    public static double func2(double x)
    {
        return Math.Exp(-x * x) - (x - 1) * (x - 1);
    }
    public static double func3(double x)
    {
        return x * x - Math.Cos(x);
    }
}