using System;
using System.Collections;
using System.Collections.Generic;
using Characters;
using Characters.Actions;
using Characters.Operations;
using PhysicsUtils;
using UnityEditor;
using UnityEngine;

namespace Level.Traps
{
	// Token: 0x02000662 RID: 1634
	public class JumpPlatform : MonoBehaviour
	{
		// Token: 0x060020CA RID: 8394 RVA: 0x000630E0 File Offset: 0x000612E0
		private void Start()
		{
			this._operationToTarget.Initialize();
			base.StartCoroutine(this.CRun());
		}

		// Token: 0x060020CB RID: 8395 RVA: 0x000630FA File Offset: 0x000612FA
		private IEnumerator CRun()
		{
			for (;;)
			{
				yield return null;
				JumpPlatform._lapper.contactFilter.SetLayerMask(this._targetLayer);
				ReadonlyBoundedList<Collider2D> results = JumpPlatform._lapper.OverlapCollider(this._trigger).results;
				if (results.Count > 0)
				{
					using (IEnumerator<Collider2D> enumerator = results.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							Character character;
							if (enumerator.Current.TryFindCharacterComponent(out character) && !(character == null) && this.SteppedOn(character))
							{
								this.Jump(character);
							}
						}
						continue;
					}
					yield break;
				}
			}
		}

		// Token: 0x060020CC RID: 8396 RVA: 0x00063109 File Offset: 0x00061309
		private bool SteppedOn(Character character)
		{
			return !(character.movement.controller.collisionState.lastStandingCollider != this._jumpPlatform) && character.movement.isGrounded;
		}

		// Token: 0x060020CD RID: 8397 RVA: 0x0006313F File Offset: 0x0006133F
		private void Jump(Character target)
		{
			this._jumpAction.TryStart();
			base.StartCoroutine(this._operationToTarget.CRun(this._character, target));
		}

		// Token: 0x04001BD8 RID: 7128
		[SerializeField]
		private Character _character;

		// Token: 0x04001BD9 RID: 7129
		[SerializeField]
		private Characters.Actions.Action _jumpAction;

		// Token: 0x04001BDA RID: 7130
		[SerializeField]
		private Collider2D _trigger;

		// Token: 0x04001BDB RID: 7131
		[SerializeField]
		private LayerMask _targetLayer;

		// Token: 0x04001BDC RID: 7132
		[SerializeField]
		private Collider2D _jumpPlatform;

		// Token: 0x04001BDD RID: 7133
		[SerializeField]
		[Subcomponent(typeof(TargetedOperationInfo))]
		private TargetedOperationInfo.Subcomponents _operationToTarget;

		// Token: 0x04001BDE RID: 7134
		private static readonly NonAllocOverlapper _lapper = new NonAllocOverlapper(15);
	}
}
