using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000185 RID: 389
public class UI_SettingPage_Popup : APopupWindow
{
	// Token: 0x06000A4F RID: 2639 RVA: 0x000267EA File Offset: 0x000249EA
	private void OnEnable()
	{
		this.button_Exit.onClick.AddListener(new UnityAction(this.OnClickButton_Exit));
	}

	// Token: 0x06000A50 RID: 2640 RVA: 0x00026808 File Offset: 0x00024A08
	private void OnDisable()
	{
		this.button_Exit.onClick.RemoveListener(new UnityAction(this.OnClickButton_Exit));
	}

	// Token: 0x06000A51 RID: 2641 RVA: 0x00026826 File Offset: 0x00024A26
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			base.CloseWindow();
		}
	}

	// Token: 0x06000A52 RID: 2642 RVA: 0x00026837 File Offset: 0x00024A37
	private void OnClickButton_Exit()
	{
		base.CloseWindow();
	}

	// Token: 0x06000A53 RID: 2643 RVA: 0x0002683F File Offset: 0x00024A3F
	private void Toggle(bool isOn)
	{
		this.animator.SetBool("isOn", isOn);
	}

	// Token: 0x06000A54 RID: 2644 RVA: 0x00026852 File Offset: 0x00024A52
	protected override void ShowWindowProc()
	{
		this.Toggle(true);
		SoundManager.PlaySound("UI", "SettingPage_Show", -1f, -1f, -1f);
	}

	// Token: 0x06000A55 RID: 2645 RVA: 0x0002687A File Offset: 0x00024A7A
	protected override void CloseWindowProc()
	{
		this.Toggle(false);
		SoundManager.PlaySound("UI", "SettingPage_Hide", -1f, -1f, -1f);
	}

	// Token: 0x040007ED RID: 2029
	[SerializeField]
	private Button button_Exit;
}
