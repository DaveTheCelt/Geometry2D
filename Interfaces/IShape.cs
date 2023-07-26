namespace Geometry2D
{
    public interface IShape : IGeometry
    {
        double calculateArea();
        bool overlaps(in v2 pt);
    }
}