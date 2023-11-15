using System;
using UnityEngine;

// Token: 0x020004A7 RID: 1191
[AddComponentMenu("KMonoBehaviour/scripts/DestroyAfter")]
public class DestroyAfter : KMonoBehaviour
{
	// Token: 0x06001AE4 RID: 6884 RVA: 0x000900F3 File Offset: 0x0008E2F3
	protected override void OnSpawn()
	{
		this.particleSystems = base.gameObject.GetComponentsInChildren<ParticleSystem>(true);
	}

	// Token: 0x06001AE5 RID: 6885 RVA: 0x00090108 File Offset: 0x0008E308
	private bool IsAlive()
	{
		for (int i = 0; i < this.particleSystems.Length; i++)
		{
			if (this.particleSystems[i].IsAlive(false))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06001AE6 RID: 6886 RVA: 0x0009013B File Offset: 0x0008E33B
	private void Update()
	{
		if (this.particleSystems != null && !this.IsAlive())
		{
			this.DeleteObject();
		}
	}

	// Token: 0x04000EF1 RID: 3825
	private ParticleSystem[] particleSystems;
}
