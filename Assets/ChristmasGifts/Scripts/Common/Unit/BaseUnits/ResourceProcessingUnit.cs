using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ChristmasGifts.Scripts.Common.Unit.BaseUnits
{
    public class ResourceProcessingUnit<T> : IProcessingUnit<T> where T : Object
    {
        private readonly string _path;
        private readonly Subject<float> _progressSubject = new Subject<float>();

        public IObservable<float> Progress => _progressSubject;
        public T Result { get; private set; }

        public ResourceProcessingUnit(string path)
        {
            _path = path;
        }

        public void Report(float value)
        {
            _progressSubject?.OnNext(value);
        }

        public async UniTask Execute(CancellationToken cancellationToken = default)
        {
            var result = await Resources.LoadAsync<T>(_path)
                .ToUniTask(progress: this, cancellationToken: cancellationToken);
            Result = result as T;
        }

        public void Dispose()
        {
            _progressSubject?.Dispose();
        }
    }
}