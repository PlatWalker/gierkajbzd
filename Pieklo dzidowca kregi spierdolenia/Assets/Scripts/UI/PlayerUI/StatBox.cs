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
        [SerializeField] private int _statValue = default;
        [SerializeField] private Text _statName = default;
        [SerializeField] private Text _statValueText = default;
        [SerializeField] private GameObject addValueButton = default;
        [SerializeField] private GameObject subtractValueButton = default;
        [SerializeField] private StatCreator statCreator = default;

        #region properties

        public int StatValue
        {
            get
            {
                return _statValue;
            }
            set
            {
                _statValue = value;
            }
        }

        public Text StatName
        {
            get
            {
                return _statName;
            }
            set
            {
                _statName.text = value.ToString();
            }
        }

        public Text StatValueText
        {
            get
            {
                return _statValueText;
            }
            set
            {
                _statValueText.text = value.ToString();
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
            _statValue += 1;
            statCreator.SubtractPointsToAdd(1);

            _statValueText.text = _statValue.ToString();
        }

        public void SubtractStatValue()
        {
            _statValue -= 1;
            statCreator.AddPointsToAdd(1);

            if(_statValue <= 0)
            {
                _statValue = 0;
                _statValueText.text = _statValue.ToString();
            }

            _statValueText.text = _statValue.ToString();
        }
    }
}

