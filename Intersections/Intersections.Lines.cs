namespace Geometry2D
{
    public static partial class Intersections
    {
        static int _ll(in LineSegment l1, in LineSegment l2)
        {
            math._lili(new(l1), new(l2), out var pt);

            if (l1.isBetween(pt) && l2.isBetween(pt))
                _buffer.Add(pt);

            return _buffer.Count;
        }
        static int _lli(in LineSegment l1, in LineInfinite l2)
        {
            math._lili(new(l1), l2, out var pt);

            if (l1.isBetween(pt))
                _buffer.Add(pt);

            return _buffer.Count;
        }
        static int _lili(in LineInfinite l1, in LineInfinite l2)
        {
            int count = math._lili(l1, l2, out var pt);
            if (count > 0)
                _buffer.Add(pt);

            return _buffer.Count;
        }
        static int _lp(in LineSegment l, in Polygon p)
        {
            for (int i = 0; i < p.Count; i++)
            {
                LineSegment ls = new(p[i], p[(i + 1) % p.Count]);
                _ll(ls, l);
            }
            return _buffer.Count;
        }
        static int _lip(in LineInfinite l, in Polygon p)
        {
            for (int i = 0; i < p.Count; i++)
            {
                LineSegment ls = new(p[i], p[(i + 1) % p.Count]);
                _lli(ls, l);
            }
            return _buffer.Count;
        }
        static int _lt(in LineSegment l, in Triangle t)
        {
            for (int i = 0; i < 3; i++)
                _ll(l, new LineSegment(t[i], t[(i + 1) % 3]));
            return _buffer.Count;
        }

        static int _lq(in LineSegment l, in Quad q)
        {
            for (int i = 0; i < 4; i++)
                _ll(l, new LineSegment(q[i], q[(i + 1) % 4]));
            return _buffer.Count;
        }

        static int _lit(in LineInfinite l, in Triangle t)
        {
            for (int i = 0; i < 3; i++)
                _lli(new LineSegment(t[i], t[(i + 1) % 3]), l);
            return _buffer.Count;
        }

        static int _liq(in LineInfinite l, in Quad q)
        {
            for (int i = 0; i < 4; i++)
                _lli(new LineSegment(q[i], q[(i + 1) % 4]), l);
            return _buffer.Count;
        }
    }
}

