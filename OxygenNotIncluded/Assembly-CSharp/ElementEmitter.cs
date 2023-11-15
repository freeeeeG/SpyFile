using System;
using UnityEngine;

// Token: 0x02000788 RID: 1928
public class ElementEmitter : SimComponent
{
	// Token: 0x170003D1 RID: 977
	// (get) Token: 0x0600356F RID: 13679 RVA: 0x00121C09 File Offset: 0x0011FE09
	// (set) Token: 0x06003570 RID: 13680 RVA: 0x00121C11 File Offset: 0x0011FE11
	public bool isEmitterBlocked { get; private set; }

	// Token: 0x06003571 RID: 13681 RVA: 0x00121C1C File Offset: 0x0011FE1C
	protected override void OnSpawn()
	{
		this.onBlockedHandle = Game.Instance.callbackManager.Add(new Game.CallbackInfo(new System.Action(this.OnEmitterBlocked), true));
		this.onUnblockedHandle = Game.Instance.callbackManager.Add(new Game.CallbackInfo(new System.Action(this.OnEmitterUnblocked), true));
		base.OnSpawn();
	}

	// Token: 0x06003572 RID: 13682 RVA: 0x00121C7D File Offset: 0x0011FE7D
	protected override void OnCleanUp()
	{
		Game.Instance.ManualReleaseHandle(this.onBlockedHandle);
		Game.Instance.ManualReleaseHandle(this.onUnblockedHandle);
		base.OnCleanUp();
	}

	// Token: 0x06003573 RID: 13683 RVA: 0x00121CA5 File Offset: 0x0011FEA5
	public void SetEmitting(bool emitting)
	{
		base.SetSimActive(emitting);
	}

	// Token: 0x06003574 RID: 13684 RVA: 0x00121CB0 File Offset: 0x0011FEB0
	protected override void OnSimActivate()
	{
		int game_cell = Grid.OffsetCell(Grid.PosToCell(base.transform.GetPosition()), (int)this.outputElement.outputElementOffset.x, (int)this.outputElement.outputElementOffset.y);
		if (this.outputElement.elementHash != (SimHashes)0 && this.outputElement.massGenerationRate > 0f && this.emissionFrequency > 0f)
		{
			float emit_temperature = (this.outputElement.minOutputTemperature == 0f) ? base.GetComponent<PrimaryElement>().Temperature : this.outputElement.minOutputTemperature;
			SimMessages.ModifyElementEmitter(this.simHandle, game_cell, (int)this.emitRange, this.outputElement.elementHash, this.emissionFrequency, this.outputElement.massGenerationRate, emit_temperature, this.maxPressure, this.outputElement.addedDiseaseIdx, this.outputElement.addedDiseaseCount);
		}
		if (this.showDescriptor)
		{
			this.statusHandle = base.GetComponent<KSelectable>().ReplaceStatusItem(this.statusHandle, Db.Get().BuildingStatusItems.ElementEmitterOutput, this);
		}
	}

	// Token: 0x06003575 RID: 13685 RVA: 0x00121DCC File Offset: 0x0011FFCC
	protected override void OnSimDeactivate()
	{
		int game_cell = Grid.OffsetCell(Grid.PosToCell(base.transform.GetPosition()), (int)this.outputElement.outputElementOffset.x, (int)this.outputElement.outputElementOffset.y);
		SimMessages.ModifyElementEmitter(this.simHandle, game_cell, (int)this.emitRange, SimHashes.Vacuum, 0f, 0f, 0f, 0f, byte.MaxValue, 0);
		if (this.showDescriptor)
		{
			this.statusHandle = base.GetComponent<KSelectable>().RemoveStatusItem(this.statusHandle, false);
		}
	}

	// Token: 0x06003576 RID: 13686 RVA: 0x00121E64 File Offset: 0x00120064
	public void ForceEmit(float mass, byte disease_idx, int disease_count, float temperature = -1f)
	{
		if (mass <= 0f)
		{
			return;
		}
		float temperature2 = (temperature > 0f) ? temperature : this.outputElement.minOutputTemperature;
		Element element = ElementLoader.FindElementByHash(this.outputElement.elementHash);
		if (element.IsGas || element.IsLiquid)
		{
			SimMessages.AddRemoveSubstance(Grid.PosToCell(base.transform.GetPosition()), this.outputElement.elementHash, CellEventLogger.Instance.ElementConsumerSimUpdate, mass, temperature2, disease_idx, disease_count, true, -1);
		}
		else if (element.IsSolid)
		{
			element.substance.SpawnResource(base.transform.GetPosition() + new Vector3(0f, 0.5f, 0f), mass, temperature2, disease_idx, disease_count, false, true, false);
		}
		PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, ElementLoader.FindElementByHash(this.outputElement.elementHash).name, base.gameObject.transform, 1.5f, false);
	}

	// Token: 0x06003577 RID: 13687 RVA: 0x00121F60 File Offset: 0x00120160
	private void OnEmitterBlocked()
	{
		this.isEmitterBlocked = true;
		base.Trigger(1615168894, this);
	}

	// Token: 0x06003578 RID: 13688 RVA: 0x00121F75 File Offset: 0x00120175
	private void OnEmitterUnblocked()
	{
		this.isEmitterBlocked = false;
		base.Trigger(-657992955, this);
	}

	// Token: 0x06003579 RID: 13689 RVA: 0x00121F8A File Offset: 0x0012018A
	protected override void OnSimRegister(HandleVector<Game.ComplexCallbackInfo<int>>.Handle cb_handle)
	{
		Game.Instance.simComponentCallbackManager.GetItem(cb_handle);
		SimMessages.AddElementEmitter(this.maxPressure, cb_handle.index, this.onBlockedHandle.index, this.onUnblockedHandle.index);
	}

	// Token: 0x0600357A RID: 13690 RVA: 0x00121FC5 File Offset: 0x001201C5
	protected override void OnSimUnregister()
	{
		ElementEmitter.StaticUnregister(this.simHandle);
	}

	// Token: 0x0600357B RID: 13691 RVA: 0x00121FD2 File Offset: 0x001201D2
	private static void StaticUnregister(int sim_handle)
	{
		global::Debug.Assert(Sim.IsValidHandle(sim_handle));
		SimMessages.RemoveElementEmitter(-1, sim_handle);
	}

	// Token: 0x0600357C RID: 13692 RVA: 0x00121FE8 File Offset: 0x001201E8
	private void OnDrawGizmosSelected()
	{
		int cell = Grid.OffsetCell(Grid.PosToCell(base.transform.GetPosition()), (int)this.outputElement.outputElementOffset.x, (int)this.outputElement.outputElementOffset.y);
		Gizmos.color = Color.green;
		Gizmos.DrawSphere(Grid.CellToPos(cell) + Vector3.right / 2f + Vector3.up / 2f, 0.2f);
	}

	// Token: 0x0600357D RID: 13693 RVA: 0x0012206D File Offset: 0x0012026D
	protected override Action<int> GetStaticUnregister()
	{
		return new Action<int>(ElementEmitter.StaticUnregister);
	}

	// Token: 0x040020B3 RID: 8371
	[SerializeField]
	public ElementConverter.OutputElement outputElement;

	// Token: 0x040020B4 RID: 8372
	[SerializeField]
	public float emissionFrequency = 1f;

	// Token: 0x040020B5 RID: 8373
	[SerializeField]
	public byte emitRange = 1;

	// Token: 0x040020B6 RID: 8374
	[SerializeField]
	public float maxPressure = 1f;

	// Token: 0x040020B7 RID: 8375
	private Guid statusHandle = Guid.Empty;

	// Token: 0x040020B8 RID: 8376
	public bool showDescriptor = true;

	// Token: 0x040020B9 RID: 8377
	private HandleVector<Game.CallbackInfo>.Handle onBlockedHandle = HandleVector<Game.CallbackInfo>.InvalidHandle;

	// Token: 0x040020BA RID: 8378
	private HandleVector<Game.CallbackInfo>.Handle onUnblockedHandle = HandleVector<Game.CallbackInfo>.InvalidHandle;
}
