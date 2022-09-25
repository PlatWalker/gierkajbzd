using System;
using jbzd.Common.InputSystem.Inputs;
using jbzd.MainHero;
using jbzdy.CharacterStats;
using jbzdy.Player;
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
		private PlayerController _playerController;
        
		[Inject]
		public void Construct(PlayerController playerController)
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
			_maxHealth = _playerStats.MaxHealth;
			currentHealth = _playerStats.Health.BaseValue;

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
