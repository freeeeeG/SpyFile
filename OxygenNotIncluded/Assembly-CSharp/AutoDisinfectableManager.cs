using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000481 RID: 1153
[AddComponentMenu("KMonoBehaviour/scripts/AutoDisinfectableManager")]
public class AutoDisinfectableManager : KMonoBehaviour, ISim1000ms
{
	// Token: 0x0600195A RID: 6490 RVA: 0x00084CCB File Offset: 0x00082ECB
	public static void DestroyInstance()
	{
		AutoDisinfectableManager.Instance = null;
	}

	// Token: 0x0600195B RID: 6491 RVA: 0x00084CD3 File Offset: 0x00082ED3
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		AutoDisinfectableManager.Instance = this;
	}

	// Token: 0x0600195C RID: 6492 RVA: 0x00084CE1 File Offset: 0x00082EE1
	public void AddAutoDisinfectable(AutoDisinfectable auto_disinfectable)
	{
		this.autoDisinfectables.Add(auto_disinfectable);
	}

	// Token: 0x0600195D RID: 6493 RVA: 0x00084CEF File Offset: 0x00082EEF
	public void RemoveAutoDisinfectable(AutoDisinfectable auto_disinfectable)
	{
		auto_disinfectable.CancelChore();
		this.autoDisinfectables.Remove(auto_disinfectable);
	}

	// Token: 0x0600195E RID: 6494 RVA: 0x00084D04 File Offset: 0x00082F04
	public void Sim1000ms(float dt)
	{
		for (int i = 0; i < this.autoDisinfectables.Count; i++)
		{
			this.autoDisinfectables[i].RefreshChore();
		}
	}

	// Token: 0x04000DFC RID: 3580
	private List<AutoDisinfectable> autoDisinfectables = new List<AutoDisinfectable>();

	// Token: 0x04000DFD RID: 3581
	public static AutoDisinfectableManager Instance;
}
