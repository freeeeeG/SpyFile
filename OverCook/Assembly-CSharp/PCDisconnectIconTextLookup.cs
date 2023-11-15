using System;
using UnityEngine;

// Token: 0x02000AF0 RID: 2800
public class PCDisconnectIconTextLookup : EmbeddedTextImageLookupBase
{
	// Token: 0x060038B7 RID: 14519 RVA: 0x0010B844 File Offset: 0x00109C44
	protected void Awake()
	{
		this.m_controllerIconLookup = GameUtils.RequireManager<ControllerIconLookup>();
		PlayerInputLookup.OnRegenerateControls = (CallbackVoid)Delegate.Combine(PlayerInputLookup.OnRegenerateControls, new CallbackVoid(base.RefreshImage));
	}

	// Token: 0x060038B8 RID: 14520 RVA: 0x0010B871 File Offset: 0x00109C71
	protected void OnDestroy()
	{
		PlayerInputLookup.OnRegenerateControls = (CallbackVoid)Delegate.Remove(PlayerInputLookup.OnRegenerateControls, new CallbackVoid(base.RefreshImage));
	}

	// Token: 0x060038B9 RID: 14521 RVA: 0x0010B893 File Offset: 0x00109C93
	protected override Sprite GetIcon(int _materialNum)
	{
		return this.m_controllerIconLookup.GetIcon(ControlPadInput.Button.A, ControllerIconLookup.IconContext.Bordered, ControllerIconLookup.DeviceContext.Pad);
	}

	// Token: 0x04002D5F RID: 11615
	private ControllerIconLookup m_controllerIconLookup;
}
