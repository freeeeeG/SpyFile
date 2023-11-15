using System;
using UnityEngine;

// Token: 0x02000A85 RID: 2693
public class DeviceIconSwap : MonoBehaviour
{
	// Token: 0x170003BB RID: 955
	// (get) Token: 0x0600353D RID: 13629 RVA: 0x000F8431 File Offset: 0x000F6831
	// (set) Token: 0x0600353E RID: 13630 RVA: 0x000F8439 File Offset: 0x000F6839
	public ControlPadInput.Button Button
	{
		get
		{
			return this.m_button;
		}
		set
		{
			this.m_button = value;
		}
	}

	// Token: 0x0600353F RID: 13631 RVA: 0x000F8444 File Offset: 0x000F6844
	protected void Awake()
	{
		this.m_controllerIconLookup = GameUtils.RequireManager<ControllerIconLookup>();
		this.m_playerManager = GameUtils.RequireManager<PlayerManager>();
		if (this.m_Image == null)
		{
			this.m_Image = base.gameObject.RequireComponent<T17Image>();
		}
		this.RefreshImage();
		PlayerInputLookup.OnRegenerateControls = (CallbackVoid)Delegate.Combine(PlayerInputLookup.OnRegenerateControls, new CallbackVoid(this.RefreshImage));
	}

	// Token: 0x06003540 RID: 13632 RVA: 0x000F84AF File Offset: 0x000F68AF
	protected void OnDestroy()
	{
		PlayerInputLookup.OnRegenerateControls = (CallbackVoid)Delegate.Remove(PlayerInputLookup.OnRegenerateControls, new CallbackVoid(this.RefreshImage));
	}

	// Token: 0x06003541 RID: 13633 RVA: 0x000F84D1 File Offset: 0x000F68D1
	private void RefreshImage()
	{
		this.m_Image.sprite = this.GetIcon();
	}

	// Token: 0x06003542 RID: 13634 RVA: 0x000F84E4 File Offset: 0x000F68E4
	private Sprite GetIcon()
	{
		ControllerIconLookup.DeviceContext device = PlayerButtonImage.GetDevice(this.m_playerManager, PlayerInputLookup.Player.One);
		return this.m_controllerIconLookup.GetIcon(this.m_button, ControllerIconLookup.IconContext.Bordered, device);
	}

	// Token: 0x04002AB1 RID: 10929
	[SerializeField]
	private ControlPadInput.Button m_button;

	// Token: 0x04002AB2 RID: 10930
	private ControllerIconLookup m_controllerIconLookup;

	// Token: 0x04002AB3 RID: 10931
	private PlayerManager m_playerManager;

	// Token: 0x04002AB4 RID: 10932
	[SerializeField]
	private T17Image m_Image;
}
