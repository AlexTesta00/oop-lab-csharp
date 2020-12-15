namespace Indexers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    /// <inheritdoc cref="IMap2D{TKey1,TKey2,TValue}" />
    public class Map2D<TKey1, TKey2, TValue> : IMap2D<TKey1, TKey2, TValue>
    {
        private IDictionary<Tuple<TKey1, TKey2>, TValue> dictionary = new Dictionary<Tuple<TKey1, TKey2>, TValue>();

        /// <inheritdoc cref="IMap2D{TKey1, TKey2, TValue}.NumberOfElements" />
        public int NumberOfElements
        {
            get { return dictionary.Count; }
        }

        /// <inheritdoc cref="IMap2D{TKey1, TKey2, TValue}.this" />
        public TValue this[TKey1 key1, TKey2 key2]
        {
            get { return dictionary[Tuple.Create(key1,key2)]; }
            set { dictionary[Tuple.Create(key1, key2)] = value; }
        }

        /// <inheritdoc cref="IMap2D{TKey1, TKey2, TValue}.GetRow(TKey1)" />
        public IList<Tuple<TKey2, TValue>> GetRow(TKey1 key1)
        {
            return dictionary.Keys
                          .Where(tuple => tuple.Item1.Equals(key1))
                          .Select(tuple => Tuple.Create(tuple.Item2, this.dictionary[tuple]))
                          .ToList();
        }

        /// <inheritdoc cref="IMap2D{TKey1, TKey2, TValue}.GetColumn(TKey2)" />
        public IList<Tuple<TKey1, TValue>> GetColumn(TKey2 key2)
        {
            return dictionary.Keys
                        .Where(tuple => tuple.Item2.Equals(key2))
                        .Select(tuple => Tuple.Create(tuple.Item1, dictionary[tuple]))
                        .ToList();
        }

        /// <inheritdoc cref="IMap2D{TKey1, TKey2, TValue}.GetElements" />
        public IList<Tuple<TKey1, TKey2, TValue>> GetElements()
        {
            return dictionary.Keys
                        .Select( t1 => Tuple.Create(t1.Item1, t1.Item2, dictionary[t1]))
                        .ToList();
        }

        /// <inheritdoc cref="IMap2D{TKey1, TKey2, TValue}.Fill(IEnumerable{TKey1}, IEnumerable{TKey2}, Func{TKey1, TKey2, TValue})" />
        public void Fill(IEnumerable<TKey1> keys1, IEnumerable<TKey2> keys2, Func<TKey1, TKey2, TValue> generator)
        {
            var key = keys2.ToArray();

            foreach (var k1 in keys1)
            {
                foreach (var k2 in key)
                {
                    this[k1, k2] = generator(k1, k2);
                }
            }
        }

        /// <inheritdoc cref="IEquatable{T}.Equals(T)" />
        public bool Equals(IMap2D<TKey1, TKey2, TValue> other)
        {
            // TODO: improve
            if (other is Map2D<TKey1, TKey2, TValue> maps)
            {
                return Equals(maps);
            }
            return false;
        }

        /// <inheritdoc cref="object.Equals(object?)" />
        public override bool Equals(object obj)
        {
            // TODO: improve
            if (obj is null)
            {
                return false;
            }

            if (obj == this)
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return Equals(obj as Map2D<TKey1, TKey2, TValue>);
        }

        /// <inheritdoc cref="object.GetHashCode"/>
        public override int GetHashCode()
        {
            // TODO: improve
            return dictionary != null ? dictionary.GetHashCode() : 0;
        }

        /// <inheritdoc cref="IMap2D{TKey1, TKey2, TValue}.ToString"/>
        public override string ToString()
        {
            // TODO: improve
            return "{ " + string.Join(", ", this.GetElements()
                   .Select(e => $"({e.Item1}, {e.Item2}) -> {e.Item3}")) + "}";
        }
    }
}
