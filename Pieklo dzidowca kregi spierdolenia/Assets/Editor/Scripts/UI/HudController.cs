using jbzdy.CharacterStats;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace jbzdy.UI.HUD
{
	public class HudController : MonoBehaviour
	{
		private PlayerStats playerStats;

		private int maxHealth;
		private int currentHealth;

		private Slider healthBar;
		private Slider manaBar;

		private Text hpNumber;

		public void Start()
		{
			playerStats = GameManager.Instance.PlayerObject.GetComponent<PlayerStats>();

			maxHealth = playerStats.MaxHealth;
			currentHealth = playerStats.Health.BaseValue;

			healthBar = GetComponentsInChildren<Slider>().First(x => x.name == "HealthBar");
			manaBar = GetComponentsInChildren<Slider>().First(x => x.name == "ManaBar");

			hpNumber = GetComponentInChildren<Text>();
		}

		public void Update()
		{
			maxHealth = playerStats.MaxHealth;
			currentHealth = playerStats.Health.BaseValue;

			healthBar.SetValueWithoutNotify(currentHealth.Remap(0, maxHealth, 0, 1));

			hpNumber.text = currentHealth.ToString() + " / " + maxHealth.ToString();
		}

	} 
}
