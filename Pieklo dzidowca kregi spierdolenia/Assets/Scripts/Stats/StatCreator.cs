using UnityEngine;
using UnityEngine.UI;
using jbzdy.CharacterStats;
using jbzdy.CharacterStats.Stats;
using System.Collections.Generic;

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
        [SerializeField] private Text pointsLeft;
        [SerializeField] private Text levelNumber;
        [SerializeField] private PlayerStats _playerStats;
        [SerializeField] private int _pointsToAdd;
        [SerializeField] private List<StatBox> statBoxes;
        [SerializeField] private List<Stat> modifiableStats;

        public int PointsToAdd 
        { 
            get
            {
                return _pointsToAdd;
            } 
        }

        public PlayerStats PlayerStats
        {
            get
            {
                return _playerStats;
            }
        }

        private void Update()
        {
            pointsLeft.text = PointsToAdd.ToString();
        }

        public void OnApplyPress()
        {
            for (int i = 0; i < statBoxes.Count; i++)
            {
                modifiableStats[i].BaseValue = (statBoxes[i].StatValue);
            }
            gameObject.SetActive(false);
        }

        public void FillStatBoxes(int pointsToAdd)
        {
            modifiableStats = PlayerStats.modifiableStatsList;
            pointsLeft.text = pointsToAdd.ToString();
            _pointsToAdd = pointsToAdd;
            levelNumber.text = PlayerStats.Level.ToString();

            if(statBoxes.Count < 5)
            {
                foreach (StatBox statBox in GetComponentsInChildren<StatBox>())
                {
                    statBoxes.Add(statBox);
                }
            }

            for (int i = 0; i < modifiableStats.Count; i++)
            {
                statBoxes[i].StatName.text = modifiableStats[i].StatName;
                statBoxes[i].StatValue = modifiableStats[i].BaseValue;
                statBoxes[i].StatValueText.text = statBoxes[i].StatValue.ToString();
            }
        }

        public void AddPointsToAdd(int value)
        {
            _pointsToAdd += value;
        }
        public void SubtractPointsToAdd(int value)
        {
            _pointsToAdd -= value;
        }
    }
}
