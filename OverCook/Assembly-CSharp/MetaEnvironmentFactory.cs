using System;
using UnityEngine;

// Token: 0x02000512 RID: 1298
public class MetaEnvironmentFactory : MonoBehaviour
{
	// Token: 0x06001838 RID: 6200 RVA: 0x0007AF2E File Offset: 0x0007932E
	public void Awake()
	{
		if (GameUtils.GetGameMetaEnvironment() == null)
		{
			this.m_gameMetaEnvironmentPrefab.InstantiateOnParent(null, true);
		}
	}

	// Token: 0x04001379 RID: 4985
	[SerializeField]
	private GameObject m_gameMetaEnvironmentPrefab;
}
