using UnityEngine;
using jbzdy.CharacterStats;

/// <summary>
/// Napisane przez sharashino   
/// 
/// Zaołożeniem tego skryptu jest monitorowanie oraz umożliwianie zapisu postępu poziomów w grze
/// </summary>

namespace jbzdy.StatCreation
{
    public class ExperienceManager : MonoBehaviour
    {
        [SerializeField] private PlayerStats playerStats;
        public StatCreator statCreator;
        public int statPoint;
        public int toNextLevel;

        private void Start()
        {
            toNextLevel = 100;
        }

        public void LevelUp()
        {
            //Wywoływanie wszystkiego co powinno się wydarzyć podczas kiedy gracz zdobywa poziom
            CalculateNextLevelXP();
            statCreator.GetPlayerStats().AddToLevel(1);
            statCreator.FillStatBoxes(statPoint);
            statCreator.gameObject.SetActive(true);
        }

        private void CalculateNextLevelXP()
        {
            //Tutaj będzie logika obliczająca wymaganą ilość XP do następnego poziomu
            toNextLevel += 100 + toNextLevel / 5;
        }

        public void AddXP(int xpPoints)
        {
            playerStats.SetExperiencePoints(xpPoints);

            if(playerStats.GetExperiencePoints() >= toNextLevel)
            {
                LevelUp();
            }
        }
    }
}

