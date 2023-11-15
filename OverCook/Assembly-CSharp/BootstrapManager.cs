using System;
using UnityEngine;

// Token: 0x020004F8 RID: 1272
public class BootstrapManager : Manager
{
	// Token: 0x1700024C RID: 588
	// (get) Token: 0x060017BA RID: 6074 RVA: 0x0007911D File Offset: 0x0007751D
	public GameObject GameSessionPrefab
	{
		get
		{
			return this.m_gameSessionPrefab;
		}
	}

	// Token: 0x060017BB RID: 6075 RVA: 0x00079125 File Offset: 0x00077525
	private void Awake()
	{
		this.EnsureSetup();
	}

	// Token: 0x060017BC RID: 6076 RVA: 0x00079130 File Offset: 0x00077530
	public virtual void EnsureSetup()
	{
		if (GameUtils.GetGameMetaEnvironment() == null)
		{
			GameObject gameObject = this.m_gameMetaEnvironmentPrefab.InstantiateOnParent(null, true);
			for (int i = 0; i < gameObject.transform.childCount; i++)
			{
				gameObject.transform.GetChild(i).gameObject.SendMessage("BootstrapAwake", SendMessageOptions.DontRequireReceiver);
			}
		}
		if (GameUtils.GetGameSession() == null && this.m_gameSessionPrefab != null)
		{
			this.m_gameSessionPrefab.InstantiateOnParent(null, true);
			base.StartCoroutine(GameUtils.GetGameSession().LoadSession());
		}
	}

	// Token: 0x04001165 RID: 4453
	[SerializeField]
	private GameObject m_gameMetaEnvironmentPrefab;

	// Token: 0x04001166 RID: 4454
	[SerializeField]
	private GameObject m_gameSessionPrefab;
}
