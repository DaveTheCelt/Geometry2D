using System;

namespace Geometry2D
{
    public static partial class Intersections
    {
        private static Buffer _buffer = new(8);

        public static int Intersects<T1, T2>(T1 shape1, T2 shape2, out Buffer buffer) where T1 : IGeometry where T2 : IGeometry
        {
            _buffer.Clear();

            switch (shape1, shape2)
            {
                //// Arcs //
                //case (Arc a1, Arc a2): _aa(a1, a2); break;
                //case (Arc a, Circle c): _ac(a, c); break;
                //case (Arc a, BiArc b): _ab(a, b); break;
                //case (Arc a, LineSegment l): _al(a, l); break;
                //case (Arc a, LineInfinite l): _ali(a, l); break;
                //case (Arc a, Triangle t): _at(a, t); break;
                //case (Arc a, Quad q): _aq(a, q); break;

                //// BiArcs//
                //case (BiArc b1, BiArc b2): _bb(b1, b2); break;
                //case (BiArc b, Arc a): _ab(a, b); break;
                //case (BiArc b, Circle c): _bc(b, c); break;
                //case (BiArc b, LineSegment l): _bl(b, l); break;
                //case (BiArc b, LineInfinite l): _bli(b, l); break;
                //case (BiArc b, Triangle t): _bt(b, t); break;
                //case (BiArc b, Quad q): _bq(b, q); break;

                // Circles //
                case (Circle c1, Circle c2): _cc(c1, c2); break;
                //case (Circle c, Arc a): _ac(a, c); break;
                //case (Circle c, BiArc b): _bc(b, c); break;
                case (Circle c, LineSegment l): _cl(c, l); break;
                case (Circle c, LineInfinite l): _cli(c, l); break;
                case (Circle c, Polygon p): _cp(c, p); break;
                case (Circle c, Triangle t): _ct(c, t); break;
                case (Circle c, Quad q): _cq(c, q); break;

                //Line Segments//
                case (LineSegment l1, LineSegment l2): _ll(l1, l2); break;
                //case (LineSegment l, Arc a): _al(a, l); break;
                //case (LineSegment l, BiArc b): _bl(b, l); break;
                case (LineSegment l, Circle c): _cl(c, l); break;
                case (LineSegment l, LineInfinite li): _lli(l, li); break;
                case (LineSegment l, Polygon p): _lp(l, p); break;
                case (LineSegment l, Triangle t): _lt(l, t); break;
                case (LineSegment l, Quad q): _lq(l, q); break;

                //Line Infinite//
                case (LineInfinite l1, LineInfinite l2): _lili(l1, l2); break;
                //case (LineInfinite l, Arc a): _ali(a, l); break;
                //case (LineInfinite l, BiArc b): _bli(b, l); break;
                case (LineInfinite li, Circle c): _cli(c, li); break;
                case (LineInfinite l1, LineSegment l2): _lli(l2, l1); break;
                case (LineInfinite li, Polygon p): _lip(li, p); break;
                case (LineInfinite l, Triangle t): _lit(l, t); break;
                case (LineInfinite l, Quad q): _liq(l, q); break;

                //Triangle//
                case (Triangle t1, Triangle t2): _tt(t1, t2); break;
                //case (Triangle t, Arc a): _at(a, t); break;
                //case (Triangle t, BiArc b): _bt(b, t); break;
                case (Triangle t, Circle c): _ct(c, t); break;
                case (Triangle t, LineSegment l): _lt(l, t); break;
                case (Triangle t, LineInfinite l): _lit(l, t); break;
                case (Triangle t, Polygon p): _pt(p, t); break;
                case (Triangle t, Quad q): _tq(t, q); break;


                //Polygon//
                case (Polygon p1, Polygon p2): _pp(p1, p2); break;
                //case (Triangle t, Arc a): _at(a, t); break;
                //case (Triangle t, BiArc b): _bt(b, t); break;
                case (Polygon p, Circle c): _cp(c, p); break;
                case (Polygon p, LineSegment l): _lp(l, p); break;
                case (Polygon p, LineInfinite li): _lip(li, p); break;
                case (Polygon p, Quad q): _pq(p, q); break;

                //Quad//
                case (Quad q1, Quad q2): _qq(q1, q2); break;
                //case (Quad q, Arc a): _aq(a, q); break;
                //case (Quad q, BiArc b): _bq(b, q); break;
                case (Quad q, Circle c): _cq(c, q); break;
                case (Quad q, LineSegment l): _lq(l, q); break;
                case (Quad q, LineInfinite l): _liq(l, q); break;
                case (Quad q, Polygon p): _pq(p, q); break;
                case (Quad q, Triangle t): _tq(t, q); break;
            }
            buffer = _buffer;
            return _buffer.Count;
        }



        private static class math
        {
            public static int _cc(in Circle c1, in Circle c2, out v2 pt1, out v2 pt2)
            {
                pt1 = pt2 = default;
                var d = c1.Origin.len(c2.Origin);
                if (d > c1.Radius + c2.Radius || d < Math.Abs(c1.Radius - c2.Radius) || d <= 0 && c1.Radius == c2.Radius)
                    return 0;

                var a = (c1.Radius * c1.Radius - c2.Radius * c2.Radius + d * d) / (2 * d);
                var h = Math.Sqrt(c1.Radius * c1.Radius - a * a);

                var p2 = c1.Origin + a * (c2.Origin - c1.Origin) / d;

                var dif = c2.Origin - c1.Origin;

                pt1 = new(p2.X + h * dif.Y / d, p2.Y - h * dif.X / d);
                pt2 = new(p2.X - h * dif.Y / d, p2.Y + h * dif.X / d);
                return 2;
            }
            public static int _cli(in Circle c, in LineInfinite l, out v2 pt1, out v2 pt2)
            {
                //http://csharphelper.com/howtos/howto_line_circle_intersection.html

                pt1 = pt2 = default;
                var d = l.B - l.A;
                var lengthSqr = l.A.lenSqr(l.B);

                var B = 2 * (d.X * (l.A.X - c.Origin.X) + d.Y * (l.A.Y - c.Origin.Y));

                var pMinuso = l.A - c.Origin;
                var C = pMinuso.X * pMinuso.X + pMinuso.Y * pMinuso.Y - c.Radius * c.Radius;

                var det = B * B - 4 * lengthSqr * C;

                // no solution
                if (lengthSqr < double.Epsilon || det < 0)
                    return 0;

                var mB = -B;
                var A2 = 2 * lengthSqr;

                if (det < double.Epsilon)
                {
                    //one solution
                    var t = mB / A2;
                    var cp = l.A + t * d;

                    pt1 = pt2 = cp;
                    return 1;
                }
                else
                {
                    //two solutions
                    var sqrtDet = Math.Sqrt(det);
                    var t = (mB + sqrtDet) / A2;
                    var cp = l.A + t * d;

                    pt1 = cp;

                    t = (mB - sqrtDet) / A2;
                    cp = l.A + t * d;

                    pt2 = cp;

                    return 2;
                }
            }

            public static int _lili(in LineInfinite l1, in LineInfinite l2, out v2 pt)
            {
                pt = default;
                var d1 = l1.A - l1.B;
                var d2 = l2.A - l2.B;

                var d = d1.X * d2.Y - d1.Y * d2.X;
                if (Math.Abs(d) <= double.Epsilon)
                    return 0;

                var x1y2 = l1.A.X * l1.B.Y;
                var y1x2 = l1.A.Y * l1.B.X;

                var x3y4 = l2.A.X * l2.B.Y;
                var y3x4 = l2.A.Y * l2.B.X;

                var dif1 = x1y2 - y1x2;
                var dif2 = x3y4 - y3x4;

                pt = ((dif1 * d2) - (dif2 * d1)) / d;
                return 1;
            }
        }
    }
}