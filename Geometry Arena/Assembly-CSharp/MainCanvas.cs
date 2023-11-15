using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Token: 0x020000BF RID: 191
public class MainCanvas : MonoBehaviour
{
	// Token: 0x06000693 RID: 1683 RVA: 0x0002575C File Offset: 0x0002395C
	private void Awake()
	{
		MainCanvas.inst = this;
		if (this.panel_NewGame != null)
		{
			this.panel_NewGame.SetActive(false);
		}
		else
		{
			Debug.LogError("Panel_NewGame null!");
		}
		if (this.panel_MainMenu != null)
		{
			this.panel_MainMenu.Open();
		}
		else
		{
			Debug.LogError("Panel_MainMenu null!");
		}
		if (this.panelManual != null)
		{
			this.panelManual.Close();
		}
		else
		{
			Debug.LogError("PanelManual null!");
		}
		if (this.panelSetting != null)
		{
			this.panelSetting.gameObject.SetActive(false);
		}
		GameObject[] array = this.objsCloseOnAwake;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(false);
		}
		if (!SaveFile.IfExistSaveByJson())
		{
			this.panelWords.gameObject.SetActive(false);
			this.panelLanguage.Open();
			this.panel_MainMenu.Close();
		}
		else
		{
			this.OpenPanel_WordsFromDeveloper();
		}
		Application.targetFrameRate = 144;
	}

	// Token: 0x06000694 RID: 1684 RVA: 0x0002585C File Offset: 0x00023A5C
	private void Start()
	{
		this.UpdateLanguage();
		Time.timeScale = 1f;
		BGMControl.inst.UpdateTimeScale_SetPitch(1f);
	}

	// Token: 0x06000695 RID: 1685 RVA: 0x000051D0 File Offset: 0x000033D0
	private void Update()
	{
	}

	// Token: 0x06000696 RID: 1686 RVA: 0x00025880 File Offset: 0x00023A80
	public void UpdateLanguage()
	{
		SaveFile saveFile = GameData.SaveFile;
		LanguageText.MainMenu mainMenu = LanguageText.Inst.mainMenu;
		if (!saveFile.ifOnBattle)
		{
			this.main_NewGameOrContinue.text = mainMenu.main_NewGame;
		}
		else
		{
			this.main_NewGameOrContinue.text = mainMenu.main_ContinueGame;
		}
		this.main_Button_UpdateLog.text = mainMenu.main_UpdateLog;
		this.main_QuitApplication.text = mainMenu.main_QuitApp;
		this.main_Button_Setting.text = mainMenu.main_Setting;
		this.preview_StartGame.text = mainMenu.preview_StartGame;
		this.preview_Return.text = mainMenu.preview_Return;
		this.main_Button_Manual.text = mainMenu.main_Manual;
		this.main_Button_RankList.text = mainMenu.main_RankLists;
		this.main_Button_Credit.text = mainMenu.main_Credits;
		this.main_Button_UpgradeList.text = mainMenu.main_UpgradeList;
		this.panelWords.UpdateLanguage();
	}

	// Token: 0x06000697 RID: 1687 RVA: 0x0002596C File Offset: 0x00023B6C
	public void Button_Main_NewGameOrContinue()
	{
		if (!GameData.SaveFile.ifOnBattle)
		{
			if (NetworkTime.Inst.ifError && TempData.inst.daily_Open)
			{
				UI_FloatTextControl.inst.NewFloatText(LanguageText.Inst.dailyChallenge.error_CantConnectTimeServer);
				TempData.inst.DailyChallenge_Close(true);
			}
			this.Panel_NewGame_Open();
			return;
		}
		this.Scene_LoadBattle();
	}

	// Token: 0x06000698 RID: 1688 RVA: 0x000051D0 File Offset: 0x000033D0
	public void Button_Main_GiveUpOldGame()
	{
	}

	// Token: 0x06000699 RID: 1689 RVA: 0x000259CE File Offset: 0x00023BCE
	public void Scene_LoadBattle()
	{
		TempData.inst.currentSceneType = EnumSceneType.BATTLE;
		TempData.inst.NewGame();
		SceneManager.LoadScene("Scene_BattleMap");
	}

	// Token: 0x0600069A RID: 1690 RVA: 0x000259EF File Offset: 0x00023BEF
	public void ExitGame()
	{
		Application.Quit();
	}

	// Token: 0x0600069B RID: 1691 RVA: 0x000259F6 File Offset: 0x00023BF6
	private IEnumerator AppQuitAfter(float s)
	{
		yield return new WaitForSecondsRealtime(s);
		Application.Quit();
		yield break;
	}

	// Token: 0x0600069C RID: 1692 RVA: 0x00025A05 File Offset: 0x00023C05
	public void Panel_NewGame_Update()
	{
		TempData.inst.battle = new Battle();
		TempData.inst.battle.UpdateBattleFacs();
		this.panelNewGame.UpdatePanel();
		TutorialMaster.inst.Activate();
	}

	// Token: 0x0600069D RID: 1693 RVA: 0x00025A3C File Offset: 0x00023C3C
	public void Button_SelectJob(int no)
	{
		int jobId = TempData.inst.jobId;
		if (no != jobId)
		{
			TempData.inst.jobId = no;
		}
		if (!GameData.inst.IfColorUnlockedToCurJob(TempData.inst.varColorId))
		{
			this.Button_SelectVarColor(0);
		}
		this.Panel_NewGame_Update();
		this.Obj_Preview_UpdateAll();
	}

	// Token: 0x0600069E RID: 1694 RVA: 0x00025A8C File Offset: 0x00023C8C
	public void Button_SelectVarColor(int no)
	{
		TempData.inst.varColorId = no;
		this.Panel_NewGame_Update();
		this.Obj_Preview_UpdateColor();
	}

	// Token: 0x0600069F RID: 1695 RVA: 0x00025AA8 File Offset: 0x00023CA8
	public void Panel_NewGame_Open()
	{
		this.panel_NewGame.SetActive(true);
		this.panel_MainMenu.Close();
		TempData tempData = TempData.inst;
		if (tempData.jobId < 0)
		{
			tempData.jobId = 0;
		}
		if (tempData.varColorId < 0)
		{
			tempData.varColorId = 0;
		}
		this.Panel_NewGame_Update();
		this.Obj_Preview_UpdateAll();
	}

	// Token: 0x060006A0 RID: 1696 RVA: 0x00025AFE File Offset: 0x00023CFE
	public void Panel_NewGame_Close()
	{
		this.panel_NewGame.SetActive(false);
		this.panel_MainMenu.Open();
	}

	// Token: 0x060006A1 RID: 1697 RVA: 0x00025B17 File Offset: 0x00023D17
	public void DestroyPlayerPrevew()
	{
		Object.Destroy(this.objPlayerPreview);
	}

	// Token: 0x060006A2 RID: 1698 RVA: 0x00025B24 File Offset: 0x00023D24
	public void Obj_Preview_UpdateAll()
	{
		float num = 5f;
		float z = MyTool.MouseToPoint0();
		if (this.objPlayerPreview != null)
		{
			Object.Destroy(this.objPlayerPreview);
		}
		TempData tempData = TempData.inst;
		this.objPlayerPreview = Object.Instantiate<GameObject>(ResourceLibrary.Inst.Prefab_Players[tempData.jobId], Vector2.zero, Quaternion.Euler(0f, 0f, z));
		this.objPlayerPreview.transform.localScale = new Vector2(num, num);
		this.Obj_Preview_UpdateColor();
	}

	// Token: 0x060006A3 RID: 1699 RVA: 0x00025BB4 File Offset: 0x00023DB4
	public void Obj_Preview_UpdateColor()
	{
		BasicUnit component = this.objPlayerPreview.GetComponent<BasicUnit>();
		Color colorRGB = DataBase.Inst.Data_VarColors[TempData.inst.varColorId].ColorRGB;
		component.mainColor = colorRGB;
		component.DyeSprsWithColor(colorRGB);
		float bodySize = TempData.inst.playerPreview.TotalFactor.bodySize;
		float scaleWithSizeShape = MyTool.GetScaleWithSizeShape(component.shapeType, bodySize);
		component.TransScale = scaleWithSizeShape;
	}

	// Token: 0x060006A4 RID: 1700 RVA: 0x00025C1D File Offset: 0x00023E1D
	public void OpenPanel_WordsFromDeveloper()
	{
		this.panelWords.Open();
	}

	// Token: 0x060006A5 RID: 1701 RVA: 0x00025C2A File Offset: 0x00023E2A
	public static void ChildPanelOpen()
	{
		if (TempData.inst.currentSceneType != EnumSceneType.BATTLE)
		{
			MainCanvas.inst.panel_MainMenu.Close();
		}
	}

	// Token: 0x060006A6 RID: 1702 RVA: 0x00025C48 File Offset: 0x00023E48
	public static void ChildPanelClose()
	{
		if (TempData.inst.currentSceneType != EnumSceneType.BATTLE)
		{
			MainCanvas.inst.panel_MainMenu.Open();
		}
	}

	// Token: 0x04000574 RID: 1396
	public static MainCanvas inst;

	// Token: 0x04000575 RID: 1397
	[SerializeField]
	private GameObject objPlayerPreview;

	// Token: 0x04000576 RID: 1398
	[Header("Panel")]
	[SerializeField]
	public UI_Panel_Main_NewGame panelNewGame;

	// Token: 0x04000577 RID: 1399
	[SerializeField]
	private UI_Panel_Setting panelSetting;

	// Token: 0x04000578 RID: 1400
	[SerializeField]
	private Panel_Language panelLanguage;

	// Token: 0x04000579 RID: 1401
	[SerializeField]
	private Panel_WordsFromDeveloper panelWords;

	// Token: 0x0400057A RID: 1402
	[SerializeField]
	private Panel_Manual panelManual;

	// Token: 0x0400057B RID: 1403
	[Header("Languages")]
	public Text main_NewGameOrContinue;

	// Token: 0x0400057C RID: 1404
	[SerializeField]
	private Text main_Button_UpdateLog;

	// Token: 0x0400057D RID: 1405
	public Text main_QuitApplication;

	// Token: 0x0400057E RID: 1406
	[SerializeField]
	private Text main_Button_Setting;

	// Token: 0x0400057F RID: 1407
	[SerializeField]
	private Text main_Button_Manual;

	// Token: 0x04000580 RID: 1408
	[SerializeField]
	private Text main_Button_RankList;

	// Token: 0x04000581 RID: 1409
	[SerializeField]
	private Text main_Button_Credit;

	// Token: 0x04000582 RID: 1410
	[SerializeField]
	private Text main_Button_UpgradeList;

	// Token: 0x04000583 RID: 1411
	public Text preview_StartGame;

	// Token: 0x04000584 RID: 1412
	public Text preview_Return;

	// Token: 0x04000585 RID: 1413
	[Header("Gameobjects")]
	[SerializeField]
	private GameObject[] objsCloseOnAwake;

	// Token: 0x04000586 RID: 1414
	[SerializeField]
	private GameObject panel_NewGame;

	// Token: 0x04000587 RID: 1415
	[SerializeField]
	public MainMenuColorManager panel_MainMenu;
}
