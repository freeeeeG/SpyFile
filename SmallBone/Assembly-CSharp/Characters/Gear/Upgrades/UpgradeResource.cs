using System;
using System.Collections.Generic;
using System.Text;
using Characters.Player;
using GameResources;
using Services;
using Singletons;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Characters.Gear.Upgrades
{
	// Token: 0x0200085C RID: 2140
	public class UpgradeResource : ScriptableObject
	{
		// Token: 0x17000947 RID: 2375
		// (get) Token: 0x06002C88 RID: 11400 RVA: 0x00087E84 File Offset: 0x00086084
		public Dictionary<string, Sprite> upgradeIconDictionary
		{
			get
			{
				return this._upgradeIconDictionary;
			}
		}

		// Token: 0x06002C89 RID: 11401 RVA: 0x00087E8C File Offset: 0x0008608C
		public Sprite GetIcon(string name)
		{
			Sprite result;
			this._upgradeIconDictionary.TryGetValue(name, out result);
			return result;
		}

		// Token: 0x17000948 RID: 2376
		// (get) Token: 0x06002C8A RID: 11402 RVA: 0x00087EA9 File Offset: 0x000860A9
		public Dictionary<string, Sprite> upgradeThumbnailDictionary
		{
			get
			{
				return this._upgradeThumbnailDictionary;
			}
		}

		// Token: 0x06002C8B RID: 11403 RVA: 0x00087EB4 File Offset: 0x000860B4
		public Sprite GetThumbnail(string name)
		{
			Sprite result;
			this._upgradeThumbnailDictionary.TryGetValue(name, out result);
			return result;
		}

		// Token: 0x17000949 RID: 2377
		// (get) Token: 0x06002C8C RID: 11404 RVA: 0x00087ED1 File Offset: 0x000860D1
		public List<UpgradeResource.Reference> upgradeReferences
		{
			get
			{
				return this._upgradeReferences;
			}
		}

		// Token: 0x1700094A RID: 2378
		// (get) Token: 0x06002C8D RID: 11405 RVA: 0x00087ED9 File Offset: 0x000860D9
		public static UpgradeResource instance
		{
			get
			{
				if (UpgradeResource._instance == null)
				{
					UpgradeResource._instance = Resources.Load<UpgradeResource>("HardmodeSetting/UpgradeResource");
					UpgradeResource._instance.Initialize();
				}
				return UpgradeResource._instance;
			}
		}

		// Token: 0x06002C8E RID: 11406 RVA: 0x00002191 File Offset: 0x00000391
		public void Initialize()
		{
		}

		// Token: 0x04002581 RID: 9601
		private const string harmodeSettingKey = "HardmodeSetting";

		// Token: 0x04002582 RID: 9602
		[SerializeField]
		private List<UpgradeResource.Reference> _upgradeReferences;

		// Token: 0x04002583 RID: 9603
		[Space]
		[SerializeField]
		private Sprite[] _upgradeIcons;

		// Token: 0x04002584 RID: 9604
		private Dictionary<string, Sprite> _upgradeIconDictionary;

		// Token: 0x04002585 RID: 9605
		[SerializeField]
		private Sprite[] _upgradeThumbnails;

		// Token: 0x04002586 RID: 9606
		private Dictionary<string, Sprite> _upgradeThumbnailDictionary;

		// Token: 0x04002587 RID: 9607
		private static UpgradeResource _instance;

		// Token: 0x0200085D RID: 2141
		[Serializable]
		public class Reference : IEquatable<UpgradeObject>, IComparable<UpgradeResource.Reference>
		{
			// Token: 0x1700094B RID: 2379
			// (get) Token: 0x06002C90 RID: 11408 RVA: 0x00087F06 File Offset: 0x00086106
			private string _key
			{
				get
				{
					return this.name;
				}
			}

			// Token: 0x1700094C RID: 2380
			// (get) Token: 0x06002C91 RID: 11409 RVA: 0x00087F0E File Offset: 0x0008610E
			private string _keyBase
			{
				get
				{
					return UpgradeResource.Reference.prefix + "/" + this._key;
				}
			}

			// Token: 0x1700094D RID: 2381
			// (get) Token: 0x06002C92 RID: 11410 RVA: 0x00087F25 File Offset: 0x00086125
			public string displayNameKey
			{
				get
				{
					return this._keyBase + "/name";
				}
			}

			// Token: 0x1700094E RID: 2382
			// (get) Token: 0x06002C93 RID: 11411 RVA: 0x00087F37 File Offset: 0x00086137
			public string displayName
			{
				get
				{
					return Localization.GetLocalizedString(this._keyBase + "/name");
				}
			}

			// Token: 0x1700094F RID: 2383
			// (get) Token: 0x06002C94 RID: 11412 RVA: 0x00087F4E File Offset: 0x0008614E
			public string flavor
			{
				get
				{
					return Localization.GetLocalizedString(this._keyBase + "/flavor");
				}
			}

			// Token: 0x17000950 RID: 2384
			// (get) Token: 0x06002C95 RID: 11413 RVA: 0x00087F65 File Offset: 0x00086165
			public string description
			{
				get
				{
					return Localization.GetLocalizedString(this._keyBase + "/desc");
				}
			}

			// Token: 0x17000951 RID: 2385
			// (get) Token: 0x06002C96 RID: 11414 RVA: 0x00087F7C File Offset: 0x0008617C
			public int price
			{
				get
				{
					if (this.prices != null && this.prices.Length != 0)
					{
						return this.prices[Mathf.Min(this.maxLevel - 1, this.GetCurrentLevel())];
					}
					return 0;
				}
			}

			// Token: 0x17000952 RID: 2386
			// (get) Token: 0x06002C97 RID: 11415 RVA: 0x00087FAB File Offset: 0x000861AB
			public static string curseText
			{
				get
				{
					return Localization.GetLocalizedString("label/upgrade/type/cursed");
				}
			}

			// Token: 0x06002C98 RID: 11416 RVA: 0x00087FB7 File Offset: 0x000861B7
			public bool Equals(UpgradeObject other)
			{
				return other.reference.Equals(this);
			}

			// Token: 0x06002C99 RID: 11417 RVA: 0x00087FC8 File Offset: 0x000861C8
			public UpgradeObject Instantiate()
			{
				AsyncOperationHandle<GameObject> handle = this.assetReference.LoadAssetAsync<GameObject>();
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(handle.WaitForCompletion());
				ReleaseAddressableHandleOnDestroy.Reserve(gameObject, handle);
				return gameObject.GetComponent<UpgradeObject>();
			}

			// Token: 0x06002C9A RID: 11418 RVA: 0x00087FFC File Offset: 0x000861FC
			public int GetCurrentLevel()
			{
				Character player = Singleton<Service>.Instance.levelManager.player;
				if (player == null)
				{
					return 0;
				}
				UpgradeInventory upgrade = player.playerComponents.inventory.upgrade;
				int num = upgrade.IndexOf(this);
				if (num == -1)
				{
					return 0;
				}
				return upgrade.upgrades[num].level;
			}

			// Token: 0x06002C9B RID: 11419 RVA: 0x00088054 File Offset: 0x00086254
			public int GetNextLevel()
			{
				Character player = Singleton<Service>.Instance.levelManager.player;
				if (player == null)
				{
					return 0;
				}
				UpgradeInventory upgrade = player.playerComponents.inventory.upgrade;
				int num = upgrade.IndexOf(this);
				if (num == -1)
				{
					return 1;
				}
				return Mathf.Min(upgrade.upgrades[num].level + 1, upgrade.upgrades[num].maxLevel);
			}

			// Token: 0x06002C9C RID: 11420 RVA: 0x000880C4 File Offset: 0x000862C4
			public string GetDescription(int targetLevel, string activateColor = "F158FF", string deactivateColor = "3C285F")
			{
				if (this.valuesForDescription.Length == 0)
				{
					return this.description;
				}
				if (this.valuesForDescription[0].values.Length == 0)
				{
					return this.description;
				}
				int argsCount = this.valuesForDescription[0].argsCount;
				if (argsCount == 1)
				{
					return string.Format(this.description, this.GetDescriptionArgs(targetLevel, 0, activateColor, deactivateColor));
				}
				if (argsCount != 2)
				{
					return this.description;
				}
				return string.Format(this.description, this.GetDescriptionArgs(targetLevel, 0, activateColor, deactivateColor), this.GetDescriptionArgs(targetLevel, 1, activateColor, deactivateColor));
			}

			// Token: 0x06002C9D RID: 11421 RVA: 0x0008814C File Offset: 0x0008634C
			public string GetCurrentDescription(string activateColor, string deactivateColor)
			{
				return this.GetDescription(this.GetCurrentLevel(), activateColor, deactivateColor);
			}

			// Token: 0x06002C9E RID: 11422 RVA: 0x0008815C File Offset: 0x0008635C
			public string GetNextLevelDescription(string activateColor = "F158FF", string deactivateColor = "3C285F")
			{
				return this.GetDescription(this.GetNextLevel(), activateColor, deactivateColor);
			}

			// Token: 0x06002C9F RID: 11423 RVA: 0x0008816C File Offset: 0x0008636C
			private string GetDescriptionArgs(int targetLevel, int argIndex, string activateColor, string deactivateColor)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("<color=#" + deactivateColor + ">");
				for (int i = 1; i <= this.valuesForDescription.Length; i++)
				{
					if (targetLevel == i)
					{
						stringBuilder.Append("<color=#" + activateColor + ">");
					}
					stringBuilder.Append(this.valuesForDescription[i - 1][argIndex]);
					if (targetLevel == i)
					{
						stringBuilder.Append("</color>");
					}
					if (i < this.valuesForDescription.Length)
					{
						stringBuilder.Append('/');
					}
				}
				stringBuilder.Append("</color>");
				return stringBuilder.ToString();
			}

			// Token: 0x06002CA0 RID: 11424 RVA: 0x00088212 File Offset: 0x00086412
			public int CompareTo(UpgradeResource.Reference other)
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

			// Token: 0x04002588 RID: 9608
			public static string prefix = "Upgrade";

			// Token: 0x04002589 RID: 9609
			public AssetReference assetReference;

			// Token: 0x0400258A RID: 9610
			public string name;

			// Token: 0x0400258B RID: 9611
			public string guid;

			// Token: 0x0400258C RID: 9612
			public Sprite icon;

			// Token: 0x0400258D RID: 9613
			public Sprite thumbnail;

			// Token: 0x0400258E RID: 9614
			public int[] prices;

			// Token: 0x0400258F RID: 9615
			public int maxLevel;

			// Token: 0x04002590 RID: 9616
			public UpgradeObject.Type type;

			// Token: 0x04002591 RID: 9617
			public bool needUnlock;

			// Token: 0x04002592 RID: 9618
			public bool removable;

			// Token: 0x04002593 RID: 9619
			public UpgradeObject.ValueForDescription[] valuesForDescription;

			// Token: 0x04002594 RID: 9620
			public int orderInShop;
		}
	}
}
