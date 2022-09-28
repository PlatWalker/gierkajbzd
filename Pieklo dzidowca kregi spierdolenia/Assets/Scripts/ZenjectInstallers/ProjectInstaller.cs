using jbzd.Quests;
using Zenject;

namespace jbzd.ZenjectInstallers
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<QuestLogManager>().AsSingle().NonLazy();
            /*
             * nie do konca tak mozna robic! Nie powinno sie incjalizowac w installerach. Na razie zostawiam takie
             * rozwiazanie, ale trzeba to wziac pod uwage. 
             */
            Container.Instantiate<Quest>();
        }
    }
}