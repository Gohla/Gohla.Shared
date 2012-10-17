using System;
using System.Collections;
using System.Collections.Generic;

namespace gIRC.Shared
{
    /**
    Dictionary with non unique keys.

    @tparam K   Type of the key.
    @tparam V   Type of the value.
    **/
    public class MultiValueDictionary<K, V> : IEnumerable<KeyValuePair<K, V>>
    {
        private Dictionary<K, List<V>> _dictionary = new Dictionary<K, List<V>>();

        /**
        Gets all keys as an IEnumerable.
    
        @return All keys as an IEnumerable.
        **/
        public IEnumerable<K> Keys
        {
            get
            {
                return _dictionary.Keys;
            }
        }

        /**
        Gets all lists of values as an IEnumerable.
    
        @return All lists of values as an IEnumerable.
        **/
        public IEnumerable<List<V>> Values
        {
            get
            {
                return _dictionary.Values;
            }
        }

        /**
        Adds a key and value to the dictionary.
    
        @param  key     The key to add.
        @param  value   The value to add.
        **/
        public void Add(K key, V value)
        {
            List<V> list;
            if(this._dictionary.TryGetValue(key, out list))
            {
                list.Add(value);
            }
            else
            {
                list = new List<V>();
                list.Add(value);
                _dictionary[key] = list;
            }
        }

        /**
        Adds a key value pair to the dictionary.
    
        @param  pair    The key value pair to add.
        **/
        public void Add(KeyValuePair<K, V> pair)
        {
            Add(pair.Key, pair.Value);
        }

        /**
        Adds a key value pair to the dictionary. Throws exception if given object is not a KeyValuePair.
    
        @param  obj The object to add.
        **/
        public void Add(Object obj)
        {
            Add((KeyValuePair<K, V>)obj);
        }

        /**
        Removes all values with given key from the dictionary.
    
        @param  key The key to remove all values for.
        **/
        public void Remove(K key)
        {
            _dictionary.Remove(key);
        }

        /**
        Removes a certain key and value from the dictionary.
    
        @param  key     The key to remove.
        @param  value   The value to remove.
        **/
        public void Remove(K key, V value)
        {
            List<V> list;
            if(_dictionary.TryGetValue(key, out list))
            {
                list.Remove(value);
            }
        }

        /**
        Tries to get all values for a given key.
    
        @param  key             The key to search for.
        @param [out]    list    The list of values for given key. Unchanged if given key does not exist.
    
        @return True if values for key were found, false if not.
        **/
        public bool TryGetValue(K key, out List<V> list)
        {
            return _dictionary.TryGetValue(key, out list);
        }

        /**
        Indexer on dictionary keys.
    
        @return List of all values for given key. Returns empty list if given key has no values.
        **/
        public List<V> this[K key]
        {
            get
            {
                List<V> list;
                if(_dictionary.TryGetValue(key, out list))
                {
                    return list;
                }
                else
                {
                    return new List<V>();
                }
            }
        }

        /**
        Returns an enumerator over every key-value pair.
    
        @return The enumerator over every key-value pair.
        **/
        public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
        {
            foreach(KeyValuePair<K, List<V>> pair in _dictionary)
            {
                foreach(V val in pair.Value)
                {
                    yield return new KeyValuePair<K, V>(pair.Key, val);
                }
            }
        }

        /**
        Returns an enumerator over every key-value pair.
    
        @return The enumerator over every key-value pair.
        **/
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}