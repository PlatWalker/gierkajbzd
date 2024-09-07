using jbzd.MinorSystems.InputSystem;
using jbzd.SavingSystem;
using UnityEngine;
using Zenject;

namespace jbzd.ZenjectInstallers
{
    public class MainMenuInstaller: MonoInstaller<MainMenuInstaller>
    {
        [SerializeField] private SaveManager saveManager;
        
        public override void InstallBindings()
        {
            if (saveManager is null)
            {
                Debug.LogError("Missing references in installer!");
            }
            
            Container.BindInterfacesAndSelfTo<InputManager>().AsSingle().NonLazy();
            Container.BindInstance(saveManager).AsSingle().NonLazy();
        }
    }
}