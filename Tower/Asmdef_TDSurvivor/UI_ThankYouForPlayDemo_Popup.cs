using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Token: 0x0200018A RID: 394
public class UI_ThankYouForPlayDemo_Popup : APopupWindow
{
	// Token: 0x06000A75 RID: 2677 RVA: 0x0002702C File Offset: 0x0002522C
	private void OnValidate()
	{
		if (this.list_CardTypes.Count == 0)
		{
			this.list_CardTypes = new List<eItemType>
			{
				eItemType._1000_BASIC_TOWER,
				eItemType._1017_DICE_TOWER,
				eItemType._1001_SCRAP_TOWER,
				eItemType._1002_CANNON_TOWER,
				eItemType._1011_DRONE_TOWER,
				eItemType._1007_DART_TOWER,
				eItemType._1010_ICICLE_TOWER,
				eItemType._1003_FLAMETHROWER_TOWER,
				eItemType._1004_LIGHTNING_TOWER,
				eItemType._1006_FREEZE_TOWER,
				eItemType._1008_POISON_TOWER,
				eItemType._1009_SNIPER_TOWER,
				eItemType._1005_BOULDER_TOWER,
				eItemType._1013_FIREBALL_TOWER,
				eItemType._1015_MISSILE_TOWER,
				eItemType._1018_SNOWBALL_TOWER
			};
		}
	}

	// Token: 0x06000A76 RID: 2678 RVA: 0x00027104 File Offset: 0x00025304
	private void OnEnable()
	{
		this.button_Wishlist.onClick.AddListener(new UnityAction(this.OnClickWishlist));
		this.button_ToTitle.onClick.AddListener(new UnityAction(this.OnClickToTitle));
		for (int i = 0; i < this.list_Cards.Count; i++)
		{
			eItemType eItemType = this.list_CardTypes[i];
			bool showCard = GameDataManager.instance.Playerdata.IsTowerBuiltInRecord(eItemType);
			this.list_Cards[i].SetCardContent(eItemType);
			this.list_Cards[i].SetShowCard(showCard);
		}
		SoundManager.PlaySound("UI", "Victory", -1f, -1f, -1f);
		base.StartCoroutine(this.CR_ShowDemoCards());
	}

	// Token: 0x06000A77 RID: 2679 RVA: 0x000271CD File Offset: 0x000253CD
	private void OnDisable()
	{
		this.button_Wishlist.onClick.RemoveListener(new UnityAction(this.OnClickWishlist));
		this.button_ToTitle.onClick.RemoveListener(new UnityAction(this.OnClickToTitle));
	}

	// Token: 0x06000A78 RID: 2680 RVA: 0x00027207 File Offset: 0x00025407
	private void OnClickWishlist()
	{
		Application.OpenURL("https://store.steampowered.com/app/2459550?utm_source=DemoEnd");
	}

	// Token: 0x06000A79 RID: 2681 RVA: 0x00027213 File Offset: 0x00025413
	private void OnClickToTitle()
	{
		base.StartCoroutine(this.CR_BackToTitle());
	}

	// Token: 0x06000A7A RID: 2682 RVA: 0x00027222 File Offset: 0x00025422
	private IEnumerator CR_BackToTitle()
	{
		EventMgr.SendEvent(eGameEvents.UI_TriggerTransition_Show);
		yield return new WaitForSeconds(1f);
		SceneManager.LoadScene("CoinPage");
		yield break;
	}

	// Token: 0x06000A7B RID: 2683 RVA: 0x0002722A File Offset: 0x0002542A
	private IEnumerator CR_ShowDemoCards()
	{
		this.list_Cards.Shuffle<UI_Obj_DemoTowerCard>();
		foreach (UI_Obj_DemoTowerCard ui_Obj_DemoTowerCard in this.list_Cards)
		{
			ui_Obj_DemoTowerCard.transform.localScale = Vector3.zero;
		}
		yield return new WaitForSeconds(1f);
		int num;
		for (int i = 0; i < this.list_Cards.Count; i = num + 1)
		{
			this.list_Cards[i].transform.DOScale(1f, 0.33f).SetEase(Ease.OutBack);
			yield return new WaitForSeconds(0.05f);
			num = i;
		}
		yield break;
	}

	// Token: 0x06000A7C RID: 2684 RVA: 0x00027239 File Offset: 0x00025439
	protected override void ShowWindowProc()
	{
		this.animator.SetBool("isOn", true);
	}

	// Token: 0x06000A7D RID: 2685 RVA: 0x0002724C File Offset: 0x0002544C
	protected override void CloseWindowProc()
	{
		this.animator.SetBool("isOn", false);
	}

	// Token: 0x04000808 RID: 2056
	[SerializeField]
	private Button button_Wishlist;

	// Token: 0x04000809 RID: 2057
	[SerializeField]
	private Button button_ToTitle;

	// Token: 0x0400080A RID: 2058
	[SerializeField]
	private List<UI_Obj_DemoTowerCard> list_Cards;

	// Token: 0x0400080B RID: 2059
	[SerializeField]
	private List<eItemType> list_CardTypes;
}
