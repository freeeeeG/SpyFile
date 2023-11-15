using System;
using UnityEngine;

// Token: 0x02000385 RID: 901
public class BlenderCosmeticDecisions : MonoBehaviour
{
	// Token: 0x0600110C RID: 4364 RVA: 0x00061B12 File Offset: 0x0005FF12
	private void Awake()
	{
		if (this.m_animator != null)
		{
			this.m_hasOnParam = this.m_animator.HasParameter(BlenderCosmeticDecisions.c_onParam);
			this.m_hasFillParam = this.m_animator.HasParameter(BlenderCosmeticDecisions.c_FillParam);
		}
	}

	// Token: 0x0600110D RID: 4365 RVA: 0x00061B51 File Offset: 0x0005FF51
	private void Start()
	{
		if (this.m_prefabLookup != null)
		{
			this.m_prefabLookup.CacheAssembledOrderNodes();
		}
	}

	// Token: 0x04000D32 RID: 3378
	[SerializeField]
	public GameObject m_contentsObject;

	// Token: 0x04000D33 RID: 3379
	[SerializeField]
	public OrderToPrefabLookup m_prefabLookup;

	// Token: 0x04000D34 RID: 3380
	[SerializeField]
	public Animator m_animator;

	// Token: 0x04000D35 RID: 3381
	[SerializeField]
	public string m_surfaceMaterialName;

	// Token: 0x04000D36 RID: 3382
	[HideInInspector]
	public bool m_hasOnParam;

	// Token: 0x04000D37 RID: 3383
	[HideInInspector]
	public bool m_hasFillParam;

	// Token: 0x04000D38 RID: 3384
	public static int c_onParam = Animator.StringToHash("On");

	// Token: 0x04000D39 RID: 3385
	public static int c_FillParam = Animator.StringToHash("Fill");
}
