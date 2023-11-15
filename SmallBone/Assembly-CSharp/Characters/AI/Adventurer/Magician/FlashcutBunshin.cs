using System;
using System.Collections;
using Characters.Actions;
using UnityEngine;

namespace Characters.AI.Adventurer.Magician
{
	// Token: 0x020013F9 RID: 5113
	public class FlashcutBunshin : MonoBehaviour
	{
		// Token: 0x060064B2 RID: 25778 RVA: 0x00123F64 File Offset: 0x00122164
		private void OnEnable()
		{
			base.StartCoroutine(this.CAttack());
		}

		// Token: 0x060064B3 RID: 25779 RVA: 0x00123F73 File Offset: 0x00122173
		private IEnumerator CAttack()
		{
			Collider2D lastStandingCollider = this._character.movement.controller.collisionState.lastStandingCollider;
			while (lastStandingCollider == null)
			{
				yield return null;
				lastStandingCollider = this._character.movement.controller.collisionState.lastStandingCollider;
			}
			this._character.ForceToLookAt(lastStandingCollider.bounds.center.x);
			this._action.TryStart();
			while (this._action.running)
			{
				yield return null;
			}
			base.gameObject.SetActive(false);
			yield break;
		}

		// Token: 0x04005132 RID: 20786
		[SerializeField]
		private Character _character;

		// Token: 0x04005133 RID: 20787
		[SerializeField]
		private GameObject _body;

		// Token: 0x04005134 RID: 20788
		[SerializeField]
		private Characters.Actions.Action _action;
	}
}
