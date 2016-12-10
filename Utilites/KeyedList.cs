using System.Collections.Generic;
using System.Linq;

namespace AcFunVideo.Utilites
{
    public class KeyedList<TKey, TItem> : List<TItem>
    {
        public TKey GroupKey { protected set; get; }

        public KeyedList(TKey key, IEnumerable<TItem> items)
            : base(items)
        {
            GroupKey = key;
        }

        public KeyedList(IGrouping<TKey, TItem> grouping)
            : base(grouping)
        {
            GroupKey = grouping.Key;
        }
    }
    public class KeyedList<TKey, TId, TItem> : List<TItem>
    {
        public TKey GroupKey { protected set; get; }
        public TId Id { get; protected set; }

        public KeyedList(TKey key, TId id, IEnumerable<TItem> items)
            : base(items)
        {
            GroupKey = key;
            Id = id;
        }

        public KeyedList(IGrouping<TKey, TItem> grouping)
            : base(grouping)
        {
            GroupKey = grouping.Key;
        }
    }

    

}
