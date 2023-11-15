using System;
using UnityEngine;

// Token: 0x020007E8 RID: 2024
[RequireComponent(typeof(PhysicalAttachment))]
public class AttachmentWindReceiver : WindAccumulator
{
	// Token: 0x060026EF RID: 9967 RVA: 0x000B8B77 File Offset: 0x000B6F77
	public override void RemoveWindSource(IWindSource _source)
	{
		base.RemoveWindSource(_source);
		this.m_volumeExitedCallback(_source);
	}

	// Token: 0x060026F0 RID: 9968 RVA: 0x000B8B8C File Offset: 0x000B6F8C
	public void RegisterVolumeExitedCallback(VoidGeneric<IWindSource> _callback)
	{
		this.m_volumeExitedCallback = (VoidGeneric<IWindSource>)Delegate.Combine(this.m_volumeExitedCallback, _callback);
	}

	// Token: 0x060026F1 RID: 9969 RVA: 0x000B8BA5 File Offset: 0x000B6FA5
	public void UnregisterVolumeExitedCallback(VoidGeneric<IWindSource> _callback)
	{
		this.m_volumeExitedCallback = (VoidGeneric<IWindSource>)Delegate.Remove(this.m_volumeExitedCallback, _callback);
	}

	// Token: 0x04001ECC RID: 7884
	private VoidGeneric<IWindSource> m_volumeExitedCallback = delegate(IWindSource _source)
	{
	};
}
