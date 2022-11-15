using System;
using jbzd.MainHero.LegacyHeroThings.Stats;
using UnityEngine;

// <summary>
// Napisane przez sharashino   
// 
// Zaołożeniem tego skryptu jest monitorowanie oraz umożliwianie zapisu postępu poziomów w grze
// </summary>

namespace jbzd.MainHero.LegacyHeroThings
{
    [Obsolete("System sharashino...")]
    public class ExperienceManager : MonoBehaviour
    {
        [SerializeField] private PlayerStats playerStats = default;
        [SerializeField] private StatCreator statCreator = default;
        [SerializeField] private int statPoint = default;
        [SerializeField] private int toNextLevel = default;

        private void Start()
        {
            toNextLevel = 100;
        }

        public void LevelUp()
        {
            //Wywoływanie wszystkiego co powinno się wydarzyć podczas kiedy gracz zdobywa poziom
            CalculateNextLevelXP();
            statCreator.PlayerStats.AddToLevel(1);
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

            if(playerStats.ExperiencePoints >= toNextLevel)
            {
                LevelUp();
            }
        }
    }
}

