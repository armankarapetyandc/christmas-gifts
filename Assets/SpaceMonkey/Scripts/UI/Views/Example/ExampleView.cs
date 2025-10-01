using Cysharp.Threading.Tasks;
using R3;
using TMPro;
using UIService.Runtime.Core;
using UIService.Runtime.Presenter.Base;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Views.Example
{
    public class ExampleView : BasePresenterWithController<ExampleController>
    {
        public class Data : IPresenterData
        {
            public string Category { get; internal set; }
            public string Name { get; internal set; }
            public string Title { get; internal set; }
            public string Description { get; internal set; }
        }
        
        [SerializeField] private TextMeshProUGUI exampleName;
        [SerializeField] private TextMeshProUGUI exampleTitle;
        [SerializeField] private TextMeshProUGUI exampleDescription;
        [SerializeField] private Button backButton;
        
        private Data _data;
        
        public override UniTask Initialize(IPresenterData data = null)
        {
            _data = data as Data;
            if(_data == null) return UniTask.CompletedTask;
            exampleName.text = _data.Name;
            exampleTitle.text = _data.Title;
            exampleDescription.text = _data.Description;

            backButton.OnClickAsObservable().Subscribe(_ => Controller.OnBack(_data.Category));
            return UniTask.CompletedTask;
        }

        public override void Dispose()
        {
        }
    }
}