using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Characters.Actions;
using UnityEngine;

namespace Characters.AI.Behaviours
{
	// Token: 0x020012D1 RID: 4817
	public sealed class HideAndSeek : Behaviour
	{
		// Token: 0x06005F4E RID: 24398 RVA: 0x00117161 File Offset: 0x00115361
		public override IEnumerator CRun(AIController controller)
		{
			HideAndSeek.<>c__DisplayClass1_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.character = controller.character;
			CS$<>8__locals1.target = controller.target;
			if (CS$<>8__locals1.target == null)
			{
				base.result = Behaviour.Result.Fail;
				yield break;
			}
			CS$<>8__locals1.targetCollisionState = CS$<>8__locals1.target.movement.controller.collisionState;
			base.result = Behaviour.Result.Doing;
			while (base.result == Behaviour.Result.Doing)
			{
				if (CS$<>8__locals1.target == null)
				{
					base.result = Behaviour.Result.Fail;
					break;
				}
				if (!this.<CRun>g__CanChase|1_0(ref CS$<>8__locals1))
				{
					yield return null;
				}
				else if (controller.FindClosestPlayerBody(controller.stopTrigger))
				{
					if (!this._attackReady.TryStart())
					{
						yield return null;
					}
					else
					{
						while (this._attackReady.running && base.result == Behaviour.Result.Doing)
						{
							yield return null;
							if (!this.<CRun>g__CanChase|1_0(ref CS$<>8__locals1))
							{
								CS$<>8__locals1.character.CancelAction();
								base.result = Behaviour.Result.Fail;
								break;
							}
						}
						if (base.result != Behaviour.Result.Fail)
						{
							base.result = Behaviour.Result.Success;
							break;
						}
						base.result = Behaviour.Result.Doing;
					}
				}
				else
				{
					float num = controller.target.transform.position.x - CS$<>8__locals1.character.transform.position.x;
					CS$<>8__locals1.character.movement.move = ((num > 0f) ? Vector2.right : Vector2.left);
					yield return null;
				}
			}
			yield break;
		}

		// Token: 0x06005F4F RID: 24399 RVA: 0x00117177 File Offset: 0x00115377
		private bool isFacingEachOther(Character character, Character target)
		{
			character.ForceToLookAt(target.transform.position.x);
			return (character.lookingDirection == Character.LookingDirection.Right && target.lookingDirection == Character.LookingDirection.Left) || (character.lookingDirection == Character.LookingDirection.Left && target.lookingDirection == Character.LookingDirection.Right);
		}

		// Token: 0x06005F51 RID: 24401 RVA: 0x001171B8 File Offset: 0x001153B8
		[CompilerGenerated]
		private bool <CRun>g__CanChase|1_0(ref HideAndSeek.<>c__DisplayClass1_0 A_1)
		{
			return !(A_1.targetCollisionState.lastStandingCollider == null) && Precondition.CanChase(A_1.character, A_1.target) && !this.isFacingEachOther(A_1.character, A_1.target);
		}

		// Token: 0x04004C92 RID: 19602
		[SerializeField]
		private Characters.Actions.Action _attackReady;
	}
}
