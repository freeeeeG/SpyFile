using System;
using UnityEngine;

// Token: 0x020004B8 RID: 1208
[AddComponentMenu("KMonoBehaviour/scripts/GasSourceManager")]
public class GasSourceManager : KMonoBehaviour, IChunkManager
{
	// Token: 0x06001B6C RID: 7020 RVA: 0x00093380 File Offset: 0x00091580
	protected override void OnPrefabInit()
	{
		GasSourceManager.Instance = this;
	}

	// Token: 0x06001B6D RID: 7021 RVA: 0x00093388 File Offset: 0x00091588
	public SubstanceChunk CreateChunk(SimHashes element_id, float mass, float temperature, byte diseaseIdx, int diseaseCount, Vector3 position)
	{
		return this.CreateChunk(ElementLoader.FindElementByHash(element_id), mass, temperature, diseaseIdx, diseaseCount, position);
	}

	// Token: 0x06001B6E RID: 7022 RVA: 0x0009339E File Offset: 0x0009159E
	public SubstanceChunk CreateChunk(Element element, float mass, float temperature, byte diseaseIdx, int diseaseCount, Vector3 position)
	{
		return GeneratedOre.CreateChunk(element, mass, temperature, diseaseIdx, diseaseCount, position);
	}

	// Token: 0x04000F4A RID: 3914
	public static GasSourceManager Instance;
}
