using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours
{
	// Token: 0x0200130E RID: 4878
	public class FlyChase : Behaviour
	{
		// Token: 0x0600606A RID: 24682 RVA: 0x0011A50C File Offset: 0x0011870C
		private void Start()
		{
			this._childs = new List<Behaviour>
			{
				this._moveToTarget,
				this._wanderWhenChaseFail,
				this._beforeRangeWander
			};
		}

		// Token: 0x0600606B RID: 24683 RVA: 0x0011A53D File Offset: 0x0011873D
		public override IEnumerator CRun(AIController controller)
		{
			Character character = controller.character;
			UnityEngine.Object target = controller.target;
			base.result = Behaviour.Result.Doing;
			if (target == null)
			{
				base.result = Behaviour.Result.Fail;
				yield break;
			}
			yield return this._moveToTarget.CRun(controller);
			if (this._moveToTarget.result == Behaviour.Result.Fail)
			{
				yield return this._beforeRangeWander.CRun(controller);
				base.result = Behaviour.Result.Fail;
				yield return this._wanderWhenChaseFail.CRun(controller);
			}
			else
			{
				base.result = Behaviour.Result.Success;
			}
			yield break;
		}

		// Token: 0x04004DA8 RID: 19880
		[UnityEditor.Subcomponent(typeof(MoveToTargetWithFly))]
		[SerializeField]
		private MoveToTargetWithFly _moveToTarget;

		// Token: 0x04004DA9 RID: 19881
		[Wander.SubcomponentAttribute(true)]
		[SerializeField]
		private Wander _wanderWhenChaseFail;

		// Token: 0x04004DAA RID: 19882
		[SerializeField]
		[UnityEditor.Subcomponent(typeof(Idle))]
		private Idle _beforeRangeWander;

		// Token: 0x0200130F RID: 4879
		[AttributeUsage(AttributeTargets.Field)]
		public new class SubcomponentAttribute : UnityEditor.SubcomponentAttribute
		{
			// Token: 0x0600606D RID: 24685 RVA: 0x0011A553 File Offset: 0x00118753
			public SubcomponentAttribute(bool allowCustom = true) : base(allowCustom, FlyChase.SubcomponentAttribute.types)
			{
			}

			// Token: 0x04004DAB RID: 19883
			public new static readonly Type[] types = new Type[]
			{
				typeof(FlyChase)
			};
		}
	}
}
