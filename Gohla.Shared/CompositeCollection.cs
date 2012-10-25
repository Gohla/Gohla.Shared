using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Reactive.Disposables;

namespace Gohla.Shared
{
    public class CompositeCollection<T> : IEnumerable<T>, INotifyCollectionChanged, IDisposable
    {
        private CompositeDisposable _subscriptions = new CompositeDisposable();
        protected List<ObservableCollection<T>> _collections = new List<ObservableCollection<T>>();

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public CompositeCollection(params ObservableCollection<T>[] collections)
        {
            _collections = new List<ObservableCollection<T>>(collections);
            foreach(ObservableCollection<T> collection in _collections)
                collection.CollectionChanged += CollectionChange;
        }

        public CompositeCollection(IEnumerable<ObservableCollection<T>> collections)
        {
            _collections = new List<ObservableCollection<T>>(collections);
            foreach(ObservableCollection<T> collection in _collections)
                collection.CollectionChanged += CollectionChange;
        }

        public void Dispose()
        {
            if(_subscriptions == null)
                return;

            _subscriptions.Dispose();
            _subscriptions = null;
        }

        public void CopyAndFollow<R>(ObservableCollection<R> collection, Func<R, ObservableCollection<T>> convert)
        {
            foreach(R r in collection)
                AddCollection(convert(r));
            Follow<R>(collection, convert);
        }

        public void Follow<R>(INotifyCollectionChanged collection, Func<R, ObservableCollection<T>> convert)
        {
            _subscriptions.Add(collection.AddedItems<R>().Subscribe(nc => this.AddCollection(convert(nc))));
            _subscriptions.Add(collection.RemovedItems<R>().Subscribe(nc => this.RemoveCollection(convert(nc))));
        }

        public void AddCollection(ObservableCollection<T> collection)
        {
            _collections.Add(collection);
            collection.CollectionChanged += CollectionChange;

            if(CollectionChanged == null)
                return;

            int offset = IndexAt(collection);
            int i = 0;
            foreach(T t in collection)
            {
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Add, 
                    t, 
                    offset + i
                ));

                ++i;
            }
        }

        public void RemoveCollection(ObservableCollection<T> collection)
        {
            int offset = IndexAt(collection);
            foreach(T t in collection)
            {
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Remove,
                    t,
                    offset
                ));
            }

            _collections.Remove(collection);
            collection.CollectionChanged -= CollectionChange;
        }

        protected ObservableCollection<T> CollectionAt(int index, out int newIndex)
        {
            int c = 0;
            foreach(ObservableCollection<T> collection in _collections)
            {
                if(c + collection.Count > index)
                {
                    newIndex = index - c;
                    return collection;
                }

                c += collection.Count;
            }

            newIndex = -1;
            return null;
        }

        protected int IndexAt(object collection)
        {
            int offset = 0;
            int i = 0;
            while(collection != _collections[i] && i < _collections.Count)
                offset += _collections[i++].Count;

            return offset;
        }

        #region IEnumerable<T>
        public IEnumerator<T> GetEnumerator()
        {
            foreach(ObservableCollection<T> collection in _collections)
                foreach(T item in collection)
                    yield return item;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion

        #region INotifyCollectionChanged
        protected void CollectionChange(object sender, NotifyCollectionChangedEventArgs e)
        {
            if(CollectionChanged == null)
                return;

            int offset = IndexAt(sender);

            switch(e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    CollectionChanged(this, new NotifyCollectionChangedEventArgs(e.Action, e.NewItems[0],
                        e.NewStartingIndex + offset));
                    break;
                case NotifyCollectionChangedAction.Remove:
                    CollectionChanged(this, new NotifyCollectionChangedEventArgs(e.Action, e.OldItems,
                        e.OldStartingIndex + offset));
                    break;
#if !WINDOWS_PHONE
                case NotifyCollectionChangedAction.Move:
                    CollectionChanged(this, new NotifyCollectionChangedEventArgs(e.Action, e.NewItems, e.OldItems,
                        e.OldStartingIndex + offset));
                    break;
#endif
                case NotifyCollectionChangedAction.Replace:
                    CollectionChanged(this, new NotifyCollectionChangedEventArgs(e.Action, e.NewItems, e.OldItems,
                        e.NewStartingIndex + offset));
                    break;
                case NotifyCollectionChangedAction.Reset:
                    CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                    break;
            }
        }
        #endregion
    }
}
