using System;
namespace Geometry2D
{
    public struct LineSegment : ISpline
    {
        readonly v2 _a, _b, _o;
        public v2 A => _a;
        public v2 B => _b;
        public v2 Origin => _o;

        public v2 this[int i]
        {
            get
            {
                if (i == 0)
                    return _a;
                if (i == 1)
                    return _b;
                throw new IndexOutOfRangeException();
            }
        }

        public LineSegment(in v2 a, in v2 b)
        {
            _a = a;
            _b = b;
            _o = (b + a) / 2;
        }
        public LineSegment(in double scale, in v2 origin, in double radianIncline)
        {
            var x = Math.Cos(radianIncline);
            var y = Math.Sin(radianIncline);
            v2 dir = new(x, y);
            _a = origin + dir * scale;
            _b = origin - dir * scale;
            _o = origin;
        }
        public LineSegment translate(in v2 vector) => new(_a + vector, _b + vector);
        public LineSegment setPosition(in v2 position) => translate(position - _o);
        public LineSegment scale(in double factor)
        {
            var a = (_a - _o).normalize() * factor + _o;
            var b = (_b - _o).normalize() * factor + _o;
            return new(a, b);
        }
        public LineSegment rotate(in double radians)
        {
            var cos = Math.Cos(radians);
            var sin = Math.Sin(radians);
            return new(_a.rotateAround(_o, cos, sin), _b.rotateAround(_o, cos, sin));
        }
        public v2 tangent(in v2 _) => _b - _a;
        public v2 tangent(in double _) => _b - _a;
        public v2 normal(in v2 _) => tangent(_).perp();
        public v2 normal(in double _) => tangent(_).perp();

        IGeometry IGeometry.translate(in v2 vector) => translate(vector);
        IGeometry IGeometry.setPosition(in v2 position) => setPosition(position);
        IGeometry IGeometry.scale(in double factor) => scale(factor);
        IGeometry IGeometry.rotate(in double radians) => rotate(radians);

        public v2 snapTo(in v2 pt) => snapTo(pt, _a, _b);

        public v2 lerp(in double t) => _a.lerp(_b, t);
        public v2 lerpLocal(in double t) => lerpLocal(_a, _b, t);
        public double inverseLerp(in v2 point) => inverseLerp(point, _a, _b);

        public bool isLeft(in v2 pt) => LineInfinite.isLeft(pt, _a, _b);

        public double calculatePerimeter() => _b.len(_a);

        public Quad calculateBounds() => new(_a.min(_b), _a.max(_b));

        public static v2 snapTo(in v2 point, in v2 a, in v2 b)
        {
            var proj = LineInfinite.snapTo(point, a, b);
            if (_isBetween(proj, a, b))
                return proj;
            return closest(point, a, b);
        }
        private static v2 closest(in v2 point, in v2 a, in v2 b) => a.lenSqr(point) <= b.lenSqr(point) ? a : b;
        public static bool isBetween(in v2 point, in v2 a, in v2 b)
        {
            var p = snapTo(point, a, b);
            return _isBetween(p, a, b);
        }
        public static bool isBetween(in v2 pt, in v2 a, in v2 b, out v2 snapPt, double threshold = 0.00001)
        {
            snapPt = snapTo(pt, a, b);
            if (snapPt.lenSqr(pt) > threshold * threshold)
                return false;

            return _isBetween(snapPt, a, b);
        }
        public static bool isBetween(in v2 pt, in v2 a, in v2 b, out v2 snapPt, out double t, double threshold = 0.00001)
        {
            bool state = isBetween(pt, a, b, out snapPt, threshold);
            t = inverseLerp(snapPt, a, b);
            return state;
        }
        public static double inverseLerp(in v2 point, in v2 a, in v2 b)
        {
            if (a == b)
                return 0;

            var proj = snapTo(point, a, b);
            return Math.Clamp(proj.lenSqr(a) / a.lenSqr(b), 0, 1);
        }
        public static v2 perp(v2 a, v2 b) => (b - a).perp();
        public static v2 lerpLocal(in v2 a, v2 b, double t)
        {
            b -= a;
            return b * t;
        }
        private static bool _isBetween(v2 point, in v2 lineP1, in v2 lineP2)
        {
            var d4 = lineP2.lenSqr(lineP1);
            var d2 = lineP1.lenSqr(point);
            var d3 = lineP2.lenSqr(point);

            return (d2 + d3 - d4) <= double.Epsilon * double.Epsilon;
        }
        public bool isBetween(in v2 pt) => isBetween(pt, _a, _b, out var _);
        public bool isBetween(in v2 pt, out v2 hitPt, in double threshold = 0.0001) => isBetween(pt, _a, _b, out hitPt, threshold);
        public bool isBetween(in v2 pt, out v2 snapPt, out double t, in double threshold = 0.0001) => isBetween(pt, _a, _b, out snapPt, out t, threshold);

        public void split(in double t, out ISpline splineA, out ISpline splineB)
        {
            var a = lerp(t);
            splineA = new LineSegment(_a, a);
            splineB = new LineSegment(a, _b);
        }

        public void split(in v2 pt, out ISpline splineA, out ISpline splineB)
        {
            var t = inverseLerp(pt);
            split(t, out splineA, out splineB);
        }

        public void slice(in double t1, in double t2, out ISpline splineA, out ISpline splineB, out ISpline splineC)
        {
            var a = lerp(t1);
            var b = lerp(t2);
            splineA = new LineSegment(_a, a);
            splineB = new LineSegment(a, b);
            splineC = new LineSegment(b, _b);
        }

        public void slice(in v2 pt1, in v2 pt2, out ISpline splineA, out ISpline splineB, out ISpline splineC)
        {
            var t1 = inverseLerp(pt1);
            var t2 = inverseLerp(pt2);
            slice(t1, t2, out splineA, out splineB, out splineC);
        }

        public void truncate(in double t, out ISpline spline)
        {
            var a = lerp(t);
            spline = new LineSegment(_a, a);
        }

        public void truncate(in v2 pt, out ISpline spline)
        {
            var t = inverseLerp(pt);
            truncate(t, out spline);
        }

        internal bool overlapsEdge(in v2 pt, out v2 snapPt, double threshold = 0.0001)
        {
            snapPt = pt;
            if (!isBetween(pt))
                return false;
            snapPt = snapTo(pt);
            return snapPt.lenSqr(pt) <= threshold * threshold;
        }
    }
}