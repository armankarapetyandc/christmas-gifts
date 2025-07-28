using System;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Views.Business.CategorySelection
{
    public class IdeaItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private Image iconImage;
        [SerializeField] private Button button;
        private IdeaInfo _ideaInfo;

        public Observable<IdeaInfo> Selected => button.OnClickAsObservable().Select(_ => _ideaInfo);

        public bool HasIdea => _ideaInfo != null;


        internal void Set(IdeaInfo ideaInfo)
        {
            _ideaInfo = ideaInfo;
            UpdateUI();
        }

        private void UpdateUI()
        {
            if (_ideaInfo == null)
            {
                throw new Exception("IdeaInfo is null");
            }

            titleText.text = _ideaInfo.Name;
            iconImage.sprite = _ideaInfo.Icon;
        }
    }
}