using System;
using TMPro;
using UnityEngine;

// Token: 0x02000289 RID: 649
public class UISetting : IUserInterface
{
	// Token: 0x0600100F RID: 4111 RVA: 0x0002B030 File Offset: 0x00029230
	public override void Initialize()
	{
		base.Initialize();
		this.m_Anim = base.GetComponent<Animator>();
		this.m_BasicSetting.Initialize();
		this.m_ShortCutSetting.Initialize();
		this.SetBattleContent();
	}

	// Token: 0x06001010 RID: 4112 RVA: 0x0002B060 File Offset: 0x00029260
	public override void Show()
	{
		base.Show();
		StaticData.LockKeyboard = true;
		this.m_BasicSetting.ShowSetting();
		this.m_GameSetting.ShowSetting();
		this.m_BattleContent.SetActive(Singleton<Game>.Instance.CurrentState == "BattleState");
		this.m_MenuContent.SetActive(Singleton<Game>.Instance.CurrentState == "MenuState");
		this.m_Anim.SetBool("isOpen", true);
	}

	// Token: 0x06001011 RID: 4113 RVA: 0x0002B0DE File Offset: 0x000292DE
	public void ThanksInfoBtnClick()
	{
		this.m_ThanksPanel.Show();
	}

	// Token: 0x06001012 RID: 4114 RVA: 0x0002B0EB File Offset: 0x000292EB
	public override void ClosePanel()
	{
		this.SaveSetting();
		this.m_Anim.SetBool("isOpen", false);
	}

	// Token: 0x06001013 RID: 4115 RVA: 0x0002B104 File Offset: 0x00029304
	private void SaveSetting()
	{
		if (!Singleton<Game>.Instance.Tutorial)
		{
			StaticData.LockKeyboard = false;
		}
		this.m_BasicSetting.SaveSetting();
	}

	// Token: 0x06001014 RID: 4116 RVA: 0x0002B124 File Offset: 0x00029324
	private void SetBattleContent()
	{
		if (!Singleton<LevelManager>.Instance.CurrentLevel.CanSaveGame)
		{
			this.ReturnTxt.text = GameMultiLang.GetTraduction("RETURNTUTORIAL");
			this.QuitTxt.text = GameMultiLang.GetTraduction("QUIT");
			return;
		}
		this.ReturnTxt.text = GameMultiLang.GetTraduction("RETURNTOMENU");
		this.QuitTxt.text = GameMultiLang.GetTraduction("QUIT2");
	}

	// Token: 0x06001015 RID: 4117 RVA: 0x0002B197 File Offset: 0x00029397
	public void RestartGameBtnClick()
	{
		this.SaveSetting();
		Singleton<GameManager>.Instance.RestartGame();
	}

	// Token: 0x06001016 RID: 4118 RVA: 0x0002B1A9 File Offset: 0x000293A9
	public void ReturnMenuBtnClick()
	{
		this.SaveSetting();
		Singleton<GameManager>.Instance.ReturnToMenu();
	}

	// Token: 0x06001017 RID: 4119 RVA: 0x0002B1BB File Offset: 0x000293BB
	public void ExitGameBtnClick()
	{
		this.SaveSetting();
		Singleton<GameManager>.Instance.QuitGame();
	}

	// Token: 0x04000854 RID: 2132
	private Animator m_Anim;

	// Token: 0x04000855 RID: 2133
	[SerializeField]
	private BasicSetting m_BasicSetting;

	// Token: 0x04000856 RID: 2134
	[SerializeField]
	private GameSetting m_GameSetting;

	// Token: 0x04000857 RID: 2135
	[SerializeField]
	private ShortCutsSetting m_ShortCutSetting;

	// Token: 0x04000858 RID: 2136
	[SerializeField]
	private GameObject m_BattleContent;

	// Token: 0x04000859 RID: 2137
	[SerializeField]
	private GameObject m_MenuContent;

	// Token: 0x0400085A RID: 2138
	[SerializeField]
	private ThanksPanel m_ThanksPanel;

	// Token: 0x0400085B RID: 2139
	[SerializeField]
	private TextMeshProUGUI ReturnTxt;

	// Token: 0x0400085C RID: 2140
	[SerializeField]
	private TextMeshProUGUI QuitTxt;
}
