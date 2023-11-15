using System;
using System.Collections.Generic;

// Token: 0x02000060 RID: 96
[Serializable]
public class MapData
{
	// Token: 0x0600025F RID: 607 RVA: 0x0000A2C8 File Offset: 0x000084C8
	public MapNodeData AddNode(eStageType type, eMapNodeState state, int step, int indexInStep)
	{
		if (this.list_MapNodeData == null)
		{
			this.list_MapNodeData = new List<MapNodeData>();
		}
		if (this.list_NodeCountInEachStep == null)
		{
			this.list_NodeCountInEachStep = new List<int>();
		}
		MapNodeData mapNodeData = new MapNodeData(this.list_MapNodeData.Count, step, indexInStep, type, state);
		this.list_MapNodeData.Add(mapNodeData);
		if (step >= this.list_NodeCountInEachStep.Count)
		{
			this.list_NodeCountInEachStep.Add(1);
		}
		else
		{
			List<int> list = this.list_NodeCountInEachStep;
			int num = list[step];
			list[step] = num + 1;
		}
		return mapNodeData;
	}

	// Token: 0x040001A9 RID: 425
	public int randomSeed;

	// Token: 0x040001AA RID: 426
	public List<MapNodeData> list_MapNodeData;

	// Token: 0x040001AB RID: 427
	public List<int> list_NodeCountInEachStep;

	// Token: 0x040001AC RID: 428
	public bool IsGenerated;
}
