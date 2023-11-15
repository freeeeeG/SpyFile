using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameModes.Horde
{
	// Token: 0x020007C9 RID: 1993
	public class HordeEnemy : MonoBehaviour
	{
		// Token: 0x04001E35 RID: 7733
		[FormerlySerializedAs("m_attackFrequencySeconds")]
		[SerializeField]
		public float m_attackTargetFrequencySeconds = 1f;

		// Token: 0x04001E36 RID: 7734
		[SerializeField]
		public int m_targetDamage = 25;

		// Token: 0x04001E37 RID: 7735
		[SerializeField]
		public float m_attackKitchenFrequencySeconds = 1f;

		// Token: 0x04001E38 RID: 7736
		[SerializeField]
		public int m_kitchenDamage = 10;

		// Token: 0x04001E39 RID: 7737
		[SerializeField]
		public int m_recipeCount = 1;

		// Token: 0x04001E3A RID: 7738
		[SerializeField]
		public float m_movementSpeed = 1f;

		// Token: 0x04001E3B RID: 7739
		[SerializeField]
		public AnimationCurve m_movementCurve = AnimationCurve.Linear(0f, 1f, 1f, 1f);
	}
}
