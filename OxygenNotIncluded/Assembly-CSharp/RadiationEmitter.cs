using System;
using UnityEngine;

// Token: 0x020008FF RID: 2303
public class RadiationEmitter : SimComponent
{
	// Token: 0x060042BF RID: 17087 RVA: 0x00175869 File Offset: 0x00173A69
	protected override void OnSpawn()
	{
		Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(base.transform, new System.Action(this.OnCellChange), "RadiationEmitter.OnSpawn");
		base.OnSpawn();
	}

	// Token: 0x060042C0 RID: 17088 RVA: 0x00175893 File Offset: 0x00173A93
	protected override void OnCleanUp()
	{
		Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(base.transform, new System.Action(this.OnCellChange));
		base.OnCleanUp();
	}

	// Token: 0x060042C1 RID: 17089 RVA: 0x001758B7 File Offset: 0x00173AB7
	public void SetEmitting(bool emitting)
	{
		base.SetSimActive(emitting);
	}

	// Token: 0x060042C2 RID: 17090 RVA: 0x001758C0 File Offset: 0x00173AC0
	public int GetEmissionCell()
	{
		return Grid.PosToCell(base.transform.GetPosition() + this.emissionOffset);
	}

	// Token: 0x060042C3 RID: 17091 RVA: 0x001758E0 File Offset: 0x00173AE0
	public void Refresh()
	{
		int emissionCell = this.GetEmissionCell();
		if (this.radiusProportionalToRads)
		{
			this.SetRadiusProportionalToRads();
		}
		SimMessages.ModifyRadiationEmitter(this.simHandle, emissionCell, this.emitRadiusX, this.emitRadiusY, this.emitRads, this.emitRate, this.emitSpeed, this.emitDirection, this.emitAngle, this.emitType);
	}

	// Token: 0x060042C4 RID: 17092 RVA: 0x0017593E File Offset: 0x00173B3E
	private void OnCellChange()
	{
		this.Refresh();
	}

	// Token: 0x060042C5 RID: 17093 RVA: 0x00175948 File Offset: 0x00173B48
	private void SetRadiusProportionalToRads()
	{
		this.emitRadiusX = (short)Mathf.Clamp(Mathf.RoundToInt(this.emitRads * 1f), 1, 128);
		this.emitRadiusY = (short)Mathf.Clamp(Mathf.RoundToInt(this.emitRads * 1f), 1, 128);
	}

	// Token: 0x060042C6 RID: 17094 RVA: 0x0017599C File Offset: 0x00173B9C
	protected override void OnSimActivate()
	{
		int emissionCell = this.GetEmissionCell();
		if (this.radiusProportionalToRads)
		{
			this.SetRadiusProportionalToRads();
		}
		SimMessages.ModifyRadiationEmitter(this.simHandle, emissionCell, this.emitRadiusX, this.emitRadiusY, this.emitRads, this.emitRate, this.emitSpeed, this.emitDirection, this.emitAngle, this.emitType);
	}

	// Token: 0x060042C7 RID: 17095 RVA: 0x001759FC File Offset: 0x00173BFC
	protected override void OnSimDeactivate()
	{
		int emissionCell = this.GetEmissionCell();
		SimMessages.ModifyRadiationEmitter(this.simHandle, emissionCell, 0, 0, 0f, 0f, 0f, 0f, 0f, this.emitType);
	}

	// Token: 0x060042C8 RID: 17096 RVA: 0x00175A40 File Offset: 0x00173C40
	protected override void OnSimRegister(HandleVector<Game.ComplexCallbackInfo<int>>.Handle cb_handle)
	{
		Game.Instance.simComponentCallbackManager.GetItem(cb_handle);
		int emissionCell = this.GetEmissionCell();
		SimMessages.AddRadiationEmitter(cb_handle.index, emissionCell, 0, 0, 0f, 0f, 0f, 0f, 0f, this.emitType);
	}

	// Token: 0x060042C9 RID: 17097 RVA: 0x00175A93 File Offset: 0x00173C93
	protected override void OnSimUnregister()
	{
		RadiationEmitter.StaticUnregister(this.simHandle);
	}

	// Token: 0x060042CA RID: 17098 RVA: 0x00175AA0 File Offset: 0x00173CA0
	private static void StaticUnregister(int sim_handle)
	{
		global::Debug.Assert(Sim.IsValidHandle(sim_handle));
		SimMessages.RemoveRadiationEmitter(-1, sim_handle);
	}

	// Token: 0x060042CB RID: 17099 RVA: 0x00175AB4 File Offset: 0x00173CB4
	private void OnDrawGizmosSelected()
	{
		int emissionCell = this.GetEmissionCell();
		Gizmos.color = Color.green;
		Gizmos.DrawSphere(Grid.CellToPos(emissionCell) + Vector3.right / 2f + Vector3.up / 2f, 0.2f);
	}

	// Token: 0x060042CC RID: 17100 RVA: 0x00175B08 File Offset: 0x00173D08
	protected override Action<int> GetStaticUnregister()
	{
		return new Action<int>(RadiationEmitter.StaticUnregister);
	}

	// Token: 0x04002B86 RID: 11142
	public bool radiusProportionalToRads;

	// Token: 0x04002B87 RID: 11143
	[SerializeField]
	public short emitRadiusX = 4;

	// Token: 0x04002B88 RID: 11144
	[SerializeField]
	public short emitRadiusY = 4;

	// Token: 0x04002B89 RID: 11145
	[SerializeField]
	public float emitRads = 10f;

	// Token: 0x04002B8A RID: 11146
	[SerializeField]
	public float emitRate = 1f;

	// Token: 0x04002B8B RID: 11147
	[SerializeField]
	public float emitSpeed = 1f;

	// Token: 0x04002B8C RID: 11148
	[SerializeField]
	public float emitDirection;

	// Token: 0x04002B8D RID: 11149
	[SerializeField]
	public float emitAngle = 360f;

	// Token: 0x04002B8E RID: 11150
	[SerializeField]
	public RadiationEmitter.RadiationEmitterType emitType;

	// Token: 0x04002B8F RID: 11151
	[SerializeField]
	public Vector3 emissionOffset = Vector3.zero;

	// Token: 0x02001753 RID: 5971
	public enum RadiationEmitterType
	{
		// Token: 0x04006E64 RID: 28260
		Constant,
		// Token: 0x04006E65 RID: 28261
		Pulsing,
		// Token: 0x04006E66 RID: 28262
		PulsingAveraged,
		// Token: 0x04006E67 RID: 28263
		SimplePulse,
		// Token: 0x04006E68 RID: 28264
		RadialBeams,
		// Token: 0x04006E69 RID: 28265
		Attractor
	}
}
