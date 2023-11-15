using System;
using UnityEngine;

// Token: 0x0200078F RID: 1935
public class TutorialPopupController : MonoBehaviour
{
	// Token: 0x06002569 RID: 9577 RVA: 0x000B12B5 File Offset: 0x000AF6B5
	public void RegisterDismissCallback(CallbackVoid _callback)
	{
		this.m_dismissedCallback = (CallbackVoid)Delegate.Combine(this.m_dismissedCallback, _callback);
	}

	// Token: 0x0600256A RID: 9578 RVA: 0x000B12CE File Offset: 0x000AF6CE
	public void DeregisterDismissCallback(CallbackVoid _callback)
	{
		this.m_dismissedCallback = (CallbackVoid)Delegate.Remove(this.m_dismissedCallback, _callback);
	}

	// Token: 0x0600256B RID: 9579 RVA: 0x000B12E7 File Offset: 0x000AF6E7
	public void OnTutorialDismissed()
	{
		this.m_dismissedCallback();
	}

	// Token: 0x04001CF8 RID: 7416
	private CallbackVoid m_dismissedCallback = delegate()
	{
	};
}
