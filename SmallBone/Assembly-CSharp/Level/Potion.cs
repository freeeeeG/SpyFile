using System;
using Characters;
using Characters.Abilities;
using Characters.Abilities.Savable;
using Characters.Abilities.Upgrades;
using Characters.Operations.Fx;
using Data;
using FX.SpriteEffects;
using GameResources;
using Services;
using Singletons;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Level
{
	// Token: 0x02000480 RID: 1152
	public class Potion : DroppedGear
	{
		// Token: 0x1700044A RID: 1098
		// (get) Token: 0x060015F4 RID: 5620 RVA: 0x00044D1A File Offset: 0x00042F1A
		protected string _keyBase
		{
			get
			{
				return "Potion/" + base.name;
			}
		}

		// Token: 0x1700044B RID: 1099
		// (get) Token: 0x060015F5 RID: 5621 RVA: 0x00044D2C File Offset: 0x00042F2C
		public string displayName
		{
			get
			{
				return Localization.GetLocalizedString(this._keyBase + "/name");
			}
		}

		// Token: 0x1700044C RID: 1100
		// (get) Token: 0x060015F6 RID: 5622 RVA: 0x00044D43 File Offset: 0x00042F43
		public string description
		{
			get
			{
				return Localization.GetLocalizedString(this._keyBase + "/desc");
			}
		}

		// Token: 0x1700044D RID: 1101
		// (get) Token: 0x060015F7 RID: 5623 RVA: 0x00044D5C File Offset: 0x00042F5C
		protected override bool _interactable
		{
			get
			{
				Character player = Singleton<Service>.Instance.levelManager.player;
				return player.ability.GetInstance<LifeChange>() != null || player.ability.GetInstance<Glutton>() != null || player.health.percent != 1.0;
			}
		}

		// Token: 0x060015F8 RID: 5624 RVA: 0x00044DB0 File Offset: 0x00042FB0
		public override void InteractWith(Character character)
		{
			if (!GameData.Currency.gold.Has(this.price))
			{
				return;
			}
			bool instance = character.ability.GetInstance<LifeChange>() != null;
			IAbilityInstance instance2 = character.ability.GetInstance<Glutton>();
			if (!instance && instance2 == null && character.health.percent == 1.0)
			{
				return;
			}
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._interactSound, base.transform.position);
			this._spawn.Run(character);
			character.spriteEffectStack.Add(new EasedColorBlend(this.priority, this._startColor, this._endColor, this._curve));
			if (character.ability.GetInstance<LifeChange>() != null)
			{
				character.playerComponents.savableAbilityManager.IncreaseStack(SavableAbilityManager.Name.LifeChange, 1f);
			}
			else
			{
				character.health.Heal(new Health.HealInfo(Health.HealthGiverType.Potion, (double)this._healAmount, true));
			}
			UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x060015F9 RID: 5625 RVA: 0x00002191 File Offset: 0x00000391
		public override void OpenPopupBy(Character character)
		{
		}

		// Token: 0x060015FA RID: 5626 RVA: 0x00002191 File Offset: 0x00000391
		public override void ClosePopup()
		{
		}

		// Token: 0x060015FB RID: 5627 RVA: 0x00002191 File Offset: 0x00000391
		public void Initialize()
		{
		}

		// Token: 0x04001332 RID: 4914
		[FormerlySerializedAs("_healthHealingPercent")]
		[SerializeField]
		private int _healAmount;

		// Token: 0x04001333 RID: 4915
		[SerializeField]
		private int priority;

		// Token: 0x04001334 RID: 4916
		[SerializeField]
		private Color _startColor;

		// Token: 0x04001335 RID: 4917
		[SerializeField]
		private Color _endColor;

		// Token: 0x04001336 RID: 4918
		[SerializeField]
		private Curve _curve;

		// Token: 0x04001337 RID: 4919
		[SerializeField]
		[Subcomponent(typeof(SpawnEffect))]
		private SpawnEffect _spawn;

		// Token: 0x04001338 RID: 4920
		[SerializeField]
		[Header("생명전환 : 저주 요소")]
		private int _potionToPowerAmount = 1;

		// Token: 0x04001339 RID: 4921
		private const string _prefix = "Potion";

		// Token: 0x02000481 RID: 1153
		public enum Size
		{
			// Token: 0x0400133B RID: 4923
			Small,
			// Token: 0x0400133C RID: 4924
			Medium,
			// Token: 0x0400133D RID: 4925
			Large
		}
	}
}
