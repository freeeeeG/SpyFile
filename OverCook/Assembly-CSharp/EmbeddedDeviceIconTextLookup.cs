using System;
using UnityEngine;

// Token: 0x02000A87 RID: 2695
public class EmbeddedDeviceIconTextLookup : EmbeddedTextImageLookupBase
{
	// Token: 0x06003547 RID: 13639 RVA: 0x000F857C File Offset: 0x000F697C
	protected void Awake()
	{
		this.m_controllerIconLookup = GameUtils.RequireManager<ControllerIconLookup>();
		this.m_playerManager = GameUtils.RequireManager<PlayerManager>();
		PlayerInputLookup.OnRegenerateControls = (CallbackVoid)Delegate.Combine(PlayerInputLookup.OnRegenerateControls, new CallbackVoid(base.RefreshImage));
	}

	// Token: 0x06003548 RID: 13640 RVA: 0x000F85B4 File Offset: 0x000F69B4
	protected void OnDestroy()
	{
		PlayerInputLookup.OnRegenerateControls = (CallbackVoid)Delegate.Remove(PlayerInputLookup.OnRegenerateControls, new CallbackVoid(base.RefreshImage));
	}

	// Token: 0x06003549 RID: 13641 RVA: 0x000F85D8 File Offset: 0x000F69D8
	protected override Sprite GetIcon(int _materialNum)
	{
		if (this.m_iconOverridesPC.Length > 0 && _materialNum < this.m_iconOverridesPC.Length && this.m_iconOverridesPC[_materialNum] != null)
		{
			return this.m_iconOverridesPC[_materialNum];
		}
		ControlPadInput.Button button = this.m_buttons.TryAtIndex(_materialNum);
		ControllerIconLookup.DeviceContext device;
		if (KeyboardUtils.IsKeyboard(PlayerInputLookup.Player.One))
		{
			device = ControllerIconLookup.DeviceContext.Keyboard;
		}
		else
		{
			device = PlayerButtonImage.GetDevice(this.m_playerManager, PlayerInputLookup.Player.One);
		}
		return this.m_controllerIconLookup.GetIcon(button, ControllerIconLookup.IconContext.Bordered, device);
	}

	// Token: 0x04002AB7 RID: 10935
	[SerializeField]
	private ControlPadInput.Button[] m_buttons = new ControlPadInput.Button[0];

	// Token: 0x04002AB8 RID: 10936
	[SerializeField]
	private Sprite[] m_iconOverridesPC = new Sprite[0];

	// Token: 0x04002AB9 RID: 10937
	private ControllerIconLookup m_controllerIconLookup;

	// Token: 0x04002ABA RID: 10938
	private PlayerManager m_playerManager;
}
