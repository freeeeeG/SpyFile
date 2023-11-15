using System;
using System.Collections;
using UnityEngine;

namespace Characters.AI.Behaviours.Attacks
{
	// Token: 0x020013E1 RID: 5089
	public class HorizontalProjectileAttack : ActionAttack
	{
		// Token: 0x0600643E RID: 25662 RVA: 0x00122EA8 File Offset: 0x001210A8
		private void Awake()
		{
			this._originalScale = base.transform.localScale;
			this._originalDircetion = base.transform.rotation.eulerAngles.z;
		}

		// Token: 0x0600643F RID: 25663 RVA: 0x00122EE4 File Offset: 0x001210E4
		public override IEnumerator CRun(AIController controller)
		{
			Character character = controller.character;
			base.result = Behaviour.Result.Doing;
			Vector3 vector = controller.target.transform.position - character.transform.position;
			float x = vector.x;
			if (vector.x < 0f)
			{
				character.lookingDirection = Character.LookingDirection.Left;
				Vector3 originalScale = this._originalScale;
				originalScale.y *= -1f;
				this._weapon.localScale = originalScale * -1f;
			}
			else
			{
				character.lookingDirection = Character.LookingDirection.Right;
				this._weapon.localScale = this._originalScale;
			}
			if (this.attack.TryStart())
			{
				this.gaveDamage = false;
				yield return this.attack.CWaitForEndOfRunning();
				if (!this.gaveDamage)
				{
					base.result = Behaviour.Result.Success;
					yield return character.chronometer.animation.WaitForSeconds(1.5f);
				}
				else
				{
					base.result = Behaviour.Result.Fail;
				}
			}
			else
			{
				base.result = Behaviour.Result.Fail;
			}
			yield break;
		}

		// Token: 0x040050DB RID: 20699
		[SerializeField]
		private Transform _weapon;

		// Token: 0x040050DC RID: 20700
		private Vector3 _originalScale;

		// Token: 0x040050DD RID: 20701
		private float _originalDircetion;
	}
}
