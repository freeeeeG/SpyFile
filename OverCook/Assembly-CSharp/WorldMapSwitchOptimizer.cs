using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000BF5 RID: 3061
public class WorldMapSwitchOptimizer : MonoBehaviour, ITileFlipAnimatorProvider
{
	// Token: 0x14000039 RID: 57
	// (add) Token: 0x06003E78 RID: 15992 RVA: 0x0012B158 File Offset: 0x00129558
	// (remove) Token: 0x06003E79 RID: 15993 RVA: 0x0012B190 File Offset: 0x00129590
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private event CallbackVoid m_OnSwitchFlipBegin = delegate()
	{
	};

	// Token: 0x1400003A RID: 58
	// (add) Token: 0x06003E7A RID: 15994 RVA: 0x0012B1C8 File Offset: 0x001295C8
	// (remove) Token: 0x06003E7B RID: 15995 RVA: 0x0012B200 File Offset: 0x00129600
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private event CallbackVoid m_OnSwitchFlipEnd = delegate()
	{
	};

	// Token: 0x06003E7C RID: 15996 RVA: 0x0012B236 File Offset: 0x00129636
	public void RegisterOnSwitchFlipBegin(CallbackVoid callback)
	{
		this.m_OnSwitchFlipBegin += callback;
	}

	// Token: 0x06003E7D RID: 15997 RVA: 0x0012B23F File Offset: 0x0012963F
	public void RegisterOnSwitchFlipEnd(CallbackVoid callback)
	{
		this.m_OnSwitchFlipEnd += callback;
	}

	// Token: 0x06003E7E RID: 15998 RVA: 0x0012B248 File Offset: 0x00129648
	public void UnRegisterOnSwitchFlipBegin(CallbackVoid callback)
	{
		this.m_OnSwitchFlipBegin -= callback;
	}

	// Token: 0x06003E7F RID: 15999 RVA: 0x0012B251 File Offset: 0x00129651
	public void UnRegisterOnSwitchFlipEnd(CallbackVoid callback)
	{
		this.m_OnSwitchFlipEnd -= callback;
	}

	// Token: 0x06003E80 RID: 16000 RVA: 0x0012B25A File Offset: 0x0012965A
	private void Awake()
	{
		if (!this.m_complete)
		{
			base.gameObject.SetRendering(false);
			this.m_complete = true;
		}
	}

	// Token: 0x06003E81 RID: 16001 RVA: 0x0012B27C File Offset: 0x0012967C
	public Animator Begin(FlipDirection _direction)
	{
		this.m_animator = base.gameObject.AddComponent<Animator>();
		this.m_animator.runtimeAnimatorController = this.m_controller;
		this.m_animatorComs = base.gameObject.AddComponent<AnimatorCommunications>();
		this.m_OnSwitchFlipBegin();
		return this.m_animator;
	}

	// Token: 0x06003E82 RID: 16002 RVA: 0x0012B2D0 File Offset: 0x001296D0
	public void End(FlipDirection _direction)
	{
		if (this.m_animatorComs != null)
		{
			UnityEngine.Object.Destroy(this.m_animatorComs);
			this.m_animatorComs = null;
		}
		if (this.m_animator != null)
		{
			UnityEngine.Object.Destroy(this.m_animator);
			this.m_animator = null;
		}
		base.gameObject.SetRendering(true);
		this.m_complete = true;
		this.m_OnSwitchFlipEnd();
	}

	// Token: 0x06003E83 RID: 16003 RVA: 0x0012B341 File Offset: 0x00129741
	public bool IsComplete()
	{
		return this.m_complete;
	}

	// Token: 0x0400322F RID: 12847
	[SerializeField]
	[AssignChild("Base", Editorbility.NonEditable)]
	private MeshRenderer m_base;

	// Token: 0x04003230 RID: 12848
	[SerializeField]
	[AssignChild("Pad", Editorbility.NonEditable)]
	private MeshRenderer m_pad;

	// Token: 0x04003231 RID: 12849
	[SerializeField]
	private RuntimeAnimatorController m_controller;

	// Token: 0x04003234 RID: 12852
	private Animator m_animator;

	// Token: 0x04003235 RID: 12853
	private AnimatorCommunications m_animatorComs;

	// Token: 0x04003236 RID: 12854
	private bool m_complete;
}
