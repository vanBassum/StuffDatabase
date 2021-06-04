using System.Collections;
using System.Collections.Generic;

namespace StuffDatabase
{
    public class LookupTable<T1, T2> : IEnumerable<KeyValuePair<T1, T2>>
    {
        Dictionary<T1, T2> dict1 = new Dictionary<T1, T2>();
        Dictionary<T2, T1> dict2 = new Dictionary<T2, T1>();

        public void Add(T1 t1, T2 t2)
        {
            dict1.Add(t1, t2);
            dict2.Add(t2, t1);
        }

        

        public T1 Lookup(T2 t2)
        {
            return dict2[t2];
        }

        public T2 Lookup(T1 t1)
        {
            return dict1[t1];
        }

        public IEnumerator<KeyValuePair<T1, T2>> GetEnumerator()
        {
            foreach (var kvp in dict1)
                yield return kvp;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (var kvp in dict1)
                yield return kvp;
        }
    }
}
