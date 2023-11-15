using System;
using UnityEngine;

// Token: 0x0200002C RID: 44
[CreateAssetMenu(fileName = "EventStageSettingData_", menuName = "設定檔/關卡/EventStageSettingData", order = 1)]
public class EventStageSettingData : ABaseStageSettingData
{
	// Token: 0x060000D7 RID: 215 RVA: 0x000048A9 File Offset: 0x00002AA9
	private void OnValidate()
	{
		this.stageType = eStageType.SPECIAL_EVENT;
	}

	// Token: 0x060000D8 RID: 216 RVA: 0x000048B2 File Offset: 0x00002AB2
	public override string GetLocalization_Description()
	{
		return "事件描述";
	}

	// Token: 0x060000D9 RID: 217 RVA: 0x000048B9 File Offset: 0x00002AB9
	public override string GetLocalization_Title()
	{
		return "事件標題";
	}

	// Token: 0x0400009D RID: 157
	[SerializeField]
	protected eEventStageType eventType;

	// Token: 0x0400009E RID: 158
	[SerializeField]
	[Header("屬於哪個世界的事件")]
	protected eWorldType worldType;

	// Token: 0x0400009F RID: 159
	[SerializeField]
	protected Sprite sprite_BGTexture;
}
