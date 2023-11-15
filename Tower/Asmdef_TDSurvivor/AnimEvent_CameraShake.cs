using System;
using UnityEngine;

// Token: 0x02000195 RID: 405
public class AnimEvent_CameraShake : MonoBehaviour
{
	// Token: 0x06000AE0 RID: 2784 RVA: 0x00028F45 File Offset: 0x00027145
	public void Anim_TriggerShake(CameraManager.eCameraShakeStrength strength)
	{
		EventMgr.SendEvent<CameraManager.eCameraShakeStrength>(eGameEvents.RequestCameraShake, strength);
	}
}
