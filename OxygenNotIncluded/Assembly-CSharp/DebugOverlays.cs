using System;

// Token: 0x02000A5C RID: 2652
public class DebugOverlays : KScreen
{
	// Token: 0x170005F8 RID: 1528
	// (get) Token: 0x0600503E RID: 20542 RVA: 0x001C7167 File Offset: 0x001C5367
	// (set) Token: 0x0600503F RID: 20543 RVA: 0x001C716E File Offset: 0x001C536E
	public static DebugOverlays instance { get; private set; }

	// Token: 0x06005040 RID: 20544 RVA: 0x001C7178 File Offset: 0x001C5378
	protected override void OnPrefabInit()
	{
		DebugOverlays.instance = this;
		KPopupMenu componentInChildren = base.GetComponentInChildren<KPopupMenu>();
		componentInChildren.SetOptions(new string[]
		{
			"None",
			"Rooms",
			"Lighting",
			"Style",
			"Flow"
		});
		KPopupMenu kpopupMenu = componentInChildren;
		kpopupMenu.OnSelect = (Action<string, int>)Delegate.Combine(kpopupMenu.OnSelect, new Action<string, int>(this.OnSelect));
		base.gameObject.SetActive(false);
	}

	// Token: 0x06005041 RID: 20545 RVA: 0x001C71F4 File Offset: 0x001C53F4
	private void OnSelect(string str, int index)
	{
		if (str != null)
		{
			if (str == "None")
			{
				SimDebugView.Instance.SetMode(OverlayModes.None.ID);
				return;
			}
			if (str == "Flow")
			{
				SimDebugView.Instance.SetMode(SimDebugView.OverlayModes.Flow);
				return;
			}
			if (str == "Lighting")
			{
				SimDebugView.Instance.SetMode(OverlayModes.Light.ID);
				return;
			}
			if (str == "Rooms")
			{
				SimDebugView.Instance.SetMode(OverlayModes.Rooms.ID);
				return;
			}
		}
		Debug.LogError("Unknown debug view: " + str);
	}
}
