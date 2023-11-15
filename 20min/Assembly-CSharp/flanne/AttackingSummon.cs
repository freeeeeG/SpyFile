using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x02000120 RID: 288
	public abstract class AttackingSummon : Summon
	{
		// Token: 0x17000084 RID: 132
		// (get) Token: 0x060007E6 RID: 2022 RVA: 0x00021A6C File Offset: 0x0001FC6C
		protected float finalAttackCooldown
		{
			get
			{
				float num = this.attackSpeedMod.ModifyInverse(base.summonAtkSpdMod.ModifyInverse(this.attackCooldown));
				num = num.NotifyModifiers(AttackingSummon.TweakAttackCDNotification, this);
				return Mathf.Max(0.1f, num);
			}
		}

		// Token: 0x060007E7 RID: 2023 RVA: 0x00021AAE File Offset: 0x0001FCAE
		private void Awake()
		{
			this.attackSpeedMod = new StatMod();
		}

		// Token: 0x060007E8 RID: 2024 RVA: 0x00021ABC File Offset: 0x0001FCBC
		private void Update()
		{
			if (this._timer >= this.finalAttackCooldown)
			{
				if (this.Attack())
				{
					this._timer -= this.finalAttackCooldown;
					if (this.animator != null)
					{
						this.animator.SetTrigger("Attack");
					}
					if (this.attackSoundFX != null)
					{
						this.attackSoundFX.Play(null);
						return;
					}
				}
			}
			else
			{
				this._timer += Time.deltaTime;
			}
		}

		// Token: 0x060007E9 RID: 2025
		protected abstract bool Attack();

		// Token: 0x040005BD RID: 1469
		public static string TweakAttackCDNotification = "AttackingSummon.TweakAttackCDNotification";

		// Token: 0x040005BE RID: 1470
		public StatMod attackSpeedMod;

		// Token: 0x040005BF RID: 1471
		[SerializeField]
		private float attackCooldown;

		// Token: 0x040005C0 RID: 1472
		[SerializeField]
		protected Animator animator;

		// Token: 0x040005C1 RID: 1473
		[SerializeField]
		private SoundEffectSO attackSoundFX;

		// Token: 0x040005C2 RID: 1474
		private float _timer;
	}
}
