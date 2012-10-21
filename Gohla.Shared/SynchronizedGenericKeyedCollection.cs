using System;
using System.Collections.Specialized;
using System.Threading;

namespace Gohla.Shared
{
    public class SynchronizedKeyedCollection<TKey, TItem> :
        System.Collections.ObjectModel.KeyedCollection<TKey, TItem>,
        IDisposable,
        IObservableCollection<TItem>
        where TItem : class, IKeyedObject<TKey>
    {
        private SynchronizationContext _context;

        public System.Collections.Generic.IEnumerable<TKey> Keys
        {
            get
            {
                return base.Dictionary.Keys;
            }
        }

        public System.Collections.Generic.IEnumerable<TItem> Values
        {
            get
            {
                return base.Dictionary.Values;
            }
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public SynchronizedKeyedCollection(SynchronizationContext context)
            : base()
        {
            _context = context;
        }

        public void Dispose()
        {
            if(CollectionChanged == null)
                return;

            CollectionChanged = null;
        }

        public new void ChangeItemKey(TItem item, TKey newKey)
        {
            base.ChangeItemKey(item, newKey);
        }

        protected override TKey GetKeyForItem(TItem item)
        {
            return item.Key;
        }

        protected override void SetItem(int index, TItem item)
        {
            _context.Send(PostSetItem, Tuple.Create(index, item));
        }

        private void PostSetItem(object state)
        {
            Tuple<int, TItem> data = state as Tuple<int, TItem>;
            base.SetItem(data.Item1, data.Item2);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, 
                data.Item2, data.Item1));
        }

        protected override void InsertItem(int index, TItem item)
        {
            _context.Send(PostInsertItem, Tuple.Create(index, item));
        }

        private void PostInsertItem(object state)
        {
            Tuple<int, TItem> data = state as Tuple<int, TItem>;
            base.InsertItem(data.Item1, data.Item2);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, 
                data.Item2, data.Item1));
        }

        protected override void ClearItems()
        {
            _context.Send(_ => PostClearItems(), null);
        }

        private void PostClearItems()
        {
            base.ClearItems();
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        protected override void RemoveItem(int index)
        {
            _context.Send(PostRemoveItem, index);
        }

        private void PostRemoveItem(object state)
        {
            int index = (int)state;
            TItem item = this[index];
            base.RemoveItem(index);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, index));
        }

        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if(CollectionChanged != null)
                CollectionChanged(this, e);
        }
    }
}
