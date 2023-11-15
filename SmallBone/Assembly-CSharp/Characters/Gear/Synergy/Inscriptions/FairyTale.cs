using System;
using System.Linq;
using Characters.Abilities;
using Characters.Gear.Items;
using Characters.Gear.Synergy.Inscriptions.FairyTaleSummon;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Characters.Gear.Synergy.Inscriptions
{
	// Token: 0x02000888 RID: 2184
	public sealed class FairyTale : InscriptionInstance
	{
		// Token: 0x170009BE RID: 2494
		// (get) Token: 0x06002DF6 RID: 11766 RVA: 0x00088DBC File Offset: 0x00086FBC
		private Stat.Category statCategory
		{
			get
			{
				return Stat.Category.PercentPoint;
			}
		}

		// Token: 0x170009BF RID: 2495
		// (get) Token: 0x06002DF7 RID: 11767 RVA: 0x0008B261 File Offset: 0x00089461
		private Stat.Kind statKind
		{
			get
			{
				return Stat.Kind.SpiritAttackCooldownSpeed;
			}
		}

		// Token: 0x06002DF8 RID: 11768 RVA: 0x0008B268 File Offset: 0x00089468
		private void SpawnOberon()
		{
			if (this._oberonInstance != null)
			{
				return;
			}
			if (!this._oberonHandle.IsValid())
			{
				this._oberonHandle = this._oberonReference.LoadAssetAsync<GameObject>();
			}
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this._oberonHandle.WaitForCompletion());
			ReleaseAddressableHandleOnDestroy.Reserve(gameObject, this._oberonHandle);
			this._oberonInstance = gameObject.GetComponent<Oberon>();
			this._oberonInstance.Initialize(base.character, this._oberonSlot);
		}

		// Token: 0x06002DF9 RID: 11769 RVA: 0x0008B2E4 File Offset: 0x000894E4
		private void SpawnDarkOberon()
		{
			if (this._darkOberonInstance != null)
			{
				return;
			}
			if (!this._darkOberonReference.IsValid())
			{
				this._darkOberonHandle = this._darkOberonReference.LoadAssetAsync<GameObject>();
			}
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this._darkOberonHandle.WaitForCompletion());
			ReleaseAddressableHandleOnDestroy.Reserve(gameObject, this._darkOberonHandle);
			this._darkOberonInstance = gameObject.GetComponent<Oberon>();
			this._darkOberonInstance.Initialize(base.character, this._oberonSlot);
		}

		// Token: 0x06002DFA RID: 11770 RVA: 0x0008B35E File Offset: 0x0008955E
		private void ReleaseOberon()
		{
			if (this._oberonInstance != null)
			{
				UnityEngine.Object.Destroy(this._oberonInstance.gameObject);
				this._oberonInstance = null;
			}
			if (this._oberonHandle.IsValid())
			{
				Addressables.Release<GameObject>(this._oberonHandle);
			}
		}

		// Token: 0x06002DFB RID: 11771 RVA: 0x0008B39D File Offset: 0x0008959D
		private void ReleaseDarkOberon()
		{
			if (this._darkOberonInstance != null)
			{
				UnityEngine.Object.Destroy(this._darkOberonInstance.gameObject);
				this._darkOberonInstance = null;
			}
			if (this._darkOberonHandle.IsValid())
			{
				Addressables.Release<GameObject>(this._darkOberonHandle);
			}
		}

		// Token: 0x06002DFC RID: 11772 RVA: 0x0008B3DC File Offset: 0x000895DC
		protected override void Initialize()
		{
			this._statBonus = new FairyTale.StatBonus(base.character);
			this._statBonus.Initialize();
			this._statBonus.icon = this._icon;
		}

		// Token: 0x06002DFD RID: 11773 RVA: 0x0008B40C File Offset: 0x0008960C
		public override void UpdateBonus(bool wasActive, bool wasOmen)
		{
			this.UpdateStat();
			if (this.keyword.step == 0)
			{
				return;
			}
			if (!this.keyword.isMaxStep)
			{
				this.ReleaseOberon();
				this.ReleaseDarkOberon();
				return;
			}
			if (this.keyword.omen)
			{
				this.ReleaseOberon();
				this.SpawnDarkOberon();
				return;
			}
			this.ReleaseDarkOberon();
			this.SpawnOberon();
		}

		// Token: 0x06002DFE RID: 11774 RVA: 0x0008B46D File Offset: 0x0008966D
		public override void Attach()
		{
			base.character.ability.Add(this._statBonus);
		}

		// Token: 0x06002DFF RID: 11775 RVA: 0x0008B486 File Offset: 0x00089686
		public override void Detach()
		{
			base.character.ability.Remove(this._statBonus);
			this.ReleaseOberon();
			this.ReleaseDarkOberon();
		}

		// Token: 0x06002E00 RID: 11776 RVA: 0x0008B4AB File Offset: 0x000896AB
		private void OnDestroy()
		{
			this.ReleaseOberon();
			this.ReleaseDarkOberon();
		}

		// Token: 0x06002E01 RID: 11777 RVA: 0x0008B4BC File Offset: 0x000896BC
		private void UpdateStat()
		{
			int num = base.character.playerComponents.inventory.item.items.Count((Item item) => !(item == null) && item.gearTag == Gear.Tag.Spirit);
			this._statBonus.currentStatBonus = this._statBonusPerSpiritCount * (double)num;
			if (this.statCategory.index == Stat.Category.Percent.index)
			{
				this._statBonus.currentStatBonus = this._statBonus.currentStatBonus * 0.01 + 1.0;
			}
			else if (this.statCategory.index == Stat.Category.PercentPoint.index)
			{
				this._statBonus.currentStatBonus *= 0.01;
			}
			this._statBonus.stat.values[0].categoryIndex = this.statCategory.index;
			this._statBonus.stat.values[0].kindIndex = this.statKind.index;
			this._statBonus.UpdateStat();
		}

		// Token: 0x04002653 RID: 9811
		[SerializeField]
		[Header("2세트 효과")]
		private Sprite _icon;

		// Token: 0x04002654 RID: 9812
		[SerializeField]
		private double _statBonusPerSpiritCount = 0.10000000149011612;

		// Token: 0x04002655 RID: 9813
		[Space]
		[Header("5세트 효과")]
		[SerializeField]
		private Transform _oberonSlot;

		// Token: 0x04002656 RID: 9814
		[SerializeField]
		[Space]
		private AssetReference _oberonReference;

		// Token: 0x04002657 RID: 9815
		[SerializeField]
		private AssetReference _darkOberonReference;

		// Token: 0x04002658 RID: 9816
		private Oberon _oberonInstance;

		// Token: 0x04002659 RID: 9817
		private Oberon _darkOberonInstance;

		// Token: 0x0400265A RID: 9818
		private AsyncOperationHandle<GameObject> _oberonHandle;

		// Token: 0x0400265B RID: 9819
		private AsyncOperationHandle<GameObject> _darkOberonHandle;

		// Token: 0x0400265C RID: 9820
		private FairyTale.StatBonus _statBonus;

		// Token: 0x02000889 RID: 2185
		private sealed class StatBonus : IAbility, IAbilityInstance
		{
			// Token: 0x170009C0 RID: 2496
			// (get) Token: 0x06002E03 RID: 11779 RVA: 0x0008B5F7 File Offset: 0x000897F7
			Character IAbilityInstance.owner
			{
				get
				{
					return this._owner;
				}
			}

			// Token: 0x170009C1 RID: 2497
			// (get) Token: 0x06002E04 RID: 11780 RVA: 0x000716FD File Offset: 0x0006F8FD
			public IAbility ability
			{
				get
				{
					return this;
				}
			}

			// Token: 0x170009C2 RID: 2498
			// (get) Token: 0x06002E05 RID: 11781 RVA: 0x0008B5FF File Offset: 0x000897FF
			// (set) Token: 0x06002E06 RID: 11782 RVA: 0x0008B607 File Offset: 0x00089807
			public float remainTime { get; set; }

			// Token: 0x170009C3 RID: 2499
			// (get) Token: 0x06002E07 RID: 11783 RVA: 0x000076D4 File Offset: 0x000058D4
			public bool attached
			{
				get
				{
					return true;
				}
			}

			// Token: 0x170009C4 RID: 2500
			// (get) Token: 0x06002E08 RID: 11784 RVA: 0x0008B610 File Offset: 0x00089810
			// (set) Token: 0x06002E09 RID: 11785 RVA: 0x0008B618 File Offset: 0x00089818
			public Sprite icon { get; set; }

			// Token: 0x170009C5 RID: 2501
			// (get) Token: 0x06002E0A RID: 11786 RVA: 0x00071719 File Offset: 0x0006F919
			public float iconFillAmount
			{
				get
				{
					return 0f;
				}
			}

			// Token: 0x170009C6 RID: 2502
			// (get) Token: 0x06002E0B RID: 11787 RVA: 0x00018EC5 File Offset: 0x000170C5
			public bool iconFillInversed
			{
				get
				{
					return false;
				}
			}

			// Token: 0x170009C7 RID: 2503
			// (get) Token: 0x06002E0C RID: 11788 RVA: 0x00018EC5 File Offset: 0x000170C5
			public bool iconFillFlipped
			{
				get
				{
					return false;
				}
			}

			// Token: 0x170009C8 RID: 2504
			// (get) Token: 0x06002E0D RID: 11789 RVA: 0x0008B621 File Offset: 0x00089821
			public int iconStacks
			{
				get
				{
					return (int)(this.currentStatBonus * 100.0);
				}
			}

			// Token: 0x170009C9 RID: 2505
			// (get) Token: 0x06002E0E RID: 11790 RVA: 0x00018EC5 File Offset: 0x000170C5
			public bool expired
			{
				get
				{
					return false;
				}
			}

			// Token: 0x170009CA RID: 2506
			// (get) Token: 0x06002E0F RID: 11791 RVA: 0x0008B634 File Offset: 0x00089834
			// (set) Token: 0x06002E10 RID: 11792 RVA: 0x0008B63C File Offset: 0x0008983C
			public float duration { get; set; }

			// Token: 0x170009CB RID: 2507
			// (get) Token: 0x06002E11 RID: 11793 RVA: 0x00018EC5 File Offset: 0x000170C5
			public int iconPriority
			{
				get
				{
					return 0;
				}
			}

			// Token: 0x170009CC RID: 2508
			// (get) Token: 0x06002E12 RID: 11794 RVA: 0x00018EC5 File Offset: 0x000170C5
			public bool removeOnSwapWeapon
			{
				get
				{
					return false;
				}
			}

			// Token: 0x06002E13 RID: 11795 RVA: 0x000716FD File Offset: 0x0006F8FD
			public IAbilityInstance CreateInstance(Character owner)
			{
				return this;
			}

			// Token: 0x06002E14 RID: 11796 RVA: 0x0008B645 File Offset: 0x00089845
			public StatBonus(Character owner)
			{
				this._owner = owner;
			}

			// Token: 0x06002E15 RID: 11797 RVA: 0x00002191 File Offset: 0x00000391
			public void Initialize()
			{
			}

			// Token: 0x06002E16 RID: 11798 RVA: 0x00002191 File Offset: 0x00000391
			public void UpdateTime(float deltaTime)
			{
			}

			// Token: 0x06002E17 RID: 11799 RVA: 0x00002191 File Offset: 0x00000391
			public void Refresh()
			{
			}

			// Token: 0x06002E18 RID: 11800 RVA: 0x0008B678 File Offset: 0x00089878
			void IAbilityInstance.Attach()
			{
				this._owner.stat.AttachValues(this.stat);
			}

			// Token: 0x06002E19 RID: 11801 RVA: 0x0008B690 File Offset: 0x00089890
			void IAbilityInstance.Detach()
			{
				this._owner.stat.DetachValues(this.stat);
			}

			// Token: 0x06002E1A RID: 11802 RVA: 0x0008B6A8 File Offset: 0x000898A8
			public void UpdateStat()
			{
				this.stat.values[0].value = this.currentStatBonus;
				this._owner.stat.SetNeedUpdate();
			}

			// Token: 0x0400265D RID: 9821
			[NonSerialized]
			public double currentStatBonus;

			// Token: 0x0400265E RID: 9822
			[NonSerialized]
			public Stat.Values stat = new Stat.Values(new Stat.Value[]
			{
				new Stat.Value(0, 0, 0.0)
			});

			// Token: 0x0400265F RID: 9823
			private Character _owner;
		}
	}
}
