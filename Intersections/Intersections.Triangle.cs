namespace Geometry2D
{
    public static partial class Intersections
    {
        static int _tt(in Triangle t1, in Triangle t2)
        {
            for (int i = 0; i < 3; i++)
            {
                LineSegment l1 = new(t1[i], t1[(i + 1) % 3]);
                for (int j = 0; j < 3; j++)
                {
                    LineSegment l2 = new(t2[j], t2[(j + 1) % 3]);
                    _ll(l1, l2);
                }
            }
            return _buffer.Count;
        }
        static int _tq(in Triangle t, in Quad q)
        {
            for (int i = 0; i < 3; i++)
            {
                LineSegment l1 = new(t[i], t[(i + 1) % 3]);
                for (int j = 0; j < 4; j++)
                {
                    LineSegment l2 = new(q[j], q[(j + 1) % 4]);
                    _ll(l1, l2);
                }
            }
            return _buffer.Count;
        }
    }
}
