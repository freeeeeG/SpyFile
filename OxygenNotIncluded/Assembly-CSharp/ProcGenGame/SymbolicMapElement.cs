using System;

namespace ProcGenGame
{
	// Token: 0x02000CAC RID: 3244
	public interface SymbolicMapElement
	{
		// Token: 0x06006758 RID: 26456
		void ConvertToMap(Chunk world, TerrainCell.SetValuesFunction SetValues, float temperatureMin, float temperatureRange, SeededRandom rnd);
	}
}
