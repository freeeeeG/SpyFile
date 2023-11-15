using System;
using System.Collections;
using Characters.Actions;
using UnityEngine;

namespace Characters.AI.Behaviours.Hero
{
	// Token: 0x020013AB RID: 5035
	public sealed class Backstep : Behaviour
	{
		// Token: 0x0600634A RID: 25418 RVA: 0x00120D3A File Offset: 0x0011EF3A
		public override IEnumerator CRun(AIController controller)
		{
			this._action.TryStart();
			while (this._action.running)
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x0600634B RID: 25419 RVA: 0x00120D4C File Offset: 0x0011EF4C
		private void LookSide(Character character)
		{
			if (character.movement.controller.collisionState.lastStandingCollider == null)
			{
				return;
			}
			float x = character.movement.controller.collisionState.lastStandingCollider.bounds.center.x;
			if (character.transform.position.x > x)
			{
				character.ForceToLookAt(Character.LookingDirection.Right);
				return;
			}
			character.ForceToLookAt(Character.LookingDirection.Left);
		}

		// Token: 0x0400500F RID: 20495
		[SerializeField]
		private Characters.Actions.Action _action;
	}
}
