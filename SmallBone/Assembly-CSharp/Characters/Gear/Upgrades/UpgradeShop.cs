using System;
using System.Collections.Generic;
using System.Linq;
using Characters.Player;
using Data;
using Data.Hardmode.Upgrades;
using Level;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Gear.Upgrades
{
	// Token: 0x0200085E RID: 2142
	public sealed class UpgradeShop : Singleton<UpgradeShop>
	{
		// Token: 0x17000953 RID: 2387
		// (get) Token: 0x06002CA3 RID: 11427 RVA: 0x00088241 File Offset: 0x00086441
		public GameData.Currency saleCurrency
		{
			get
			{
				return GameData.Currency.heartQuartz;
			}
		}

		// Token: 0x17000954 RID: 2388
		// (get) Token: 0x06002CA4 RID: 11428 RVA: 0x00088248 File Offset: 0x00086448
		public GameData.Currency removeCurrency
		{
			get
			{
				return GameData.Currency.gold;
			}
		}

		// Token: 0x1400007F RID: 127
		// (add) Token: 0x06002CA5 RID: 11429 RVA: 0x00088250 File Offset: 0x00086450
		// (remove) Token: 0x06002CA6 RID: 11430 RVA: 0x00088288 File Offset: 0x00086488
		public event Action onChanged;

		// Token: 0x14000080 RID: 128
		// (add) Token: 0x06002CA7 RID: 11431 RVA: 0x000882C0 File Offset: 0x000864C0
		// (remove) Token: 0x06002CA8 RID: 11432 RVA: 0x000882F8 File Offset: 0x000864F8
		public event Action<UpgradeResource.Reference> onUpgraded;

		// Token: 0x14000081 RID: 129
		// (add) Token: 0x06002CA9 RID: 11433 RVA: 0x00088330 File Offset: 0x00086530
		// (remove) Token: 0x06002CAA RID: 11434 RVA: 0x00088368 File Offset: 0x00086568
		public event Action<UpgradeResource.Reference> onLevelUp;

		// Token: 0x06002CAB RID: 11435 RVA: 0x0008839D File Offset: 0x0008659D
		protected override void Awake()
		{
			base.Awake();
			base.name = "UpgradeShop";
			this.Sort();
			this._upgradableCache = new List<UpgradeResource.Reference>(this._sortedUpgradables.Count);
			this.CreateNewContext();
		}

		// Token: 0x06002CAC RID: 11436 RVA: 0x000883D4 File Offset: 0x000865D4
		private void Sort()
		{
			if (this._sortedUpgradables == null)
			{
				this._sortedUpgradables = new List<UpgradeResource.Reference>();
			}
			else
			{
				this._sortedUpgradables.Clear();
			}
			foreach (UpgradeResource.Reference item in Singleton<UpgradeManager>.Instance.GetAllUnlockedList())
			{
				this._sortedUpgradables.Add(item);
			}
			this._sortedUpgradables.Sort(delegate(UpgradeResource.Reference refer1, UpgradeResource.Reference refer2)
			{
				if (refer1.orderInShop > refer2.orderInShop)
				{
					return 1;
				}
				if (refer1.orderInShop < refer2.orderInShop)
				{
					return -1;
				}
				return 0;
			});
		}

		// Token: 0x06002CAD RID: 11437 RVA: 0x0008847C File Offset: 0x0008667C
		public int GetRemoveCost(UpgradeObject.Type type)
		{
			return this._context.GetRemoveCost(type);
		}

		// Token: 0x06002CAE RID: 11438 RVA: 0x0008848C File Offset: 0x0008668C
		public void LoadCusredLineUp()
		{
			Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
			this._random = new System.Random((int)(GameData.Save.instance.randomSeed + 2028506624 + currentChapter.type * (Chapter.Type)256 + currentChapter.stageIndex * 16 + currentChapter.currentStage.pathIndex));
			List<UpgradeResource.Reference> list = Singleton<UpgradeManager>.Instance.GetList(UpgradeObject.Type.Cursed);
			list.PseudoShuffle(this._random);
			if (this._cursedLineUp == null)
			{
				this._cursedLineUp = new List<UpgradeResource.Reference>();
			}
			else
			{
				this._cursedLineUp.Clear();
			}
			int num = 0;
			int num2 = 0;
			int balance = this.saleCurrency.balance;
			string typeName = Gear.Type.Upgrade.ToString();
			while (num < 2 && num2 < list.Count)
			{
				UpgradeResource.Reference reference = list[num2++];
				if ((!reference.needUnlock || GameData.Gear.IsUnlocked(typeName, reference.name)) && reference.price <= balance)
				{
					this._cursedLineUp.Add(reference);
					num++;
				}
			}
			if (num < 2)
			{
				num = 0;
				num2 = 0;
				this._cursedLineUp.Clear();
				while (num < 2 && num2 < list.Count)
				{
					UpgradeResource.Reference reference2 = list[num2++];
					if (!reference2.needUnlock || GameData.Gear.IsUnlocked(typeName, reference2.name))
					{
						this._cursedLineUp.Add(reference2);
						num++;
					}
				}
			}
		}

		// Token: 0x06002CAF RID: 11439 RVA: 0x000885EC File Offset: 0x000867EC
		public List<UpgradeResource.Reference> GetRiskObjects()
		{
			return this._cursedLineUp;
		}

		// Token: 0x06002CB0 RID: 11440 RVA: 0x000885F4 File Offset: 0x000867F4
		public List<UpgradeResource.Reference> GetUpgradables()
		{
			this.Sort();
			return this._sortedUpgradables;
		}

		// Token: 0x06002CB1 RID: 11441 RVA: 0x00088602 File Offset: 0x00086802
		public bool TryUpgrade(UpgradeResource.Reference reference)
		{
			if (!this.CheckPurchasable(reference))
			{
				return false;
			}
			if (!Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.upgrade.Has(reference))
			{
				this.Upgrade(reference);
				return true;
			}
			return false;
		}

		// Token: 0x06002CB2 RID: 11442 RVA: 0x00088640 File Offset: 0x00086840
		public bool TryLevelUp(UpgradeResource.Reference reference)
		{
			if (!this.CheckPurchasable(reference))
			{
				return false;
			}
			UpgradeInventory upgrade = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.upgrade;
			int num = upgrade.IndexOf(reference);
			if (num == -1)
			{
				return false;
			}
			UpgradeObject upgradeObject = upgrade.upgrades[num];
			if (reference.GetCurrentLevel() + 1 <= upgradeObject.maxLevel)
			{
				this.saleCurrency.Consume(reference.price);
				this.LevelUp(upgradeObject);
				Action action = this.onChanged;
				if (action != null)
				{
					action();
				}
				Action<UpgradeResource.Reference> action2 = this.onLevelUp;
				if (action2 != null)
				{
					action2(reference);
				}
				return true;
			}
			return false;
		}

		// Token: 0x06002CB3 RID: 11443 RVA: 0x000886E4 File Offset: 0x000868E4
		private void Upgrade(UpgradeResource.Reference reference)
		{
			UpgradeInventory upgrade = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.upgrade;
			this.saleCurrency.Consume(reference.price);
			upgrade.TryEquip(reference);
			Action action = this.onChanged;
			if (action != null)
			{
				action();
			}
			Action<UpgradeResource.Reference> action2 = this.onUpgraded;
			if (action2 == null)
			{
				return;
			}
			action2(reference);
		}

		// Token: 0x06002CB4 RID: 11444 RVA: 0x0008874C File Offset: 0x0008694C
		public bool TryRemoveUpgradeObjet(UpgradeResource.Reference reference, ref int index)
		{
			int removeCost = this.GetRemoveCost(reference.type);
			if (!this.CheckRemovable(removeCost))
			{
				return false;
			}
			UpgradeInventory upgrade = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.upgrade;
			index = upgrade.IndexOf(reference);
			if (index == -1)
			{
				Debug.LogError("해당 강화 요소를 가지고 있지 않습니다.");
				return false;
			}
			UpgradeObject @object = upgrade.upgrades[index];
			upgrade.Remove(index);
			upgrade.Trim();
			this.SettleRemovedObject(@object, removeCost);
			Action action = this.onChanged;
			if (action != null)
			{
				action();
			}
			return true;
		}

		// Token: 0x06002CB5 RID: 11445 RVA: 0x000887E1 File Offset: 0x000869E1
		public bool CheckRemovable(UpgradeResource.Reference reference)
		{
			return this.CheckRemovable(this.GetRemoveCost(reference.type));
		}

		// Token: 0x06002CB6 RID: 11446 RVA: 0x000887F5 File Offset: 0x000869F5
		private void CreateNewContext()
		{
			this._context = new UpgradeShopContext();
		}

		// Token: 0x06002CB7 RID: 11447 RVA: 0x00088802 File Offset: 0x00086A02
		private bool CheckRemovable(int cost)
		{
			return this.removeCurrency.Has(cost);
		}

		// Token: 0x06002CB8 RID: 11448 RVA: 0x00088815 File Offset: 0x00086A15
		private void SettleRemovedObject(UpgradeObject @object, int cost)
		{
			this.removeCurrency.Consume(cost);
			this.saleCurrency.Earn(@object.returnCost);
		}

		// Token: 0x06002CB9 RID: 11449 RVA: 0x00088838 File Offset: 0x00086A38
		private bool CheckPurchasable(UpgradeResource.Reference reference)
		{
			UpgradeInventory upgrade2 = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.upgrade;
			return (upgrade2.upgrades.Any((UpgradeObject upgrade) => upgrade == null) || upgrade2.Has(reference)) && reference != null && reference.GetCurrentLevel() != reference.maxLevel && this.saleCurrency.Has(reference.price);
		}

		// Token: 0x06002CBA RID: 11450 RVA: 0x000888C6 File Offset: 0x00086AC6
		private void LevelUp(UpgradeObject @object)
		{
			@object.LevelUpOrigin();
		}

		// Token: 0x04002595 RID: 9621
		public const int cursedCount = 2;

		// Token: 0x04002596 RID: 9622
		private const int _randomSeed = 2028506624;

		// Token: 0x04002597 RID: 9623
		private System.Random _random;

		// Token: 0x04002598 RID: 9624
		private UpgradeShopContext _context;

		// Token: 0x04002599 RID: 9625
		private List<UpgradeResource.Reference> _cursedLineUp;

		// Token: 0x0400259D RID: 9629
		private List<UpgradeResource.Reference> _sortedUpgradables;

		// Token: 0x0400259E RID: 9630
		private List<UpgradeResource.Reference> _upgradableCache;
	}
}
