using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace ChristmasGifts.Scripts.Common.Unit
{
    public interface IProcessingUnit : IProgress<float>, IDisposable
    {
        IObservable<float> Progress { get; }
        UniTask Execute(CancellationToken cancellationToken = default);
    }

    public interface IProcessingUnit<out T> : IProcessingUnit
    {
        T Result { get; }
    }
}