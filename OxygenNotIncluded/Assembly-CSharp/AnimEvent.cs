using System;
using UnityEngine;

// Token: 0x0200043C RID: 1084
[Serializable]
public class AnimEvent
{
	// Token: 0x170000A0 RID: 160
	// (get) Token: 0x060016E4 RID: 5860 RVA: 0x000768C3 File Offset: 0x00074AC3
	// (set) Token: 0x060016E5 RID: 5861 RVA: 0x000768CB File Offset: 0x00074ACB
	[SerializeField]
	public string name { get; private set; }

	// Token: 0x170000A1 RID: 161
	// (get) Token: 0x060016E6 RID: 5862 RVA: 0x000768D4 File Offset: 0x00074AD4
	// (set) Token: 0x060016E7 RID: 5863 RVA: 0x000768DC File Offset: 0x00074ADC
	[SerializeField]
	public string file { get; private set; }

	// Token: 0x170000A2 RID: 162
	// (get) Token: 0x060016E8 RID: 5864 RVA: 0x000768E5 File Offset: 0x00074AE5
	// (set) Token: 0x060016E9 RID: 5865 RVA: 0x000768ED File Offset: 0x00074AED
	[SerializeField]
	public int frame { get; private set; }

	// Token: 0x060016EA RID: 5866 RVA: 0x000768F6 File Offset: 0x00074AF6
	public AnimEvent()
	{
	}

	// Token: 0x060016EB RID: 5867 RVA: 0x00076900 File Offset: 0x00074B00
	public AnimEvent(string file, string name, int frame)
	{
		this.file = ((file == "") ? null : file);
		if (this.file != null)
		{
			this.fileHash = new KAnimHashedString(this.file);
		}
		this.name = name;
		this.frame = frame;
	}

	// Token: 0x060016EC RID: 5868 RVA: 0x00076954 File Offset: 0x00074B54
	public void Play(AnimEventManager.EventPlayerData behaviour)
	{
		if (this.IsFilteredOut(behaviour))
		{
			return;
		}
		if (behaviour.previousFrame < behaviour.currentFrame)
		{
			if (behaviour.previousFrame < this.frame && behaviour.currentFrame >= this.frame)
			{
				this.OnPlay(behaviour);
				return;
			}
		}
		else if (behaviour.previousFrame > behaviour.currentFrame && (behaviour.previousFrame < this.frame || this.frame <= behaviour.currentFrame))
		{
			this.OnPlay(behaviour);
		}
	}

	// Token: 0x060016ED RID: 5869 RVA: 0x000769D6 File Offset: 0x00074BD6
	private void DebugAnimEvent(string ev_name, AnimEventManager.EventPlayerData behaviour)
	{
	}

	// Token: 0x060016EE RID: 5870 RVA: 0x000769D8 File Offset: 0x00074BD8
	public virtual void OnPlay(AnimEventManager.EventPlayerData behaviour)
	{
	}

	// Token: 0x060016EF RID: 5871 RVA: 0x000769DA File Offset: 0x00074BDA
	public virtual void OnUpdate(AnimEventManager.EventPlayerData behaviour)
	{
	}

	// Token: 0x060016F0 RID: 5872 RVA: 0x000769DC File Offset: 0x00074BDC
	public virtual void Stop(AnimEventManager.EventPlayerData behaviour)
	{
	}

	// Token: 0x060016F1 RID: 5873 RVA: 0x000769DE File Offset: 0x00074BDE
	protected bool IsFilteredOut(AnimEventManager.EventPlayerData behaviour)
	{
		return this.file != null && !behaviour.controller.HasAnimationFile(this.fileHash);
	}

	// Token: 0x04000CB7 RID: 3255
	[SerializeField]
	private KAnimHashedString fileHash;

	// Token: 0x04000CB9 RID: 3257
	public bool OnExit;
}
