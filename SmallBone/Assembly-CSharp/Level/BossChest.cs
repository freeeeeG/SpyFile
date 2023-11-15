using System;
using System.Collections.Generic;
using System.Linq;
using Characters;
using Characters.Gear;
using Data;
using FX;
using GameResources;
using Services;
using Singletons;
using UnityEngine;

namespace Level
{
	// Token: 0x02000490 RID: 1168
	public class BossChest : InteractiveObject
	{
		// Token: 0x1400001A RID: 26
		// (add) Token: 0x0600163A RID: 5690 RVA: 0x00045AC0 File Offset: 0x00043CC0
		// (remove) Token: 0x0600163B RID: 5691 RVA: 0x00045AF8 File Offset: 0x00043CF8
		public event Action OnOpen;

		// Token: 0x0600163C RID: 5692 RVA: 0x00045B30 File Offset: 0x00043D30
		protected override void Awake()
		{
			base.Awake();
			Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
			this._random = new System.Random((int)(GameData.Save.instance.randomSeed + 1638136763 + currentChapter.type * (Chapter.Type)256 + currentChapter.stageIndex * 16 + currentChapter.currentStage.pathIndex));
			this._rewards = new Gear[this._count];
		}

		// Token: 0x0600163D RID: 5693 RVA: 0x00045BA4 File Offset: 0x00043DA4
		private void EvaluateGearRarity()
		{
			this._rarity = Singleton<Service>.Instance.levelManager.currentChapter.currentStage.gearPossibilities.Evaluate(this._random);
			this._gearRarity = Settings.instance.containerPossibilities[this._rarity].Evaluate(this._random);
		}

		// Token: 0x0600163E RID: 5694 RVA: 0x00045C04 File Offset: 0x00043E04
		public override void InteractWith(Character character)
		{
			BossChest.<>c__DisplayClass25_0 CS$<>8__locals1 = new BossChest.<>c__DisplayClass25_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.character = character;
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._interactSound, base.transform.position);
			base.StartCoroutine(CS$<>8__locals1.<InteractWith>g__CDropGold|1());
			base.StartCoroutine(CS$<>8__locals1.<InteractWith>g__CDelayedDrop|0());
			base.Deactivate();
			Action onOpen = this.OnOpen;
			if (onOpen == null)
			{
				return;
			}
			onOpen();
		}

		// Token: 0x0600163F RID: 5695 RVA: 0x00045C74 File Offset: 0x00043E74
		private Gear DropBossGear(List<DropMovement> dropMovements, int itemCount)
		{
			Gear gear = null;
			do
			{
				BossChest.BossGears.Property[] values = this._bossGears.values;
				double num = this._random.NextDouble() * (double)values.Sum((BossChest.BossGears.Property a) => a.weight);
				for (int i = 0; i < values.Length; i++)
				{
					num -= (double)values[i].weight;
					if (num <= 0.0)
					{
						gear = values[i].gear;
						break;
					}
				}
			}
			while (this.ContainsInRewards(gear));
			return Singleton<Service>.Instance.levelManager.DropGear(gear, this._dropPoint.transform.position);
		}

		// Token: 0x06001640 RID: 5696 RVA: 0x00045D1C File Offset: 0x00043F1C
		private void DropPotion(List<DropMovement> dropMovements)
		{
			Potion potion = this._potionPossibilities.Get();
			potion = CommonResource.instance.potions[Potion.Size.Large];
			if (potion != null)
			{
				Singleton<Service>.Instance.levelManager.DropPotion(potion, this._dropPoint.transform.position);
			}
		}

		// Token: 0x06001641 RID: 5697 RVA: 0x00045D70 File Offset: 0x00043F70
		private bool ContainsInRewards(Gear target)
		{
			if (target == null)
			{
				return true;
			}
			foreach (Gear gear in this._rewards)
			{
				if (gear == null)
				{
					return false;
				}
				if (gear.name.Equals(target.name, StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001642 RID: 5698 RVA: 0x00045DC4 File Offset: 0x00043FC4
		private bool CheckAlreadyDropAllBossItem()
		{
			for (int i = 0; i < this._bossGears.values.Length; i++)
			{
				BossChest.BossGears.Property property = this._bossGears.values[i];
				if (!this.ContainsInRewards(property.gear))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06001643 RID: 5699 RVA: 0x00045E08 File Offset: 0x00044008
		public override void OnActivate()
		{
			base.OnActivate();
			this._animator.Play(InteractiveObject._activateHash);
		}

		// Token: 0x06001644 RID: 5700 RVA: 0x00045E20 File Offset: 0x00044020
		public override void OnDeactivate()
		{
			base.OnDeactivate();
			this._animator.Play(InteractiveObject._deactivateHash);
		}

		// Token: 0x06001645 RID: 5701 RVA: 0x00045E38 File Offset: 0x00044038
		private void OnDiscard(Gear gear)
		{
			Array.Sort<Gear>(this._rewards, delegate(Gear gear1, Gear gear2)
			{
				if (gear1.type != gear2.type)
				{
					if (gear1.type == Gear.Type.Item)
					{
						return 1;
					}
					if (gear2.type == Gear.Type.Item)
					{
						return -1;
					}
				}
				if (gear1.rarity < gear2.rarity)
				{
					return -1;
				}
				return 1;
			});
			bool flag = false;
			for (int i = 0; i < this._count; i++)
			{
				if (!flag)
				{
					this._discardCount++;
					flag = true;
				}
				if (this._discardCount >= 2)
				{
					this._rewards[i].destructible = false;
					this._rewards[i].onDiscard -= this.OnDiscard;
				}
				this._rewards[i].dropped.onLoot -= this.OnLoot;
				if (this._rewards[i].state == Gear.State.Dropped && this._rewards[i] != null)
				{
					this._rewards[i].gameObject.SetActive(false);
				}
			}
		}

		// Token: 0x06001646 RID: 5702 RVA: 0x00045F1C File Offset: 0x0004411C
		private void OnLoot(Character character)
		{
			for (int i = 0; i < this._count; i++)
			{
				this._rewards[i].dropped.onLoot -= this.OnLoot;
				this._rewards[i].onDiscard -= this.OnDiscard;
				if (this._rewards[i].state == Gear.State.Dropped)
				{
					this._rewards[i].destructible = false;
					this._rewards[i].gameObject.SetActive(false);
				}
			}
		}

		// Token: 0x0400137C RID: 4988
		private const int _randomSeed = 1638136763;

		// Token: 0x0400137D RID: 4989
		private const float _delayToDrop = 0.5f;

		// Token: 0x0400137E RID: 4990
		private const float _horizontalNoise = 0.5f;

		// Token: 0x0400137F RID: 4991
		[SerializeField]
		private Transform _dropPoint;

		// Token: 0x04001380 RID: 4992
		[SerializeField]
		private int _count;

		// Token: 0x04001381 RID: 4993
		[Range(0f, 100f)]
		[SerializeField]
		[Header("Boss Gears")]
		private int _bossItemDropChance = 10;

		// Token: 0x04001382 RID: 4994
		[SerializeField]
		private BossChest.BossGears _bossGears;

		// Token: 0x04001383 RID: 4995
		[GetComponent]
		[SerializeField]
		private Animator _animator;

		// Token: 0x04001384 RID: 4996
		[Header("Gold")]
		[SerializeField]
		private SoundInfo _goldDropSound;

		// Token: 0x04001385 RID: 4997
		[SerializeField]
		private int _goldAmount;

		// Token: 0x04001386 RID: 4998
		[SerializeField]
		private int _goldCount;

		// Token: 0x04001387 RID: 4999
		[SerializeField]
		[Header("Potion")]
		private PotionPossibilities _potionPossibilities;

		// Token: 0x04001388 RID: 5000
		private Rarity _rarity;

		// Token: 0x04001389 RID: 5001
		private Rarity _gearRarity;

		// Token: 0x0400138A RID: 5002
		private System.Random _random;

		// Token: 0x0400138C RID: 5004
		private Gear[] _rewards;

		// Token: 0x0400138D RID: 5005
		private Gear _selected;

		// Token: 0x0400138E RID: 5006
		private int _discardCount;

		// Token: 0x0400138F RID: 5007
		private bool _alreadyDiscardGear;

		// Token: 0x02000491 RID: 1169
		[Serializable]
		private class BossGears : ReorderableArray<BossChest.BossGears.Property>
		{
			// Token: 0x02000492 RID: 1170
			[Serializable]
			internal class Property
			{
				// Token: 0x17000456 RID: 1110
				// (get) Token: 0x06001649 RID: 5705 RVA: 0x00045FB9 File Offset: 0x000441B9
				public float weight
				{
					get
					{
						return this._weight;
					}
				}

				// Token: 0x17000457 RID: 1111
				// (get) Token: 0x0600164A RID: 5706 RVA: 0x00045FC1 File Offset: 0x000441C1
				public Gear gear
				{
					get
					{
						return this._gear;
					}
				}

				// Token: 0x04001390 RID: 5008
				[SerializeField]
				private float _weight;

				// Token: 0x04001391 RID: 5009
				[SerializeField]
				private Gear _gear;
			}
		}
	}
}
