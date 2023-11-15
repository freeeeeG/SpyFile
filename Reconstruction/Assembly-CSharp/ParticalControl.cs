using System;
using UnityEngine;

// Token: 0x02000177 RID: 375
public class ParticalControl : ReusableObject
{
	// Token: 0x06000982 RID: 2434 RVA: 0x00019248 File Offset: 0x00017448
	private void Awake()
	{
		this.ps = base.GetComponent<ParticleSystem>();
	}

	// Token: 0x06000983 RID: 2435 RVA: 0x00019256 File Offset: 0x00017456
	private void FixedUpdate()
	{
		if (!this.ps.IsAlive())
		{
			Singleton<ObjectPool>.Instance.UnSpawn(this);
		}
	}

	// Token: 0x06000984 RID: 2436 RVA: 0x00019270 File Offset: 0x00017470
	public void PlayEffect()
	{
		this.ps.Play();
	}

	// Token: 0x040004DD RID: 1245
	private ParticleSystem ps;
}
