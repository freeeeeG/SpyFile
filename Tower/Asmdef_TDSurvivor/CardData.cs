using System;
using UnityEngine;

// Token: 0x02000021 RID: 33
[Serializable]
public class CardData
{
	// Token: 0x17000004 RID: 4
	// (get) Token: 0x060000A3 RID: 163 RVA: 0x00003FBC File Offset: 0x000021BC
	public eItemType ItemType
	{
		get
		{
			return this.itemType;
		}
	}

	// Token: 0x060000A4 RID: 164 RVA: 0x00003FC4 File Offset: 0x000021C4
	public static CardData CreateCardData(AItemSettingData data, bool isFromPlayerStorage)
	{
		return new CardData(data, isFromPlayerStorage);
	}

	// Token: 0x060000A5 RID: 165 RVA: 0x00003FD0 File Offset: 0x000021D0
	public CardData(AItemSettingData data, bool isFromPlayerStorage)
	{
		this.data = data;
		this.itemType = data.GetItemType();
		if (data is TowerSettingData)
		{
			this.CardType = eCardType.TOWER_CARD;
		}
		else if (data is PanelSettingData)
		{
			this.CardType = eCardType.PANEL_CARD;
		}
		else if (data is ABaseBuffSettingData)
		{
			this.CardType = eCardType.BUFF_CARD;
		}
		else
		{
			Debug.LogError(string.Format("Undefined Card Found: {0}", data.GetItemType()));
			this.CardType = eCardType.UNDEFINED;
		}
		this.siblingIndexOnCreate = -1;
		this.IsFromPlayerStorage = isFromPlayerStorage;
	}

	// Token: 0x060000A6 RID: 166 RVA: 0x00004063 File Offset: 0x00002263
	public CardData()
	{
	}

	// Token: 0x060000A7 RID: 167 RVA: 0x00004072 File Offset: 0x00002272
	public Sprite GetCardIcon()
	{
		if (this.data == null)
		{
			Debug.LogError("data是空的!!!");
			return null;
		}
		return this.data.GetCardIcon();
	}

	// Token: 0x060000A8 RID: 168 RVA: 0x00004099 File Offset: 0x00002299
	public ICardDataSource GetDataSource()
	{
		return this.data;
	}

	// Token: 0x0400006E RID: 110
	[SerializeField]
	public eCardType CardType;

	// Token: 0x0400006F RID: 111
	[SerializeField]
	public AItemSettingData data;

	// Token: 0x04000070 RID: 112
	[SerializeField]
	[HideInInspector]
	public bool IsFromPlayerStorage;

	// Token: 0x04000071 RID: 113
	[SerializeField]
	public int siblingIndexOnCreate = -1;

	// Token: 0x04000072 RID: 114
	[SerializeField]
	[Header("飛入時的起始位置")]
	public Vector3 flyInOriginPosition;

	// Token: 0x04000073 RID: 115
	[SerializeField]
	private eItemType itemType;
}
