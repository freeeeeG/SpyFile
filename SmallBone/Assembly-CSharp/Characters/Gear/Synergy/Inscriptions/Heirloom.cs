using System;
using System.Collections;
using Characters.Abilities;
using Characters.Operations;
using Characters.Operations.Fx;
using UnityEditor;
using UnityEngine;

namespace Characters.Gear.Synergy.Inscriptions
{
	// Token: 0x0200088E RID: 2190
	public sealed class Heirloom : InscriptionInstance
	{
		// Token: 0x06002E31 RID: 11825 RVA: 0x0008BA1C File Offset: 0x00089C1C
		protected override void Initialize()
		{
			this._ability = new Heirloom.HeirloomAbility(base.character, this);
			this._ability.Initialize();
			this._ability.icon = this._icon;
			this._cooldownAbility.Initialize();
		}

		// Token: 0x06002E32 RID: 11826 RVA: 0x0008BA57 File Offset: 0x00089C57
		public override void UpdateBonus(bool wasActive, bool wasOmen)
		{
			if (this.keyword.step < 1)
			{
				base.character.ability.Remove(this._ability);
				base.character.ability.Remove(this._cooldownAbility);
			}
		}

		// Token: 0x06002E33 RID: 11827 RVA: 0x0008BA95 File Offset: 0x00089C95
		public override void Attach()
		{
			this._attachReference.Stop();
			this._attachReference = this.StartCoroutineWithReference("CStartAttachLoop");
			this._canUse = true;
		}

		// Token: 0x06002E34 RID: 11828 RVA: 0x0008BABC File Offset: 0x00089CBC
		public override void Detach()
		{
			this._attachReference.Stop();
			base.character.ability.Remove(this._ability);
			base.character.ability.Remove(this._cooldownAbility);
			base.StopCoroutine("CStartAttachLoop");
			this.StopSpawningMotionTrail();
		}

		// Token: 0x06002E35 RID: 11829 RVA: 0x0008BB13 File Offset: 0x00089D13
		private IEnumerator CStartAttachLoop()
		{
			for (;;)
			{
				yield return null;
				if (this.keyword.step >= 1 && this._canUse)
				{
					base.character.ability.Add(this._ability);
				}
			}
			yield break;
		}

		// Token: 0x06002E36 RID: 11830 RVA: 0x0008BB22 File Offset: 0x00089D22
		private IEnumerator CCooldown()
		{
			this._canUse = false;
			while (base.character.ability.Contains(this._cooldownAbility))
			{
				yield return null;
			}
			this._canUse = true;
			yield break;
		}

		// Token: 0x06002E37 RID: 11831 RVA: 0x0008BB31 File Offset: 0x00089D31
		private void SpawnMotionTrail()
		{
			this._motionTrailOperation.Run(base.character);
		}

		// Token: 0x06002E38 RID: 11832 RVA: 0x0008BB44 File Offset: 0x00089D44
		private void StartSpawningMotionTrail()
		{
			base.StartCoroutine("CSpawnTrail");
		}

		// Token: 0x06002E39 RID: 11833 RVA: 0x0008BB52 File Offset: 0x00089D52
		private void StopSpawningMotionTrail()
		{
			base.StopCoroutine("CSpawnTrail");
		}

		// Token: 0x06002E3A RID: 11834 RVA: 0x0008BB5F File Offset: 0x00089D5F
		private IEnumerator CSpawnTrail()
		{
			for (;;)
			{
				this.SpawnMotionTrail();
				yield return Chronometer.global.WaitForSeconds(this._motionTrailInterval);
			}
			yield break;
		}

		// Token: 0x04002671 RID: 9841
		[SerializeField]
		[Header("2세트 효과")]
		private float _cooldownTime;

		// Token: 0x04002672 RID: 9842
		[SerializeField]
		private Sprite _icon;

		// Token: 0x04002673 RID: 9843
		[SerializeField]
		[Space]
		private MotionTrail _motionTrailOperation;

		// Token: 0x04002674 RID: 9844
		[SerializeField]
		private float _motionTrailInterval;

		// Token: 0x04002675 RID: 9845
		[Space]
		[SerializeField]
		[Subcomponent(typeof(OperationInfo))]
		private OperationInfo.Subcomponents _onAttach;

		// Token: 0x04002676 RID: 9846
		[SerializeField]
		[Subcomponent(typeof(OperationInfo))]
		private OperationInfo.Subcomponents _onDetach;

		// Token: 0x04002677 RID: 9847
		[SerializeField]
		private Nothing _cooldownAbility;

		// Token: 0x04002678 RID: 9848
		[SerializeField]
		[Header("4세트 효과")]
		private float _damagePercentMultiplier;

		// Token: 0x04002679 RID: 9849
		private Heirloom.HeirloomAbility _ability;

		// Token: 0x0400267A RID: 9850
		private bool _canUse;

		// Token: 0x0400267B RID: 9851
		private CoroutineReference _attachReference;

		// Token: 0x0200088F RID: 2191
		private sealed class HeirloomAbility : IAbility, IAbilityInstance
		{
			// Token: 0x170009D1 RID: 2513
			// (get) Token: 0x06002E3C RID: 11836 RVA: 0x0008BB6E File Offset: 0x00089D6E
			Character IAbilityInstance.owner
			{
				get
				{
					return this._owner;
				}
			}

			// Token: 0x170009D2 RID: 2514
			// (get) Token: 0x06002E3D RID: 11837 RVA: 0x000716FD File Offset: 0x0006F8FD
			public IAbility ability
			{
				get
				{
					return this;
				}
			}

			// Token: 0x170009D3 RID: 2515
			// (get) Token: 0x06002E3E RID: 11838 RVA: 0x0008BB76 File Offset: 0x00089D76
			// (set) Token: 0x06002E3F RID: 11839 RVA: 0x0008BB7E File Offset: 0x00089D7E
			public float remainTime { get; set; }

			// Token: 0x170009D4 RID: 2516
			// (get) Token: 0x06002E40 RID: 11840 RVA: 0x000076D4 File Offset: 0x000058D4
			public bool attached
			{
				get
				{
					return true;
				}
			}

			// Token: 0x170009D5 RID: 2517
			// (get) Token: 0x06002E41 RID: 11841 RVA: 0x0008BB87 File Offset: 0x00089D87
			// (set) Token: 0x06002E42 RID: 11842 RVA: 0x0008BB8F File Offset: 0x00089D8F
			public Sprite icon { get; set; }

			// Token: 0x170009D6 RID: 2518
			// (get) Token: 0x06002E43 RID: 11843 RVA: 0x00071719 File Offset: 0x0006F919
			public float iconFillAmount
			{
				get
				{
					return 0f;
				}
			}

			// Token: 0x170009D7 RID: 2519
			// (get) Token: 0x06002E44 RID: 11844 RVA: 0x00018EC5 File Offset: 0x000170C5
			public bool iconFillInversed
			{
				get
				{
					return false;
				}
			}

			// Token: 0x170009D8 RID: 2520
			// (get) Token: 0x06002E45 RID: 11845 RVA: 0x00018EC5 File Offset: 0x000170C5
			public bool iconFillFlipped
			{
				get
				{
					return false;
				}
			}

			// Token: 0x170009D9 RID: 2521
			// (get) Token: 0x06002E46 RID: 11846 RVA: 0x00018EC5 File Offset: 0x000170C5
			public int iconStacks
			{
				get
				{
					return 0;
				}
			}

			// Token: 0x170009DA RID: 2522
			// (get) Token: 0x06002E47 RID: 11847 RVA: 0x00018EC5 File Offset: 0x000170C5
			public bool expired
			{
				get
				{
					return false;
				}
			}

			// Token: 0x170009DB RID: 2523
			// (get) Token: 0x06002E48 RID: 11848 RVA: 0x0008BB98 File Offset: 0x00089D98
			// (set) Token: 0x06002E49 RID: 11849 RVA: 0x0008BBA0 File Offset: 0x00089DA0
			public float duration { get; set; }

			// Token: 0x170009DC RID: 2524
			// (get) Token: 0x06002E4A RID: 11850 RVA: 0x00018EC5 File Offset: 0x000170C5
			public int iconPriority
			{
				get
				{
					return 0;
				}
			}

			// Token: 0x170009DD RID: 2525
			// (get) Token: 0x06002E4B RID: 11851 RVA: 0x00018EC5 File Offset: 0x000170C5
			public bool removeOnSwapWeapon
			{
				get
				{
					return false;
				}
			}

			// Token: 0x06002E4C RID: 11852 RVA: 0x000716FD File Offset: 0x0006F8FD
			public IAbilityInstance CreateInstance(Character owner)
			{
				return this;
			}

			// Token: 0x06002E4D RID: 11853 RVA: 0x0008BBA9 File Offset: 0x00089DA9
			public HeirloomAbility(Character owner, Heirloom heirloom)
			{
				this._owner = owner;
				this._heirloom = heirloom;
			}

			// Token: 0x06002E4E RID: 11854 RVA: 0x00002191 File Offset: 0x00000391
			public void Initialize()
			{
			}

			// Token: 0x06002E4F RID: 11855 RVA: 0x00002191 File Offset: 0x00000391
			public void UpdateTime(float deltaTime)
			{
			}

			// Token: 0x06002E50 RID: 11856 RVA: 0x00002191 File Offset: 0x00000391
			public void Refresh()
			{
			}

			// Token: 0x06002E51 RID: 11857 RVA: 0x0008BBC0 File Offset: 0x00089DC0
			void IAbilityInstance.Attach()
			{
				this._owner.health.onTakeDamage.Add(200, new TakeDamageDelegate(this.OnTakeDamage));
				this._heirloom.StartSpawningMotionTrail();
				this._owner.onGiveDamage.Add(int.MaxValue, new GiveDamageDelegate(this.HanldeOnGiveDamage));
				this._owner.StartCoroutine(this._heirloom._onAttach.CRun(this._owner));
			}

			// Token: 0x06002E52 RID: 11858 RVA: 0x0008BC41 File Offset: 0x00089E41
			private bool HanldeOnGiveDamage(ITarget target, ref Damage damage)
			{
				if (!this._heirloom.keyword.isMaxStep)
				{
					return false;
				}
				if (damage.attackType == Damage.AttackType.None)
				{
					return false;
				}
				damage.percentMultiplier *= (double)this._heirloom._damagePercentMultiplier;
				return false;
			}

			// Token: 0x06002E53 RID: 11859 RVA: 0x0008BC78 File Offset: 0x00089E78
			void IAbilityInstance.Detach()
			{
				this._heirloom.SpawnMotionTrail();
				this._heirloom.StopSpawningMotionTrail();
				this._owner.health.onTakeDamage.Remove(new TakeDamageDelegate(this.OnTakeDamage));
				this._owner.onGiveDamage.Remove(new GiveDamageDelegate(this.HanldeOnGiveDamage));
				this._owner.ability.Add(this._heirloom._cooldownAbility);
				this._owner.StartCoroutine(this._heirloom.CCooldown());
				this._owner.StartCoroutine(this._heirloom._onDetach.CRun(this._owner));
			}

			// Token: 0x06002E54 RID: 11860 RVA: 0x0008BD30 File Offset: 0x00089F30
			private bool OnTakeDamage(ref Damage damage)
			{
				if (damage.attackType == Damage.AttackType.None || damage.attackType == Damage.AttackType.Additional)
				{
					return false;
				}
				if (damage.amount < 1.0)
				{
					return false;
				}
				if (this._owner.invulnerable.value || this._owner.evasion.value || damage.@null)
				{
					return false;
				}
				damage.@null = true;
				this._owner.ability.Remove(this);
				return true;
			}

			// Token: 0x0400267C RID: 9852
			private readonly Character _owner;

			// Token: 0x0400267D RID: 9853
			private readonly Heirloom _heirloom;
		}
	}
}
