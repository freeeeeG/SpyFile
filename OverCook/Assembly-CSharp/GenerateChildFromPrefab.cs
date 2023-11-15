using System;
using UnityEngine;

// Token: 0x02000138 RID: 312
[ExecuteInEditMode]
public class GenerateChildFromPrefab : MonoBehaviour
{
	// Token: 0x060005AB RID: 1451 RVA: 0x0002AB27 File Offset: 0x00028F27
	private void OnEnable()
	{
		if (this.m_childPrefab != null && this.m_childInstance == null)
		{
			this.m_childInstance = this.m_childPrefab.InstantiateOnParent(base.transform, true);
		}
	}

	// Token: 0x060005AC RID: 1452 RVA: 0x0002AB63 File Offset: 0x00028F63
	private void OnDisable()
	{
		if (this.m_childInstance != null && !Application.isPlaying)
		{
			UnityEngine.Object.DestroyImmediate(this.m_childInstance);
		}
	}

	// Token: 0x040004B9 RID: 1209
	[SerializeField]
	private GameObject m_childPrefab;

	// Token: 0x040004BA RID: 1210
	[SerializeField]
	[HideInInspector]
	private GameObject m_childInstance;
}
