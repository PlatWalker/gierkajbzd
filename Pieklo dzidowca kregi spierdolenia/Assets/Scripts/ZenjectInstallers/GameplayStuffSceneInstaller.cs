using jbzd.Common.RunnerThing;
using jbzd.Dialogues;
using jbzd.Items;
using jbzd.UI;
using jbzd.MainHero;
using jbzd.MinorSystems.Cutscenes;
using jbzd.MinorSystems.InputSystem;
using jbzd.QuestSystem;
using jbzd.SavingSystem;
using jbzd.UI.LoadingScene;
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
        [SerializeField]
        private SaveManager SaveManager;
        [SerializeField]
        private LoadingUI LoadingUI;

        public override void InstallBindings()
        {
            if (playerManager is null ||
                UIManager is null ||
                QuestManager is null ||
                DialogueManager is null ||
                SaveManager is null ||
                LoadingUI is null)
            {
                Debug.LogError("Missing references in installer!");
            }
            
            Container.BindInstance(playerManager).AsSingle().NonLazy();
            Container.BindInstance(QuestManager).AsSingle().NonLazy();
            Container.BindInstance(DialogueManager).AsSingle().NonLazy();
            Container.BindInstance(UIManager).AsSingle().NonLazy();
            Container.BindInstance(SaveManager).AsSingle().NonLazy();
            Container.BindInstance(LoadingUI).AsSingle().NonLazy();
            
            Container.Bind<CutscenesManager>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<InputManager>().AsSingle().NonLazy();
            Container.BindFactory<Object, Item, Item.Factory>().FromFactory<PrefabFactory<Item>>();
            Container.BindFactory<object, Runner, RunnerFactory>().FromFactory<CustomRunnerFactory>();
        }
    }
}