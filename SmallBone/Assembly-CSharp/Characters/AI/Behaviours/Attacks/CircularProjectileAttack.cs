using System;
using System.Collections;
using UnityEngine;

namespace Characters.AI.Behaviours.Attacks
{
	// Token: 0x020013DE RID: 5086
	public class CircularProjectileAttack : ActionAttack
	{
		// Token: 0x0600642E RID: 25646 RVA: 0x00122A87 File Offset: 0x00120C87
		private void Awake()
		{
			this._originalScale = Vector3.one;
		}

		// Token: 0x0600642F RID: 25647 RVA: 0x00122A94 File Offset: 0x00120C94
		public override IEnumerator CRun(AIController controller)
		{
			Character character = controller.character;
			Character target = controller.target;
			base.result = Behaviour.Result.Doing;
			Vector3 vector = target.transform.position - character.transform.position;
			character.lookingDirection = ((vector.x > 0f) ? Character.LookingDirection.Right : Character.LookingDirection.Left);
			Vector3 vector2 = target.transform.position - character.transform.position;
			float num = Mathf.Atan2(vector2.y, vector2.x) * 57.29578f;
			Vector3 originalScale = this._originalScale;
			Vector3 originalScale2 = this._originalScale;
			if ((num > 90f && num < 270f) || num < -90f)
			{
				originalScale.x *= -1f;
				originalScale2.y *= -1f;
				originalScale2.x *= -1f;
			}
			this._weaponAxisPosition.localScale = originalScale;
			this._centerAxisPosition.localScale = originalScale2;
			if (this._autoAim)
			{
				yield return this.TakeAim(character, target);
			}
			if (this.attack.TryStart())
			{
				while (this.attack.running)
				{
					yield return null;
					if (this._continuousLooking)
					{
						vector = target.transform.position - character.transform.position;
						character.lookingDirection = ((vector.x > 0f) ? Character.LookingDirection.Right : Character.LookingDirection.Left);
						vector2 = target.transform.position - character.transform.position;
						num = Mathf.Atan2(vector2.y, vector2.x) * 57.29578f;
						originalScale = this._originalScale;
						originalScale2 = this._originalScale;
						if ((num > 90f && num < 270f) || num < -90f)
						{
							originalScale.x *= -1f;
							originalScale2.y *= -1f;
							originalScale2.x *= -1f;
						}
						this._weaponAxisPosition.localScale = originalScale;
						this._centerAxisPosition.localScale = originalScale2;
					}
				}
				yield return this.idle.CRun(controller);
			}
			yield break;
		}

		// Token: 0x06006430 RID: 25648 RVA: 0x00122AAA File Offset: 0x00120CAA
		private IEnumerator TakeAim(Character character, Character target)
		{
			while (this.attack.running)
			{
				Vector3 vector = target.transform.position - character.transform.position;
				float num = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
				this._centerAxisPosition.rotation = Quaternion.Euler(0f, 0f, this._originalDirection + num);
				yield return null;
			}
			yield break;
		}

		// Token: 0x040050CA RID: 20682
		private Vector3 _originalScale;

		// Token: 0x040050CB RID: 20683
		private float _originalDirection;

		// Token: 0x040050CC RID: 20684
		[SerializeField]
		private Transform _centerAxisPosition;

		// Token: 0x040050CD RID: 20685
		[SerializeField]
		private Transform _weaponAxisPosition;

		// Token: 0x040050CE RID: 20686
		[SerializeField]
		private bool _continuousLooking;

		// Token: 0x040050CF RID: 20687
		[SerializeField]
		private bool _autoAim;
	}
}
