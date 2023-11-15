using System;
using UnityEngine;

// Token: 0x020000F1 RID: 241
public class RatManager : Manager
{
	// Token: 0x04000403 RID: 1027
	[SerializeField]
	private GameObject m_ratPrefab;

	// Token: 0x04000404 RID: 1028
	[SerializeField]
	private float m_ratSpawnTime = 5f;

	// Token: 0x04000405 RID: 1029
	[SerializeField]
	private float m_droppedIngredientCooldownTime = 2f;

	// Token: 0x04000406 RID: 1030
	[SerializeField]
	private float m_minimumSpawnDistance = 5f;
}
