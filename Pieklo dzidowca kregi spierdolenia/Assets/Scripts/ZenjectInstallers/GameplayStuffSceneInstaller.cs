using jbzd.Common.InputSystem;
using jbzd.UI;
using jbzd.MainHero;
using UnityEngine;
using Zenject;

namespace jbzd.ZenjectInstallers
{
    public class GameplayStuffSceneInstaller : MonoInstaller<GameplayStuffSceneInstaller>
    {
        [SerializeField]
        private PlayerController playerGameObject;
        [SerializeField]
        private UserInterfaceManager UIManager;
        public override void InstallBindings()
        {
            if (playerGameObject is null ||
                UIManager is null)
            {
                Debug.LogError("Missing references in installer!");
            }
            
            Container.BindInstance(playerGameObject).AsSingle().NonLazy();
            Container.BindInstance(UIManager).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<InputManager>().AsSingle().NonLazy();
        }
    }
}