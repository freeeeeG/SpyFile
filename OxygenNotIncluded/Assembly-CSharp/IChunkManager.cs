using System;
using UnityEngine;

// Token: 0x02000512 RID: 1298
public interface IChunkManager
{
	// Token: 0x06001F2F RID: 7983
	SubstanceChunk CreateChunk(Element element, float mass, float temperature, byte diseaseIdx, int diseaseCount, Vector3 position);

	// Token: 0x06001F30 RID: 7984
	SubstanceChunk CreateChunk(SimHashes element_id, float mass, float temperature, byte diseaseIdx, int diseaseCount, Vector3 position);
}
