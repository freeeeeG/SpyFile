using System;
using System.Collections.Generic;

// Token: 0x0200002F RID: 47
public class WaveInfoData
{
	// Token: 0x060000F0 RID: 240 RVA: 0x00004EA2 File Offset: 0x000030A2
	public WaveInfoData()
	{
		this.list_Data = new List<WaveInfoMonsterData>();
	}

	// Token: 0x060000F1 RID: 241 RVA: 0x00004EB8 File Offset: 0x000030B8
	public void AddData(eMonsterType type, int count)
	{
		if (this.list_Data.Exists((WaveInfoMonsterData a) => a.type == type))
		{
			this.list_Data.Find((WaveInfoMonsterData a) => a.type == type).count += count;
			return;
		}
		this.list_Data.Add(new WaveInfoMonsterData(type, count));
	}

	// Token: 0x060000F2 RID: 242 RVA: 0x00004F27 File Offset: 0x00003127
	public List<WaveInfoMonsterData> GetData()
	{
		return this.list_Data;
	}

	// Token: 0x040000AB RID: 171
	private List<WaveInfoMonsterData> list_Data;
}
