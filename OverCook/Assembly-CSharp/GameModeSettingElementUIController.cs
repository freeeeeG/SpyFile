using System;
using GameModes;
using UnityEngine;

// Token: 0x02000A97 RID: 2711
public class GameModeSettingElementUIController : UIControllerBase
{
	// Token: 0x0600359B RID: 13723 RVA: 0x000FA710 File Offset: 0x000F8B10
	private void Start()
	{
	}

	// Token: 0x0600359C RID: 13724 RVA: 0x000FA714 File Offset: 0x000F8B14
	public void SetData(SettingKind kind, ModeSettingUIData uiData, T17EventSystem eventSystem)
	{
		this.m_ready = false;
		this.m_kind = kind;
		this.m_uiData = uiData;
		this.m_title.SetLocalisedTextCatchAll(this.m_uiData.m_nameLocalisationKey);
		GameSession gameSession = GameUtils.GetGameSession();
		this.m_enableToggle.SetEventSystem(eventSystem);
		this.m_enableToggle.isOn = gameSession.GetGameModeSetting(this.m_kind);
		this.m_ready = true;
	}

	// Token: 0x0600359D RID: 13725 RVA: 0x000FA77C File Offset: 0x000F8B7C
	public void OnToggle(bool value)
	{
		if (this.m_ready)
		{
			GameSession gameSession = GameUtils.GetGameSession();
			gameSession.SetGameModeSetting(this.m_kind, this.m_enableToggle.isOn);
		}
	}

	// Token: 0x04002B19 RID: 11033
	[HideInInspector]
	private ModeSettingUIData m_uiData;

	// Token: 0x04002B1A RID: 11034
	private SettingKind m_kind;

	// Token: 0x04002B1B RID: 11035
	[SerializeField]
	private T17Text m_title;

	// Token: 0x04002B1C RID: 11036
	[SerializeField]
	private T17Toggle m_enableToggle;

	// Token: 0x04002B1D RID: 11037
	private bool m_ready;
}
