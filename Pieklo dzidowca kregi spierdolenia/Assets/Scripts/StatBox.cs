using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Napisane przez sharashino
/// 
/// 
/// </summary>
namespace jbzdy.StatCreation
{
    public class StatBox : MonoBehaviour
    {
        public int statValue;
        public Text statName;
        public Text statValueText;
        public GameObject addValueButton;
        public GameObject subtractValueButton;
        public StatCreator statCreator;
        
        private void Start()
        {
            statCreator = gameObject.GetComponentInParent<StatCreator>();
        }

        private void Update()
        {
            if(statCreator.GetPointsToAdd() != 0)
            {
                addValueButton.SetActive(true);
                subtractValueButton.SetActive(false);
            }
            else
            {
                addValueButton.SetActive(false);
                subtractValueButton.SetActive(true);
            }
        }

        public void SetStatValue(int value)
        {
            statValue = value;
        }

        public void AddStatValue()
        {
            statValue += 1;
            statCreator.SubtractPointsToAdd(1);

            statValueText.text = statValue.ToString();
        }

        public void SubtractStatValue()
        {
            statValue -= 1;
            statCreator.AddPointsToAdd(1);

            if(statValue <= 0)
            {
                statValue = 0;
                statValueText.text = statValue.ToString();
            }

            statValueText.text = statValue.ToString();
        }
    }
}

