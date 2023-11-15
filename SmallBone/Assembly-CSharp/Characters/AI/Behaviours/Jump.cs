using System;
using System.Collections;
using Characters.Actions;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours
{
	// Token: 0x020012EA RID: 4842
	public class Jump : Behaviour
	{
		// Token: 0x06005FBD RID: 24509 RVA: 0x00118538 File Offset: 0x00116738
		public override IEnumerator CRun(AIController controller)
		{
			Character character = controller.character;
			this._elapsedTime = 0f;
			this._jump.TryStart();
			if (!this._waitForJump)
			{
				yield break;
			}
			yield return this.CDoBehaviourAtInflectionPoint(controller);
			while (this._waitForGrounded && !character.movement.isGrounded)
			{
				yield return null;
			}
			if (!this._skipIdle)
			{
				yield return this._idle.CRun(controller);
			}
			yield break;
		}

		// Token: 0x06005FBE RID: 24510 RVA: 0x0011854E File Offset: 0x0011674E
		private IEnumerator CDoBehaviourAtInflectionPoint(AIController controller)
		{
			Character character = controller.character;
			while (character.movement.verticalVelocity > 0f)
			{
				this._elapsedTime += character.chronometer.animation.deltaTime;
				yield return null;
			}
			while (this._jump.running)
			{
				this._elapsedTime += character.chronometer.animation.deltaTime;
				yield return null;
			}
			if (this._elapsedTime > this._minimumTimeForFallAction)
			{
				Behaviour onFallBehaviour = this._onFallBehaviour;
				yield return (onFallBehaviour != null) ? onFallBehaviour.CRun(controller) : null;
			}
			yield break;
		}

		// Token: 0x06005FBF RID: 24511 RVA: 0x00118564 File Offset: 0x00116764
		public bool CanUse()
		{
			return this._jump.canUse;
		}

		// Token: 0x04004CFF RID: 19711
		[SerializeField]
		private Characters.Actions.Action _jump;

		// Token: 0x04004D00 RID: 19712
		[SerializeField]
		private Behaviour _onFallBehaviour;

		// Token: 0x04004D01 RID: 19713
		[SerializeField]
		private bool _waitForJump;

		// Token: 0x04004D02 RID: 19714
		[SerializeField]
		private bool _waitForGrounded;

		// Token: 0x04004D03 RID: 19715
		[UnityEditor.Subcomponent(typeof(Idle))]
		[SerializeField]
		private Idle _idle;

		// Token: 0x04004D04 RID: 19716
		[SerializeField]
		private bool _skipIdle = true;

		// Token: 0x04004D05 RID: 19717
		[SerializeField]
		private float _minimumTimeForFallAction;

		// Token: 0x04004D06 RID: 19718
		private float _elapsedTime;
	}
}
