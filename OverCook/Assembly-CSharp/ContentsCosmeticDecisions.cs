using System;
using UnityEngine;

// Token: 0x020003A1 RID: 929
public class ContentsCosmeticDecisions : AnimationInspectionBase
{
	// Token: 0x0600116E RID: 4462 RVA: 0x00063C3A File Offset: 0x0006203A
	private void Awake()
	{
		if (this.m_animator != null)
		{
			this.m_hasOnParam = this.m_animator.HasParameter(ContentsCosmeticDecisions.c_onParam);
			this.m_hasProgressParam = this.m_animator.HasParameter(ContentsCosmeticDecisions.c_progressParam);
		}
	}

	// Token: 0x0600116F RID: 4463 RVA: 0x00063C79 File Offset: 0x00062079
	private void Start()
	{
		if (this.m_prefabLookup != null)
		{
			this.m_prefabLookup.CacheAssembledOrderNodes();
		}
	}

	// Token: 0x04000D8A RID: 3466
	[SerializeField]
	public GameObject m_gameObject;

	// Token: 0x04000D8B RID: 3467
	[SerializeField]
	public GameObject m_contentsObject;

	// Token: 0x04000D8C RID: 3468
	[SerializeField]
	public OrderToPrefabLookup m_prefabLookup;

	// Token: 0x04000D8D RID: 3469
	[SerializeField]
	public string m_surfaceMaterialName;

	// Token: 0x04000D8E RID: 3470
	[SerializeField]
	public string m_bubbleMaterialName;

	// Token: 0x04000D8F RID: 3471
	[SerializeField]
	public float m_contentsYPositionWhenFull = 0.2f;

	// Token: 0x04000D90 RID: 3472
	[SerializeField]
	public float m_contentsYPositionWhenEmpty;

	// Token: 0x04000D91 RID: 3473
	[SerializeField]
	public ContentsCosmeticDecisions.ContentsScale m_contentsScale = new ContentsCosmeticDecisions.ContentsScale();

	// Token: 0x04000D92 RID: 3474
	[HideInInspector]
	public bool m_hasOnParam;

	// Token: 0x04000D93 RID: 3475
	[HideInInspector]
	public bool m_hasProgressParam;

	// Token: 0x04000D94 RID: 3476
	public static int c_onParam = Animator.StringToHash("On");

	// Token: 0x04000D95 RID: 3477
	public static int c_progressParam = Animator.StringToHash("Progress");

	// Token: 0x020003A2 RID: 930
	[Serializable]
	public class ContentsScale
	{
		// Token: 0x04000D96 RID: 3478
		[SerializeField]
		public Vector3 m_empty = new Vector3(1f, 1f, 1f);

		// Token: 0x04000D97 RID: 3479
		[SerializeField]
		public Vector3 m_full = new Vector3(1f, 1f, 1f);

		// Token: 0x04000D98 RID: 3480
		[SerializeField]
		public AnimationCurve m_curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
	}
}
