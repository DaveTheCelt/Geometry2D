using System;
using System.Collections.Generic;

namespace Geometry2D
{
    public readonly struct Polygon : IShape
    {
        readonly v2 _o;
        readonly List<v2> _vertices;

        public IReadOnlyList<v2> Vertices => _vertices;
        public v2 Origin => _o;
        public int Count => _vertices.Count;
        public v2 this[int index] => _vertices[index];

        public Polygon(in List<v2> verts)
        {
            if (verts.Count < 3)
                throw new Exception("Polygon requires at least 3 vertices to be valid");

            _vertices = verts;
            _o = verts[0];
            for (int i = 1; i < verts.Count; i++)
                _o += verts[i];
            _o /= verts.Count;
        }


        public double calculateArea()
        {
            double area = 0;

            for (int i = 0; i < _vertices.Count; i++)
            {
                var a = _vertices[i];
                var b = _vertices[(i + 1) % _vertices.Count];
                area += (a.X * b.Y) - (a.Y * b.X);
            }

            return area / 2;
        }

        public Quad calculateBounds()
        {
            v2 min = default;
            v2 max = default;

            foreach (var v in _vertices)
            {
                min = min.min(v);
                max = max.max(v);
            }
            return new Quad(min, max);
        }

        public double calculatePerimeter()
        {
            double p = 0;
            for (int i = 0; i < _vertices.Count; i++)
            {
                var a = _vertices[i];
                var b = _vertices[(i + 1) % _vertices.Count];
                p += a.len(b);
            }
            return p;
        }

        public v2 normal(in v2 pt)
        {
            double dist = double.PositiveInfinity;
            v2 normal = default;
            for (int i = 0; i < _vertices.Count; i++)
            {
                LineSegment l = new(_vertices[i], _vertices[(i + 1) % _vertices.Count]);
                var p = l.snapTo(pt);
                var d = p.lenSqr(pt);
                if (d < dist)
                {
                    dist = d;
                    normal = l.normal(pt);
                }
            }
            return normal;
        }

        public bool overlaps(in v2 pt)
        {
            v2 max = pt;

            foreach (var v in _vertices)
                max = max.max(v);

            max += 100;
            var line1 = new LineSegment(pt, max);
            int count = 0;
            for (int i = 0; i < _vertices.Count; i++)
            {
                var line2 = new LineSegment(_vertices[i], _vertices[(i + 1) % _vertices.Count]);
                count += Intersections.Intersects(line1, line2, out _);
            }
            return count % 2 != 0;
        }

        public IGeometry rotate(in double radians)
        {
            for (int i = 0; i < _vertices.Count; i++)
                _vertices[i] = _vertices[i].rotateAround(_o, radians);
            return this;
        }

        public IGeometry scale(in double factor)
        {
            for (int i = 0; i < _vertices.Count; i++)
            {
                var d = _vertices[i].len(_o);
                _vertices[i] = (_vertices[i] - _o).normalize() * d * factor + _o;
            }
            return this;
        }

        public IGeometry setPosition(in v2 position) => translate(position - _o);

        public v2 snapTo(in v2 pt)
        {
            double dist = double.PositiveInfinity;
            v2 newPt = default;
            for (int i = 0; i < _vertices.Count; i++)
            {
                LineSegment l = new(_vertices[i], _vertices[(i + 1) % _vertices.Count]);
                var p = l.snapTo(pt);
                var d = p.lenSqr(pt);
                if (d < dist)
                {
                    dist = d;
                    newPt = p;
                }
            }
            return newPt;
        }

        public IGeometry translate(in v2 vector)
        {
            for (int i = 0; i < _vertices.Count; i++)
                _vertices[i] += vector;
            return this;
        }
    }
}