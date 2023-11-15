using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Token: 0x02000059 RID: 89
public class CoinPage : MonoBehaviour
{
	// Token: 0x060001EE RID: 494 RVA: 0x00008A1C File Offset: 0x00006C1C
	private void OnEnable()
	{
		this.button_StartGame.onClick.AddListener(new UnityAction(this.OnClick_StartGame));
		this.button_Continue.onClick.AddListener(new UnityAction(this.OnClick_Continue));
		this.button_Options.onClick.AddListener(new UnityAction(this.OnClick_Options));
		this.button_Wishlist.onClick.AddListener(new UnityAction(this.OnClick_Wishlist));
		this.button_Exit.onClick.AddListener(new UnityAction(this.OnClick_Exit));
	}

	// Token: 0x060001EF RID: 495 RVA: 0x00008AB8 File Offset: 0x00006CB8
	private void OnDisable()
	{
		this.button_StartGame.onClick.RemoveListener(new UnityAction(this.OnClick_StartGame));
		this.button_Continue.onClick.RemoveListener(new UnityAction(this.OnClick_Continue));
		this.button_Options.onClick.RemoveListener(new UnityAction(this.OnClick_Options));
		this.button_Wishlist.onClick.RemoveListener(new UnityAction(this.OnClick_Wishlist));
		this.button_Exit.onClick.RemoveListener(new UnityAction(this.OnClick_Exit));
	}

	// Token: 0x060001F0 RID: 496 RVA: 0x00008B54 File Offset: 0x00006D54
	private void Awake()
	{
		bool interactable = GameDataManager.instance.GameplayData.IsGameInProgress();
		this.button_Continue.interactable = interactable;
	}

	// Token: 0x060001F1 RID: 497 RVA: 0x00008B80 File Offset: 0x00006D80
	private void OnClick_StartGame()
	{
		if (this.isButtonClicked)
		{
			return;
		}
		if (GameDataManager.instance.GameplayData.IsGameInProgress())
		{
			APopupWindow.CreateWindow<UI_CheckStartNewGamePopup>(APopupWindow.ePopupWindowLayer.TOP, null, true).RegisterResultCallback(delegate(bool result)
			{
				if (result)
				{
					GameDataManager.instance.StartNewGame(-1);
					base.StartCoroutine(this.CR_StartGame());
				}
			});
			return;
		}
		this.isButtonClicked = true;
		GameDataManager.instance.StartNewGame(-1);
		base.StartCoroutine(this.CR_StartGame());
	}

	// Token: 0x060001F2 RID: 498 RVA: 0x00008BE0 File Offset: 0x00006DE0
	private void OnClick_Continue()
	{
		if (this.isButtonClicked)
		{
			return;
		}
		this.isButtonClicked = true;
		base.StartCoroutine(this.CR_StartGame());
	}

	// Token: 0x060001F3 RID: 499 RVA: 0x00008BFF File Offset: 0x00006DFF
	private void OnClick_Wishlist()
	{
		Application.OpenURL("https://store.steampowered.com/app/2459550?utm_source=DemoTitle");
	}

	// Token: 0x060001F4 RID: 500 RVA: 0x00008C0B File Offset: 0x00006E0B
	private void OnClick_Options()
	{
		if (this.isButtonClicked)
		{
			return;
		}
		APopupWindow.CreateWindow<UI_SettingPage_Popup>(APopupWindow.ePopupWindowLayer.TOP, null, false);
	}

	// Token: 0x060001F5 RID: 501 RVA: 0x00008C1F File Offset: 0x00006E1F
	private void OnClick_Exit()
	{
		if (this.isButtonClicked)
		{
			return;
		}
		Application.Quit();
	}

	// Token: 0x060001F6 RID: 502 RVA: 0x00008C2F File Offset: 0x00006E2F
	private IEnumerator CR_StartGame()
	{
		EventMgr.SendEvent(eGameEvents.UI_TriggerTransition_Show);
		yield return new WaitForSeconds(1f);
		SceneManager.LoadScene("MapScene");
		yield break;
	}

	// Token: 0x060001F7 RID: 503 RVA: 0x00008C37 File Offset: 0x00006E37
	private IEnumerator Start()
	{
		EventMgr.SendEvent(eGameEvents.UI_TriggerTransition_Hide);
		yield return new WaitForSeconds(0.45f);
		EventMgr.SendEvent(eGameEvents.UI_ShowWelcomeToDemoUI);
		yield break;
	}

	// Token: 0x060001F8 RID: 504 RVA: 0x00008C3F File Offset: 0x00006E3F
	private void Update()
	{
	}

	// Token: 0x04000178 RID: 376
	[SerializeField]
	private Button button_StartGame;

	// Token: 0x04000179 RID: 377
	[SerializeField]
	private Button button_Continue;

	// Token: 0x0400017A RID: 378
	[SerializeField]
	private Button button_Options;

	// Token: 0x0400017B RID: 379
	[SerializeField]
	private Button button_Wishlist;

	// Token: 0x0400017C RID: 380
	[SerializeField]
	private Button button_Exit;

	// Token: 0x0400017D RID: 381
	private bool isButtonClicked;
}
