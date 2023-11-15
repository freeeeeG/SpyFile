using System;
using UnityEngine;

// Token: 0x0200003D RID: 61
[CreateAssetMenu(fileName = "Data", menuName = "設定檔/PanelSettingData", order = 1)]
public class PanelSettingData : ATowerComponentSettingData
{
	// Token: 0x1700001B RID: 27
	// (get) Token: 0x06000123 RID: 291 RVA: 0x000056D9 File Offset: 0x000038D9
	public GameObject Prefab
	{
		get
		{
			return this.prefab;
		}
	}

	// Token: 0x06000124 RID: 292 RVA: 0x000056E1 File Offset: 0x000038E1
	public GameObject GetPrefab()
	{
		return this.prefab;
	}

	// Token: 0x06000125 RID: 293 RVA: 0x000056E9 File Offset: 0x000038E9
	public Color GetSpriteColor()
	{
		return this.spriteColor;
	}

	// Token: 0x06000126 RID: 294 RVA: 0x000056F4 File Offset: 0x000038F4
	public override string GetLocNameString(bool isPrefix = true)
	{
		this.loc_Name = LocalizationManager.Instance.GetString("TowerType", this.itemType.ToString(), Array.Empty<object>());
		if (!isPrefix)
		{
			this.loc_Name += LocalizationManager.Instance.GetString("TowerType", "PANEL", Array.Empty<object>());
		}
		return this.loc_Name;
	}

	// Token: 0x040000CC RID: 204
	[SerializeField]
	[Header("底座的Prefab")]
	private GameObject prefab;

	// Token: 0x040000CD RID: 205
	[SerializeField]
	private Color spriteColor = Color.white;
}
