using System;
using System.Collections;
using Characters.Abilities;
using Characters.Operations.Fx;
using UnityEngine;

namespace Characters.Gear.Synergy.Inscriptions
{
	// Token: 0x0200089E RID: 2206
	public class Mirage : InscriptionInstance
	{
		// Token: 0x06002ECB RID: 11979 RVA: 0x0008CD72 File Offset: 0x0008AF72
		protected override void Initialize()
		{
			this._ability = new Mirage.Ability(base.character, this);
			this._ability.Initialize();
			this._ability.icon = this._icon;
		}

		// Token: 0x06002ECC RID: 11980 RVA: 0x0008CDA2 File Offset: 0x0008AFA2
		public override void UpdateBonus(bool wasActive, bool wasOmen)
		{
			this._ability.duration = this._cooldownTimeByLevel[this.keyword.step];
		}

		// Token: 0x06002ECD RID: 11981 RVA: 0x0008CDC1 File Offset: 0x0008AFC1
		public override void Attach()
		{
			base.character.ability.Add(this._ability);
		}

		// Token: 0x06002ECE RID: 11982 RVA: 0x0008CDDA File Offset: 0x0008AFDA
		public override void Detach()
		{
			base.character.ability.Remove(this._ability);
			this.StopSpawningMotionTrail();
		}

		// Token: 0x06002ECF RID: 11983 RVA: 0x0008CDF9 File Offset: 0x0008AFF9
		private void SpawnMotionTrail()
		{
			this._motionTrailOperation.Run(base.character);
		}

		// Token: 0x06002ED0 RID: 11984 RVA: 0x0008BB44 File Offset: 0x00089D44
		private void StartSpawningMotionTrail()
		{
			base.StartCoroutine("CSpawnTrail");
		}

		// Token: 0x06002ED1 RID: 11985 RVA: 0x0008BB52 File Offset: 0x00089D52
		private void StopSpawningMotionTrail()
		{
			base.StopCoroutine("CSpawnTrail");
		}

		// Token: 0x06002ED2 RID: 11986 RVA: 0x0008CE0C File Offset: 0x0008B00C
		private IEnumerator CSpawnTrail()
		{
			for (;;)
			{
				this.SpawnMotionTrail();
				yield return Chronometer.global.WaitForSeconds(this._motionTrailInterval);
			}
			yield break;
		}

		// Token: 0x040026DD RID: 9949
		[SerializeField]
		private Sprite _icon;

		// Token: 0x040026DE RID: 9950
		[SerializeField]
		[Space]
		private MotionTrail _motionTrailOperation;

		// Token: 0x040026DF RID: 9951
		[SerializeField]
		private float _motionTrailInterval;

		// Token: 0x040026E0 RID: 9952
		[SerializeField]
		[Space]
		private float[] _cooldownTimeByLevel = new float[]
		{
			0f,
			50f,
			40f,
			30f,
			20f,
			10f
		};

		// Token: 0x040026E1 RID: 9953
		private Mirage.Ability _ability;

		// Token: 0x0200089F RID: 2207
		protected class Ability : IAbility, IAbilityInstance
		{
			// Token: 0x170009FF RID: 2559
			// (get) Token: 0x06002ED4 RID: 11988 RVA: 0x0008CE3A File Offset: 0x0008B03A
			Character IAbilityInstance.owner
			{
				get
				{
					return this._owner;
				}
			}

			// Token: 0x17000A00 RID: 2560
			// (get) Token: 0x06002ED5 RID: 11989 RVA: 0x000716FD File Offset: 0x0006F8FD
			public IAbility ability
			{
				get
				{
					return this;
				}
			}

			// Token: 0x17000A01 RID: 2561
			// (get) Token: 0x06002ED6 RID: 11990 RVA: 0x0008CE42 File Offset: 0x0008B042
			// (set) Token: 0x06002ED7 RID: 11991 RVA: 0x0008CE4A File Offset: 0x0008B04A
			public float remainTime { get; set; }

			// Token: 0x17000A02 RID: 2562
			// (get) Token: 0x06002ED8 RID: 11992 RVA: 0x000076D4 File Offset: 0x000058D4
			public bool attached
			{
				get
				{
					return true;
				}
			}

			// Token: 0x17000A03 RID: 2563
			// (get) Token: 0x06002ED9 RID: 11993 RVA: 0x0008CE53 File Offset: 0x0008B053
			// (set) Token: 0x06002EDA RID: 11994 RVA: 0x0008CE5B File Offset: 0x0008B05B
			public Sprite icon { get; set; }

			// Token: 0x17000A04 RID: 2564
			// (get) Token: 0x06002EDB RID: 11995 RVA: 0x0008CE64 File Offset: 0x0008B064
			public float iconFillAmount
			{
				get
				{
					return this.remainTime / this.duration;
				}
			}

			// Token: 0x17000A05 RID: 2565
			// (get) Token: 0x06002EDC RID: 11996 RVA: 0x00018EC5 File Offset: 0x000170C5
			public bool iconFillInversed
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17000A06 RID: 2566
			// (get) Token: 0x06002EDD RID: 11997 RVA: 0x00018EC5 File Offset: 0x000170C5
			public bool iconFillFlipped
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17000A07 RID: 2567
			// (get) Token: 0x06002EDE RID: 11998 RVA: 0x00018EC5 File Offset: 0x000170C5
			public int iconStacks
			{
				get
				{
					return 0;
				}
			}

			// Token: 0x17000A08 RID: 2568
			// (get) Token: 0x06002EDF RID: 11999 RVA: 0x00018EC5 File Offset: 0x000170C5
			public bool expired
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17000A09 RID: 2569
			// (get) Token: 0x06002EE0 RID: 12000 RVA: 0x0008CE73 File Offset: 0x0008B073
			// (set) Token: 0x06002EE1 RID: 12001 RVA: 0x0008CE7B File Offset: 0x0008B07B
			public float duration { get; set; }

			// Token: 0x17000A0A RID: 2570
			// (get) Token: 0x06002EE2 RID: 12002 RVA: 0x00018EC5 File Offset: 0x000170C5
			public int iconPriority
			{
				get
				{
					return 0;
				}
			}

			// Token: 0x17000A0B RID: 2571
			// (get) Token: 0x06002EE3 RID: 12003 RVA: 0x00018EC5 File Offset: 0x000170C5
			public bool removeOnSwapWeapon
			{
				get
				{
					return false;
				}
			}

			// Token: 0x06002EE4 RID: 12004 RVA: 0x000716FD File Offset: 0x0006F8FD
			public IAbilityInstance CreateInstance(Character owner)
			{
				return this;
			}

			// Token: 0x06002EE5 RID: 12005 RVA: 0x0008CE84 File Offset: 0x0008B084
			public Ability(Character owner, Mirage mirage)
			{
				this._owner = owner;
				this._mirage = mirage;
			}

			// Token: 0x06002EE6 RID: 12006 RVA: 0x00002191 File Offset: 0x00000391
			public void Initialize()
			{
			}

			// Token: 0x06002EE7 RID: 12007 RVA: 0x0008CE9A File Offset: 0x0008B09A
			public void UpdateTime(float deltaTime)
			{
				if (this.remainTime < 0f)
				{
					return;
				}
				this.remainTime -= deltaTime;
				if (this.remainTime < 0f)
				{
					this._mirage.StartSpawningMotionTrail();
				}
			}

			// Token: 0x06002EE8 RID: 12008 RVA: 0x00002191 File Offset: 0x00000391
			public void Refresh()
			{
			}

			// Token: 0x06002EE9 RID: 12009 RVA: 0x0008CED0 File Offset: 0x0008B0D0
			void IAbilityInstance.Attach()
			{
				this._owner.health.onTakeDamage.Add(int.MinValue, new TakeDamageDelegate(this.OnTakeDamage));
			}

			// Token: 0x06002EEA RID: 12010 RVA: 0x0008CEF8 File Offset: 0x0008B0F8
			void IAbilityInstance.Detach()
			{
				this._owner.health.onTakeDamage.Remove(new TakeDamageDelegate(this.OnTakeDamage));
			}

			// Token: 0x06002EEB RID: 12011 RVA: 0x0008CF1C File Offset: 0x0008B11C
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
				if (this.remainTime < 0f)
				{
					this._mirage.SpawnMotionTrail();
					this._mirage.StopSpawningMotionTrail();
					this.remainTime = this.duration;
					return true;
				}
				return false;
			}

			// Token: 0x040026E2 RID: 9954
			private readonly Character _owner;

			// Token: 0x040026E3 RID: 9955
			private readonly Mirage _mirage;
		}
	}
}
