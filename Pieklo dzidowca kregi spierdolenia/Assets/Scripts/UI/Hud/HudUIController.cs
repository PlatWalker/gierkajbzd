using System;
using jbzd.Common.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace jbzd.UI.Hud
{
	public class HudUIController : UserInterfaceController
	{
		private int _maxHealth;
		[SerializeField]
		private int currentHealth;
		[SerializeField]
		private Slider healthBar;
		[SerializeField]
		private Slider manaBar;
		private Text _hpNumber;
		
		public void Start()
		{
			_hpNumber = GetComponentInChildren<Text>();
		}

		public void Init(int maxHealth)
		{
			_maxHealth = maxHealth;
			currentHealth = maxHealth;
		}
        
		public void UpdateHealthBar(int currentHealthUpdate)
		{
			currentHealth = currentHealthUpdate;
			healthBar.SetValueWithoutNotify(currentHealth.Remap(0, _maxHealth, 0, 1));

			float floatCurrentHealth = currentHealth;
			float floatMaxHealth = _maxHealth;
			
			var hpPercentage = Math.Floor(floatCurrentHealth / floatMaxHealth * 100);
			_hpNumber.text = hpPercentage + " %";
		}

		public void DeathSimulation()
		{
			_hpNumber.resizeTextForBestFit = true;
			_hpNumber.text = "Smierc gracza nie zaimplementowana ze wzgledu na koniec projektu xD";
		}
		
		public override bool InitialActivationState() => true;
	} 
}
