using System;
using UnityEngine;

// Token: 0x0200079A RID: 1946
[ExecuteInEditMode]
public class MaterialTimeController : MonoBehaviour
{
	// Token: 0x06002599 RID: 9625 RVA: 0x000B1D7B File Offset: 0x000B017B
	private void Start()
	{
		this.m_TimeID = Shader.PropertyToID("_ControlledTime");
	}

	// Token: 0x0600259A RID: 9626 RVA: 0x000B1D8D File Offset: 0x000B018D
	private void Update()
	{
		if (!TimeManager.IsPaused(TimeManager.PauseLayer.Main))
		{
			this.m_controlledTime += ClientTime.DeltaTime();
		}
		Shader.SetGlobalFloat(this.m_TimeID, this.m_controlledTime);
	}

	// Token: 0x04001D28 RID: 7464
	private int m_TimeID;

	// Token: 0x04001D29 RID: 7465
	private float m_controlledTime;
}
