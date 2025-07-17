using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace SpaceMonkey.Scripts.Installers.Bootstrap
{
    public class BootstrapInstaller : MonoInstaller<BootstrapInstaller>
    {
        [SerializeField] private BootstrapParams @params;
        [SerializeField] private EventSystem eventSystem;

        public override void InstallBindings()
        {
            DontDestroyOnLoad(eventSystem);
            BootstrapLoader.Installer.Install(Container, @params);
        }
    }
}