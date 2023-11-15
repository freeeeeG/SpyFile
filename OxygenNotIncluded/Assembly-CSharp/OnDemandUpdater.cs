using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020008C9 RID: 2249
public class OnDemandUpdater : MonoBehaviour
{
	// Token: 0x0600411F RID: 16671 RVA: 0x0016CA63 File Offset: 0x0016AC63
	public static void DestroyInstance()
	{
		OnDemandUpdater.Instance = null;
	}

	// Token: 0x06004120 RID: 16672 RVA: 0x0016CA6B File Offset: 0x0016AC6B
	private void Awake()
	{
		OnDemandUpdater.Instance = this;
	}

	// Token: 0x06004121 RID: 16673 RVA: 0x0016CA73 File Offset: 0x0016AC73
	public void Register(IUpdateOnDemand updater)
	{
		if (!this.Updaters.Contains(updater))
		{
			this.Updaters.Add(updater);
		}
	}

	// Token: 0x06004122 RID: 16674 RVA: 0x0016CA8F File Offset: 0x0016AC8F
	public void Unregister(IUpdateOnDemand updater)
	{
		if (this.Updaters.Contains(updater))
		{
			this.Updaters.Remove(updater);
		}
	}

	// Token: 0x06004123 RID: 16675 RVA: 0x0016CAAC File Offset: 0x0016ACAC
	private void Update()
	{
		for (int i = 0; i < this.Updaters.Count; i++)
		{
			if (this.Updaters[i] != null)
			{
				this.Updaters[i].UpdateOnDemand();
			}
		}
	}

	// Token: 0x04002A67 RID: 10855
	private List<IUpdateOnDemand> Updaters = new List<IUpdateOnDemand>();

	// Token: 0x04002A68 RID: 10856
	public static OnDemandUpdater Instance;
}
