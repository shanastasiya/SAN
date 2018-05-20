using System;
using СНАУ;
public static class Derivative
{
    static double dx = 0.000001;
    public static  double first(double a,Form1.one only_one)
    {
        return (only_one(a + dx) - only_one(a)) / dx;
    }
    public static double second(double a, double first, Form1.one only_one)
    {
        return (first - (only_one(a) - only_one(a - dx)) / dx) / dx;
    }
}