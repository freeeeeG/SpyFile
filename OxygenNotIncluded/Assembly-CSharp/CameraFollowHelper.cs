using System;
using UnityEngine;

// Token: 0x02000488 RID: 1160
[AddComponentMenu("KMonoBehaviour/scripts/CameraFollowHelper")]
public class CameraFollowHelper : KMonoBehaviour
{
	// Token: 0x060019EA RID: 6634 RVA: 0x00089676 File Offset: 0x00087876
	private void LateUpdate()
	{
		if (CameraController.Instance != null)
		{
			CameraController.Instance.UpdateFollowTarget();
		}
	}
}
