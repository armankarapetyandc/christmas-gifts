using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;
using UnityEngine;

namespace SpaceMonkey.Scripts.UI.Views.DemoComplete
{
    public class DemoCompleteController : BasePresenterController
    {
        public DemoCompleteController(PresenterService presenterService) : base(presenterService)
        {
        }


        public void SurveyButtonClicked()
        {
            Application.OpenURL("https://docs.google.com/forms/d/1Cw_3UQsaG3KD9MUhTClgocEuDuYQf1xZkK7mY9-BVYo/edit");
        }

    }
}