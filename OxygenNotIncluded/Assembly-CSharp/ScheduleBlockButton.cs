using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

// Token: 0x02000BE1 RID: 3041
[AddComponentMenu("KMonoBehaviour/scripts/ScheduleBlockButton")]
public class ScheduleBlockButton : KMonoBehaviour
{
	// Token: 0x170006AA RID: 1706
	// (get) Token: 0x0600603D RID: 24637 RVA: 0x0023937D File Offset: 0x0023757D
	// (set) Token: 0x0600603E RID: 24638 RVA: 0x00239385 File Offset: 0x00237585
	public int idx { get; private set; }

	// Token: 0x0600603F RID: 24639 RVA: 0x00239390 File Offset: 0x00237590
	public void Setup(int idx, Dictionary<string, ColorStyleSetting> paintStyles, int totalBlocks)
	{
		this.idx = idx;
		this.paintStyles = paintStyles;
		if (idx < TRAITS.EARLYBIRD_SCHEDULEBLOCK)
		{
			base.GetComponent<HierarchyReferences>().GetReference<RectTransform>("MorningIcon").gameObject.SetActive(true);
		}
		else if (idx >= totalBlocks - 3)
		{
			base.GetComponent<HierarchyReferences>().GetReference<RectTransform>("NightIcon").gameObject.SetActive(true);
		}
		base.gameObject.name = "ScheduleBlock_" + idx.ToString();
	}

	// Token: 0x06006040 RID: 24640 RVA: 0x00239410 File Offset: 0x00237610
	public void SetBlockTypes(List<ScheduleBlockType> blockTypes)
	{
		ScheduleGroup scheduleGroup = Db.Get().ScheduleGroups.FindGroupForScheduleTypes(blockTypes);
		if (scheduleGroup != null && this.paintStyles.ContainsKey(scheduleGroup.Id))
		{
			this.image.colorStyleSetting = this.paintStyles[scheduleGroup.Id];
			this.image.ApplyColorStyleSetting();
			this.toolTip.SetSimpleTooltip(scheduleGroup.GetTooltip());
			return;
		}
		this.toolTip.SetSimpleTooltip("UNKNOWN");
	}

	// Token: 0x04004185 RID: 16773
	[SerializeField]
	private KImage image;

	// Token: 0x04004186 RID: 16774
	[SerializeField]
	private ToolTip toolTip;

	// Token: 0x04004188 RID: 16776
	private Dictionary<string, ColorStyleSetting> paintStyles;
}
