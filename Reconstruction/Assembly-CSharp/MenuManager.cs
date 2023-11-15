using System;
using UnityEngine;

// Token: 0x0200026E RID: 622
public class MenuManager : Singleton<MenuManager>
{
	// Token: 0x06000F6F RID: 3951 RVA: 0x00029528 File Offset: 0x00027728
	public void Initinal()
	{
		this.m_Canvas = base.GetComponent<Canvas>();
		Singleton<LevelManager>.Instance.LoadGame();
		this.m_UIMenu.Initialize();
		this.m_UIMode.Initialize();
		this.m_UITujian.Initialize();
		this.m_UISetting.Initialize();
		this.m_UIBillboard.Initialize();
		this.m_UIWechat.Initialize();
		this.m_ThanksPanel.Initialize();
		this.m_UIRecipeSet.Initialize();
		this.m_UIRuleSet.Initialize();
		this.m_TechInfoTips.Initialize();
	}

	// Token: 0x06000F70 RID: 3952 RVA: 0x000295B9 File Offset: 0x000277B9
	public void Release()
	{
	}

	// Token: 0x06000F71 RID: 3953 RVA: 0x000295BB File Offset: 0x000277BB
	public void OpenBillboard(int leaderBoardType)
	{
		this.m_UIBillboard.ShowLeaderBoard((LeaderBoard)leaderBoardType);
	}

	// Token: 0x06000F72 RID: 3954 RVA: 0x000295C9 File Offset: 0x000277C9
	public void OpenWechat()
	{
		this.m_UIWechat.Show();
	}

	// Token: 0x06000F73 RID: 3955 RVA: 0x000295D6 File Offset: 0x000277D6
	public void OpenMode()
	{
		this.m_UIMenu.ClosePanel();
		this.m_UIMode.Show();
	}

	// Token: 0x06000F74 RID: 3956 RVA: 0x000295EE File Offset: 0x000277EE
	public void ContinueGame()
	{
		Singleton<LevelManager>.Instance.ContinueLastGame();
	}

	// Token: 0x06000F75 RID: 3957 RVA: 0x000295FA File Offset: 0x000277FA
	public void ShowMenu()
	{
		this.m_UIMenu.Show();
	}

	// Token: 0x06000F76 RID: 3958 RVA: 0x00029607 File Offset: 0x00027807
	public void GameUpdate()
	{
	}

	// Token: 0x06000F77 RID: 3959 RVA: 0x00029609 File Offset: 0x00027809
	public void OpenTujian()
	{
		this.m_UIMenu.ClosePanel();
		this.m_UITujian.Show();
	}

	// Token: 0x06000F78 RID: 3960 RVA: 0x00029624 File Offset: 0x00027824
	private void SetCanvasPos(Transform tr, Vector2 pos)
	{
		Vector2 v;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(this.m_Canvas.transform as RectTransform, pos, this.m_Canvas.worldCamera, out v);
		tr.position = this.m_Canvas.transform.TransformPoint(v);
	}

	// Token: 0x06000F79 RID: 3961 RVA: 0x00029671 File Offset: 0x00027871
	public void OpenSetting()
	{
		this.m_UISetting.Show();
	}

	// Token: 0x06000F7A RID: 3962 RVA: 0x0002967E File Offset: 0x0002787E
	internal void HideTips()
	{
		this.m_TechInfoTips.CloseTips();
	}

	// Token: 0x06000F7B RID: 3963 RVA: 0x0002968B File Offset: 0x0002788B
	public void QuitGameBtnClick()
	{
		Singleton<Game>.Instance.QuitGame();
	}

	// Token: 0x040007DD RID: 2013
	private Canvas m_Canvas;

	// Token: 0x040007DE RID: 2014
	[SerializeField]
	private UIMenu m_UIMenu;

	// Token: 0x040007DF RID: 2015
	[SerializeField]
	private UIMode m_UIMode;

	// Token: 0x040007E0 RID: 2016
	[SerializeField]
	private UITujian m_UITujian;

	// Token: 0x040007E1 RID: 2017
	[SerializeField]
	private UISetting m_UISetting;

	// Token: 0x040007E2 RID: 2018
	[SerializeField]
	private UIBillBoard m_UIBillboard;

	// Token: 0x040007E3 RID: 2019
	[SerializeField]
	private UIWechat m_UIWechat;

	// Token: 0x040007E4 RID: 2020
	[SerializeField]
	private ThanksPanel m_ThanksPanel;

	// Token: 0x040007E5 RID: 2021
	[SerializeField]
	private UIRecipeSet m_UIRecipeSet;

	// Token: 0x040007E6 RID: 2022
	[SerializeField]
	private UIRuleSet m_UIRuleSet;

	// Token: 0x040007E7 RID: 2023
	[SerializeField]
	private TechInfoTips m_TechInfoTips;
}
