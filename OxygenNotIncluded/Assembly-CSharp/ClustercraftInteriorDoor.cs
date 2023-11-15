using System;
using STRINGS;

// Token: 0x02000998 RID: 2456
public class ClustercraftInteriorDoor : KMonoBehaviour, ISidescreenButtonControl
{
	// Token: 0x17000540 RID: 1344
	// (get) Token: 0x060048E0 RID: 18656 RVA: 0x0019A94E File Offset: 0x00198B4E
	public string SidescreenButtonText
	{
		get
		{
			return UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.BUTTONVIEWEXTERIOR.LABEL;
		}
	}

	// Token: 0x17000541 RID: 1345
	// (get) Token: 0x060048E1 RID: 18657 RVA: 0x0019A95A File Offset: 0x00198B5A
	public string SidescreenButtonTooltip
	{
		get
		{
			return this.SidescreenButtonInteractable() ? UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.BUTTONVIEWEXTERIOR.LABEL : UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.BUTTONVIEWEXTERIOR.INVALID;
		}
	}

	// Token: 0x060048E2 RID: 18658 RVA: 0x0019A975 File Offset: 0x00198B75
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Components.ClusterCraftInteriorDoors.Add(this);
	}

	// Token: 0x060048E3 RID: 18659 RVA: 0x0019A988 File Offset: 0x00198B88
	protected override void OnCleanUp()
	{
		Components.ClusterCraftInteriorDoors.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x060048E4 RID: 18660 RVA: 0x0019A99B File Offset: 0x00198B9B
	public bool SidescreenEnabled()
	{
		return true;
	}

	// Token: 0x060048E5 RID: 18661 RVA: 0x0019A9A0 File Offset: 0x00198BA0
	public bool SidescreenButtonInteractable()
	{
		WorldContainer myWorld = base.gameObject.GetMyWorld();
		return myWorld.ParentWorldId != 255 && myWorld.ParentWorldId != myWorld.id;
	}

	// Token: 0x060048E6 RID: 18662 RVA: 0x0019A9D9 File Offset: 0x00198BD9
	public void OnSidescreenButtonPressed()
	{
		ClusterManager.Instance.SetActiveWorld(base.gameObject.GetMyWorld().ParentWorldId);
	}

	// Token: 0x060048E7 RID: 18663 RVA: 0x0019A9F5 File Offset: 0x00198BF5
	public int ButtonSideScreenSortOrder()
	{
		return 20;
	}

	// Token: 0x060048E8 RID: 18664 RVA: 0x0019A9F9 File Offset: 0x00198BF9
	public void SetButtonTextOverride(ButtonMenuTextOverride text)
	{
		throw new NotImplementedException();
	}

	// Token: 0x060048E9 RID: 18665 RVA: 0x0019AA00 File Offset: 0x00198C00
	public int HorizontalGroupID()
	{
		return -1;
	}
}
