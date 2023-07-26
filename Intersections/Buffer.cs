using System.Collections.Generic;

namespace Geometry2D
{
    public readonly struct Buffer
    {
        readonly List<v2> _b;
        public IReadOnlyList<v2> Points => _b;
        public v2 this[int index] => _b[index];
        public Buffer(int size = 8)
        {
            _b = new List<v2>(size);
        }
        public void RemoveAtSwapBack(in int index)
        {
            var lastElement = _b[^1];
            var targetElement = _b[index];

            _b[index] = lastElement;
            _b[^1] = targetElement;

            _b.RemoveAt(_b.Count - 1);
        }
        public void Add(in v2 v) => _b.Add(v);
        public void Clear() => _b.Clear();
        public int Count => _b.Count;
    }
}