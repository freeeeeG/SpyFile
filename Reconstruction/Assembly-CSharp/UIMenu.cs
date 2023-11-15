using System;
using UnityEngine;

// Token: 0x02000279 RID: 633
public class UIMenu : IUserInterface
{
	// Token: 0x06000FB1 RID: 4017 RVA: 0x0002A187 File Offset: 0x00028387
	public override void Initialize()
	{
		base.Initialize();
		this.anim = base.GetComponent<Animator>();
		this.m_ContinueGameBtn.SetActive(Singleton<LevelManager>.Instance.LastGameSave.HasLastGame);
	}

	// Token: 0x06000FB2 RID: 4018 RVA: 0x0002A1B5 File Offset: 0x000283B5
	public override void Show()
	{
		base.Show();
		this.anim.SetBool("Show", true);
	}

	// Token: 0x06000FB3 RID: 4019 RVA: 0x0002A1CE File Offset: 0x000283CE
	public override void ClosePanel()
	{
		base.ClosePanel();
		this.anim.SetBool("Show", false);
	}

	// Token: 0x06000FB4 RID: 4020 RVA: 0x0002A1E7 File Offset: 0x000283E7
	public void ContinueBtnClick()
	{
		Singleton<MenuManager>.Instance.ContinueGame();
	}

	// Token: 0x06000FB5 RID: 4021 RVA: 0x0002A1F3 File Offset: 0x000283F3
	public void StartGameBtnClick()
	{
		Singleton<MenuManager>.Instance.OpenMode();
	}

	// Token: 0x06000FB6 RID: 4022 RVA: 0x0002A1FF File Offset: 0x000283FF
	public void TujianBtnClick()
	{
		Singleton<MenuManager>.Instance.OpenTujian();
	}

	// Token: 0x06000FB7 RID: 4023 RVA: 0x0002A20B File Offset: 0x0002840B
	public void SettingBtnClick()
	{
		Singleton<MenuManager>.Instance.OpenSetting();
	}

	// Token: 0x06000FB8 RID: 4024 RVA: 0x0002A217 File Offset: 0x00028417
	public void SteamPage()
	{
		Application.OpenURL("https://store.steampowered.com/app/1664670/_Refactor");
	}

	// Token: 0x06000FB9 RID: 4025 RVA: 0x0002A223 File Offset: 0x00028423
	public void JoinDiscord()
	{
		Application.OpenURL("https://discord.gg/bPgMZ6kgBH");
	}

	// Token: 0x06000FBA RID: 4026 RVA: 0x0002A22F File Offset: 0x0002842F
	public void JoinQQ()
	{
		Application.OpenURL("https://jq.qq.com/?_wv=1027&k=wuuN4Bll");
	}

	// Token: 0x06000FBB RID: 4027 RVA: 0x0002A23B File Offset: 0x0002843B
	public void JoinWechat()
	{
		Singleton<MenuManager>.Instance.OpenWechat();
	}

	// Token: 0x06000FBC RID: 4028 RVA: 0x0002A247 File Offset: 0x00028447
	public void QuitGameBtnClick()
	{
		Singleton<Game>.Instance.QuitGame();
	}

	// Token: 0x04000812 RID: 2066
	private Animator anim;

	// Token: 0x04000813 RID: 2067
	[SerializeField]
	private GameObject m_ContinueGameBtn;
}
