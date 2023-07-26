using System;

namespace Geometry2D
{
    public readonly struct Quad : IShape
    {
        readonly v2 _min, _max, _o;
        public v2 Min => _min;
        public v2 Max => _max;
        public v2 TopLeft => new(_min.X, _max.Y);
        public v2 TopRight => _max;
        public v2 BottomLeft => _min;
        public v2 BottomRight => new(_max.X, _min.Y);

        public v2 Origin => _o;

        public v2 this[int i]
        {
            get
            {
                if (i == 0)
                    return Min;
                if (i == 1)
                    return TopLeft;
                if (i == 2)
                    return Max;
                if (i == 3)
                    return BottomRight;

                throw new IndexOutOfRangeException();
            }
        }

        private Quad(Quad quad, in v2 center)
        {
            var dif = quad.Origin - center;
            this = quad;
            _o = center;
            _min -= dif;
            _max -= dif;
        }

        public Quad(in v2 min, in v2 max)
        {
            _min = min;
            _max = max;
            _o = new((_max.X + _min.X) / 2, (_max.Y + _min.Y) / 2);
        }
        public Quad(in v2 center, in double spanX, in double spanY)
        {
            var span = new v2(spanX, spanY).abs();
            _min = center - span;
            _max = center + span;
            _o = center;
        }
        public Quad merge(in Quad quad) => new Quad(_min.min(quad.Min), _max.max(quad.Max));
        public bool overlaps(in Quad other) => Min.X <= other.Max.X && Max.X >= other.Min.X && Min.Y <= other.Max.Y && Max.Y >= other.Min.Y;
        public bool overlaps(in v2 pt) => pt.X >= Min.X && pt.X <= Max.X && pt.Y >= Min.Y && pt.Y <= Max.Y;
        public IGeometry translate(in v2 vector) => new Quad(_min + vector, _max + vector);
        public IGeometry setPosition(in v2 position) => new Quad(this, position);
        public IGeometry scale(in double factor) => new Quad(_o, factor, factor);
        public IGeometry rotate(in double radians)
        {
            var cos = Math.Cos(radians);
            var sin = Math.Sin(radians);
            var min = _min.rotateAround(_o, cos, sin);
            var max = _max.rotateAround(_o, cos, sin);
            return new Quad(min, max);
        }
        public Quad calculateBounds() => this;
        public v2 tangent(in v2 pt)
        {
            var dist = double.PositiveInfinity;
            v2 tangent = default;
            for (int i = 0; i < 4; i++)
            {
                var line = new LineSegment(this[i], this[(i + 1) % 4]);
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
            for (int i = 0; i < 4; i++)
            {
                var line = new LineSegment(this[i], this[(i + 1) % 4]);
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
        public double calculateArea() => Math.Abs(_min.X - _max.X) * Math.Abs(_min.Y - _max.Y);
        public double calculatePerimeter() => Math.Abs(_min.X - _max.X) * 2 + Math.Abs(_min.Y - _max.Y) * 2;
        public bool overlapsEdge(in v2 pt, out v2 snapPt, in double threshold = 0.0001)
        {
            snapPt = snapTo(pt);
            return snapPt.lenSqr(pt) <= threshold * threshold;
        }

        public v2 snapTo(in v2 pt)
        {
            double dist = double.PositiveInfinity;
            v2 result = pt;
            for (int i = 0; i < 4; i++)
            {
                var l = new LineSegment(this[i], this[(i + 1) % 4]);
                var p = l.snapTo(pt);
                var d = p.lenSqr(pt);
                if (dist >= d)
                {
                    dist = d;
                    result = p;
                }
            }
            return result;
        }
    }
}