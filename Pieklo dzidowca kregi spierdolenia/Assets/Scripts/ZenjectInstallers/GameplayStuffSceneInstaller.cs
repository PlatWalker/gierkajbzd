using jbzd.Common.InputSystem;
using jbzd.Items;
using jbzd.UI;
using jbzd.MainHero;
using UnityEngine;
using Zenject;

namespace jbzd.ZenjectInstallers
{
    public class GameplayStuffSceneInstaller : MonoInstaller<GameplayStuffSceneInstaller>
    {
        [SerializeField]
        private PlayerManager playerManager;
        [SerializeField]
        private UserInterfaceManager UIManager;
        public override void InstallBindings()
        {
            if (playerManager is null ||
                UIManager is null)
            {
                Debug.LogError("Missing references in installer!");
            }
            
            Container.BindInstance(playerManager).AsSingle().NonLazy();
            Container.BindInstance(UIManager).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<InputManager>().AsSingle().NonLazy();
            
            Container.BindFactory<Object, Item, Item.Factory>().FromFactory<PrefabFactory<Item>>();
        }
    }
}