namespace Geometry2D
{
    public interface ISpline : IGeometry
    {
        v2 A { get; }
        v2 B { get; }
        public v2 lerp(in double t);
        public double inverseLerp(in v2 pt);
        v2 tangent(in double t) => tangent(lerp(t));
        v2 normal(in double t) => normal(lerp(t));
        bool isBetween(in v2 pt);
        bool isBetween(in v2 pt, out v2 snapPt, in double threshold = 0.0001);
        bool isBetween(in v2 pt, out v2 snapPt, out double t, in double threshold = 0.0001);
        void split(in double t, out ISpline splineA, out ISpline splineB);
        void split(in v2 pt, out ISpline splineA, out ISpline splineB);
        void slice(in double t1, in double t2, out ISpline splineA, out ISpline splineB, out ISpline splineC);
        void slice(in v2 pt1, in v2 pt2, out ISpline splineA, out ISpline splineB, out ISpline splineC);
        void truncate(in double t, out ISpline spline);
        void truncate(in v2 pt, out ISpline spline);
        bool isLeft(in v2 pt);
    }
}