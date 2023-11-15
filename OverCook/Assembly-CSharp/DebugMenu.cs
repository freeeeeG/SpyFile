using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200041F RID: 1055
public class DebugMenu : DebugRootMenu
{
	// Token: 0x06001326 RID: 4902 RVA: 0x0006B990 File Offset: 0x00069D90
	public override bool Show(GamepadUser currentGamer, BaseMenuBehaviour parent, GameObject invoker, bool hideInvoker = true)
	{
		if (!base.Show(currentGamer, parent, invoker, hideInvoker))
		{
			return false;
		}
		IT17EventHelper[] componentsInChildren = base.GetComponentsInChildren<IT17EventHelper>(true);
		Dictionary<string, bool> options = DebugManager.Instance.GetOptions();
		foreach (KeyValuePair<string, bool> keyValuePair in options)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.m_prefab);
			this.m_entrys.Add(gameObject);
			DebugMenuButton component = gameObject.GetComponent<DebugMenuButton>();
			component.SetName(keyValuePair.Key);
			component.SetStatus(keyValuePair.Value);
			gameObject.transform.SetParent(this.m_container.transform);
		}
		this.m_scrollView.Show(currentGamer, parent, invoker, hideInvoker);
		return true;
	}

	// Token: 0x06001327 RID: 4903 RVA: 0x0006BA70 File Offset: 0x00069E70
	public override void Close()
	{
		base.Close();
		for (int i = 0; i < this.m_entrys.Count; i++)
		{
			UnityEngine.Object.Destroy(this.m_entrys[i]);
		}
		this.m_entrys.Clear();
		this.m_scrollView.Hide(true, false);
	}

	// Token: 0x04000F0A RID: 3850
	[SerializeField]
	public GameObject m_container;

	// Token: 0x04000F0B RID: 3851
	[SerializeField]
	public GameObject m_prefab;

	// Token: 0x04000F0C RID: 3852
	[SerializeField]
	private T17ScrollView m_scrollView;

	// Token: 0x04000F0D RID: 3853
	private List<GameObject> m_entrys = new List<GameObject>();
}
