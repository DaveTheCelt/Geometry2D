using System;

namespace Geometry2D
{
    public readonly struct LineInfinite : ISpline
    {
        readonly v2 _a, _b, _o;
        public v2 A => _a;
        public v2 B => _b;
        public v2 Origin => _o;

        public LineInfinite(in v2 a, in v2 b)
        {
            _a = a;
            _b = b;
            _o = (_b + _a) / 2;
        }
        public LineInfinite(in LineSegment lineSegment)
        {
            _a = lineSegment.A;
            _b = lineSegment.B;
            _o = (_b + _a) / 2;
        }
        public LineInfinite translate(in v2 vector) => new(_a + vector, _b + vector);
        public LineInfinite setPosition(in v2 position) => translate(position - _o);
        public LineInfinite scale(in double factor) => throw new Exception("Cannot scale an infinite line");
        public LineInfinite rotate(in double radians)
        {
            var cos = Math.Cos(radians);
            var sin = Math.Sin(radians);
            return new(_a.rotateAround(_o, cos, sin), _b.rotateAround(_o, cos, sin));
        }
        public bool isLeft(in v2 point) => isLeft(point, _a, _b);
        public Quad calculateBounds() => new(new(double.NegativeInfinity, double.NegativeInfinity), new(double.PositiveInfinity, double.PositiveInfinity));
        public v2 tangent(in v2 _) => _b - _a;
        public v2 tangent(in double _) => _b - _a;
        public v2 normal(in v2 _) => tangent(_).perp();
        public v2 normal(in double _) => normal(_).perp();
        public bool overlapsEdge(in v2 pt, out v2 snapPt, in double threshold = 0.0001) => overlaps(pt, _a, _b, out snapPt, threshold);
        public v2 snapTo(in v2 pt) => snapTo(pt, _a, _b);
        public v2 lerp(in double t) => throw new Exception("Cannot lerp infinite lines");
        public double inverseLerp(in v2 pt) => throw new Exception("Cannot inverse lerp infinite lines");
        public double calculatePerimeter() => double.PositiveInfinity;
        public bool isBetween(in v2 pt)
        {
            var p = snapTo(pt);
            return isBetween(pt, out var _);
        }
        public bool isBetween(in v2 pt, out v2 snapPt, in double threshold = 0.0001)
        {
            snapPt = snapTo(pt);
            return true;
        }
        public bool isBetween(in v2 pt, out v2 snapPt, out double t, in double threshold = 0.0001)
        {
            snapPt = snapTo(pt);
            t = 1;
            return snapPt.lenSqr(pt) <= threshold * threshold;
        }
        IGeometry IGeometry.translate(in v2 vector) => translate(vector);
        IGeometry IGeometry.setPosition(in v2 position) => setPosition(position);
        IGeometry IGeometry.scale(in double factor) => scale(factor);
        IGeometry IGeometry.rotate(in double radians) => rotate(radians);

        //manipulations
        #region
        public void split(in double t, out ISpline splineA, out ISpline splineB) => throw new Exception("Cannot split an infinite line");
        public void split(in v2 pt, out ISpline splineA, out ISpline splineB) => throw new Exception("Cannot split an infinite line");
        public void slice(in double t1, in double t2, out ISpline splineA, out ISpline splineB, out ISpline splineC) => throw new Exception("Cannot slice an infinite line");
        public void slice(in v2 pt1, in v2 pt2, out ISpline splineA, out ISpline splineB, out ISpline splineC) => throw new Exception("Cannot slice an infinite line");
        public void truncate(in double t, out ISpline spline) => throw new Exception("Cannot truncate an infinite line");
        public void truncate(in v2 pt, out ISpline spline) => throw new Exception("Cannot truncate an infinite line");
        #endregion

        //static functions
        #region
        public static bool overlaps(in v2 p2, in v2 linePt1, in v2 linePt2, out v2 snapPt, in double threshold = 0.001f)
        {
            snapPt = snapTo(p2, linePt1, linePt2);
            return snapPt.lenSqr(p2) <= threshold * threshold;
        }
        public static v2 snapTo(in v2 point, in v2 p1, in v2 p2)
        {
            var dir = (p2 - p1).normalize();
            var v = point - p1;
            var d = v.dot(dir);
            return p1 + dir * d;
        }
        public static bool isLeft(in v2 point, in v2 p1, in v2 p2)
        {
            var dir = (p2 - p1).normalize();
            var ap = point - p1;
            var cross = ap.cross(dir);
            return Math.Sign(cross) > 0;
        }
        #endregion

    }
}