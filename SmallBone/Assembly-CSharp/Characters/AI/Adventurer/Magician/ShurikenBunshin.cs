using System;
using System.Collections;
using Characters.Actions;
using UnityEngine;

namespace Characters.AI.Adventurer.Magician
{
	// Token: 0x020013FB RID: 5115
	public class ShurikenBunshin : MonoBehaviour
	{
		// Token: 0x060064BB RID: 25787 RVA: 0x00124088 File Offset: 0x00122288
		private void OnEnable()
		{
			base.StartCoroutine(this.CAttack());
		}

		// Token: 0x060064BC RID: 25788 RVA: 0x00124097 File Offset: 0x00122297
		private IEnumerator CAttack()
		{
			Collider2D lastStandingCollider = this._character.movement.controller.collisionState.lastStandingCollider;
			while (lastStandingCollider == null)
			{
				yield return null;
				lastStandingCollider = this._character.movement.controller.collisionState.lastStandingCollider;
			}
			this._character.ForceToLookAt(lastStandingCollider.bounds.center.x);
			this._jump.TryStart();
			while (this._character.movement.velocity.y > 0f)
			{
				yield return null;
			}
			while (this._jump.running)
			{
				yield return null;
			}
			this._attack.TryStart();
			while (this._attack.running)
			{
				yield return null;
			}
			base.gameObject.SetActive(false);
			yield break;
		}

		// Token: 0x04005138 RID: 20792
		[SerializeField]
		private Character _character;

		// Token: 0x04005139 RID: 20793
		[SerializeField]
		private GameObject _base;

		// Token: 0x0400513A RID: 20794
		[SerializeField]
		private Characters.Actions.Action _jump;

		// Token: 0x0400513B RID: 20795
		[SerializeField]
		private Characters.Actions.Action _attack;
	}
}
