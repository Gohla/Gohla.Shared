﻿using System.Collections.Specialized;

namespace Gohla.Shared
{
    public interface IKeyedObject<TKey>
    {
        TKey Key { get; }
    }

    public class KeyedCollection<TKey, TItem> : System.Collections.ObjectModel.KeyedCollection<TKey, TItem>,
        IObservableCollection<TItem>
        where TItem : class, IKeyedObject<TKey>
    {
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

        public KeyedCollection()
            : base()
        {

        }

        protected override TKey GetKeyForItem(TItem item)
        {
            return item.Key;
        }

        protected override void SetItem(int index, TItem item)
        {
            base.SetItem(index, item);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, item, index));
        }

        protected override void InsertItem(int index, TItem item)
        {
            base.InsertItem(index, item);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
        }

        protected override void ClearItems()
        {
            base.ClearItems();
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        protected override void RemoveItem(int index)
        {
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
