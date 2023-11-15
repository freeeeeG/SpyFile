using System;
using System.Collections.ObjectModel;
using Characters.Player;
using GameResources;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Characters.Gear.Synergy.Inscriptions
{
	// Token: 0x02000897 RID: 2199
	public sealed class Inscription
	{
		// Token: 0x170009E7 RID: 2535
		// (get) Token: 0x06002E7B RID: 11899 RVA: 0x0008C2A2 File Offset: 0x0008A4A2
		public static ReadOnlyCollection<Inscription.Key> keys
		{
			get
			{
				return EnumValues<Inscription.Key>.Values;
			}
		}

		// Token: 0x170009E8 RID: 2536
		// (get) Token: 0x06002E7C RID: 11900 RVA: 0x0008C2A9 File Offset: 0x0008A4A9
		// (set) Token: 0x06002E7D RID: 11901 RVA: 0x0008C2B1 File Offset: 0x0008A4B1
		public Inscription.Key key { get; private set; }

		// Token: 0x170009E9 RID: 2537
		// (get) Token: 0x06002E7E RID: 11902 RVA: 0x0008C2BA File Offset: 0x0008A4BA
		// (set) Token: 0x06002E7F RID: 11903 RVA: 0x0008C2C2 File Offset: 0x0008A4C2
		public InscriptionSettings settings { get; private set; }

		// Token: 0x170009EA RID: 2538
		// (get) Token: 0x06002E80 RID: 11904 RVA: 0x0008C2CB File Offset: 0x0008A4CB
		// (set) Token: 0x06002E81 RID: 11905 RVA: 0x0008C2D3 File Offset: 0x0008A4D3
		public Character character { get; private set; }

		// Token: 0x170009EB RID: 2539
		// (get) Token: 0x06002E82 RID: 11906 RVA: 0x0008C2DC File Offset: 0x0008A4DC
		// (set) Token: 0x06002E83 RID: 11907 RVA: 0x0008C2E4 File Offset: 0x0008A4E4
		public int step { get; private set; }

		// Token: 0x170009EC RID: 2540
		// (get) Token: 0x06002E84 RID: 11908 RVA: 0x0008C2ED File Offset: 0x0008A4ED
		// (set) Token: 0x06002E85 RID: 11909 RVA: 0x0008C2F5 File Offset: 0x0008A4F5
		public ReadOnlyCollection<int> steps { get; private set; }

		// Token: 0x170009ED RID: 2541
		// (get) Token: 0x06002E86 RID: 11910 RVA: 0x0008C2FE File Offset: 0x0008A4FE
		public string name
		{
			get
			{
				return Inscription.GetName(this.key);
			}
		}

		// Token: 0x170009EE RID: 2542
		// (get) Token: 0x06002E87 RID: 11911 RVA: 0x0008C30B File Offset: 0x0008A50B
		public Sprite icon
		{
			get
			{
				if (this.isMaxStep)
				{
					return this.fullActiveIcon;
				}
				if (this.active)
				{
					return this.activeIcon;
				}
				return this.deactiveIcon;
			}
		}

		// Token: 0x170009EF RID: 2543
		// (get) Token: 0x06002E88 RID: 11912 RVA: 0x0008C331 File Offset: 0x0008A531
		// (set) Token: 0x06002E89 RID: 11913 RVA: 0x0008C339 File Offset: 0x0008A539
		public Sprite activeIcon { get; private set; }

		// Token: 0x170009F0 RID: 2544
		// (get) Token: 0x06002E8A RID: 11914 RVA: 0x0008C342 File Offset: 0x0008A542
		// (set) Token: 0x06002E8B RID: 11915 RVA: 0x0008C34A File Offset: 0x0008A54A
		public Sprite deactiveIcon { get; private set; }

		// Token: 0x170009F1 RID: 2545
		// (get) Token: 0x06002E8C RID: 11916 RVA: 0x0008C353 File Offset: 0x0008A553
		// (set) Token: 0x06002E8D RID: 11917 RVA: 0x0008C35B File Offset: 0x0008A55B
		public Sprite fullActiveIcon { get; private set; }

		// Token: 0x170009F2 RID: 2546
		// (get) Token: 0x06002E8E RID: 11918 RVA: 0x0008C364 File Offset: 0x0008A564
		// (set) Token: 0x06002E8F RID: 11919 RVA: 0x0008C36C File Offset: 0x0008A56C
		public bool active { get; private set; }

		// Token: 0x170009F3 RID: 2547
		// (get) Token: 0x06002E90 RID: 11920 RVA: 0x0008C375 File Offset: 0x0008A575
		// (set) Token: 0x06002E91 RID: 11921 RVA: 0x0008C37D File Offset: 0x0008A57D
		public bool omen { get; private set; }

		// Token: 0x170009F4 RID: 2548
		// (get) Token: 0x06002E92 RID: 11922 RVA: 0x0008C386 File Offset: 0x0008A586
		// (set) Token: 0x06002E93 RID: 11923 RVA: 0x0008C38E File Offset: 0x0008A58E
		public bool isMaxStep { get; private set; }

		// Token: 0x170009F5 RID: 2549
		// (get) Token: 0x06002E94 RID: 11924 RVA: 0x0008C397 File Offset: 0x0008A597
		// (set) Token: 0x06002E95 RID: 11925 RVA: 0x0008C39F File Offset: 0x0008A59F
		public int maxStep { get; private set; }

		// Token: 0x06002E96 RID: 11926 RVA: 0x0008C3A8 File Offset: 0x0008A5A8
		public static Sprite GetActiveIcon(Inscription.Key key)
		{
			return CommonResource.instance.keywordIconDictionary[key.ToString()];
		}

		// Token: 0x06002E97 RID: 11927 RVA: 0x0008C3C6 File Offset: 0x0008A5C6
		public static Sprite GetDeactiveIcon(Inscription.Key key)
		{
			return CommonResource.instance.keywordDeactiveIconDictionary[key.ToString()];
		}

		// Token: 0x06002E98 RID: 11928 RVA: 0x0008C3E4 File Offset: 0x0008A5E4
		public static Sprite GetFullActiveIcon(Inscription.Key key)
		{
			return CommonResource.instance.keywordFullactiveIconDictionary[key.ToString()];
		}

		// Token: 0x06002E99 RID: 11929 RVA: 0x0008C402 File Offset: 0x0008A602
		public static string GetName(Inscription.Key key)
		{
			return Localization.GetLocalizedString(string.Format("synergy/key/{0}/name", key));
		}

		// Token: 0x06002E9A RID: 11930 RVA: 0x0008C41C File Offset: 0x0008A61C
		public void Initialize(Inscription.Key key, InscriptionSettings settings, Character character)
		{
			this.key = key;
			this.settings = settings;
			this.character = character;
			this._itemInventory = character.playerComponents.inventory.item;
			this.steps = Array.AsReadOnly<int>(settings.steps);
			if (key != Inscription.Key.None)
			{
				this.activeIcon = Inscription.GetActiveIcon(key);
				this.deactiveIcon = Inscription.GetDeactiveIcon(key);
				this.fullActiveIcon = Inscription.GetFullActiveIcon(key);
			}
			this.maxStep = ((this.steps.Count == 0) ? 0 : this.steps[this.steps.Count - 1]);
		}

		// Token: 0x06002E9B RID: 11931 RVA: 0x0008C4BA File Offset: 0x0008A6BA
		public void Clear()
		{
			if (this._instance == null)
			{
				return;
			}
			UnityEngine.Object.Destroy(this._instance.gameObject);
			this._instance = null;
			this.UnloadReference();
		}

		// Token: 0x06002E9C RID: 11932 RVA: 0x0008C4E8 File Offset: 0x0008A6E8
		public void Update()
		{
			int num = this.steps.Count;
			if (num == 0)
			{
				return;
			}
			int num2 = 0;
			int num3 = 0;
			while (num3 < num && this.count >= this.steps[num3])
			{
				num2 = num3;
				num3++;
			}
			bool omen = this.omen;
			if (this.settings.omenItem != null)
			{
				this.omen = this._itemInventory.Has(this.settings.omenItem.AssetGUID);
			}
			if (num2 == this.step && omen == this.omen)
			{
				return;
			}
			bool active = this.active;
			this.active = (num2 > 0);
			this.step = num2;
			this.isMaxStep = (this.step == num - 1);
			if (!this.active)
			{
				if (active)
				{
					if (this._instance == null)
					{
						Debug.LogError(string.Format("WasActive is true, but there is no inscription instance of {0}.", this.key));
					}
					else
					{
						this._instance.Detach();
					}
					this.Clear();
				}
				return;
			}
			if (active)
			{
				this._instance.UpdateBonus(active, omen);
				return;
			}
			if (this._instance != null)
			{
				return;
			}
			this.LoadReference();
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this._handle.WaitForCompletion(), this.character.transform);
			gameObject.transform.position = this.character.transform.position;
			this._instance = gameObject.GetComponent<InscriptionInstance>();
			this._instance.keyword = this;
			this._instance.Initialize(this.character);
			this._instance.Attach();
			this._instance.UpdateBonus(active, omen);
		}

		// Token: 0x06002E9D RID: 11933 RVA: 0x0008C688 File Offset: 0x0008A888
		private void LoadReference()
		{
			if (this._handle.IsValid())
			{
				return;
			}
			this._handle = this.settings.reference.LoadAssetAsync<GameObject>();
		}

		// Token: 0x06002E9E RID: 11934 RVA: 0x0008C6AE File Offset: 0x0008A8AE
		private void UnloadReference()
		{
			if (!this._handle.IsValid())
			{
				return;
			}
			Addressables.Release<GameObject>(this._handle);
		}

		// Token: 0x06002E9F RID: 11935 RVA: 0x0008C6C9 File Offset: 0x0008A8C9
		public Sprite GetIconForStep(int step)
		{
			if (step >= this.maxStep)
			{
				return this.fullActiveIcon;
			}
			if (step >= this.settings.steps[1])
			{
				return this.activeIcon;
			}
			return this.deactiveIcon;
		}

		// Token: 0x06002EA0 RID: 11936 RVA: 0x0008C6F8 File Offset: 0x0008A8F8
		public string GetDescription()
		{
			return this.GetDescription(this.step);
		}

		// Token: 0x06002EA1 RID: 11937 RVA: 0x0008C706 File Offset: 0x0008A906
		public string GetDescription(int step)
		{
			return Localization.GetLocalizedString(string.Format("synergy/key/{0}/desc/{1}", this.key, step));
		}

		// Token: 0x04002695 RID: 9877
		public int count;

		// Token: 0x04002696 RID: 9878
		public int bonusCount;

		// Token: 0x04002697 RID: 9879
		private ItemInventory _itemInventory;

		// Token: 0x04002698 RID: 9880
		private InscriptionInstance _instance;

		// Token: 0x04002699 RID: 9881
		private AsyncOperationHandle<GameObject> _handle;

		// Token: 0x02000898 RID: 2200
		public enum Key
		{
			// Token: 0x040026A7 RID: 9895
			None,
			// Token: 0x040026A8 RID: 9896
			Antique,
			// Token: 0x040026A9 RID: 9897
			Arms,
			// Token: 0x040026AA RID: 9898
			Artifact,
			// Token: 0x040026AB RID: 9899
			Bone,
			// Token: 0x040026AC RID: 9900
			Brave,
			// Token: 0x040026AD RID: 9901
			FairyTale,
			// Token: 0x040026AE RID: 9902
			Duel,
			// Token: 0x040026AF RID: 9903
			Fortress,
			// Token: 0x040026B0 RID: 9904
			Arson,
			// Token: 0x040026B1 RID: 9905
			Execution,
			// Token: 0x040026B2 RID: 9906
			Strike,
			// Token: 0x040026B3 RID: 9907
			Manatech,
			// Token: 0x040026B4 RID: 9908
			Soar,
			// Token: 0x040026B5 RID: 9909
			Relic,
			// Token: 0x040026B6 RID: 9910
			Heirloom,
			// Token: 0x040026B7 RID: 9911
			Mutation,
			// Token: 0x040026B8 RID: 9912
			Chase,
			// Token: 0x040026B9 RID: 9913
			ManaCycle,
			// Token: 0x040026BA RID: 9914
			Misfortune,
			// Token: 0x040026BB RID: 9915
			AbsoluteZero,
			// Token: 0x040026BC RID: 9916
			Spoils,
			// Token: 0x040026BD RID: 9917
			Brawl,
			// Token: 0x040026BE RID: 9918
			SunAndMoon,
			// Token: 0x040026BF RID: 9919
			Rapidity,
			// Token: 0x040026C0 RID: 9920
			Revenge,
			// Token: 0x040026C1 RID: 9921
			Poisoning,
			// Token: 0x040026C2 RID: 9922
			ExcessiveBleeding,
			// Token: 0x040026C3 RID: 9923
			Wisdom,
			// Token: 0x040026C4 RID: 9924
			Masterpiece,
			// Token: 0x040026C5 RID: 9925
			HiddenBlade,
			// Token: 0x040026C6 RID: 9926
			Heritage,
			// Token: 0x040026C7 RID: 9927
			Treasure,
			// Token: 0x040026C8 RID: 9928
			Dizziness,
			// Token: 0x040026C9 RID: 9929
			Omen,
			// Token: 0x040026CA RID: 9930
			Sin
		}
	}
}
