using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceMonkey.Scripts.Core.SimpleSerializableDictionary
{
    [Serializable]
    public class SerializableDictionary<TKey, TValue>
    {
        [Serializable]
        private struct KeyValuePair
        {
            public TKey key;
            public TValue value;
        }

        [SerializeField] private List<KeyValuePair> list = new();

        private Dictionary<TKey, TValue> dictionary = new();

        public Dictionary<TKey, TValue> ToDictionary()
        {
            dictionary.Clear();
            foreach (var kvp in list)
            {
                if (!dictionary.ContainsKey(kvp.key))
                    dictionary[kvp.key] = kvp.value;
            }
            return dictionary;
        }

        public void FromDictionary(Dictionary<TKey, TValue> dict)
        {
            list.Clear();
            foreach (var kvp in dict)
            {
                list.Add(new KeyValuePair { key = kvp.Key, value = kvp.Value });
            }
        }
    }
}