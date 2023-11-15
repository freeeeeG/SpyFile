using System;
using UnityEngine;

// Token: 0x02000C23 RID: 3107
public class IncubatorSideScreen : ReceptacleSideScreen
{
	// Token: 0x0600624E RID: 25166 RVA: 0x00244B5F File Offset: 0x00242D5F
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<EggIncubator>() != null;
	}

	// Token: 0x0600624F RID: 25167 RVA: 0x00244B70 File Offset: 0x00242D70
	protected override void SetResultDescriptions(GameObject go)
	{
		string text = "";
		InfoDescription component = go.GetComponent<InfoDescription>();
		if (component)
		{
			text += component.description;
		}
		this.descriptionLabel.SetText(text);
	}

	// Token: 0x06006250 RID: 25168 RVA: 0x00244BAB File Offset: 0x00242DAB
	protected override bool RequiresAvailableAmountToDeposit()
	{
		return false;
	}

	// Token: 0x06006251 RID: 25169 RVA: 0x00244BAE File Offset: 0x00242DAE
	protected override Sprite GetEntityIcon(Tag prefabTag)
	{
		return Def.GetUISprite(Assets.GetPrefab(prefabTag), "ui", false).first;
	}

	// Token: 0x06006252 RID: 25170 RVA: 0x00244BC8 File Offset: 0x00242DC8
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		EggIncubator incubator = target.GetComponent<EggIncubator>();
		this.continuousToggle.ChangeState(incubator.autoReplaceEntity ? 0 : 1);
		this.continuousToggle.onClick = delegate()
		{
			incubator.autoReplaceEntity = !incubator.autoReplaceEntity;
			this.continuousToggle.ChangeState(incubator.autoReplaceEntity ? 0 : 1);
		};
	}

	// Token: 0x040042FF RID: 17151
	public DescriptorPanel RequirementsDescriptorPanel;

	// Token: 0x04004300 RID: 17152
	public DescriptorPanel HarvestDescriptorPanel;

	// Token: 0x04004301 RID: 17153
	public DescriptorPanel EffectsDescriptorPanel;

	// Token: 0x04004302 RID: 17154
	public MultiToggle continuousToggle;
}
