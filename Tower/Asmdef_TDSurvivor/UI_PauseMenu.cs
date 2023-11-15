using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Token: 0x0200017F RID: 383
public class UI_PauseMenu : AUISituational
{
	// Token: 0x06000A1A RID: 2586 RVA: 0x00025D08 File Offset: 0x00023F08
	private void OnEnable()
	{
		EventMgr.Register<bool>(eGameEvents.UI_TogglePauseMenu, new Action<bool>(this.OnTogglePauseMenu));
		this.button_Resume.onClick.AddListener(new UnityAction(this.OnClickButton_Resume));
		this.button_Settings.onClick.AddListener(new UnityAction(this.OnClickButton_Settings));
		this.button_Help.onClick.AddListener(new UnityAction(this.OnClickButton_Help));
		this.button_MainMenu.onClick.AddListener(new UnityAction(this.OnClickButton_MainMenu));
	}

	// Token: 0x06000A1B RID: 2587 RVA: 0x00025DA0 File Offset: 0x00023FA0
	private void OnDisable()
	{
		EventMgr.Remove<bool>(eGameEvents.UI_TogglePauseMenu, new Action<bool>(this.OnTogglePauseMenu));
		this.button_Resume.onClick.RemoveListener(new UnityAction(this.OnClickButton_Resume));
		this.button_Settings.onClick.RemoveListener(new UnityAction(this.OnClickButton_Settings));
		this.button_Help.onClick.RemoveListener(new UnityAction(this.OnClickButton_Help));
		this.button_MainMenu.onClick.RemoveListener(new UnityAction(this.OnClickButton_MainMenu));
	}

	// Token: 0x06000A1C RID: 2588 RVA: 0x00025E38 File Offset: 0x00024038
	private void Update()
	{
		if (!base.IsUIActivated)
		{
			return;
		}
		this.image_BG.material.SetFloat("_UnscaledTime", Time.unscaledTime);
		if (this.timeSinceUIOpen < 0.25f)
		{
			this.timeSinceUIOpen += Time.unscaledDeltaTime;
			return;
		}
		if (Input.GetKeyDown(KeyCode.Escape) && !this.isSettingWindowOn && !this.isConfirmWindowOn)
		{
			EventMgr.SendEvent(eGameEvents.RequestEndPause);
		}
	}

	// Token: 0x06000A1D RID: 2589 RVA: 0x00025EB0 File Offset: 0x000240B0
	private void OnTogglePauseMenu(bool isOn)
	{
		base.Toggle(isOn);
		this.timeSinceUIOpen = 0f;
		if (isOn)
		{
			SoundManager.PlaySound("UI", "PauseGame", -1f, -1f, -1f);
			return;
		}
		SoundManager.PlaySound("UI", "UnpauseGame", -1f, -1f, -1f);
	}

	// Token: 0x06000A1E RID: 2590 RVA: 0x00025F11 File Offset: 0x00024111
	private void OnClickButton_Resume()
	{
		if (!this.isSettingWindowOn)
		{
			EventMgr.SendEvent(eGameEvents.RequestEndPause);
		}
	}

	// Token: 0x06000A1F RID: 2591 RVA: 0x00025F28 File Offset: 0x00024128
	private void OnClickButton_Settings()
	{
		this.isSettingWindowOn = true;
		UI_SettingPage_Popup ui_SettingPage_Popup = APopupWindow.CreateWindow<UI_SettingPage_Popup>(APopupWindow.ePopupWindowLayer.TOP, null, false);
		ui_SettingPage_Popup.OnWindowFinished = (Action)Delegate.Combine(ui_SettingPage_Popup.OnWindowFinished, new Action(this.OnSettingWindowFinished));
	}

	// Token: 0x06000A20 RID: 2592 RVA: 0x00025F5A File Offset: 0x0002415A
	private void OnSettingWindowFinished()
	{
		this.isSettingWindowOn = false;
	}

	// Token: 0x06000A21 RID: 2593 RVA: 0x00025F63 File Offset: 0x00024163
	private void OnClickButton_Help()
	{
		throw new NotImplementedException();
	}

	// Token: 0x06000A22 RID: 2594 RVA: 0x00025F6A File Offset: 0x0002416A
	private void OnClickButton_MainMenu()
	{
		base.StartCoroutine(this.CR_BackToTitle());
	}

	// Token: 0x06000A23 RID: 2595 RVA: 0x00025F79 File Offset: 0x00024179
	private IEnumerator CR_BackToTitle()
	{
		bool doBackToTitle = false;
		this.isConfirmWindowOn = true;
		UI_Window_YesNo_Popup window = APopupWindow.CreateWindow<UI_Window_YesNo_Popup>(APopupWindow.ePopupWindowLayer.TOP, null, false);
		window.SetupContent("BACK_TO_TITLE_CONFIRM", "BACK_TO_TITLE_YES", "BACK_TO_TITLE_NO", delegate(bool result)
		{
			doBackToTitle = result;
		});
		while (!window.IsWindowFinished)
		{
			yield return null;
		}
		this.isConfirmWindowOn = false;
		if (doBackToTitle)
		{
			EventMgr.SendEvent<float>(eGameEvents.RequestModifySystemGameSpeed, 1f);
			EventMgr.SendEvent(eGameEvents.UI_TriggerTransition_Show);
			yield return new WaitForSeconds(1f);
			SceneManager.LoadScene("CoinPage");
		}
		yield break;
	}

	// Token: 0x040007D2 RID: 2002
	[SerializeField]
	private Image image_BG;

	// Token: 0x040007D3 RID: 2003
	[SerializeField]
	private Button button_Resume;

	// Token: 0x040007D4 RID: 2004
	[SerializeField]
	private Button button_Settings;

	// Token: 0x040007D5 RID: 2005
	[SerializeField]
	private Button button_Help;

	// Token: 0x040007D6 RID: 2006
	[SerializeField]
	private Button button_MainMenu;

	// Token: 0x040007D7 RID: 2007
	private bool isSettingWindowOn;

	// Token: 0x040007D8 RID: 2008
	private bool isConfirmWindowOn;

	// Token: 0x040007D9 RID: 2009
	private float timeSinceUIOpen;
}
