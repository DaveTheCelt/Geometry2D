using System;

namespace Geometry2D
{
    public readonly struct Circle : IShape
    {
        readonly v2 _o;
        readonly double _r;
        public v2 Origin => _o;
        public double Radius => _r;
        public Circle(in v2 origin, in double radius)
        {
            _o = origin;
            _r = radius;
        }
        public double calculateArea() => Radius * Radius * Math.PI;
        IGeometry IGeometry.translate(in v2 vector) => translate(vector);
        public Circle translate(in v2 vector) => new(_o + vector, _r);
        IGeometry IGeometry.setPosition(in v2 position) => setPosition(position);
        public Circle setPosition(in v2 position) => new(position, _r);
        IGeometry IGeometry.scale(in double factor) => scale(factor);
        public Circle scale(in double factor) => new(_o, factor);
        IGeometry IGeometry.rotate(in double _) => rotate(_);
        public Circle rotate(in double _) => this;
        public v2 normal(in v2 pt) => pt - _o;
        public Quad calculateBounds() => new(_o - _r, _o + _r);
        public double calculatePerimeter() => _r * 2 * Math.PI;
        public bool overlapsEdge(in v2 pt, out v2 snapPoint, in double threshold = 0.0001)
        {
            var dist = pt.lenSqr(_o);
            snapPoint = snapTo(pt);
            return Math.Abs(dist - _r * _r) <= threshold * threshold;
        }
        public bool overlaps(in v2 pt) => pt.lenSqr(_o) <= _r * _r;
        public v2 snapTo(in v2 pt) => (pt - _o).normalize() * _r + _o;
        public int quadrant(in v2 pt) => quadrant(pt, _o);
        public double angle(in double t) => 2 * Math.PI * Math.Clamp(t, 0, 1);
        public double angle(in v2 point)
        {
            var p = _o - point;
            return Math.Atan2(p.Y, p.X);
        }


        #region static methods
        public static explicit operator Circle(Arc v) => new(v.Origin, v.Radius);
        public static implicit operator Circle(CirclePacket cp) => new(cp.Origin, cp.Radius);
        public static int quadrant(in v2 pt, in v2 _o)
        {
            // 1st quadrant
            if (pt.X > _o.X && pt.Y >= _o.Y)
                return 1;

            // 2nd quadrant
            if (pt.X <= _o.X && pt.Y > _o.Y)
                return 2;

            // 3rd quadrant
            if (pt.X < _o.X && pt.Y <= _o.Y)
                return 3;

            // 4th quadrant
            if (pt.X >= _o.X && pt.Y < _o.Y)
                return 4;

            return -1;
        }
        /// <summary>
        /// Create a circle from 3 points in a plane
        /// </summary>
        public static bool FromThreePoints(in v2 a, in v2 b, in v2 c, out CirclePacket circle)
        {
            var abMid = (a + b) / 2f;
            var bcMid = (b + c) / 2f;

            var abPerp = (b - a).perp();
            var bcPerp = (c - b).perp();

            var line1 = new LineInfinite(abMid, abPerp + abMid);
            var line2 = new LineInfinite(bcMid, bcMid + bcPerp);

            if (Intersections.Intersects(line1, line2, out var result) > 0)
            {
                var _o = result[0];
                circle = new(_o, a.len(a - _o));
                return true;
            }

            circle = default;
            return false;
        }
        /// <summary>
        /// Create a circle from a start point, a tangent and an end point.
        /// </summary>
        public static bool FromPointTangentPoint(in v2 a, in v2 tangentA, in v2 b, out CirclePacket circle)
        {
            var abMid = (a + b) / 2f;
            var abPerp = abMid.perp();
            var tanPerp = tangentA.perp();

            var line = new LineInfinite(abMid, abPerp + abMid);
            var line2 = new LineInfinite(abMid, tanPerp + abMid);

            if (Intersections.Intersects(line, line2, out var result) > 0)
            {
                var _o = result[0];
                double radius = a.len(_o);
                circle = new(_o, radius);
                return true;
            }
            circle = default;
            return false;
        }
        /// <summary>
        /// Create a circle from 2 points in a plane
        /// </summary>
        public static CirclePacket FromTwoPoints(in v2 a, in v2 b) => new((a + b) / 2f, b.len(a));
        #endregion
    }
}