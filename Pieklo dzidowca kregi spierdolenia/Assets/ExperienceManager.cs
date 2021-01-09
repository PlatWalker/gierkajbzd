using UnityEngine;

/// <summary>
/// Napisane przez sharashino   
/// 
/// Zaołożeniem tego skryptu jest monitorowanie oraz umożliwianie zapisu postępu poziomów w grze
/// </summary>

namespace jbzdy.StatCreation
{
    public class ExperienceManager : MonoBehaviour
    {
        public StatCreator statCreator;
        public int statPoint;
        public int toNextLevel;

        private void Start()
        {
            toNextLevel = 100;
        }

        public void LevelUp()
        {
            toNextLevel += 100 + toNextLevel / 5;

            statCreator.TurnStatCreator(statPoint);
        }
    }
}

