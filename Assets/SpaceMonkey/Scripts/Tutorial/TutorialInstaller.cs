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
                new TutorialStep1()
            };
            
            foreach (var step in steps)
            {
                Container.Inject(step);
            }
            
            Container.BindInterfacesAndSelfTo<TutorialService>().AsSingle().WithArguments(tutorialView, steps);
        }
    }
}