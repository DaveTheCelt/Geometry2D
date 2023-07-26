using System;

namespace Geometry2D
{
    public readonly struct Triangle : IShape
    {
        readonly v2 _a, _b, _c, _o;
        public v2 A => _a;
        public v2 B => _b;
        public v2 C => _c;
        public v2 Origin => _o;
        public v2 this[int i] => i == 0 ? _a : i == 1 ? _b : i == 2 ? _c : throw new IndexOutOfRangeException();
        public Triangle(in v2 a, in v2 b, in v2 c, in double scale)
        {

            _o = (a + b + c) / 3;
            var lenA = a.len(_o);
            var lenB = b.len(_o);
            var lenC = c.len(_o);

            _a = _o + (a - _o).normalize() * scale * lenA;
            _b = _o + (b - _o).normalize() * scale * lenB;
            _c = _o + (c - _o).normalize() * scale * lenC;
        }
        public Triangle(in v2 a, in v2 b, in v2 c)
        {
            _a = a;
            _b = b;
            _c = c;
            _o = (_a + _b + _c) / 3;
        }
        private Triangle(in v2 a, in v2 b, in v2 c, in v2 o)
        {
            _a = a;
            _b = b;
            _c = c;
            _o = o;
        }
        public v2 nearestVertex(in v2 pt)
        {
            var distA = _a.lenSqr(pt);
            var distB = _b.lenSqr(pt);
            var distC = _c.lenSqr(pt);

            if (distA <= distB)
                return distC <= distA ? _c : _a;
            else
                return distC <= distB ? _c : _b;
        }
        public v2 snapTo(in v2 pt)
        {
            var p1 = LineSegment.snapTo(pt, _a, _b);
            var p2 = LineSegment.snapTo(pt, _a, _c);
            var p3 = LineSegment.snapTo(pt, _b, _c);

            var distA = pt.lenSqr(p1);
            var distB = pt.lenSqr(p2);
            var distC = pt.lenSqr(p3);

            if (distA <= distB)
                return distC <= distA ? p3 : p1;
            else
                return distC <= distB ? p3 : p2;
        }
        public double calculateArea()
        {
            var sum = (_b.X - _a.X) * (_b.Y + _a.Y);
            sum += (_c.X - _b.X) * (_c.Y + _b.Y);
            return Math.Abs(sum) * 0.5;
        }
        public Quad calculateBounds()
        {
            var min = _a.min(_b.min(_c));
            var max = _a.max(_b.max(_c));
            return new(min, max);
        }
        public double calculatePerimeter() => (_a - _b).magnitude() + (_b - _c).magnitude() + (_c - _a).magnitude();
        public bool isEquilateral() => _a == _b && _a == _c;
        public bool isIsosceles() => _a == _c || _a == _c || _b == _c;
        public bool isScalene() => _a != _b && _a != _c && _b != _c;
        public bool isClockwise() => (_b - _a).cross(_c - _a) > 0;
        public Triangle translate(in v2 vector) => new(_a + vector, _b + vector, _c + vector, _o + vector);
        public Triangle setPosition(in v2 vector)
        {
            return translate(vector - _o);
        }
        public Triangle scale(in double factor)
        {
            var len = _a.len(_o);
            var frac = factor / len;
            var a = _o + (_a - _o) * frac;
            var b = _o + (_b - _o) * frac;
            var c = _o + (_c - _o) * frac;
            return new(a, b, c, _o);
        }
        public Triangle rotate(in double radians)
        {
            var cos = Math.Cos(radians);
            var sin = Math.Sin(radians);
            return new(_a.rotateAround(_o, cos, sin), _b.rotateAround(_o, cos, sin), _c.rotateAround(_o, cos, sin));
        }
        public v2 tangent(in v2 pt)
        {
            var dist = double.PositiveInfinity;
            v2 tangent = default;
            for (int i = 0; i < 3; i++)
            {
                var line = new LineSegment(this[i], this[(i + 1) % 3]);
                var p = line.snapTo(pt);
                var d = p.lenSqr(pt);
                if (dist >= d)
                {
                    dist = d;
                    tangent = line.tangent(pt);
                }
            }
            return tangent;
        }

        public v2 normal(in v2 pt)
        {
            var dist = double.PositiveInfinity;
            v2 normal = default;
            for (int i = 0; i < 3; i++)
            {
                var line = new LineSegment(this[i], this[(i + 1) % 3]);
                var p = line.snapTo(pt);
                var d = p.lenSqr(pt);
                if (dist >= d)
                {
                    dist = d;
                    normal = line.normal(pt);
                }
            }
            return normal;
        }

        IGeometry IGeometry.translate(in v2 vector) => translate(vector);
        IGeometry IGeometry.setPosition(in v2 position) => setPosition(position);
        IGeometry IGeometry.scale(in double factor) => scale(factor);
        IGeometry IGeometry.rotate(in double radians) => rotate(radians);

        public bool overlapsEdge(in v2 pt, out v2 snapPt, in double threshold = 0.0001)
        {
            snapPt = snapTo(pt);
            return snapPt.lenSqr(pt) <= threshold * threshold;
        }
        public bool overlaps(in v2 pt)
        {
            v2 max = this[0];
            for (int i = 1; i < 3; i++)
                max = max.max(this[i]);
            max = new(max.X + 100, max.Y + 100);

            var line1 = new LineSegment(pt, max);

            int count = 0;
            for (int i = 0; i < 3; i++)
            {
                var line2 = new LineSegment(this[i], this[(i + 1) % 3]);
                count += Intersections.Intersects(line1, line2, out _);
            }
            return count % 2 != 0;
        }
    }
}