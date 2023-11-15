using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C50 RID: 3152
public class SpecialCargoBayClusterSideScreen : ReceptacleSideScreen
{
	// Token: 0x060063E1 RID: 25569 RVA: 0x0024EB9F File Offset: 0x0024CD9F
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x060063E2 RID: 25570 RVA: 0x0024EBA7 File Offset: 0x0024CDA7
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<SpecialCargoBayClusterReceptacle>() != null;
	}

	// Token: 0x060063E3 RID: 25571 RVA: 0x0024EBB5 File Offset: 0x0024CDB5
	protected override bool RequiresAvailableAmountToDeposit()
	{
		return false;
	}

	// Token: 0x060063E4 RID: 25572 RVA: 0x0024EBB8 File Offset: 0x0024CDB8
	protected override void UpdateState(object data)
	{
		base.UpdateState(data);
		this.SetDescriptionSidescreenFoldState(this.targetReceptacle != null && this.targetReceptacle.Occupant == null);
	}

	// Token: 0x060063E5 RID: 25573 RVA: 0x0024EBEC File Offset: 0x0024CDEC
	protected override void SetResultDescriptions(GameObject go)
	{
		base.SetResultDescriptions(go);
		if (this.targetReceptacle != null && this.targetReceptacle.Occupant != null)
		{
			this.descriptionLabel.SetText("");
			this.SetDescriptionSidescreenFoldState(false);
			return;
		}
		this.SetDescriptionSidescreenFoldState(true);
	}

	// Token: 0x060063E6 RID: 25574 RVA: 0x0024EC40 File Offset: 0x0024CE40
	public void SetDescriptionSidescreenFoldState(bool visible)
	{
		this.descriptionContent.minHeight = (visible ? this.descriptionLayoutDefaultSize : 0f);
	}

	// Token: 0x04004426 RID: 17446
	public LayoutElement descriptionContent;

	// Token: 0x04004427 RID: 17447
	public float descriptionLayoutDefaultSize = -1f;
}
