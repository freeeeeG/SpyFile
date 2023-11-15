using System;
using Characters.Actions;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Characters.Abilities.Items
{
	// Token: 0x02000D0B RID: 3339
	[Serializable]
	public sealed class WindMail : Ability
	{
		// Token: 0x0600434F RID: 17231 RVA: 0x000C4579 File Offset: 0x000C2779
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new WindMail.Instance(owner, this);
		}

		// Token: 0x04003374 RID: 13172
		[SerializeField]
		[Subcomponent(typeof(OperationInfo))]
		private OperationInfo.Subcomponents _operations;

		// Token: 0x04003375 RID: 13173
		[SerializeField]
		private float _cooldownTime;

		// Token: 0x04003376 RID: 13174
		[SerializeField]
		private float _doubleDashTime;

		// Token: 0x02000D0C RID: 3340
		public class Instance : AbilityInstance<WindMail>
		{
			// Token: 0x06004351 RID: 17233 RVA: 0x000C4582 File Offset: 0x000C2782
			public Instance(Character owner, WindMail ability) : base(owner, ability)
			{
			}

			// Token: 0x06004352 RID: 17234 RVA: 0x000C458C File Offset: 0x000C278C
			protected override void OnAttach()
			{
				this.owner.onStartAction += this.HandleOnStartAction;
			}

			// Token: 0x06004353 RID: 17235 RVA: 0x000C45A5 File Offset: 0x000C27A5
			public override void UpdateTime(float deltaTime)
			{
				base.UpdateTime(deltaTime);
				this._remainTime -= deltaTime;
				this._remainCooldownTime -= deltaTime;
				if (this._remainTime < 0f)
				{
					this._canUse = false;
				}
			}

			// Token: 0x06004354 RID: 17236 RVA: 0x000C45E0 File Offset: 0x000C27E0
			private void HandleOnStartAction(Characters.Actions.Action action)
			{
				if (action.type != Characters.Actions.Action.Type.Dash)
				{
					return;
				}
				if (this._remainCooldownTime > 0f)
				{
					return;
				}
				if (!this._canUse)
				{
					this._remainTime = this.ability._doubleDashTime;
					this._canUse = true;
					return;
				}
				this._remainCooldownTime = this.ability._cooldownTime;
				this.owner.StartCoroutine(this.ability._operations.CRun(this.owner));
			}

			// Token: 0x06004355 RID: 17237 RVA: 0x000C4658 File Offset: 0x000C2858
			protected override void OnDetach()
			{
				this.owner.onStartAction -= this.HandleOnStartAction;
			}

			// Token: 0x04003377 RID: 13175
			private float _remainCooldownTime;

			// Token: 0x04003378 RID: 13176
			private float _remainTime;

			// Token: 0x04003379 RID: 13177
			private bool _canUse;
		}
	}
}
