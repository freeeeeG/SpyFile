using System;
using UnityEngine;

// Token: 0x02000137 RID: 311
public class GarbageCollectionManager : MonoBehaviour
{
	// Token: 0x060005A9 RID: 1449 RVA: 0x0002AB07 File Offset: 0x00028F07
	private void Update()
	{
		if (Time.frameCount % this.m_framesBeforeCollection == 0)
		{
			GC.Collect();
		}
	}

	// Token: 0x040004B8 RID: 1208
	[SerializeField]
	private int m_framesBeforeCollection = 30;
}
