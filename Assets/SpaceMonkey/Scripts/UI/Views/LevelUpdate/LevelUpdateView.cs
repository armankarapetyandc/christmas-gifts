using System;
using Cysharp.Threading.Tasks;
using R3;
using SpaceMonkey.Scripts.UI.Components;
using TMPro;
using UIService.Runtime.Core;
using UIService.Runtime.Presenter.Base;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Views.LevelUpdate
{
    public class LevelUpdateView : BasePresenterWithController<LevelUpdateController>
    {
        public class  Data : IPresenterData
        {
            public int LevelNumber { get; internal set; }
            public Sprite LevelSprite { get; internal set; }
            public bool IsUpgraded { get; internal set; }
            public TMP_FontAsset FontAsset { get; internal set; }
            
            public int Score { get; internal set; }
        }
        
        [SerializeField] private TextMeshProUGUI numberText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private Button nextButton;
        [SerializeField] private Image leveIcon;
        [SerializeField] private Color updateBackgroundColor;
        [SerializeField] private Color repairedBackgroundColor;
        [SerializeField] private Image backgroundImage;
        private Data _data;
        
        public override UniTask Initialize(IPresenterData data = null)
        {
            _data = data as Data;
            if(_data == null) return UniTask.CompletedTask;
            numberText.font = _data.FontAsset; 
            numberText.text = (_data.LevelNumber + 1).ToString();
            backgroundImage.color = _data.IsUpgraded ? updateBackgroundColor : repairedBackgroundColor;
            descriptionText.fontSize = _data.IsUpgraded ? 128 : 80;
            descriptionText.text = _data.IsUpgraded ? $"Level {_data.LevelNumber + 1}" : $"Production\nRepaired!";
            leveIcon.sprite = _data.LevelSprite;
            nextButton.OnClickAsObservable().Subscribe(_ => Controller.OnNext(_data.LevelNumber));
            
            SpawnScoreParticles(_data.Score).Forget();
            return UniTask.CompletedTask;
        }

        private async UniTask SpawnScoreParticles(int score)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(1));
            if (score > 0)
            {
                XPParticleEffector.SpawnXpParticles(score, 
                    new Vector2(Screen.width, Screen.height) * 0.5f, transform).Forget();
            }
        }

        public override void Dispose()
        {
        }
    }
}