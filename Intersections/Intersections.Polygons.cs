namespace Geometry2D
{
    public static partial class Intersections
    {
        static int _qq(in Quad q1, in Quad q2)
        {
            for (int i = 0; i < 4; i++)
            {
                LineSegment l1 = new LineSegment(q1[i], q1[(i + 1) % 4]);
                for (int j = 0; j < 4; j++)
                {
                    LineSegment l2 = new LineSegment(q2[j], q2[(j + 1) % 4]);
                    _ll(l1, l2);
                }
            }
            return _buffer.Count;
        }
    }
}
