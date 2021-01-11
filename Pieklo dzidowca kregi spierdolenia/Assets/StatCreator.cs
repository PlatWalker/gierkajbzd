using System.Collections.Generic;
using UnityEngine;
using jbzdy.CharacterStats;
using UnityEngine.UI;

/// <summary>
/// Napisane przez sharashino
/// 
/// Skrypt zarządzający dodawaniem i odejmowaniem bazowych ilości statystyk gracza
/// 
/// Przygotowany do pierwszej kreacji postaci, oraz w przyszłości przy levelowaniu
/// </summary>
namespace jbzdy.StatCreation
{
    [System.Serializable]
    public class StatCreator : MonoBehaviour
    {
        [SerializeField] private PlayerStats playerStats;
        [SerializeField] private List<StatBox> statBoxes;
        [SerializeField] private List<Stat> modifiableStats;
        [SerializeField] private Text pointsLeft;
        [SerializeField] private Text levelNumber;
        [SerializeField] private int pointsToAdd;

        private void Start()
        {
        }

        private void Update()
        {
            pointsLeft.text = GetPointsToAdd().ToString();
        }

        public void OnApplyPress()
        {
            for (int i = 0; i < statBoxes.Count; i++)
            {
                Debug.Log(modifiableStats[i].SetBaseValue(statBoxes[i].statValue));
                modifiableStats[i].SetBaseValue(statBoxes[i].statValue);

            }
            gameObject.SetActive(false);
        }

        public void FillStatBoxes(int pointsToAdd)
        {
            modifiableStats = playerStats.modifiableStatsList;
            pointsLeft.text = pointsToAdd.ToString();
            this.pointsToAdd = pointsToAdd;
            levelNumber.text = playerStats.ReturnLevel().ToString();

            if(statBoxes.Count < 5)
            {
                foreach (StatBox statBox in GetComponentsInChildren<StatBox>())
                {
                    statBoxes.Add(statBox);
                }
            }


            for (int i = 0; i < modifiableStats.Count; i++)
            {
                statBoxes[i].statName.text = modifiableStats[i].statName;
                statBoxes[i].statValue = modifiableStats[i].GetBaseValue();
                statBoxes[i].statValueText.text = statBoxes[i].statValue.ToString();
            }
        }

        

        public PlayerStats GetPlayerStats()
        {
            return playerStats;
        }

        public int GetPointsToAdd()
        {
            return pointsToAdd;
        }

        public void AddPointsToAdd(int value)
        {
            pointsToAdd += value;
        }
        public void SubtractPointsToAdd(int value)
        {
            pointsToAdd -= value;
        }
    }

    public enum StatType
    {
        Strenght,
        Agility,
        Intelligence,
        Vitality,
        Luck
    }
}
