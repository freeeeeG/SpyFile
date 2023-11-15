using System;
using UnityEngine;

// Token: 0x020004CA RID: 1226
[AddComponentMenu("KMonoBehaviour/scripts/LiquidSourceManager")]
public class LiquidSourceManager : KMonoBehaviour, IChunkManager
{
	// Token: 0x06001BF3 RID: 7155 RVA: 0x00094F3F File Offset: 0x0009313F
	protected override void OnPrefabInit()
	{
		LiquidSourceManager.Instance = this;
	}

	// Token: 0x06001BF4 RID: 7156 RVA: 0x00094F47 File Offset: 0x00093147
	public SubstanceChunk CreateChunk(SimHashes element_id, float mass, float temperature, byte diseaseIdx, int diseaseCount, Vector3 position)
	{
		return this.CreateChunk(ElementLoader.FindElementByHash(element_id), mass, temperature, diseaseIdx, diseaseCount, position);
	}

	// Token: 0x06001BF5 RID: 7157 RVA: 0x00094F5D File Offset: 0x0009315D
	public SubstanceChunk CreateChunk(Element element, float mass, float temperature, byte diseaseIdx, int diseaseCount, Vector3 position)
	{
		return GeneratedOre.CreateChunk(element, mass, temperature, diseaseIdx, diseaseCount, position);
	}

	// Token: 0x04000F76 RID: 3958
	public static LiquidSourceManager Instance;
}
