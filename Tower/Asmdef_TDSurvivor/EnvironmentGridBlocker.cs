using System;
using UnityEngine;

// Token: 0x0200007D RID: 125
[SelectionBase]
public class EnvironmentGridBlocker : MonoBehaviour
{
	// Token: 0x06000296 RID: 662 RVA: 0x0000AFA0 File Offset: 0x000091A0
	private void Reset()
	{
		if (this.renderer == null)
		{
			this.renderer = base.GetComponent<MeshRenderer>();
		}
		base.gameObject.SetLayer(LayerMask.NameToLayer("PathObstacle"), true);
	}

	// Token: 0x06000297 RID: 663 RVA: 0x0000AFD2 File Offset: 0x000091D2
	private void Awake()
	{
		this.renderer.enabled = false;
	}

	// Token: 0x04000316 RID: 790
	[SerializeField]
	private MeshRenderer renderer;

	// Token: 0x04000317 RID: 791
	[SerializeField]
	private bool hideRendererInPlayMode = true;
}
