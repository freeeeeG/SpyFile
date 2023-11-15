using System;
using System.Collections.Generic;

// Token: 0x02000AFD RID: 2813
public class FabricatorListScreen : KToggleMenu
{
	// Token: 0x060056CE RID: 22222 RVA: 0x001FBC4C File Offset: 0x001F9E4C
	private void Refresh()
	{
		List<KToggleMenu.ToggleInfo> list = new List<KToggleMenu.ToggleInfo>();
		foreach (Fabricator fabricator in Components.Fabricators.Items)
		{
			KSelectable component = fabricator.GetComponent<KSelectable>();
			list.Add(new KToggleMenu.ToggleInfo(component.GetName(), fabricator, global::Action.NumActions));
		}
		base.Setup(list);
	}

	// Token: 0x060056CF RID: 22223 RVA: 0x001FBCC8 File Offset: 0x001F9EC8
	protected override void OnSpawn()
	{
		base.onSelect += this.OnClickFabricator;
	}

	// Token: 0x060056D0 RID: 22224 RVA: 0x001FBCDC File Offset: 0x001F9EDC
	protected override void OnActivate()
	{
		base.OnActivate();
		this.Refresh();
	}

	// Token: 0x060056D1 RID: 22225 RVA: 0x001FBCEC File Offset: 0x001F9EEC
	private void OnClickFabricator(KToggleMenu.ToggleInfo toggle_info)
	{
		Fabricator fabricator = (Fabricator)toggle_info.userData;
		SelectTool.Instance.Select(fabricator.GetComponent<KSelectable>(), false);
	}
}
