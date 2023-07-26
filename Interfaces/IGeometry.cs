namespace Geometry2D
{
    public interface IGeometry
    {
        v2 Origin { get; }
        v2 tangent(in v2 pt) => normal(pt).perp();
        v2 normal(in v2 pt);
        IGeometry translate(in v2 vector);
        IGeometry setPosition(in v2 position);
        IGeometry scale(in double factor);
        IGeometry rotate(in double radians);
        Quad calculateBounds();
        bool overlapsEdge(in v2 pt, out v2 snapPt, in double threshold = 0.0001)
        {
            snapPt = snapTo(pt);
            return snapPt.lenSqr(pt) <= threshold * threshold;
        }
        public v2 snapTo(in v2 pt);
        public double calculatePerimeter();
    }
}