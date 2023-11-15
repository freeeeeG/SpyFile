using System;
using UnityEngine;

// Token: 0x020005D5 RID: 1493
[RequireComponent(typeof(Teleportal))]
public class TeleportalPlayerSender : BaseTeleportalSender
{
	// Token: 0x06001C7E RID: 7294 RVA: 0x0008B474 File Offset: 0x00089874
	public void RegisterAnimationFinishedCallback(GenericVoid<string> _callback)
	{
		this.m_animFinishedCallback = (GenericVoid<string>)Delegate.Combine(this.m_animFinishedCallback, _callback);
	}

	// Token: 0x06001C7F RID: 7295 RVA: 0x0008B48D File Offset: 0x0008988D
	public void DeregisterAnimationFinishedCallback(GenericVoid<string> _callback)
	{
		this.m_animFinishedCallback = (GenericVoid<string>)Delegate.Remove(this.m_animFinishedCallback, _callback);
	}

	// Token: 0x06001C80 RID: 7296 RVA: 0x0008B4A6 File Offset: 0x000898A6
	public void OnAnimationFinished(string _animName)
	{
		this.m_animFinishedCallback(_animName);
	}

	// Token: 0x04001648 RID: 5704
	private GenericVoid<string> m_animFinishedCallback = delegate(string _animName)
	{
	};

	// Token: 0x04001649 RID: 5705
	public ManualAnimation m_SenderAnimation;
}
