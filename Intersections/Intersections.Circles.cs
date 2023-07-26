namespace Geometry2D
{
    public static partial class Intersections
    {
        static int _cc(in Circle c1, in Circle c2)
        {
            int count = math._cc(c1, c2, out var pt1, out var pt2);
            if (count > 0)
                _buffer.Add(pt1);
            if (count > 1)
                _buffer.Add(pt2);
            return _buffer.Count;
        }
        static int _cl(in Circle c, in LineSegment l)
        {
            int count = math._cli(c, new(l), out var pt1, out var pt2);
            if (count > 0)
            {
                if (l.isBetween(pt1))
                    _buffer.Add(pt1);
                if (count > 1 && l.isBetween(pt2))
                    _buffer.Add(pt2);
            }
            return _buffer.Count;
        }
        static int _cli(in Circle c, in LineInfinite l)
        {
            int count = math._cli(c, l, out var pt1, out var pt2);
            if (count > 0)
            {
                _buffer.Add(pt1);
                if (count > 1)
                    _buffer.Add(pt2);
            }
            return _buffer.Count;
        }
        static int _cp(in Circle c, in Polygon p)
        {
            for (int i = 0; i < p.Count; i++)
            {
                LineSegment l = new LineSegment(p[i], p[(i + 1) % p.Count]);
                _cl(c, l);
            }
            return _buffer.Count;
        }
        static int _ct(in Circle c, in Triangle t)
        {
            for (int i = 0; i < 3; i++)
                _cl(c, new LineSegment(t[i], t[(i + 1) % 3]));
            return _buffer.Count;
        }
        static int _cq(in Circle c, in Quad q)
        {
            for (int i = 0; i < 4; i++)
                _cl(c, new LineSegment(q[i], q[(i + 1) % 4]));
            return _buffer.Count;
        }
    }
}