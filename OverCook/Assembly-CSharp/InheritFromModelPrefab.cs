using System;
using UnityEngine;

// Token: 0x0200013B RID: 315
[AddComponentMenu("Scripts/Core/Components/InheritFromModelPrefab")]
public class InheritFromModelPrefab : MonoBehaviour
{
	// Token: 0x060005B3 RID: 1459 RVA: 0x0002AB93 File Offset: 0x00028F93
	public GameObject GetParentPrefab()
	{
		return this.m_prefab;
	}

	// Token: 0x040004BB RID: 1211
	[SerializeField]
	private GameObject m_prefab;
}
