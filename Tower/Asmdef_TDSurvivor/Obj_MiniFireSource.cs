using System;
using UnityEngine;

// Token: 0x020000C7 RID: 199
public class Obj_MiniFireSource : MonoBehaviour
{
	// Token: 0x06000496 RID: 1174 RVA: 0x000127BD File Offset: 0x000109BD
	private void Start()
	{
	}

	// Token: 0x06000497 RID: 1175 RVA: 0x000127BF File Offset: 0x000109BF
	private void Update()
	{
	}

	// Token: 0x06000498 RID: 1176 RVA: 0x000127C1 File Offset: 0x000109C1
	public void Toggle(bool isOn)
	{
		if (this.isActive == isOn)
		{
			return;
		}
		if (isOn)
		{
			this.particle_Fire.Play();
		}
		else
		{
			this.particle_Fire.Stop();
		}
		this.isActive = isOn;
	}

	// Token: 0x04000474 RID: 1140
	[SerializeField]
	private ParticleSystem particle_Fire;

	// Token: 0x04000475 RID: 1141
	[SerializeField]
	private bool isActive;
}
