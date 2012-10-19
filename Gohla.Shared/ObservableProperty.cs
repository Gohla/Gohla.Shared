using System;
using System.Reactive.Subjects;

namespace Gohla.Shared
{
    public class ObservableProperty<T> : IObservable<T>
    {
        private T _value;
        private Subject<T> _subject;

        public T Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                _subject.OnNext(value);
            }
        }

        public ObservableProperty(T value)
        {
            _value = value;
            _subject = new Subject<T>();
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            return _subject.Subscribe(observer);
        }

        public static implicit operator T(ObservableProperty<T> p)
        {
            return p.Value;
        }
    }
}
