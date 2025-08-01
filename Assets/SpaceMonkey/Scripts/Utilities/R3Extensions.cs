using R3;

namespace SpaceMonkey.Scripts.Utilities
{
    public static class R3Extensions
    {
        public static Observable<T> StartWithValue<T>(this Observable<T> source, T initialValue)
        {
            return Observable.Create<T>(observer =>
            {
                observer.OnNext(initialValue);
                return source.Subscribe(observer);
            });
        }
    }
}