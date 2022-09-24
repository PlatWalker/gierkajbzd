using jbzd.Common.InputSystem;
using jbzd.Common.InputSystem.Inputs;
using jbzdy.Player;
using UnityEngine;
using Zenject;

namespace jbzd.ZenjectInstallers
{
    public class GameplayStuffSceneInstaller : MonoInstaller<GameplayStuffSceneInstaller>
    {
        [SerializeField]
        private GameObject playerGameObject;
        public override void InstallBindings()
        {
            Container.BindInstance(playerGameObject.GetComponent<PlayerController>()).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<InputController>().AsSingle().NonLazy();
        }
    }
}