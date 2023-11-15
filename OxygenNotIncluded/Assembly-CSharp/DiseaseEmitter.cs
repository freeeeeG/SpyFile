using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using UnityEngine;

// Token: 0x0200076E RID: 1902
[AddComponentMenu("KMonoBehaviour/scripts/DiseaseEmitter")]
public class DiseaseEmitter : KMonoBehaviour
{
	// Token: 0x170003BE RID: 958
	// (get) Token: 0x06003489 RID: 13449 RVA: 0x0011A606 File Offset: 0x00118806
	public float EmitRate
	{
		get
		{
			return this.emitRate;
		}
	}

	// Token: 0x0600348A RID: 13450 RVA: 0x0011A610 File Offset: 0x00118810
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.emitDiseases != null)
		{
			this.simHandles = new int[this.emitDiseases.Length];
			for (int i = 0; i < this.simHandles.Length; i++)
			{
				this.simHandles[i] = -1;
			}
		}
		this.SimRegister();
	}

	// Token: 0x0600348B RID: 13451 RVA: 0x0011A660 File Offset: 0x00118860
	protected override void OnCleanUp()
	{
		this.SimUnregister();
		base.OnCleanUp();
	}

	// Token: 0x0600348C RID: 13452 RVA: 0x0011A66E File Offset: 0x0011886E
	public void SetEnable(bool enable)
	{
		if (this.enableEmitter == enable)
		{
			return;
		}
		this.enableEmitter = enable;
		if (this.enableEmitter)
		{
			this.SimRegister();
			return;
		}
		this.SimUnregister();
	}

	// Token: 0x0600348D RID: 13453 RVA: 0x0011A698 File Offset: 0x00118898
	private void OnCellChanged()
	{
		if (this.simHandles == null || !this.enableEmitter)
		{
			return;
		}
		int cell = Grid.PosToCell(this);
		if (Grid.IsValidCell(cell))
		{
			for (int i = 0; i < this.emitDiseases.Length; i++)
			{
				if (Sim.IsValidHandle(this.simHandles[i]))
				{
					SimMessages.ModifyDiseaseEmitter(this.simHandles[i], cell, this.emitRange, this.emitDiseases[i], this.emitRate, this.emitCount);
				}
			}
		}
	}

	// Token: 0x0600348E RID: 13454 RVA: 0x0011A710 File Offset: 0x00118910
	private void SimRegister()
	{
		if (this.simHandles == null || !this.enableEmitter)
		{
			return;
		}
		Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(base.transform, new System.Action(this.OnCellChanged), "DiseaseEmitter.Modify");
		for (int i = 0; i < this.simHandles.Length; i++)
		{
			if (this.simHandles[i] == -1)
			{
				this.simHandles[i] = -2;
				SimMessages.AddDiseaseEmitter(Game.Instance.simComponentCallbackManager.Add(new Action<int, object>(DiseaseEmitter.OnSimRegisteredCallback), this, "DiseaseEmitter").index);
			}
		}
	}

	// Token: 0x0600348F RID: 13455 RVA: 0x0011A7A8 File Offset: 0x001189A8
	private void SimUnregister()
	{
		if (this.simHandles == null)
		{
			return;
		}
		for (int i = 0; i < this.simHandles.Length; i++)
		{
			if (Sim.IsValidHandle(this.simHandles[i]))
			{
				SimMessages.RemoveDiseaseEmitter(-1, this.simHandles[i]);
			}
			this.simHandles[i] = -1;
		}
		Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(base.transform, new System.Action(this.OnCellChanged));
	}

	// Token: 0x06003490 RID: 13456 RVA: 0x0011A813 File Offset: 0x00118A13
	private static void OnSimRegisteredCallback(int handle, object data)
	{
		((DiseaseEmitter)data).OnSimRegistered(handle);
	}

	// Token: 0x06003491 RID: 13457 RVA: 0x0011A824 File Offset: 0x00118A24
	private void OnSimRegistered(int handle)
	{
		bool flag = false;
		if (this != null)
		{
			for (int i = 0; i < this.simHandles.Length; i++)
			{
				if (this.simHandles[i] == -2)
				{
					this.simHandles[i] = handle;
					flag = true;
					break;
				}
			}
			this.OnCellChanged();
		}
		if (!flag)
		{
			SimMessages.RemoveDiseaseEmitter(-1, handle);
		}
	}

	// Token: 0x06003492 RID: 13458 RVA: 0x0011A878 File Offset: 0x00118A78
	public void SetDiseases(List<Disease> diseases)
	{
		this.emitDiseases = new byte[diseases.Count];
		for (int i = 0; i < diseases.Count; i++)
		{
			this.emitDiseases[i] = Db.Get().Diseases.GetIndex(diseases[i].id);
		}
	}

	// Token: 0x04001FBA RID: 8122
	[Serialize]
	public float emitRate = 1f;

	// Token: 0x04001FBB RID: 8123
	[Serialize]
	public byte emitRange;

	// Token: 0x04001FBC RID: 8124
	[Serialize]
	public int emitCount;

	// Token: 0x04001FBD RID: 8125
	[Serialize]
	public byte[] emitDiseases;

	// Token: 0x04001FBE RID: 8126
	public int[] simHandles;

	// Token: 0x04001FBF RID: 8127
	[Serialize]
	private bool enableEmitter;
}
