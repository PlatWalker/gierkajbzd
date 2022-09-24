using System;
using jbzdy.CharacterStats;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using jbzd;
using jbzd.MainHero;
using jbzd.UI.NewUI;
using jbzdy.Player;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace jbzdy.UI.HUD
{
	public class HudController : UserInterfaceController
	{
		private PlayerStats playerStats;

		private int maxHealth;
		[SerializeField]
		private int currentHealth;
		
		[SerializeField]
		private Slider healthBar;
		[SerializeField]
		private Slider manaBar;

		private Text hpNumber;
		
		private PlayerController _playerController;
        
		[Inject]
		public void Construct(PlayerController playerController)
		{
			_playerController = playerController;
		}
		
		public void Start()
		{
			playerStats = _playerController.gameObject.GetComponent<PlayerStats>();

			hpNumber = GetComponentInChildren<Text>();
		}

		public void Update()
		{
			maxHealth = playerStats.MaxHealth;
			currentHealth = playerStats.Health.BaseValue;

			healthBar.SetValueWithoutNotify(currentHealth.Remap(0, maxHealth, 0, 1));

			float floatCurrentHealth = currentHealth;
			float floatMaxHealth = maxHealth;
			
			var hpPercentage = Math.Floor(floatCurrentHealth / floatMaxHealth * 100);
			hpNumber.text = hpPercentage + " %";
		}

	} 
}
