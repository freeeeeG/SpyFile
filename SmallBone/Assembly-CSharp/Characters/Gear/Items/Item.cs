using System;
using System.Collections;
using Characters.Abilities;
using Characters.Gear.Synergy;
using Characters.Gear.Synergy.Inscriptions;
using Characters.Player;
using Data;
using FX;
using GameResources;
using Level;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Gear.Items
{
	// Token: 0x020008FA RID: 2298
	public class Item : Gear
	{
		// Token: 0x17000A68 RID: 2664
		// (get) Token: 0x06003104 RID: 12548 RVA: 0x000076D4 File Offset: 0x000058D4
		public override Gear.Type type
		{
			get
			{
				return Gear.Type.Item;
			}
		}

		// Token: 0x17000A69 RID: 2665
		// (get) Token: 0x06003105 RID: 12549 RVA: 0x00018EC5 File Offset: 0x000170C5
		public override GameData.Currency.Type currencyTypeByDiscard
		{
			get
			{
				return GameData.Currency.Type.Gold;
			}
		}

		// Token: 0x17000A6A RID: 2666
		// (get) Token: 0x06003106 RID: 12550 RVA: 0x000928A8 File Offset: 0x00090AA8
		public override int currencyByDiscard
		{
			get
			{
				if (base.dropped.price <= 0 && this.destructible)
				{
					return WitchBonus.instance.soul.ancientAlchemy.GetGoldByDiscard(this);
				}
				return 0;
			}
		}

		// Token: 0x17000A6B RID: 2667
		// (get) Token: 0x06003107 RID: 12551 RVA: 0x000928D7 File Offset: 0x00090AD7
		protected override string _prefix
		{
			get
			{
				return "item";
			}
		}

		// Token: 0x06003108 RID: 12552 RVA: 0x000928E0 File Offset: 0x00090AE0
		protected override void Awake()
		{
			base.Awake();
			Singleton<Service>.Instance.gearManager.RegisterItemInstance(this);
			Item.BonusKeyword[] array = this.bonusKeyword;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Initialize(this);
			}
		}

		// Token: 0x06003109 RID: 12553 RVA: 0x00092924 File Offset: 0x00090B24
		private void OnDestroy()
		{
			if (Service.quitting)
			{
				return;
			}
			Singleton<Service>.Instance.gearManager.UnregisterItemInstance(this);
			this._abilityAttacher.StopAttach();
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
			if (this.destructible)
			{
				GameData.Progress.encounterItemCount++;
				PersistentSingleton<SoundManager>.Instance.PlaySound(GlobalSoundSettings.instance.gearDestroying, base.transform.position);
				Collider2D component = base.dropped.GetComponent<Collider2D>();
				if (component == null)
				{
					Item.Assets.destroyItem.Spawn(base.transform.position, 0f, 1f);
				}
				else
				{
					Item.Assets.destroyItem.Spawn(component.bounds.center, 0f, 1f);
				}
			}
			if (this.currencyByDiscard == 0)
			{
				return;
			}
			int count = 1;
			if (this.currencyByDiscard > 0)
			{
				switch (base.rarity)
				{
				case Rarity.Common:
					count = 5;
					break;
				case Rarity.Rare:
					count = 8;
					break;
				case Rarity.Unique:
					count = 15;
					break;
				case Rarity.Legendary:
					count = 25;
					break;
				}
			}
			levelManager.DropGold(this.currencyByDiscard, count);
		}

		// Token: 0x0600310A RID: 12554 RVA: 0x00092A70 File Offset: 0x00090C70
		protected override void OnLoot(Character character)
		{
			character.playerComponents.inventory.item.TryEquip(this);
		}

		// Token: 0x0600310B RID: 12555 RVA: 0x000860BA File Offset: 0x000842BA
		public void SetOwner(Character character)
		{
			base.owner = character;
		}

		// Token: 0x0600310C RID: 12556 RVA: 0x00092A8C File Offset: 0x00090C8C
		protected override void OnEquipped()
		{
			base.OnEquipped();
			Item.BonusKeyword[] array = this.bonusKeyword;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Evaluate();
			}
			this._abilityAttacher.Initialize(base.owner);
			this._abilityAttacher.StartAttach();
		}

		// Token: 0x0600310D RID: 12557 RVA: 0x00092AD8 File Offset: 0x00090CD8
		protected override void OnDropped()
		{
			base.OnDropped();
			this._abilityAttacher.StopAttach();
		}

		// Token: 0x0600310E RID: 12558 RVA: 0x00092AEC File Offset: 0x00090CEC
		public void DiscardOnInventory()
		{
			if (base.state == Gear.State.Dropped)
			{
				UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
			ItemInventory item = base.owner.playerComponents.inventory.item;
			item.Discard(this);
			item.Trim();
			Item.BonusKeyword[] array = this.bonusKeyword;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Dispose();
			}
		}

		// Token: 0x0600310F RID: 12559 RVA: 0x00092B4C File Offset: 0x00090D4C
		public void RemoveOnInventory()
		{
			if (base.state == Gear.State.Dropped)
			{
				this.destructible = false;
				UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
			ItemInventory item = base.owner.playerComponents.inventory.item;
			item.Remove(this);
			item.Trim();
			Item.BonusKeyword[] array = this.bonusKeyword;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Dispose();
			}
		}

		// Token: 0x06003110 RID: 12560 RVA: 0x00092BB4 File Offset: 0x00090DB4
		public void ChangeOnInventory(Item item)
		{
			if (base.state == Gear.State.Dropped)
			{
				Debug.LogError("Tried change item " + base.name + " but it's not on inventory");
				return;
			}
			base.owner.playerComponents.inventory.item.Change(this, item);
			item.owner = base.owner;
			item.state = Gear.State.Equipped;
		}

		// Token: 0x06003111 RID: 12561 RVA: 0x00092C14 File Offset: 0x00090E14
		public void EvaluateBonusKeyword(EnumArray<Inscription.Key, int> keywordCounts)
		{
			Item.BonusKeyword[] array = this.bonusKeyword;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Evaluate(keywordCounts);
			}
		}

		// Token: 0x06003112 RID: 12562 RVA: 0x00092C3F File Offset: 0x00090E3F
		public Item Instantiate()
		{
			Item item = UnityEngine.Object.Instantiate<Item>(this);
			item.name = base.name;
			item.Initialize();
			return item;
		}

		// Token: 0x04002857 RID: 10327
		public Inscription.Key keyword1;

		// Token: 0x04002858 RID: 10328
		public Inscription.Key keyword2;

		// Token: 0x04002859 RID: 10329
		public Item.BonusKeyword[] bonusKeyword;

		// Token: 0x0400285A RID: 10330
		[AbilityAttacher.SubcomponentAttribute]
		[SerializeField]
		private AbilityAttacher.Subcomponents _abilityAttacher;

		// Token: 0x020008FB RID: 2299
		private class Assets
		{
			// Token: 0x0400285B RID: 10331
			internal static EffectInfo destroyItem = new EffectInfo(CommonResource.instance.destroyItem);
		}

		// Token: 0x020008FC RID: 2300
		[Serializable]
		public class BonusKeyword
		{
			// Token: 0x17000A6C RID: 2668
			// (get) Token: 0x06003116 RID: 12566 RVA: 0x00092C6F File Offset: 0x00090E6F
			public EnumArray<Inscription.Key, int> Values
			{
				get
				{
					return this._keywordBonusCounts;
				}
			}

			// Token: 0x06003117 RID: 12567 RVA: 0x00092C77 File Offset: 0x00090E77
			public void Initialize(Item ownerItem)
			{
				this._ownerItem = ownerItem;
			}

			// Token: 0x06003118 RID: 12568 RVA: 0x00092C80 File Offset: 0x00090E80
			public void Evaluate()
			{
				Synergy synergy = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.synergy;
				switch (this.type)
				{
				case Item.BonusKeyword.Type.All:
					using (IEnumerator enumerator = Enum.GetValues(typeof(Inscription.Key)).GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							object obj = enumerator.Current;
							Inscription.Key key = (Inscription.Key)obj;
							this._keywordBonusCounts[key] = this.count;
						}
						return;
					}
					break;
				case Item.BonusKeyword.Type.Possesion:
					break;
				case Item.BonusKeyword.Type.Items:
					goto IL_10D;
				case Item.BonusKeyword.Type.Single:
					this._keywordBonusCounts[this.keyword] = this.count;
					return;
				default:
					return;
				}
				using (IEnumerator enumerator = Enum.GetValues(typeof(Inscription.Key)).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						object obj2 = enumerator.Current;
						Inscription.Key key2 = (Inscription.Key)obj2;
						if (synergy.inscriptions[key2].count > 0)
						{
							this._keywordBonusCounts[key2] = this.count;
						}
						else
						{
							this._keywordBonusCounts[key2] = 0;
						}
					}
					return;
				}
				IL_10D:
				this._keywordBonusCounts[this._ownerItem.keyword1] = this.count;
				this._keywordBonusCounts[this._ownerItem.keyword2] = this.count;
			}

			// Token: 0x06003119 RID: 12569 RVA: 0x00092E08 File Offset: 0x00091008
			public void Evaluate(EnumArray<Inscription.Key, int> keywordCounts)
			{
				Inscription.Key key;
				switch (this.type)
				{
				case Item.BonusKeyword.Type.All:
					using (IEnumerator enumerator = Enum.GetValues(typeof(Inscription.Key)).GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							object obj = enumerator.Current;
							key = (Inscription.Key)obj;
							keywordCounts[key] += this.count;
						}
						return;
					}
					break;
				case Item.BonusKeyword.Type.Possesion:
					break;
				case Item.BonusKeyword.Type.Items:
					goto IL_E0;
				case Item.BonusKeyword.Type.Single:
					key = this.keyword;
					keywordCounts[key] += this.count;
					return;
				default:
					return;
				}
				using (IEnumerator enumerator = Enum.GetValues(typeof(Inscription.Key)).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						object obj2 = enumerator.Current;
						Inscription.Key key2 = (Inscription.Key)obj2;
						if (keywordCounts[key2] > 0)
						{
							key = key2;
							keywordCounts[key] += this.count;
						}
					}
					return;
				}
				IL_E0:
				key = this._ownerItem.keyword1;
				keywordCounts[key] += this.count;
				key = this._ownerItem.keyword2;
				keywordCounts[key] += this.count;
			}

			// Token: 0x0600311A RID: 12570 RVA: 0x00092F78 File Offset: 0x00091178
			public void Dispose()
			{
				if (this.type == Item.BonusKeyword.Type.Possesion)
				{
					Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.item.onChanged -= this.OnChangedInventory;
				}
			}

			// Token: 0x0600311B RID: 12571 RVA: 0x00092FB2 File Offset: 0x000911B2
			private void OnChangedInventory()
			{
				this.Evaluate();
			}

			// Token: 0x0400285C RID: 10332
			public Item.BonusKeyword.Type type;

			// Token: 0x0400285D RID: 10333
			public int count;

			// Token: 0x0400285E RID: 10334
			[Header("Type이 Single일 경우에만 입력")]
			public Inscription.Key keyword;

			// Token: 0x0400285F RID: 10335
			private EnumArray<Inscription.Key, int> _keywordBonusCounts = new EnumArray<Inscription.Key, int>();

			// Token: 0x04002860 RID: 10336
			private Item _ownerItem;

			// Token: 0x020008FD RID: 2301
			public enum Type
			{
				// Token: 0x04002862 RID: 10338
				All,
				// Token: 0x04002863 RID: 10339
				Possesion,
				// Token: 0x04002864 RID: 10340
				Items,
				// Token: 0x04002865 RID: 10341
				Single
			}
		}
	}
}
