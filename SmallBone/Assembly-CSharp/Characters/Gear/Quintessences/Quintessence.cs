using System;
using Characters.Abilities;
using Characters.Cooldowns;
using Characters.Gear.Quintessences.Constraints;
using Characters.Gear.Quintessences.Effects;
using Characters.Player;
using Data;
using FX;
using GameResources;
using Level;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Gear.Quintessences
{
	// Token: 0x020008D9 RID: 2265
	public class Quintessence : Gear
	{
		// Token: 0x17000A50 RID: 2640
		// (get) Token: 0x0600304C RID: 12364 RVA: 0x000147BD File Offset: 0x000129BD
		public override Gear.Type type
		{
			get
			{
				return Gear.Type.Quintessence;
			}
		}

		// Token: 0x17000A51 RID: 2641
		// (get) Token: 0x0600304D RID: 12365 RVA: 0x00090DC3 File Offset: 0x0008EFC3
		public CooldownSerializer cooldown
		{
			get
			{
				return this._cooldown;
			}
		}

		// Token: 0x17000A52 RID: 2642
		// (get) Token: 0x0600304E RID: 12366 RVA: 0x00090DCB File Offset: 0x0008EFCB
		public Constraint.Subcomponents constraints
		{
			get
			{
				return this._constraints;
			}
		}

		// Token: 0x17000A53 RID: 2643
		// (get) Token: 0x0600304F RID: 12367 RVA: 0x00090DD3 File Offset: 0x0008EFD3
		protected override string _prefix
		{
			get
			{
				return "quintessence";
			}
		}

		// Token: 0x17000A54 RID: 2644
		// (get) Token: 0x06003050 RID: 12368 RVA: 0x000855C1 File Offset: 0x000837C1
		public string activeName
		{
			get
			{
				return Localization.GetLocalizedString(base._keyBase + "/active/name");
			}
		}

		// Token: 0x17000A55 RID: 2645
		// (get) Token: 0x06003051 RID: 12369 RVA: 0x000855D8 File Offset: 0x000837D8
		public string activeDescription
		{
			get
			{
				return Localization.GetLocalizedString(base._keyBase + "/active/desc");
			}
		}

		// Token: 0x17000A56 RID: 2646
		// (get) Token: 0x06003052 RID: 12370 RVA: 0x00090DDA File Offset: 0x0008EFDA
		public Sprite hudIcon
		{
			get
			{
				return GearResource.instance.GetQuintessenceHudIcon(base.name) ?? base.icon;
			}
		}

		// Token: 0x17000A57 RID: 2647
		// (get) Token: 0x06003053 RID: 12371 RVA: 0x00090DF8 File Offset: 0x0008EFF8
		public override int currencyByDiscard
		{
			get
			{
				switch (base.rarity)
				{
				case Rarity.Common:
					return 3;
				case Rarity.Rare:
					return 5;
				case Rarity.Unique:
					return 10;
				case Rarity.Legendary:
					return 15;
				default:
					return 0;
				}
			}
		}

		// Token: 0x17000A58 RID: 2648
		// (get) Token: 0x06003054 RID: 12372 RVA: 0x00090E2F File Offset: 0x0008F02F
		public static string currencySpriteKey
		{
			get
			{
				return "<sprite name=\"Others/Heart_Icon\">";
			}
		}

		// Token: 0x17000A59 RID: 2649
		// (get) Token: 0x06003055 RID: 12373 RVA: 0x00090E36 File Offset: 0x0008F036
		public static string currencyTextColorCode
		{
			get
			{
				return "AF00C5";
			}
		}

		// Token: 0x06003056 RID: 12374 RVA: 0x00090E40 File Offset: 0x0008F040
		protected override void OnLoot(Character character)
		{
			QuintessenceInventory quintessence = character.playerComponents.inventory.quintessence;
			if (!quintessence.TryEquip(this))
			{
				quintessence.EquipAt(this, 0);
			}
		}

		// Token: 0x06003057 RID: 12375 RVA: 0x00090E6F File Offset: 0x0008F06F
		public void SetOwner(Character character)
		{
			base.owner = character;
			QuintessenceEffect.Subcomponents passive = this._passive;
			if (passive == null)
			{
				return;
			}
			passive.Invoke(this);
		}

		// Token: 0x14000084 RID: 132
		// (add) Token: 0x06003058 RID: 12376 RVA: 0x00090E8C File Offset: 0x0008F08C
		// (remove) Token: 0x06003059 RID: 12377 RVA: 0x00090EC4 File Offset: 0x0008F0C4
		public event Action onUse;

		// Token: 0x0600305A RID: 12378 RVA: 0x00090EF9 File Offset: 0x0008F0F9
		protected override void Awake()
		{
			base.Awake();
			Singleton<Service>.Instance.gearManager.RegisterEssenceInstance(this);
			this._cooldown.Serialize();
		}

		// Token: 0x0600305B RID: 12379 RVA: 0x00090F1C File Offset: 0x0008F11C
		private void OnDestroy()
		{
			if (Service.quitting)
			{
				return;
			}
			Singleton<Service>.Instance.gearManager.UnregisterEssenceInstance(this);
			Action<Gear> onDiscard = this._onDiscard;
			if (onDiscard != null)
			{
				onDiscard(this);
			}
			LevelManager levelManager = Singleton<Service>.Instance.levelManager;
			if (levelManager.player == null || !levelManager.player.liveAndActive)
			{
				return;
			}
			if (!this.destructible)
			{
				return;
			}
			int currencyByDiscard = this.currencyByDiscard;
			levelManager.player.playerComponents.savableAbilityManager.IncreaseStack(SavableAbilityManager.Name.EssenceSpirit, (float)currencyByDiscard);
			Collider2D component = base.dropped.GetComponent<Collider2D>();
			if (component == null)
			{
				Quintessence.Assets.destryoEssence.Spawn(base.transform.position, 0f, 1f);
			}
			else
			{
				Quintessence.Assets.destryoEssence.Spawn(component.bounds.center, 0f, 1f);
			}
			PersistentSingleton<SoundManager>.Instance.PlaySound(GlobalSoundSettings.instance.gearDestroying, base.transform.position);
			GameData.Progress.encounterEssenceCount++;
		}

		// Token: 0x0600305C RID: 12380 RVA: 0x00091029 File Offset: 0x0008F229
		protected override void OnEquipped()
		{
			base.OnEquipped();
			if (this._cooldown.type == CooldownSerializer.Type.Time)
			{
				this._cooldown.time.GetCooldownSpeed = new Func<float>(base.owner.stat.GetQuintessenceCooldownSpeed);
			}
		}

		// Token: 0x0600305D RID: 12381 RVA: 0x00091068 File Offset: 0x0008F268
		public void Use()
		{
			if (!this._constraints.components.Pass())
			{
				return;
			}
			if (this._cooldown.Consume())
			{
				QuintessenceEffect.Subcomponents active = this._active;
				if (active != null)
				{
					active.Invoke(this);
				}
				Action action = this.onUse;
				if (action == null)
				{
					return;
				}
				action();
			}
		}

		// Token: 0x0600305E RID: 12382 RVA: 0x000910B8 File Offset: 0x0008F2B8
		public void ChangeOnInventory(Quintessence item)
		{
			if (base.state == Gear.State.Dropped)
			{
				Debug.LogError("Tried change essence " + base.gameObject.name + " but it's not on inventory");
				return;
			}
			base.owner.playerComponents.inventory.quintessence.Change(this, item);
			item.owner = base.owner;
			item.state = Gear.State.Equipped;
		}

		// Token: 0x0600305F RID: 12383 RVA: 0x0009111C File Offset: 0x0008F31C
		public Quintessence Instantiate()
		{
			Quintessence quintessence = UnityEngine.Object.Instantiate<Quintessence>(this);
			quintessence.name = base.name;
			quintessence.Initialize();
			return quintessence;
		}

		// Token: 0x04002806 RID: 10246
		[SerializeField]
		private CooldownSerializer _cooldown;

		// Token: 0x04002807 RID: 10247
		[SerializeField]
		[Constraint.SubcomponentAttribute]
		private Constraint.Subcomponents _constraints;

		// Token: 0x04002808 RID: 10248
		[QuintessenceEffect.SubcomponentAttribute]
		[SerializeField]
		private QuintessenceEffect.Subcomponents _passive;

		// Token: 0x04002809 RID: 10249
		[SerializeField]
		[QuintessenceEffect.SubcomponentAttribute]
		private QuintessenceEffect.Subcomponents _active;

		// Token: 0x020008DA RID: 2266
		private class Assets
		{
			// Token: 0x0400280B RID: 10251
			internal static EffectInfo destryoEssence = new EffectInfo(CommonResource.instance.destroyEssence);
		}
	}
}
