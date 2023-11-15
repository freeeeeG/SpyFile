using System;
using System.Collections;
using Characters.Actions;
using UnityEngine;

namespace Characters.AI.Behaviours
{
	// Token: 0x020012FB RID: 4859
	public sealed class RunActionWhileStuck : Behaviour
	{
		// Token: 0x06006014 RID: 24596 RVA: 0x001192D2 File Offset: 0x001174D2
		public override IEnumerator CRun(AIController controller)
		{
			base.result = Behaviour.Result.Doing;
			this._stayCollision = false;
			Character character = controller.character;
			yield return this.CUpdate(controller);
			base.result = Behaviour.Result.Success;
			yield break;
		}

		// Token: 0x06006015 RID: 24597 RVA: 0x001192E8 File Offset: 0x001174E8
		private void OnEnterCollision(RaycastHit2D obj)
		{
			this._stayCollision = true;
		}

		// Token: 0x06006016 RID: 24598 RVA: 0x001192F1 File Offset: 0x001174F1
		private IEnumerator CUpdate(AIController controller)
		{
			Character character = controller.character;
			float elapsed = 0f;
			if (!this._action.TryStart())
			{
				base.result = Behaviour.Result.Fail;
				yield break;
			}
			while (this._action.running && base.result == Behaviour.Result.Doing && elapsed <= this._maxTime && !this.CheckCollision(character))
			{
				this.Move(controller.character);
				yield return null;
				elapsed += controller.character.chronometer.master.deltaTime;
			}
			yield break;
		}

		// Token: 0x06006017 RID: 24599 RVA: 0x00119308 File Offset: 0x00117508
		private bool CheckCollision(Character character)
		{
			return (character.lookingDirection == Character.LookingDirection.Right && character.movement.controller.collisionState.right) || (character.lookingDirection == Character.LookingDirection.Left && character.movement.controller.collisionState.left);
		}

		// Token: 0x06006018 RID: 24600 RVA: 0x0011935C File Offset: 0x0011755C
		private void Move(Character character)
		{
			int num = (character.lookingDirection == Character.LookingDirection.Right) ? 0 : 180;
			character.movement.Move((float)num);
		}

		// Token: 0x04004D4D RID: 19789
		[SerializeField]
		private Characters.Actions.Action _action;

		// Token: 0x04004D4E RID: 19790
		[SerializeField]
		private float _maxTime = 10f;

		// Token: 0x04004D4F RID: 19791
		private bool _stayCollision;
	}
}
