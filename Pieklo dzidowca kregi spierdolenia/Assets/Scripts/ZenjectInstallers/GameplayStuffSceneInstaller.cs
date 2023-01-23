using jbzd.Common.InputSystem;
using jbzd.Items;
using jbzd.UI;
using jbzd.MainHero;
using jbzd.QuestSystem;
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
        [SerializeField] 
        private QuestManager QuestManager;
        public override void InstallBindings()
        {
            if (playerManager is null ||
                UIManager is null ||
                QuestManager is null)
            {
                Debug.LogError("Missing references in installer!");
            }

            Container.BindInstance(QuestManager).AsSingle().NonLazy();
            Container.BindInstance(playerManager).AsSingle().NonLazy();
            Container.BindInstance(UIManager).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<InputManager>().AsSingle().NonLazy();
            Container.BindFactory<Object, Item, Item.Factory>().FromFactory<PrefabFactory<Item>>();
        }
    }
}