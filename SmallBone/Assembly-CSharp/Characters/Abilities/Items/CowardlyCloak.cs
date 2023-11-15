using System;
using Characters.Actions;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Characters.Abilities.Items
{
	// Token: 0x02000CAC RID: 3244
	[Serializable]
	public sealed class CowardlyCloak : Ability
	{
		// Token: 0x060041EC RID: 16876 RVA: 0x000BFE5C File Offset: 0x000BE05C
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new CowardlyCloak.Instance(owner, this);
		}

		// Token: 0x0400327E RID: 12926
		[SerializeField]
		[Subcomponent(typeof(OperationInfo))]
		private OperationInfo.Subcomponents _operations;

		// Token: 0x0400327F RID: 12927
		[SerializeField]
		private float _cooldownTime;

		// Token: 0x04003280 RID: 12928
		[SerializeField]
		private float _jumpTime;

		// Token: 0x02000CAD RID: 3245
		public class Instance : AbilityInstance<CowardlyCloak>
		{
			// Token: 0x060041EE RID: 16878 RVA: 0x000BFE65 File Offset: 0x000BE065
			public Instance(Character owner, CowardlyCloak ability) : base(owner, ability)
			{
			}

			// Token: 0x060041EF RID: 16879 RVA: 0x000BFE6F File Offset: 0x000BE06F
			protected override void OnAttach()
			{
				this.owner.onStartAction += this.HandleOnStartAction;
			}

			// Token: 0x060041F0 RID: 16880 RVA: 0x000BFE88 File Offset: 0x000BE088
			public override void UpdateTime(float deltaTime)
			{
				base.UpdateTime(deltaTime);
				this._remainTime -= deltaTime;
				this._remainCooldownTime -= deltaTime;
				if (this._remainTime <= 0f)
				{
					this._canUse = false;
				}
			}

			// Token: 0x060041F1 RID: 16881 RVA: 0x000BFEC4 File Offset: 0x000BE0C4
			private void HandleOnStartAction(Characters.Actions.Action action)
			{
				if (action.type != Characters.Actions.Action.Type.Dash && action.type != Characters.Actions.Action.Type.Jump)
				{
					return;
				}
				if (this._remainCooldownTime > 0f)
				{
					return;
				}
				if (action.type == Characters.Actions.Action.Type.Dash)
				{
					this._remainTime = this.ability._jumpTime;
					this._canUse = true;
					return;
				}
				if (action.type == Characters.Actions.Action.Type.Jump && this._canUse)
				{
					this._remainTime = 0f;
					this._canUse = false;
					this._remainCooldownTime = this.ability._cooldownTime;
					this.owner.StartCoroutine(this.ability._operations.CRun(this.owner));
				}
			}

			// Token: 0x060041F2 RID: 16882 RVA: 0x000BFF68 File Offset: 0x000BE168
			protected override void OnDetach()
			{
				this.owner.onStartAction -= this.HandleOnStartAction;
			}

			// Token: 0x04003281 RID: 12929
			private float _remainCooldownTime;

			// Token: 0x04003282 RID: 12930
			private float _remainTime;

			// Token: 0x04003283 RID: 12931
			private bool _canUse;
		}
	}
}
