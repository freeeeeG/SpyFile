using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000240 RID: 576
public class FinalToggle : MonoBehaviour
{
	// Token: 0x06000ECD RID: 3789 RVA: 0x00026CF8 File Offset: 0x00024EF8
	private void Awake()
	{
		this.m_Toggle = base.GetComponent<Toggle>();
		this.m_Toggle.onValueChanged.AddListener(new UnityAction<bool>(this.OnToggleChange));
	}

	// Token: 0x06000ECE RID: 3790 RVA: 0x00026D22 File Offset: 0x00024F22
	private void Start()
	{
		this.OnToggleChange(this.m_Toggle.isOn);
	}

	// Token: 0x06000ECF RID: 3791 RVA: 0x00026D38 File Offset: 0x00024F38
	private void OnToggleChange(bool value)
	{
		if (value)
		{
			this.m_Text.color = this.hiligtedColor;
			if (this.needCanvas)
			{
				this.m_canvasGroup.alpha = 1f;
				this.m_canvasGroup.blocksRaycasts = true;
				return;
			}
		}
		else
		{
			this.m_Text.color = this.normalColor;
			if (this.needCanvas)
			{
				this.m_canvasGroup.alpha = 0f;
				this.m_canvasGroup.blocksRaycasts = false;
			}
		}
	}

	// Token: 0x04000731 RID: 1841
	[SerializeField]
	private CanvasGroup m_canvasGroup;

	// Token: 0x04000732 RID: 1842
	[SerializeField]
	private Text m_Text;

	// Token: 0x04000733 RID: 1843
	[SerializeField]
	private Color normalColor;

	// Token: 0x04000734 RID: 1844
	[SerializeField]
	private Color hiligtedColor;

	// Token: 0x04000735 RID: 1845
	[SerializeField]
	private bool needCanvas;

	// Token: 0x04000736 RID: 1846
	private Toggle m_Toggle;
}
