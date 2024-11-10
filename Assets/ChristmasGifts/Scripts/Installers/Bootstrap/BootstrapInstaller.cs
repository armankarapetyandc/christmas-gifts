using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace ChristmasGifts.Scripts.Installers.Bootstrap
{
    public class BootstrapInstaller : MonoInstaller<BootstrapInstaller>
    {
        [SerializeField] private EventSystem eventSystem;

        public override void InstallBindings()
        {
            DontDestroyOnLoad(eventSystem);
            BootstrapLoader.Installer.Install(Container);
        }
    }
}