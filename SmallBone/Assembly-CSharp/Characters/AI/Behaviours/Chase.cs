using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours
{
	// Token: 0x02001297 RID: 4759
	public class Chase : Behaviour
	{
		// Token: 0x06005E5F RID: 24159 RVA: 0x0011551C File Offset: 0x0011371C
		private void Start()
		{
			this._childs = new List<Behaviour>
			{
				this._moveToTarget,
				this._wanderWhenChaseFail,
				this._beforeRangeWander
			};
		}

		// Token: 0x06005E60 RID: 24160 RVA: 0x0011554D File Offset: 0x0011374D
		public override IEnumerator CRun(AIController controller)
		{
			Character character = controller.character;
			Character target = controller.target;
			base.result = Behaviour.Result.Doing;
			if (target == null)
			{
				base.result = Behaviour.Result.Fail;
				yield break;
			}
			if (!character.movement.controller.isGrounded)
			{
				yield break;
			}
			if (!target.movement.controller.isGrounded)
			{
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

		// Token: 0x04004BD5 RID: 19413
		[UnityEditor.Subcomponent(typeof(MoveToTarget))]
		[SerializeField]
		private MoveToTarget _moveToTarget;

		// Token: 0x04004BD6 RID: 19414
		[Wander.SubcomponentAttribute(true)]
		[SerializeField]
		private Wander _wanderWhenChaseFail;

		// Token: 0x04004BD7 RID: 19415
		[SerializeField]
		[UnityEditor.Subcomponent(typeof(Idle))]
		private Idle _beforeRangeWander;

		// Token: 0x02001298 RID: 4760
		[AttributeUsage(AttributeTargets.Field)]
		public new class SubcomponentAttribute : UnityEditor.SubcomponentAttribute
		{
			// Token: 0x06005E62 RID: 24162 RVA: 0x00115563 File Offset: 0x00113763
			public SubcomponentAttribute(bool allowCustom = true) : base(allowCustom, Chase.SubcomponentAttribute.types)
			{
			}

			// Token: 0x04004BD8 RID: 19416
			public new static readonly Type[] types = new Type[]
			{
				typeof(Chase),
				typeof(FlyChase)
			};
		}
	}
}
