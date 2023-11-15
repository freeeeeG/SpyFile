using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000A83 RID: 2691
[RequireComponent(typeof(Image))]
public class ButtonImage : MonoBehaviour
{
	// Token: 0x06003532 RID: 13618 RVA: 0x000F82A8 File Offset: 0x000F66A8
	public void SetData(ControlPadInput.Button _button, ControllerIconLookup.DeviceContext _deviceContext)
	{
		this.m_buton = _button;
		this.m_device = _deviceContext;
		ControllerIconLookup controllerIconLookup = GameUtils.RequireManager<ControllerIconLookup>();
		this.m_image.sprite = controllerIconLookup.GetIcon(this.m_buton, this.m_context, this.m_device);
		this.m_awake = true;
	}

	// Token: 0x06003533 RID: 13619 RVA: 0x000F82F3 File Offset: 0x000F66F3
	private void Awake()
	{
		if (!this.m_awake)
		{
			this.SetData(this.m_buton, this.m_device);
			this.m_awake = true;
		}
	}

	// Token: 0x04002AA9 RID: 10921
	[SerializeField]
	[AssignComponent(Editorbility.Editable)]
	private Image m_image;

	// Token: 0x04002AAA RID: 10922
	[SerializeField]
	private ControlPadInput.Button m_buton = ControlPadInput.Button.A;

	// Token: 0x04002AAB RID: 10923
	[SerializeField]
	private AmbiPadButton m_ambiButton;

	// Token: 0x04002AAC RID: 10924
	[SerializeField]
	private ControllerIconLookup.IconContext m_context;

	// Token: 0x04002AAD RID: 10925
	[SerializeField]
	private ControllerIconLookup.DeviceContext m_device = ControllerIconLookup.DeviceContext.Pad;

	// Token: 0x04002AAE RID: 10926
	private bool m_awake;
}
