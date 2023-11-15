using System;
using UnityEngine;

// Token: 0x020009DD RID: 2525
public class ClientDynamicLandscapeParenting : MonoBehaviour
{
	// Token: 0x0600315A RID: 12634 RVA: 0x000E75F2 File Offset: 0x000E59F2
	private void Awake()
	{
		if (GameUtils.GetLevelConfig().m_disableDynamicParenting)
		{
			UnityEngine.Object.Destroy(this);
			return;
		}
	}

	// Token: 0x040027A2 RID: 10146
	private DynamicLandscapeParenting m_dynamicLandscapeParenting;

	// Token: 0x040027A3 RID: 10147
	private GroundCast m_groundCast;
}
