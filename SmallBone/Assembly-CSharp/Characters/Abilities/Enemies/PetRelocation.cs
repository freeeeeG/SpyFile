using System;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Characters.Abilities.Enemies
{
	// Token: 0x02000B9B RID: 2971
	[Serializable]
	public sealed class PetRelocation : Ability
	{
		// Token: 0x06003D7F RID: 15743 RVA: 0x000B2DA9 File Offset: 0x000B0FA9
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new PetRelocation.Instance(owner, this);
		}

		// Token: 0x04002F92 RID: 12178
		[SerializeField]
		private float _checkInterval = 1f;

		// Token: 0x04002F93 RID: 12179
		[SerializeField]
		private Character _target;

		// Token: 0x04002F94 RID: 12180
		[SerializeField]
		[Subcomponent(typeof(OperationInfo))]
		private OperationInfo.Subcomponents _operations;

		// Token: 0x02000B9C RID: 2972
		public class Instance : AbilityInstance<PetRelocation>
		{
			// Token: 0x06003D81 RID: 15745 RVA: 0x000B2DC5 File Offset: 0x000B0FC5
			public Instance(Character owner, PetRelocation ability) : base(owner, ability)
			{
			}

			// Token: 0x06003D82 RID: 15746 RVA: 0x000B2DCF File Offset: 0x000B0FCF
			protected override void OnAttach()
			{
				this._elapsed = 0f;
			}

			// Token: 0x06003D83 RID: 15747 RVA: 0x00002191 File Offset: 0x00000391
			protected override void OnDetach()
			{
			}

			// Token: 0x06003D84 RID: 15748 RVA: 0x000B2DDC File Offset: 0x000B0FDC
			public override void Refresh()
			{
				base.Refresh();
				this._elapsed = 0f;
			}

			// Token: 0x06003D85 RID: 15749 RVA: 0x000B2DF0 File Offset: 0x000B0FF0
			public override void UpdateTime(float deltaTime)
			{
				base.UpdateTime(deltaTime);
				Character target = this.ability._target;
				if (target == null || target.health.dead)
				{
					return;
				}
				this._elapsed += deltaTime;
				if (this._elapsed > this.ability._checkInterval)
				{
					this.TeleportToTarget();
					this._elapsed -= this.ability._checkInterval;
				}
			}

			// Token: 0x06003D86 RID: 15750 RVA: 0x000B2E68 File Offset: 0x000B1068
			private void TeleportToTarget()
			{
				Collider2D lastStandingCollider = this.ability._target.movement.controller.collisionState.lastStandingCollider;
				if (lastStandingCollider == null)
				{
					return;
				}
				Collider2D lastStandingCollider2 = this.owner.movement.controller.collisionState.lastStandingCollider;
				if (lastStandingCollider2 == null)
				{
					return;
				}
				if (lastStandingCollider == lastStandingCollider2)
				{
					return;
				}
				this.owner.StartCoroutine(this.ability._operations.CRun(this.owner));
			}

			// Token: 0x04002F95 RID: 12181
			private float _elapsed;
		}
	}
}
