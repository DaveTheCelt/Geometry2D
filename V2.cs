using System;

namespace Geometry2D
{
    public readonly struct v2 : IEquatable<v2>
    {
        readonly double _x, _y;
        public double X => _x;
        public double Y => _y;

        public v2(in double value)
        {
            _x = value;
            _y = value;
        }
        public v2(in double x, in double y)
        {
            _x = x;
            _y = y;
        }
        public v2 perp() => new(Y, -X);
        public v2 flipXY() => new(Y, X);
        /// <summary>
        /// Clamp the magnitude of the vector
        /// </summary>
        public v2 clampMagnitude(in double magnitude)
        {
            double length = this.magnitude();
            double x = (X / length) * magnitude;
            double y = (Y / length) * magnitude;
            return new v2(x, y);
        }
        /// <summary>
        /// Clamp the components of the vector between min and max
        /// </summary>
        public v2 clampComponents(in v2 min, in v2 max)
        {
            double x = Math.Clamp(X, min.X, max.Y);
            double y = Math.Clamp(Y, min.Y, max.Y);
            return new(x, y);
        }
        public v2 setLength(in double length) => normalize() * length;
        public v2 min(in v2 other) => min(this, other);
        public v2 max(in v2 other) => max(this, other);
        public v2 lerp(in v2 other, in double step) => lerp(this, other, step);
        public double inverseLerp(in v2 b, in v2 c) => inverseLerp(this, b, c);
        public v2 saturateComponents()
        {
            double x = Math.Clamp(_x, 0, 1);
            double y = Math.Clamp(_y, 0, 1);
            return new(x, y);
        }
        public v2 normalize()
        {
            double length = magnitude();
            return new(X / length, Y / length);
        }
        public v2 rotateAround(in v2 origin, in double radians) => rotateAround(origin, Math.Cos(radians), Math.Sin(radians));
        public v2 rotateAround(in v2 origin, in double cos, in double sin)
        {
            var x = _x - origin.X;
            var y = _y - origin.Y;
            var xnew = x * cos - y * sin;
            var ynew = x * sin + y * cos;
            return new(xnew + origin.X, ynew + origin.Y);
        }
        public v2 abs() => new(Math.Abs(X), Math.Abs(Y));
        public v2 to(in v2 other) => other - this;
        public v2 from(in v2 other) => this - other;
        public int isOrthoganol(in v2 other) => isOrthoganol(this, other);
        public int isParallel(in v2 other) => isParallel(this, other);
        public double dot(in v2 other) => dot(this, other);
        public double cross(in v2 other) => cross(this, other);
        public double magnitude() => (double)Math.Sqrt(magnitudeSqr());
        public double magnitudeSqr() => (double)(X * X + Y * Y);
        public double len(in v2 a) => (a - this).magnitude();
        public double lenSqr(in v2 a) => (a - this).magnitudeSqr();
        public bool Equals(v2 other) => other.X == X && other.Y == Y;
        public override bool Equals(object obj) => obj is v2 point && Equals(point);
        public override int GetHashCode() => X.GetHashCode() + 3 * Y.GetHashCode();
        public override string ToString() => "(" + X + " , " + Y + ")";
        public static v2 operator +(v2 a, v2 b) => new(a.X + b.X, a.Y + b.Y);
        public static v2 operator +(v2 a, double b) => new(a.X + b, a.Y + b);
        public static v2 operator +(double b, v2 a) => new(a.X + b, a.Y + b);
        public static v2 operator -(v2 a, v2 b) => new(a.X - b.X, a.Y - b.Y);
        public static v2 operator -(v2 a, double b) => new(a.X - b, a.Y - b);
        public static v2 operator -(double b, v2 a) => new(a.X - b, a.Y - b);
        public static v2 operator *(v2 a, v2 b) => new(a.X * b.X, a.Y * b.Y);
        public static v2 operator *(v2 a, double b) => new(a.X * b, a.Y * b);
        public static v2 operator *(double b, v2 a) => new(a.X * b, a.Y * b);
        public static v2 operator /(v2 a, v2 b) => new(a.X / b.X, a.Y / b.Y);
        public static v2 operator /(v2 a, double b) => new(a.X / b, a.Y / b);
        public static v2 operator /(double b, v2 a) => new(a.X / b, a.Y / b);
        public static v2 operator %(v2 a, v2 b) => new(a.X % b.X, a.Y % b.Y);
        public static v2 operator %(v2 a, double b) => new(a.X % b, a.Y % b);
        public static v2 operator %(double b, v2 a) => new(a.X % b, a.Y % b);
        public static v2 operator ^(v2 a, v2 b) => new(Math.Pow(a.X, b.X), Math.Pow(a.Y, b.Y));
        public static v2 operator ^(v2 a, double b) => new(Math.Pow(a.X, b), Math.Pow(a.Y, b));
        public static v2 operator ^(double b, v2 a) => new(Math.Pow(a.X, b), Math.Pow(a.Y, b));
        public static bool operator ==(v2 a, v2 b) => a.X == b.X && a.Y == b.Y;
        public static bool operator !=(v2 a, v2 b) => !(a == b);
        public static double dot(v2 a, v2 b) => a.X * b.X + a.Y * b.Y;
        public static double cross(v2 a, v2 b) => a.X * b.Y - a.Y * b.X;
        public static int isParallel(v2 a, v2 b) => Math.Abs(cross(a, b)) <= double.Epsilon ? 1 : 0;
        public static int isOrthoganol(v2 a, v2 b) => Math.Abs(dot(a, b)) <= double.Epsilon ? 1 : 0;
        public static v2 lerp(in v2 a, in v2 b, in double step)
        {
            double x = a.X + (b.X - a.X) * step;
            double y = a.Y + (b.Y - a.Y) * step;
            return new(x, y);
        }
        public static double inverseLerp(v2 a, v2 b, v2 point)
        {
            if (a == b)
                return 0;
            return a.lenSqr(point) / a.lenSqr(b);
        }
        public static double len(in v2 a, in v2 b) => (b - a).magnitude();
        public static double lenSqr(in v2 a, in v2 b) => (b - a).magnitudeSqr();
        public static v2 min(in v2 a, in v2 b) => new(Math.Min(a.X, b.X), Math.Min(a.Y, b.Y));
        public static v2 max(in v2 a, in v2 b) => new(Math.Max(a.X, b.X), Math.Max(a.Y, b.Y));
    }
}