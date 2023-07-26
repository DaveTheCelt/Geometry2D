namespace Geometry2D
{
    public static partial class Intersections
    {
        static int _pp(in Polygon p1, in Polygon p2)
        {
            for (int i = 0; i < p1.Count; i++)
            {
                LineSegment l1 = new LineSegment(p1[i], p1[(i + 1) % p1.Count]);
                for (int j = 0; j < p2.Count; j++)
                {
                    LineSegment l2 = new LineSegment(p2[j], p2[(j + 1) % p2.Count]);
                    _ll(l1, l2);
                }
            }
            return _buffer.Count;
        }

        static int _pt(in Polygon p, in Triangle t)
        {
            for (int i = 0; i < p.Count; i++)
            {
                LineSegment l1 = new(p[i], p[(i + 1) % p.Count]);
                for (int j = 0; j < 3; j++)
                {
                    LineSegment l2 = new(t[j], t[(j + 1) % 3]);
                    _ll(l1, l2);
                }
            }
            return _buffer.Count;
        }

        static int _pq(in Polygon p, in Quad q)
        {
            for (int i = 0; i < p.Count; i++)
            {
                LineSegment l1 = new LineSegment(p[i], p[(i + 1) % p.Count]);
                for (int j = 0; j < 4; j++)
                {
                    LineSegment l2 = new LineSegment(q[j], q[(j + 1) % 4]);
                    _ll(l1, l2);
                }
            }
            return _buffer.Count;
        }

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
