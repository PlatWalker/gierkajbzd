using System;
using jbzd.Common.Extensions;
using jbzd.Common.InputSystem.Inputs;
using jbzd.MainHero;
using jbzd.MainHero.LegacyHeroThings.Stats;
using jbzdy.CharacterStats;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace jbzd.UI.Hud
{
	public class HudUIController : UserInterfaceController
	{
		private PlayerStats _playerStats;
		private int _maxHealth;
		[SerializeField]
		private int currentHealth;
		[SerializeField]
		private Slider healthBar;
		[SerializeField]
		private Slider manaBar;
		private Text _hpNumber;
		private PlayerManager _playerController;
        
		[Inject]
		public void Construct(PlayerManager playerController)
		{
			_playerController = playerController;
		}
		
		public void Start()
		{
			_playerStats = _playerController.gameObject.GetComponent<PlayerStats>();

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

		public override void ConnectInputToHandler(UserInterfaceInput input) { }

		public override bool InitialActivationState() => true;
	} 
}
