using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

// Token: 0x02000183 RID: 387
public class UI_PlayerTowerList : MonoBehaviour
{
	// Token: 0x06000A42 RID: 2626 RVA: 0x000265C2 File Offset: 0x000247C2
	private void OnEnable()
	{
		EventMgr.Register<List<TowerIngameData>, int>(eGameEvents.OnTowerCardChanged, new Action<List<TowerIngameData>, int>(this.OnTowerCardChanged));
	}

	// Token: 0x06000A43 RID: 2627 RVA: 0x000265DC File Offset: 0x000247DC
	private void OnDisable()
	{
		EventMgr.Remove<List<TowerIngameData>, int>(eGameEvents.OnTowerCardChanged, new Action<List<TowerIngameData>, int>(this.OnTowerCardChanged));
	}

	// Token: 0x06000A44 RID: 2628 RVA: 0x000265F6 File Offset: 0x000247F6
	private void OnTowerCardChanged(List<TowerIngameData> list_Data, int index)
	{
		this.UpdateCards(list_Data);
	}

	// Token: 0x06000A45 RID: 2629 RVA: 0x00026600 File Offset: 0x00024800
	private void Start()
	{
		List<TowerIngameData> list_LoadoutTowerData = GameDataManager.instance.GameplayData.list_LoadoutTowerData;
		this.UpdateCards(list_LoadoutTowerData);
	}

	// Token: 0x06000A46 RID: 2630 RVA: 0x00026624 File Offset: 0x00024824
	private void Update()
	{
		this.cardJumpTimer += Time.deltaTime;
		if (this.cardJumpTimer >= this.cardJumpAnimationInterval)
		{
			this.cardJumpTimer = 0f;
			base.StartCoroutine(this.CR_CardJumpAnim());
		}
	}

	// Token: 0x06000A47 RID: 2631 RVA: 0x0002665E File Offset: 0x0002485E
	private IEnumerator CR_CardJumpAnim()
	{
		int num;
		for (int i = 0; i < this.list_Cards.Count; i = num + 1)
		{
			if (this.list_Cards[i].IsHaveCardData())
			{
				this.list_Cards[i].transform.DOLocalJump(this.list_Cards[i].transform.localPosition, 16f, 1, 0.5f, false);
			}
			else
			{
				this.list_Cards[i].transform.DOLocalJump(this.list_Cards[i].transform.localPosition, 4f, 1, 0.5f, false);
			}
			yield return new WaitForSeconds(0.1f);
			num = i;
		}
		yield break;
	}

	// Token: 0x06000A48 RID: 2632 RVA: 0x00026670 File Offset: 0x00024870
	private void UpdateCards(List<TowerIngameData> list_Data)
	{
		for (int i = 0; i < this.list_Cards.Count; i++)
		{
			if (i < list_Data.Count)
			{
				this.list_Cards[i].SetIsLocked(false);
				this.list_Cards[i].SetContent(list_Data[i].ItemType, i);
			}
			else
			{
				this.list_Cards[i].SetIsLocked(true);
			}
		}
	}

	// Token: 0x040007E8 RID: 2024
	[SerializeField]
	private List<Obj_UI_MapSceneTowerCard> list_Cards;

	// Token: 0x040007E9 RID: 2025
	[SerializeField]
	[Header("卡片跳動動畫間隔")]
	private float cardJumpAnimationInterval = 3f;

	// Token: 0x040007EA RID: 2026
	private float cardJumpTimer;
}
