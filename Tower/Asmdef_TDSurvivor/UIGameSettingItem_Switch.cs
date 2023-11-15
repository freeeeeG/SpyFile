using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x0200013D RID: 317
public class UIGameSettingItem_Switch : AUIGameSettingItem
{
	// Token: 0x06000830 RID: 2096 RVA: 0x0001F10A File Offset: 0x0001D30A
	private void Start()
	{
		this.UpdateDisplay();
	}

	// Token: 0x06000831 RID: 2097 RVA: 0x0001F112 File Offset: 0x0001D312
	protected override void OnEnable()
	{
		base.OnEnable();
		this.button_Previous.onClick.AddListener(new UnityAction(this.PreviousOption));
		this.button_Next.onClick.AddListener(new UnityAction(this.NextOption));
	}

	// Token: 0x06000832 RID: 2098 RVA: 0x0001F152 File Offset: 0x0001D352
	protected override void OnDisable()
	{
		base.OnDisable();
		this.button_Previous.onClick.RemoveListener(new UnityAction(this.PreviousOption));
		this.button_Next.onClick.RemoveListener(new UnityAction(this.NextOption));
	}

	// Token: 0x06000833 RID: 2099 RVA: 0x0001F194 File Offset: 0x0001D394
	public void PreviousOption()
	{
		this.curValue = (this.curValue - 1 + this.list_OptionsData.Count) % this.list_OptionsData.Count;
		this.UpdateDisplay();
		this.ApplySetting();
		SoundManager.PlaySound("UI", "Settings_Switch_Trigger", -1f, -1f, -1f);
	}

	// Token: 0x06000834 RID: 2100 RVA: 0x0001F1F4 File Offset: 0x0001D3F4
	public void NextOption()
	{
		this.curValue = (this.curValue + 1) % this.list_OptionsData.Count;
		this.UpdateDisplay();
		this.ApplySetting();
		SoundManager.PlaySound("UI", "Settings_Switch_Trigger", -1f, -1f, -1f);
	}

	// Token: 0x06000835 RID: 2101 RVA: 0x0001F246 File Offset: 0x0001D446
	protected override void ApplySetting()
	{
		base.ApplySetting();
	}

	// Token: 0x06000836 RID: 2102 RVA: 0x0001F24E File Offset: 0x0001D44E
	protected override void ResetToDefault()
	{
		base.ResetToDefault();
		this.UpdateDisplay();
	}

	// Token: 0x06000837 RID: 2103 RVA: 0x0001F25C File Offset: 0x0001D45C
	protected override void UpdateDisplay()
	{
		if (this.list_OptionsData[this.curValue].isLocKey)
		{
			this.text_CurrentOption.text = LocalizationManager.Instance.GetString("GameSettings", this.list_OptionsData[this.curValue].content, Array.Empty<object>());
			return;
		}
		this.text_CurrentOption.text = this.list_OptionsData[this.curValue].content;
	}

	// Token: 0x040006A5 RID: 1701
	[SerializeField]
	protected List<UIGameSettingItem_Switch.SwitchOption> list_OptionsData;

	// Token: 0x040006A6 RID: 1702
	[SerializeField]
	protected TMP_Text text_CurrentOption;

	// Token: 0x040006A7 RID: 1703
	[SerializeField]
	protected Button button_Previous;

	// Token: 0x040006A8 RID: 1704
	[SerializeField]
	protected Button button_Next;

	// Token: 0x0200027E RID: 638
	[Serializable]
	public class SwitchOption
	{
		// Token: 0x04000BDE RID: 3038
		public string content;

		// Token: 0x04000BDF RID: 3039
		public bool isLocKey;
	}
}
