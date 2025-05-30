﻿using System;

namespace C__NET5;

public class Shape
{
    public const double PI = Math.PI;
    protected double x, y;
        
    public Shape()
    {
    }
        
    public Shape(double x, double y)
    {
        this.x = x;
        this.y = y;
    }

    public virtual double Area() // не вызовется, все Area наследников вызовутся
    {
        return x * y;
    }
}

public class Circle : Shape
{
    public Circle(double r) : base(r, 0)
    {
    }

    public override double Area()
    {
        return PI * x * x;
    }
}

class Sphere : Shape
{
    public Sphere(double r) : base(r, 0)
    {
    }

    public override double Area()
    {
        return 4 * PI * x * x;
    }
}

class Cylinder : Shape
{
    public Cylinder(double r, double h) : base(r, h)
    {
    }

    public override double Area()
    {
        return 2 * PI * x * x + 2 * PI * x * y;
    }
}


