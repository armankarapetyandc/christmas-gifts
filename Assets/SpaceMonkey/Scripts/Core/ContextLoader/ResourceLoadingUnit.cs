using System;
using System.Threading;
using ContextLoaderService.Runtime;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SpaceMonkey.Scripts.Core.ContextLoader
{
    public class ResourceLoadingUnit<T> : ILoadUnit<T> where T : Object
    {
        private readonly string _path;
        private readonly Subject<float> _progressSubject = new Subject<float>();

        public IObservable<float> Progress => _progressSubject.AsSystemObservable();
        public T Result { get; private set; }

        public ResourceLoadingUnit(string path)
        {
            _path = path;
        }

        public void Report(float value) => _progressSubject.OnNext(value);

        public void Dispose() => _progressSubject.Dispose();

        public async UniTask Load(CancellationToken cancellationToken = default)
        {
            var result = await Resources.LoadAsync<T>(_path)
                .ToUniTask(progress: this, cancellationToken: cancellationToken);
            Result = result as T;
        }
    }
}