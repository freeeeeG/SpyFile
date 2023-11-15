using System;
using UnityEngine;

// Token: 0x020003DB RID: 987
[RequireComponent(typeof(PilotMovement))]
public class MovingPlatformCosmeticDecisions : MonoBehaviour
{
	// Token: 0x0600123A RID: 4666 RVA: 0x000670C8 File Offset: 0x000654C8
	private void Awake()
	{
		ParticleSystem[] array = base.gameObject.RequestComponentsRecursive<ParticleSystem>();
		this.LightPFX = array.AllRemoved_Predicate((ParticleSystem x) => x.name != this.LightPFXName);
		this.OnPilotStatusChanged(false);
	}

	// Token: 0x0600123B RID: 4667 RVA: 0x00067100 File Offset: 0x00065500
	public void OnPilotStatusChanged(bool _hasPilot)
	{
		for (int i = 0; i < this.LightPFX.Length; i++)
		{
			this.LightPFX[i].gameObject.SetActive(_hasPilot);
		}
		Material material = (!_hasPilot) ? this.InactiveLightstripMaterial : this.ActiveLightstripMaterial;
		for (int j = 0; j < this.LightStripMeshes.Length; j++)
		{
			this.LightStripMeshes[j].material = material;
		}
	}

	// Token: 0x04000E41 RID: 3649
	public Material ActiveLightstripMaterial;

	// Token: 0x04000E42 RID: 3650
	public Material InactiveLightstripMaterial;

	// Token: 0x04000E43 RID: 3651
	public MeshRenderer[] LightStripMeshes = new MeshRenderer[0];

	// Token: 0x04000E44 RID: 3652
	public string LightPFXName = "glow (2)";

	// Token: 0x04000E45 RID: 3653
	[HideInInspector]
	public ParticleSystem[] LightPFX;
}
