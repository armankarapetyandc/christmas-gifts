using System.Collections.Generic;
using SpaceMonkey.Scripts.Tutorial.Steps;
using UnityEngine;
using Zenject;

namespace SpaceMonkey.Scripts.Tutorial
{
    public class TutorialInstaller : MonoInstaller
    {
        [SerializeField] private TutorialView tutorialView;
        
        public override void InstallBindings()
        {
            var steps = new List<ITutorialStep>
            {
                new TutorialStep1(),
                new TutorialStep2(),
                new TutorialStep3(),
                new TutorialStep4(),
                new TutorialStep5(),
                new TutorialStep6(),
                new TutorialStep7(),
                new TutorialStep8(),
                new TutorialStep9(),
                new TutorialStep10(),
                new TutorialStep11(),
                // new TutorialStep12(),
                // new TutorialStep13(),
                // new TutorialStep14()
                new TutorialStep15()
            };
            
            foreach (var step in steps)
            {
                Container.Inject(step);
            }
            
            Container.BindInterfacesAndSelfTo<TutorialService>().AsSingle().WithArguments(tutorialView, steps);
        }
    }
}