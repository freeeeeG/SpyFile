using System;
using UnityEngine;

namespace Characters.Abilities.Customs
{
	// Token: 0x02000D59 RID: 3417
	public class GraveDiggerPassiveComponent : AbilityComponent<GraveDiggerPassive>
	{
		// Token: 0x060044F1 RID: 17649 RVA: 0x000C855B File Offset: 0x000C675B
		public void SpawnGrave(Vector3 position)
		{
			base.baseAbility.SpawnGrave(position);
		}

		// Token: 0x060044F2 RID: 17650 RVA: 0x000C8569 File Offset: 0x000C6769
		public void SpawnCorpse(Vector3 position)
		{
			base.baseAbility.SpawnCorpse(position);
		}

		// Token: 0x060044F3 RID: 17651 RVA: 0x000C8577 File Offset: 0x000C6777
		private void OnDestroy()
		{
			base.baseAbility.OnDestroy();
		}
	}
}
