using System;
using UnityEngine;

// Token: 0x02000B64 RID: 2916
public class PortraitTableColumn : TableColumn
{
	// Token: 0x06005A2A RID: 23082 RVA: 0x00210614 File Offset: 0x0020E814
	public PortraitTableColumn(Action<IAssignableIdentity, GameObject> on_load_action, Comparison<IAssignableIdentity> sort_comparison, bool double_click_to_target = true) : base(on_load_action, sort_comparison, null, null, null, false, "")
	{
		this.double_click_to_target = double_click_to_target;
	}

	// Token: 0x06005A2B RID: 23083 RVA: 0x00210643 File Offset: 0x0020E843
	public override GameObject GetDefaultWidget(GameObject parent)
	{
		GameObject gameObject = Util.KInstantiateUI(this.prefab_portrait, parent, true);
		gameObject.GetComponent<CrewPortrait>().targetImage.enabled = true;
		return gameObject;
	}

	// Token: 0x06005A2C RID: 23084 RVA: 0x00210663 File Offset: 0x0020E863
	public override GameObject GetHeaderWidget(GameObject parent)
	{
		return Util.KInstantiateUI(this.prefab_portrait, parent, true);
	}

	// Token: 0x06005A2D RID: 23085 RVA: 0x00210674 File Offset: 0x0020E874
	public override GameObject GetMinionWidget(GameObject parent)
	{
		GameObject gameObject = Util.KInstantiateUI(this.prefab_portrait, parent, true);
		if (this.double_click_to_target)
		{
			gameObject.GetComponent<KButton>().onClick += delegate()
			{
				parent.GetComponent<TableRow>().SelectMinion();
			};
			gameObject.GetComponent<KButton>().onDoubleClick += delegate()
			{
				parent.GetComponent<TableRow>().SelectAndFocusMinion();
			};
		}
		return gameObject;
	}

	// Token: 0x04003D1C RID: 15644
	public GameObject prefab_portrait = Assets.UIPrefabs.TableScreenWidgets.MinionPortrait;

	// Token: 0x04003D1D RID: 15645
	private bool double_click_to_target;
}
