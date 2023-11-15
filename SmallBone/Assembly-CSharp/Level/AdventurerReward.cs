using System;
using System.Collections;
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
	// Token: 0x0200048B RID: 1163
	public class AdventurerReward : MonoBehaviour, ILootable
	{
		// Token: 0x14000019 RID: 25
		// (add) Token: 0x06001620 RID: 5664 RVA: 0x00045468 File Offset: 0x00043668
		// (remove) Token: 0x06001621 RID: 5665 RVA: 0x000454A0 File Offset: 0x000436A0
		public event Action onLoot;

		// Token: 0x17000452 RID: 1106
		// (get) Token: 0x06001622 RID: 5666 RVA: 0x000454D5 File Offset: 0x000436D5
		// (set) Token: 0x06001623 RID: 5667 RVA: 0x000454DD File Offset: 0x000436DD
		public bool looted
		{
			get
			{
				return this._looted;
			}
			private set
			{
				if (!this._looted && value)
				{
					PersistentSingleton<SoundManager>.Instance.PlaySound(this._buySound, base.transform.position);
				}
				this._looted = value;
			}
		}

		// Token: 0x06001624 RID: 5668 RVA: 0x0004550F File Offset: 0x0004370F
		private void Awake()
		{
			this._choiceTable.sprite = Singleton<Service>.Instance.levelManager.currentChapter.gateChoiceTable;
		}

		// Token: 0x06001625 RID: 5669 RVA: 0x00045530 File Offset: 0x00043730
		private void Load()
		{
			Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
			System.Random random = new System.Random((int)(GameData.Save.instance.randomSeed + -1222149140 + currentChapter.type * (Chapter.Type)256 + currentChapter.stageIndex * 16 + currentChapter.currentStage.pathIndex));
			this._gearInfosToDrop = new GearReference[this._slots.Length];
			this._gearRequests = new GearRequest[this._slots.Length];
			for (int i = 0; i < this._slots.Length; i++)
			{
				this._slots[i].gameObject.SetActive(true);
				RarityPossibilities gearPossibilities = Singleton<Service>.Instance.levelManager.currentChapter.currentStage.gearPossibilities;
				GearManager gearManager = Singleton<Service>.Instance.gearManager;
				WeaponReference weaponToTake = gearManager.GetWeaponToTake(random, gearPossibilities.Evaluate(random));
				EssenceReference quintessenceToTake = gearManager.GetQuintessenceToTake(random, gearPossibilities.Evaluate(random));
				ItemReference itemToTake = gearManager.GetItemToTake(random, gearPossibilities.Evaluate(random));
				this._gearInfosToDrop[0] = weaponToTake;
				this._gearInfosToDrop[1] = quintessenceToTake;
				this._gearInfosToDrop[2] = itemToTake;
			}
			for (int j = 0; j < this._slots.Length; j++)
			{
				this._gearRequests[j] = this._gearInfosToDrop[j].LoadAsync();
			}
		}

		// Token: 0x06001626 RID: 5670 RVA: 0x00045677 File Offset: 0x00043877
		public void Activate()
		{
			this.Load();
			base.StartCoroutine(this.CDisplayItems());
		}

		// Token: 0x06001627 RID: 5671 RVA: 0x0004568C File Offset: 0x0004388C
		private IEnumerator CDisplayItems()
		{
			int num;
			for (int i = 0; i < this._slots.Length; i = num + 1)
			{
				while (!this._gearRequests[i].isDone)
				{
					yield return null;
				}
				Gear gear = Singleton<Service>.Instance.levelManager.DropGear(this._gearRequests[i], this._slots[i].displayPosition);
				gear.onDiscard += this.OnDiscard2;
				bool destructible = gear.destructible;
				this._slots[i].droppedGear = gear.dropped;
				if (destructible)
				{
					gear.dropped.onLoot += this.OnLoot2;
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x06001628 RID: 5672 RVA: 0x0004569C File Offset: 0x0004389C
		private void OnLoot2(Character character)
		{
			Action action = this.onLoot;
			if (action != null)
			{
				action();
			}
			this.looted = true;
			for (int i = 0; i < this._slots.Length; i++)
			{
				AdventurerRewardSlot adventurerRewardSlot = this._slots[i];
				adventurerRewardSlot.droppedGear.onLoot -= this.OnLoot2;
				adventurerRewardSlot.droppedGear.gear.onDiscard -= this.OnDiscard2;
				if (adventurerRewardSlot.droppedGear.gear.state == Gear.State.Dropped)
				{
					adventurerRewardSlot.droppedGear.gear.destructible = false;
					adventurerRewardSlot.droppedGear.gear.gameObject.SetActive(false);
				}
			}
		}

		// Token: 0x06001629 RID: 5673 RVA: 0x0004574C File Offset: 0x0004394C
		private void OnDiscard2(Gear gear)
		{
			Action action = this.onLoot;
			if (action != null)
			{
				action();
			}
			this.looted = true;
			for (int i = 0; i < this._slots.Length; i++)
			{
				AdventurerRewardSlot adventurerRewardSlot = this._slots[i];
				adventurerRewardSlot.droppedGear.onLoot -= this.OnLoot2;
				adventurerRewardSlot.droppedGear.gear.onDiscard -= this.OnDiscard2;
				if (gear != adventurerRewardSlot.droppedGear.gear && adventurerRewardSlot.droppedGear.gear.state == Gear.State.Dropped)
				{
					adventurerRewardSlot.droppedGear.gear.destructible = false;
					adventurerRewardSlot.droppedGear.gear.gameObject.SetActive(false);
				}
			}
		}

		// Token: 0x04001363 RID: 4963
		private const int _randomSeed = -1222149140;

		// Token: 0x04001364 RID: 4964
		[SerializeField]
		private SpriteRenderer _choiceTable;

		// Token: 0x04001365 RID: 4965
		[SerializeField]
		private SoundInfo _buySound;

		// Token: 0x04001366 RID: 4966
		[SerializeField]
		private AdventurerRewardSlot[] _slots;

		// Token: 0x04001367 RID: 4967
		[SerializeField]
		private BonusCurrencyWithDroppedGear _hardmodeBonus;

		// Token: 0x04001368 RID: 4968
		private GearReference[] _gearInfosToDrop;

		// Token: 0x04001369 RID: 4969
		private GearRequest[] _gearRequests;

		// Token: 0x0400136B RID: 4971
		private int _discardCount;

		// Token: 0x0400136C RID: 4972
		private bool _looted;
	}
}
