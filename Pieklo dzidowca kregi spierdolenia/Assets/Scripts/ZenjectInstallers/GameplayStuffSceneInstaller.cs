using jbzd.Common.InputSystem;
using jbzd.Common.RunnerThing;
using jbzd.Cutscenes;
using jbzd.Dialogues;
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
        [SerializeField] 
        private DialogueManager DialogueManager;
        
        public override void InstallBindings()
        {
            if (playerManager is null ||
                UIManager is null ||
                QuestManager is null ||
                DialogueManager is null)
            {
                Debug.LogError("Missing references in installer!");
            }

            Container.Bind<CutscenesManager>().AsSingle().NonLazy();
            Container.BindInstance(playerManager).AsSingle().NonLazy();
            Container.BindInstance(QuestManager).AsSingle().NonLazy();
            Container.BindInstance(DialogueManager).AsSingle().NonLazy();
            Container.BindInstance(UIManager).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<InputManager>().AsSingle().NonLazy();
            Container.BindFactory<Object, Item, Item.Factory>().FromFactory<PrefabFactory<Item>>();
            Container.BindFactory<object, Runner, RunnerFactory>().FromFactory<CustomRunnerFactory>();
        }
    }
}