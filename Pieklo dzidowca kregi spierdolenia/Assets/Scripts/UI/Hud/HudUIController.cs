using System;
using jbzd.Common.Extensions;
using jbzd.Common.InputSystem.Inputs;
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

		public void Update()
		{
			_maxHealth = 100;
			currentHealth = 100;

			healthBar.SetValueWithoutNotify(currentHealth.Remap(0, _maxHealth, 0, 1));

			float floatCurrentHealth = currentHealth;
			float floatMaxHealth = _maxHealth;
			
			var hpPercentage = Math.Floor(floatCurrentHealth / floatMaxHealth * 100);
			_hpNumber.text = hpPercentage + " %";
		}

		public override bool InitialActivationState() => true;
	} 
}
