using System;
using Unity.Mathematics;

// Token: 0x02000061 RID: 97
[Serializable]
public class Record
{
	// Token: 0x060003B1 RID: 945 RVA: 0x00017290 File Offset: 0x00015490
	public void NewRecord()
	{
		int num = DataBase.Inst.Data_Upgrades.Length;
		this.upgradeGain = new int[num];
		for (int i = 0; i < num; i++)
		{
			this.upgradeGain[i] = 0;
		}
		this.InitTutorialFlags();
	}

	// Token: 0x060003B2 RID: 946 RVA: 0x000172D4 File Offset: 0x000154D4
	public void InitTutorialFlags()
	{
		int num = DataBase.Inst.dataTutorials.Length;
		this.tutorial_Flags = new bool[num];
		for (int i = 0; i < num; i++)
		{
			this.tutorial_Flags[i] = false;
		}
	}

	// Token: 0x060003B3 RID: 947 RVA: 0x00017310 File Offset: 0x00015510
	public void Clone(Record clone)
	{
		int num = DataBase.Inst.Data_Upgrades.Length;
		this.upgradeGain = new int[num];
		this.NewRecord();
		if (clone == null)
		{
			return;
		}
		int num2 = math.min(num, clone.upgradeGain.Length);
		for (int i = 0; i < num2; i++)
		{
			this.upgradeGain[i] = clone.upgradeGain[i];
		}
		this.tutorial_Inited = clone.tutorial_Inited;
		if (clone.tutorial_Flags != null)
		{
			int num3 = math.min(this.tutorial_Flags.Length, clone.tutorial_Flags.Length);
			for (int j = 0; j < num3; j++)
			{
				this.tutorial_Flags[j] = clone.tutorial_Flags[j];
			}
		}
	}

	// Token: 0x060003B4 RID: 948 RVA: 0x000173B7 File Offset: 0x000155B7
	public void Upgrade_GainOnce(int ID)
	{
		if (ID < 0)
		{
			return;
		}
		this.upgradeGain[ID]++;
	}

	// Token: 0x0400032A RID: 810
	public int[] upgradeGain;

	// Token: 0x0400032B RID: 811
	public bool tutorial_Inited;

	// Token: 0x0400032C RID: 812
	public bool[] tutorial_Flags;
}
