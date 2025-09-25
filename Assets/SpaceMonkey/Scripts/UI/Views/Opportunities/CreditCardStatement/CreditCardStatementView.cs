using System;
using R3;
using UIService.Runtime.Core;
using UIService.Runtime.Presenter.Base;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Views.Opportunities.CreditCardStatement
{
    public class CreditCardStatementView : BasePresenterWithController<CreditCardStatementView.Data, CreditCardStatementViewController>
    {
        [SerializeField] private Button backButton;

        protected override void InternalInit()
        {
            backButton.OnClickAsObservable().Subscribe(_ =>
            {
                if (PresenterData?.OnClose != null)
                {
                    Controller.CloseView();
                    PresenterData.OnClose.Invoke();
                    return;
                }
                Controller.OnBack();
            }).AddTo(this);
        }

        public override void Dispose()
        {
        }
        
        public class Data : IPresenterData
        {
            public Action OnClose { get; set; }
        }
    }
}