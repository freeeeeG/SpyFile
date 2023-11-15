using System;
using System.Collections;
using UnityEngine;

namespace Characters.AI.Behaviours.Attacks
{
	// Token: 0x020013E3 RID: 5091
	public class MultiCircularProjectileAttack : ActionAttack
	{
		// Token: 0x17001400 RID: 5120
		// (get) Token: 0x06006447 RID: 25671 RVA: 0x00123084 File Offset: 0x00121284
		// (set) Token: 0x06006448 RID: 25672 RVA: 0x0012308C File Offset: 0x0012128C
		public Vector3 lookTarget { get; set; }

		// Token: 0x06006449 RID: 25673 RVA: 0x00123095 File Offset: 0x00121295
		private void Awake()
		{
			this._originalScale = Vector3.one;
		}

		// Token: 0x0600644A RID: 25674 RVA: 0x001230A2 File Offset: 0x001212A2
		public override IEnumerator CRun(AIController controller)
		{
			Character character = controller.character;
			Vector3 lookTarget = this.lookTarget;
			Vector3 lookTarget2 = this.lookTarget;
			base.result = Behaviour.Result.Doing;
			character.ForceToLookAt(lookTarget2.x);
			Vector3 vector = lookTarget2 - character.transform.position;
			float num = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
			Vector3 originalScale = this._originalScale;
			if ((num > 90f && num < 270f) || num < -90f)
			{
				originalScale.x *= -1f;
			}
			this._weaponAxisPosition.localScale = originalScale;
			foreach (Transform transform in this._centerAxisPositions)
			{
				Vector3 originalScale2 = this._originalScale;
				if ((num > 90f && num < 270f) || num < -90f)
				{
					originalScale2.y *= -1f;
					originalScale2.x *= -1f;
				}
				transform.localScale = originalScale2;
			}
			if (this.attack.TryStart())
			{
				while (this.attack.running)
				{
					yield return null;
				}
				yield return this.idle.CRun(controller);
			}
			yield break;
		}

		// Token: 0x040050E3 RID: 20707
		private Vector3 _originalScale;

		// Token: 0x040050E4 RID: 20708
		[SerializeField]
		private Transform[] _centerAxisPositions;

		// Token: 0x040050E5 RID: 20709
		[SerializeField]
		private Transform _weaponAxisPosition;
	}
}
