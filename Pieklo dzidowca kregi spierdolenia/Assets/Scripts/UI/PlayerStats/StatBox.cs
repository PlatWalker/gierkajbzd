using jbzd.MainHero.LegacyHeroThings.Stats;
using UnityEngine;
using UnityEngine.UI;

// <summary>
// Napisane przez sharashino
// 
// 
// </summary>
namespace jbzdy.StatCreation
{
    public class StatBox : MonoBehaviour
    {
        [SerializeField] private int statValue = default;
        [SerializeField] private Text statName = default;
        [SerializeField] private Text statValueText = default;
        [SerializeField] private GameObject addValueButton = default;
        [SerializeField] private GameObject subtractValueButton = default;
        [SerializeField] private StatCreator statCreator = default;

        #region properties

        public int StatValue
        {
            get
            {
                return statValue;
            }
            set
            {
                statValue = value;
            }
        }

        public Text StatName
        {
            get
            {
                return statName;
            }
            set
            {
                statName.text = value.ToString();
            }
        }

        public Text StatValueText
        {
            get
            {
                return statValueText;
            }
            set
            {
                statValueText.text = value.ToString();
            }
        }

        #endregion

        private void Start()
        {
            statCreator = gameObject.GetComponentInParent<StatCreator>();
        }

        private void Update()
        {
            if(statCreator.PointsToAdd != 0)
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

