using jbzd.UI;

namespace jbzd
{
    public class GameManager : Singleton<GameManager>
    {
        public UIControllerLegacy UIControllerInstance { get; private set; }
        
        private void Awake()
        {
            Init();
        }

        public void Init()
        {
            UIControllerInstance = FindObjectOfType<UIControllerLegacy>();
        }
    }
}
