using UnityEngine;
using UnityEngine.UI;
using jbzdy.CharacterStats;
using jbzdy.CharacterStats.Stats;
using System.Collections.Generic;

// <summary>
// Napisane przez sharashino
// 
// Skrypt zarządzający dodawaniem i odejmowaniem bazowych ilości statystyk gracza
// 
// Przygotowany do pierwszej kreacji postaci, oraz w przyszłości przy levelowaniu
// </summary>
namespace jbzdy.StatCreation
{
    [System.Serializable]
    public class StatCreator : MonoBehaviour
    {
        [SerializeField] private Text pointsLeft = default;
        [SerializeField] private Text levelNumber = default;
        [SerializeField] private PlayerStats playerStats = default;
        [SerializeField] private int pointsToAdd = default;
        [SerializeField] private List<StatBox> statBoxes = default;
        [SerializeField] private List<Stat> modifiableStats = default;
        private StatBox[] _statBoxes;

        public int PointsToAdd => pointsToAdd;
        public PlayerStats PlayerStats => playerStats;

        private void Start()
        {
            _statBoxes = GetComponentsInChildren<StatBox>();
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

        public void FillStatBoxes(int statPointsToAdd)
        {
            modifiableStats = PlayerStats.modifiableStatsList;
            pointsLeft.text = statPointsToAdd.ToString();
            this.pointsToAdd = statPointsToAdd;
            levelNumber.text = PlayerStats.Level.ToString();

            if(statBoxes.Count < 5)
            {
                foreach (StatBox statBox in _statBoxes)
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
            pointsToAdd += value;
        }
        public void SubtractPointsToAdd(int value)
        {
            pointsToAdd -= value;
        }
    }
}
