using jbzd.Common.InputSystem;
using jbzd.Common.InputSystem.Inputs;
using jbzd.MainHero;
using jbzd.UI.NewUI;
using jbzdy.Player;
using UnityEngine;
using Zenject;

namespace jbzd.ZenjectInstallers
{
    public class GameplayStuffSceneInstaller : MonoInstaller<GameplayStuffSceneInstaller>
    {
        [SerializeField]
        private GameObject playerGameObject;
        [SerializeField]
        private UserInterfaceManager UIManager;
        public override void InstallBindings()
        {
            Container.BindInstance(playerGameObject.GetComponent<PlayerController>()).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<InputManager>().AsSingle().NonLazy();
            Container.BindInstance(UIManager).AsSingle().NonLazy();
        }
    }
}