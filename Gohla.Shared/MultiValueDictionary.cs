using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Gohla.Shared
{
    public class MultiValueDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        private Dictionary<TKey, List<TValue>> _dictionary;

        public MultiValueDictionary()
        {
            _dictionary = new Dictionary<TKey, List<TValue>>();
        }

        public MultiValueDictionary(Dictionary<TKey, List<TValue>> dictionary)
        {
            _dictionary = new Dictionary<TKey, List<TValue>>(dictionary);
        }

        public MultiValueDictionary(IEqualityComparer<TKey> comparer)
        {
            _dictionary = new Dictionary<TKey, List<TValue>>(comparer);
        }

        public MultiValueDictionary(int capacity)
        {
            _dictionary = new Dictionary<TKey, List<TValue>>(capacity);
        }

        public MultiValueDictionary(Dictionary<TKey, List<TValue>> dictionary, IEqualityComparer<TKey> comparer)
        {
            _dictionary = new Dictionary<TKey, List<TValue>>(dictionary, comparer);
        }

        public MultiValueDictionary(int capacity, IEqualityComparer<TKey> comparer)
        {
            _dictionary = new Dictionary<TKey, List<TValue>>(capacity, comparer);
        }

        public IEnumerable<TKey> Keys
        {
            get
            {
                return _dictionary.Keys;
            }
        }

        public IEnumerable<TValue> Values
        {
            get
            {
                return _dictionary.Values.SelectMany(x => x);
            }
        }

        public IEnumerable<List<TValue>> UniqueValues
        {
            get
            {
                return _dictionary.Values;
            }
        }

        public IEnumerable<TValue> this[TKey key]
        {
            get
            {
                List<TValue> list;
                if(_dictionary.TryGetValue(key, out list))
                    return list;
                else
                    return Enumerable.Empty<TValue>();
            }
        }

        public void Add(TKey key, TValue value)
        {
            List<TValue> list;
            if(this._dictionary.TryGetValue(key, out list))
            {
                list.Add(value);
            }
            else
            {
                list = new List<TValue>();
                list.Add(value);
                _dictionary[key] = list;
            }
        }

        public void Add(TKey key, IEnumerable<TValue> values)
        {
            List<TValue> list;
            if(this._dictionary.TryGetValue(key, out list))
            {
                list.AddRange(values);
            }
            else
            {
                list = new List<TValue>(values);
                _dictionary[key] = list;
            }
        }

        public void Add(KeyValuePair<TKey, TValue> pair)
        {
            Add(pair.Key, pair.Value);
        }

        public void Set(TKey key, List<TValue> values)
        {
            _dictionary[key] = values;
        }

        public bool ContainsKey(TKey key)
        {
            return _dictionary.ContainsKey(key);
        }

        public IEnumerable<TValue> Get(TKey key)
        {
            List<TValue> list;
            if(_dictionary.TryGetValue(key, out list))
                return list;
            else
                return Enumerable.Empty<TValue>();
        }

        public bool TryGetValue(TKey key, out IEnumerable<TValue> values)
        {
            List<TValue> list;
            bool found = _dictionary.TryGetValue(key, out list);
            values = list;
            return found;
        }

        public bool Remove(TKey key)
        {
            return _dictionary.Remove(key);
        }

        public bool Remove(TKey key, TValue value)
        {
            List<TValue> list;
            if(_dictionary.TryGetValue(key, out list))
                return list.Remove(value);
            return false;
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            foreach(KeyValuePair<TKey, List<TValue>> pair in _dictionary)
            {
                foreach(TValue val in pair.Value)
                {
                    yield return new KeyValuePair<TKey, TValue>(pair.Key, val);
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}