using System;
using Characters.Player;
using Level;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Gear.Upgrades
{
	// Token: 0x02000859 RID: 2137
	public sealed class UpgradeObject : MonoBehaviour, IComparable<UpgradeObject>
	{
		// Token: 0x17000933 RID: 2355
		// (get) Token: 0x06002C66 RID: 11366 RVA: 0x00087C48 File Offset: 0x00085E48
		public int orderInShop
		{
			get
			{
				return this._orderInShop;
			}
		}

		// Token: 0x17000934 RID: 2356
		// (get) Token: 0x06002C67 RID: 11367 RVA: 0x00087C50 File Offset: 0x00085E50
		public string displayNameKey
		{
			get
			{
				return this._reference.displayNameKey;
			}
		}

		// Token: 0x17000935 RID: 2357
		// (get) Token: 0x06002C68 RID: 11368 RVA: 0x00087C5D File Offset: 0x00085E5D
		public string displayName
		{
			get
			{
				return this._reference.displayName;
			}
		}

		// Token: 0x17000936 RID: 2358
		// (get) Token: 0x06002C69 RID: 11369 RVA: 0x00087C6A File Offset: 0x00085E6A
		public string flavor
		{
			get
			{
				return this._reference.flavor;
			}
		}

		// Token: 0x17000937 RID: 2359
		// (get) Token: 0x06002C6A RID: 11370 RVA: 0x00087C77 File Offset: 0x00085E77
		public string description
		{
			get
			{
				return this._reference.description;
			}
		}

		// Token: 0x17000938 RID: 2360
		// (get) Token: 0x06002C6B RID: 11371 RVA: 0x00087C84 File Offset: 0x00085E84
		public bool needUnlock
		{
			get
			{
				return this._needUnlock;
			}
		}

		// Token: 0x17000939 RID: 2361
		// (get) Token: 0x06002C6C RID: 11372 RVA: 0x00087C8C File Offset: 0x00085E8C
		public int level
		{
			get
			{
				return this._level;
			}
		}

		// Token: 0x1700093A RID: 2362
		// (get) Token: 0x06002C6D RID: 11373 RVA: 0x00087C94 File Offset: 0x00085E94
		public int price
		{
			get
			{
				return this._prices[this._level];
			}
		}

		// Token: 0x1700093B RID: 2363
		// (get) Token: 0x06002C6E RID: 11374 RVA: 0x00087CA3 File Offset: 0x00085EA3
		public int[] prices
		{
			get
			{
				return this._prices;
			}
		}

		// Token: 0x1700093C RID: 2364
		// (get) Token: 0x06002C6F RID: 11375 RVA: 0x00087CAB File Offset: 0x00085EAB
		public Sprite icon
		{
			get
			{
				return this._reference.icon;
			}
		}

		// Token: 0x1700093D RID: 2365
		// (get) Token: 0x06002C70 RID: 11376 RVA: 0x00087CB8 File Offset: 0x00085EB8
		public Sprite thumbnail
		{
			get
			{
				return this._reference.thumbnail;
			}
		}

		// Token: 0x1700093E RID: 2366
		// (get) Token: 0x06002C71 RID: 11377 RVA: 0x00087CC5 File Offset: 0x00085EC5
		public UpgradeObject.Type type
		{
			get
			{
				return this._type;
			}
		}

		// Token: 0x1700093F RID: 2367
		// (get) Token: 0x06002C72 RID: 11378 RVA: 0x00087CCD File Offset: 0x00085ECD
		public UpgradeObject.ValueForDescription[] valuesByLevel
		{
			get
			{
				return this._valuesByLevel;
			}
		}

		// Token: 0x17000940 RID: 2368
		// (get) Token: 0x06002C73 RID: 11379 RVA: 0x00087CD5 File Offset: 0x00085ED5
		public bool removable
		{
			get
			{
				return this._removable;
			}
		}

		// Token: 0x17000941 RID: 2369
		// (get) Token: 0x06002C74 RID: 11380 RVA: 0x00087CDD File Offset: 0x00085EDD
		public int maxLevel
		{
			get
			{
				return this._upgrade.maxLevel;
			}
		}

		// Token: 0x17000942 RID: 2370
		// (get) Token: 0x06002C75 RID: 11381 RVA: 0x00087CEC File Offset: 0x00085EEC
		public int returnCost
		{
			get
			{
				int num = 0;
				for (int i = 0; i < this._level; i++)
				{
					num += this._prices[i];
				}
				return num;
			}
		}

		// Token: 0x17000943 RID: 2371
		// (get) Token: 0x06002C76 RID: 11382 RVA: 0x00087D18 File Offset: 0x00085F18
		public string dataKey
		{
			get
			{
				return this.displayNameKey;
			}
		}

		// Token: 0x17000944 RID: 2372
		// (get) Token: 0x06002C77 RID: 11383 RVA: 0x00087D20 File Offset: 0x00085F20
		// (set) Token: 0x06002C78 RID: 11384 RVA: 0x00087D28 File Offset: 0x00085F28
		public UpgradeResource.Reference reference
		{
			get
			{
				return this._reference;
			}
			set
			{
				this._reference = value;
			}
		}

		// Token: 0x06002C79 RID: 11385 RVA: 0x00087D34 File Offset: 0x00085F34
		public UpgradeObject GetOrigin()
		{
			LevelManager levelManager = Singleton<Service>.Instance.levelManager;
			if (levelManager == null)
			{
				return this;
			}
			Character player = levelManager.player;
			if (player == null)
			{
				return this;
			}
			UpgradeInventory upgrade = player.playerComponents.inventory.upgrade;
			int num = upgrade.IndexOf(this);
			if (num == -1)
			{
				return this;
			}
			return upgrade.upgrades[num];
		}

		// Token: 0x06002C7A RID: 11386 RVA: 0x00087D94 File Offset: 0x00085F94
		public UpgradeAbility GetCurrentAbility()
		{
			return this._upgrade.GetAbility(this._level);
		}

		// Token: 0x06002C7B RID: 11387 RVA: 0x00087DA7 File Offset: 0x00085FA7
		public int GetCurrentLevel()
		{
			return this._reference.GetCurrentLevel();
		}

		// Token: 0x06002C7C RID: 11388 RVA: 0x00087DB4 File Offset: 0x00085FB4
		public UpgradeObject LevelUpOrigin()
		{
			UpgradeObject origin = this.GetOrigin();
			origin.LevelUp();
			return origin;
		}

		// Token: 0x06002C7D RID: 11389 RVA: 0x00087DC2 File Offset: 0x00085FC2
		public void SetLevel(int level)
		{
			this._level = level;
			this._upgrade.LevelUp(this._level);
		}

		// Token: 0x06002C7E RID: 11390 RVA: 0x00087DDC File Offset: 0x00085FDC
		private void LevelUp()
		{
			this._level = Mathf.Min(this.level + 1, this.maxLevel);
			this._upgrade.LevelUp(this._level);
		}

		// Token: 0x06002C7F RID: 11391 RVA: 0x00087E08 File Offset: 0x00086008
		public void Attach(Character target)
		{
			this._level = 1;
			this._upgrade.Attach(target);
		}

		// Token: 0x06002C80 RID: 11392 RVA: 0x00087E1D File Offset: 0x0008601D
		public void Detach()
		{
			this._upgrade.Detach();
		}

		// Token: 0x06002C81 RID: 11393 RVA: 0x00087E2A File Offset: 0x0008602A
		private void OnDestroy()
		{
			this.Detach();
		}

		// Token: 0x06002C82 RID: 11394 RVA: 0x00087E32 File Offset: 0x00086032
		public int CompareTo(UpgradeObject other)
		{
			if (this.maxLevel > other.maxLevel)
			{
				return 1;
			}
			if (this.maxLevel < other.maxLevel)
			{
				return -1;
			}
			return 0;
		}

		// Token: 0x04002573 RID: 9587
		[SerializeField]
		private int _orderInShop;

		// Token: 0x04002574 RID: 9588
		[SerializeField]
		private Upgrade _upgrade;

		// Token: 0x04002575 RID: 9589
		[Header("Description용 변수")]
		[SerializeField]
		private UpgradeObject.ValueForDescription[] _valuesByLevel;

		// Token: 0x04002576 RID: 9590
		[Tooltip("제거 가능한가")]
		[SerializeField]
		private bool _removable = true;

		// Token: 0x04002577 RID: 9591
		[SerializeField]
		private bool _needUnlock;

		// Token: 0x04002578 RID: 9592
		[SerializeField]
		private int[] _prices;

		// Token: 0x04002579 RID: 9593
		[SerializeField]
		private UpgradeObject.Type _type;

		// Token: 0x0400257A RID: 9594
		[SerializeField]
		private UpgradeResource.Reference _reference;

		// Token: 0x0400257B RID: 9595
		private int _level;

		// Token: 0x0400257C RID: 9596
		public static string prefix = "Upgrade";

		// Token: 0x0200085A RID: 2138
		[Serializable]
		public class ValueForDescription
		{
			// Token: 0x17000945 RID: 2373
			// (get) Token: 0x06002C85 RID: 11397 RVA: 0x00087E70 File Offset: 0x00086070
			public int argsCount
			{
				get
				{
					return this.values.Length;
				}
			}

			// Token: 0x17000946 RID: 2374
			public string this[int index]
			{
				get
				{
					return this.values[index];
				}
			}

			// Token: 0x0400257D RID: 9597
			public string[] values;
		}

		// Token: 0x0200085B RID: 2139
		public enum Type
		{
			// Token: 0x0400257F RID: 9599
			Normal,
			// Token: 0x04002580 RID: 9600
			Cursed
		}
	}
}
