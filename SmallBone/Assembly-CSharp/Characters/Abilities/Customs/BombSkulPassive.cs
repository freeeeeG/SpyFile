using System;
using System.Collections.Generic;
using Characters.Abilities.Constraints;
using Characters.Actions;
using Characters.Gear.Weapons.Gauges;
using Characters.Operations;
using FX;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Abilities.Customs
{
	// Token: 0x02000D2D RID: 3373
	[Serializable]
	public class BombSkulPassive : Ability, IAbilityInstance
	{
		// Token: 0x17000E1E RID: 3614
		// (get) Token: 0x060043FE RID: 17406 RVA: 0x000C5A1C File Offset: 0x000C3C1C
		// (set) Token: 0x060043FF RID: 17407 RVA: 0x000C5A24 File Offset: 0x000C3C24
		public Character owner { get; set; }

		// Token: 0x17000E1F RID: 3615
		// (get) Token: 0x06004400 RID: 17408 RVA: 0x000716FD File Offset: 0x0006F8FD
		public IAbility ability
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000E20 RID: 3616
		// (get) Token: 0x06004401 RID: 17409 RVA: 0x00071719 File Offset: 0x0006F919
		// (set) Token: 0x06004402 RID: 17410 RVA: 0x00002191 File Offset: 0x00000391
		public float remainTime
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		// Token: 0x17000E21 RID: 3617
		// (get) Token: 0x06004403 RID: 17411 RVA: 0x000076D4 File Offset: 0x000058D4
		public bool attached
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000E22 RID: 3618
		// (get) Token: 0x06004404 RID: 17412 RVA: 0x000C5A2D File Offset: 0x000C3C2D
		public Sprite icon
		{
			get
			{
				if (this._damageStacks <= 0)
				{
					return null;
				}
				return this._defaultIcon;
			}
		}

		// Token: 0x17000E23 RID: 3619
		// (get) Token: 0x06004405 RID: 17413 RVA: 0x00071719 File Offset: 0x0006F919
		public float iconFillAmount
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000E24 RID: 3620
		// (get) Token: 0x06004406 RID: 17414 RVA: 0x000C5A40 File Offset: 0x000C3C40
		public int iconStacks
		{
			get
			{
				return this._damageStacks;
			}
		}

		// Token: 0x17000E25 RID: 3621
		// (get) Token: 0x06004407 RID: 17415 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool expired
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06004408 RID: 17416 RVA: 0x000C5A48 File Offset: 0x000C3C48
		public override void Initialize()
		{
			base.Initialize();
			this._operations.Initialize();
			this._onUpgradeSucceeded.Initialize();
			this._onUpgradeFailed.Initialize();
		}

		// Token: 0x06004409 RID: 17417 RVA: 0x000C5A71 File Offset: 0x000C3C71
		public override IAbilityInstance CreateInstance(Character owner)
		{
			this.owner = owner;
			return this;
		}

		// Token: 0x0600440A RID: 17418 RVA: 0x000C5A7B File Offset: 0x000C3C7B
		public void UpdateTime(float deltaTime)
		{
			if (!this._gaugeConstraints.Pass())
			{
				return;
			}
			this.AddGauge(this._gaugeAmountPerSecond * deltaTime);
		}

		// Token: 0x0600440B RID: 17419 RVA: 0x00002191 File Offset: 0x00000391
		public void Refresh()
		{
		}

		// Token: 0x0600440C RID: 17420 RVA: 0x000C5A9C File Offset: 0x000C3C9C
		public void Attach()
		{
			this._damageStacks = 0;
			this._upgradedCount = 0;
			this._riskyUpgrade.cooldown.stacks = this._upgradablecount;
			this._gauge.defaultBarColor = this._defaultGaugeColor;
			this.ResetGauge();
			this.owner.onGiveDamage.Remove(new GiveDamageDelegate(this.OnGiveDamage));
			Singleton<Service>.Instance.levelManager.onMapLoaded += this.RemoveAllSmallBombs;
		}

		// Token: 0x0600440D RID: 17421 RVA: 0x000C5B1C File Offset: 0x000C3D1C
		public void Detach()
		{
			this.owner.onGiveDamage.Remove(new GiveDamageDelegate(this.OnGiveDamage));
			Singleton<Service>.Instance.levelManager.onMapLoaded -= this.RemoveAllSmallBombs;
			this.RemoveAllSmallBombs();
		}

		// Token: 0x0600440E RID: 17422 RVA: 0x000C5B5C File Offset: 0x000C3D5C
		public void DetachEvent()
		{
			Singleton<Service>.Instance.levelManager.onMapLoaded -= this.RemoveAllSmallBombs;
		}

		// Token: 0x0600440F RID: 17423 RVA: 0x000C5B79 File Offset: 0x000C3D79
		public void AddDamageStack(int amount)
		{
			this._damageStacks += amount;
			this._gauge.defaultBarColor = Color.Lerp(this._defaultGaugeColor, this._damageStackedGaugeColor, (float)this._damageStacks / 100f);
		}

		// Token: 0x06004410 RID: 17424 RVA: 0x000C5BB2 File Offset: 0x000C3DB2
		public void AddGauge(float amount)
		{
			this._gauge.Add(amount);
			if (this._gauge.currentValue < this._gauge.maxValue)
			{
				return;
			}
			this.Explode();
		}

		// Token: 0x06004411 RID: 17425 RVA: 0x000C5BDF File Offset: 0x000C3DDF
		public void ResetGauge()
		{
			this._fuseSound.Stop();
			this._fuseSound.Play();
			this._gauge.Clear();
		}

		// Token: 0x06004412 RID: 17426 RVA: 0x000C5C04 File Offset: 0x000C3E04
		public void Explode()
		{
			this._gauge.Clear();
			this.owner.onGiveDamage.Add(0, new GiveDamageDelegate(this.OnGiveDamage));
			this._operations.Run(this.owner);
			foreach (OperationRunner operationRunner in this._smallBombs)
			{
				operationRunner.operationInfos.Run(this.owner);
			}
			this._smallBombs.Clear();
			this.owner.onGiveDamage.Remove(new GiveDamageDelegate(this.OnGiveDamage));
			this.owner.playerComponents.inventory.weapon.NextWeapon(true);
		}

		// Token: 0x06004413 RID: 17427 RVA: 0x000C5CDC File Offset: 0x000C3EDC
		public void RiskyUpgrade()
		{
			if (!MMMaths.PercentChance(this._upgradeChances[Math.Min(this._upgradedCount, this._upgradeChances.Length - 1)]))
			{
				this._onUpgradeFailed.Run(this.owner);
				this.Explode();
				return;
			}
			this._onUpgradeSucceeded.Run(this.owner);
			this.ResetGauge();
			this._upgradedCount++;
			this.AddDamageStack(this._damageStacksByUpgrade);
		}

		// Token: 0x06004414 RID: 17428 RVA: 0x000C5D55 File Offset: 0x000C3F55
		private bool OnGiveDamage(ITarget target, ref Damage damage)
		{
			damage.multiplier += (double)this._damageStacks * 0.01;
			return false;
		}

		// Token: 0x06004415 RID: 17429 RVA: 0x000C5D73 File Offset: 0x000C3F73
		private void RemoveAllSmallBombs()
		{
			if (this._smallBombs.Count == 0)
			{
				return;
			}
			this._smallBombs[0].poolObject.DespawnAllSiblings();
			this._smallBombs.Clear();
		}

		// Token: 0x06004416 RID: 17430 RVA: 0x000C5DA4 File Offset: 0x000C3FA4
		public void RegisterSmallBomb(OperationRunner smallBomb)
		{
			if (this._smallBombs.Count >= 100)
			{
				this._smallBombs[0].operationInfos.Run(this.owner);
				this._smallBombs.RemoveAt(0);
			}
			this._smallBombs.Add(smallBomb);
		}

		// Token: 0x040033D4 RID: 13268
		private const int _maxSmallBombs = 100;

		// Token: 0x040033D5 RID: 13269
		[Header("Gauge")]
		[SerializeField]
		private ValueGauge _gauge;

		// Token: 0x040033D6 RID: 13270
		[SerializeField]
		private Color _defaultGaugeColor;

		// Token: 0x040033D7 RID: 13271
		[SerializeField]
		[Tooltip("피해량 증가 스택에 비례해 게이지 색깔이 이 색깔로 변합니다. 100일 때 완전히 변합니다.")]
		private Color _damageStackedGaugeColor;

		// Token: 0x040033D8 RID: 13272
		[SerializeField]
		private float _gaugeAmountPerSecond;

		// Token: 0x040033D9 RID: 13273
		[Constraint.SubcomponentAttribute]
		[SerializeField]
		private Constraint.Subcomponents _gaugeConstraints;

		// Token: 0x040033DA RID: 13274
		[Header("Risky Upgrade")]
		[SerializeField]
		private Characters.Actions.Action _riskyUpgrade;

		// Token: 0x040033DB RID: 13275
		[SerializeField]
		private int _upgradablecount;

		// Token: 0x040033DC RID: 13276
		[SerializeField]
		private int _damageStacksByUpgrade;

		// Token: 0x040033DD RID: 13277
		[SerializeField]
		private int[] _upgradeChances;

		// Token: 0x040033DE RID: 13278
		private int _upgradedCount;

		// Token: 0x040033DF RID: 13279
		[SerializeField]
		[CharacterOperation.SubcomponentAttribute]
		private CharacterOperation.Subcomponents _onUpgradeSucceeded;

		// Token: 0x040033E0 RID: 13280
		[SerializeField]
		[CharacterOperation.SubcomponentAttribute]
		private CharacterOperation.Subcomponents _onUpgradeFailed;

		// Token: 0x040033E1 RID: 13281
		[Space]
		[SerializeField]
		private PlaySoundInfo _fuseSound;

		// Token: 0x040033E2 RID: 13282
		private int _damageStacks;

		// Token: 0x040033E3 RID: 13283
		[SerializeField]
		[CharacterOperation.SubcomponentAttribute]
		private CharacterOperation.Subcomponents _operations;

		// Token: 0x040033E4 RID: 13284
		private readonly List<OperationRunner> _smallBombs = new List<OperationRunner>(100);
	}
}
