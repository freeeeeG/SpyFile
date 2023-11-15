using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x0200065E RID: 1630
public class ServerCutsceneController : ServerSynchroniserBase
{
	// Token: 0x06001F0B RID: 7947 RVA: 0x00097805 File Offset: 0x00095C05
	public override EntityType GetEntityType()
	{
		return EntityType.Cutscene;
	}

	// Token: 0x06001F0C RID: 7948 RVA: 0x00097809 File Offset: 0x00095C09
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_controller = (CutsceneController)synchronisedObject;
		this.m_controller.RegisterSkipCallback(new CallbackVoid(this.OnCutsceneSkipped));
	}

	// Token: 0x06001F0D RID: 7949 RVA: 0x00097835 File Offset: 0x00095C35
	private void OnCutsceneSkipped()
	{
		this.SendServerEvent(this.m_data);
	}

	// Token: 0x040017B8 RID: 6072
	private CutsceneController m_controller;

	// Token: 0x040017B9 RID: 6073
	private readonly CutsceneStateMessage m_data = new CutsceneStateMessage();
}
