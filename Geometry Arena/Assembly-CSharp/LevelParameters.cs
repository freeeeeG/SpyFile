using System;
using UnityEngine;

// Token: 0x0200002B RID: 43
[Serializable]
public class LevelParameters
{
	// Token: 0x040001DB RID: 475
	public string name = "Uninited";

	// Token: 0x040001DC RID: 476
	public int no = -1;

	// Token: 0x040001DD RID: 477
	[SerializeField]
	public WavePara[] waveParas = new WavePara[4];
}
