using UnityEngine;
using Zenject;

namespace SpaceMonkey.Scripts.UI.Views.AllReviewView
{
    public class AllReviewInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<AllReviewController>().AsSingle().NonLazy();
        }
    }
}