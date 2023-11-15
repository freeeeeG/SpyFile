using System;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Characters.Abilities.Items
{
	// Token: 0x02000CDD RID: 3293
	[Serializable]
	public sealed class MiracleGrail : Ability
	{
		// Token: 0x060042A7 RID: 17063 RVA: 0x000C220C File Offset: 0x000C040C
		public override void Initialize()
		{
			base.Initialize();
			this._targetOperationInfo.Initialize();
		}

		// Token: 0x060042A8 RID: 17064 RVA: 0x000C221F File Offset: 0x000C041F
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new MiracleGrail.Instance(owner, this);
		}

		// Token: 0x04003301 RID: 13057
		[SerializeField]
		private int _cycle;

		// Token: 0x04003302 RID: 13058
		[Header("Percent Point")]
		[SerializeField]
		private float _multiplier;

		// Token: 0x04003303 RID: 13059
		[SerializeField]
		private Transform _targetPoint;

		// Token: 0x04003304 RID: 13060
		[Subcomponent(typeof(TargetedOperationInfo))]
		[SerializeField]
		private TargetedOperationInfo.Subcomponents _targetOperationInfo;

		// Token: 0x02000CDE RID: 3294
		public sealed class Instance : AbilityInstance<MiracleGrail>
		{
			// Token: 0x060042AA RID: 17066 RVA: 0x000C2228 File Offset: 0x000C0428
			public Instance(Character owner, MiracleGrail ability) : base(owner, ability)
			{
			}

			// Token: 0x060042AB RID: 17067 RVA: 0x000C2232 File Offset: 0x000C0432
			protected override void OnAttach()
			{
				this.owner.status.onApplyBleed += this.HandleOnApplyBleed;
			}

			// Token: 0x060042AC RID: 17068 RVA: 0x000C2250 File Offset: 0x000C0450
			private void HandleOnApplyBleed(Character attacker, Character target)
			{
				this._stack++;
				if (this._stack + 1 >= this.ability._cycle)
				{
					this.owner.onGiveDamage.Add(int.MaxValue, new GiveDamageDelegate(this.HandleOnGiveDamage));
					this._stack = -1;
				}
			}

			// Token: 0x060042AD RID: 17069 RVA: 0x000C22A8 File Offset: 0x000C04A8
			private bool HandleOnGiveDamage(ITarget target, ref Damage damage)
			{
				if (!damage.key.Equals(CharacterStatus.AttackKeyBleed, StringComparison.OrdinalIgnoreCase))
				{
					return false;
				}
				this.ability._targetPoint.position = target.collider.bounds.center;
				this.owner.StartCoroutine(this.ability._targetOperationInfo.CRun(this.owner, target.character));
				damage.percentMultiplier *= (double)this.ability._multiplier;
				this.owner.onGiveDamage.Remove(new GiveDamageDelegate(this.HandleOnGiveDamage));
				return false;
			}

			// Token: 0x060042AE RID: 17070 RVA: 0x000C2349 File Offset: 0x000C0549
			protected override void OnDetach()
			{
				this.owner.status.onApplyBleed -= this.HandleOnApplyBleed;
				this.owner.onGiveDamage.Remove(new GiveDamageDelegate(this.HandleOnGiveDamage));
			}

			// Token: 0x04003305 RID: 13061
			private int _stack;
		}
	}
}
