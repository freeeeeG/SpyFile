using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000A11 RID: 2577
[AddComponentMenu("KMonoBehaviour/scripts/UserNameable")]
public class UserNameable : KMonoBehaviour
{
	// Token: 0x06004D32 RID: 19762 RVA: 0x001B136D File Offset: 0x001AF56D
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (string.IsNullOrEmpty(this.savedName))
		{
			this.SetName(base.gameObject.GetProperName());
			return;
		}
		this.SetName(this.savedName);
	}

	// Token: 0x06004D33 RID: 19763 RVA: 0x001B13A0 File Offset: 0x001AF5A0
	public void SetName(string name)
	{
		KSelectable component = base.GetComponent<KSelectable>();
		base.name = name;
		if (component != null)
		{
			component.SetName(name);
		}
		base.gameObject.name = name;
		NameDisplayScreen.Instance.UpdateName(base.gameObject);
		if (base.GetComponent<CommandModule>() != null)
		{
			SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(base.GetComponent<LaunchConditionManager>()).SetRocketName(name);
		}
		else if (base.GetComponent<Clustercraft>() != null)
		{
			ClusterNameDisplayScreen.Instance.UpdateName(base.GetComponent<Clustercraft>());
		}
		this.savedName = name;
		base.Trigger(1102426921, name);
	}

	// Token: 0x0400325B RID: 12891
	[Serialize]
	public string savedName = "";
}
