using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine.SceneManagement;

namespace ChristmasGifts.Scripts.Common.Unit.BaseUnits
{
    public class SceneProcessingUnit : IProcessingUnit
    {
        private readonly string _sceneName;
        private readonly LoadSceneMode _loadSceneMode;
        private readonly Subject<float> _progressSubject = new Subject<float>();

        public IObservable<float> Progress => _progressSubject;

        public SceneProcessingUnit(string sceneName, LoadSceneMode loadSceneMode)
        {
            _sceneName = sceneName;
            _loadSceneMode = loadSceneMode;
        }


        public async UniTask Execute(CancellationToken cancellationToken = default)
        {
            await SceneManager.LoadSceneAsync(_sceneName, _loadSceneMode)
                .ToUniTask(progress: this, cancellationToken: cancellationToken);
        }

        public void Report(float value)
        {
            _progressSubject?.OnNext(value);
        }

        public void Dispose()
        {
            _progressSubject?.Dispose();
        }
    }
}