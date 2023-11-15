using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020005AA RID: 1450
[AddComponentMenu("KMonoBehaviour/scripts/BuildingElementEmitter")]
public class BuildingElementEmitter : KMonoBehaviour, IGameObjectEffectDescriptor, IElementEmitter, ISim200ms
{
	// Token: 0x170001A2 RID: 418
	// (get) Token: 0x0600236F RID: 9071 RVA: 0x000C2384 File Offset: 0x000C0584
	public float AverageEmitRate
	{
		get
		{
			return Game.Instance.accumulators.GetAverageRate(this.accumulator);
		}
	}

	// Token: 0x170001A3 RID: 419
	// (get) Token: 0x06002370 RID: 9072 RVA: 0x000C239B File Offset: 0x000C059B
	public float EmitRate
	{
		get
		{
			return this.emitRate;
		}
	}

	// Token: 0x170001A4 RID: 420
	// (get) Token: 0x06002371 RID: 9073 RVA: 0x000C23A3 File Offset: 0x000C05A3
	public SimHashes Element
	{
		get
		{
			return this.element;
		}
	}

	// Token: 0x06002372 RID: 9074 RVA: 0x000C23AB File Offset: 0x000C05AB
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.accumulator = Game.Instance.accumulators.Add("Element", this);
		base.Subscribe<BuildingElementEmitter>(824508782, BuildingElementEmitter.OnActiveChangedDelegate);
		this.SimRegister();
	}

	// Token: 0x06002373 RID: 9075 RVA: 0x000C23E5 File Offset: 0x000C05E5
	protected override void OnCleanUp()
	{
		Game.Instance.accumulators.Remove(this.accumulator);
		this.SimUnregister();
		base.OnCleanUp();
	}

	// Token: 0x06002374 RID: 9076 RVA: 0x000C2409 File Offset: 0x000C0609
	private void OnActiveChanged(object data)
	{
		this.simActive = ((Operational)data).IsActive;
		this.dirty = true;
	}

	// Token: 0x06002375 RID: 9077 RVA: 0x000C2423 File Offset: 0x000C0623
	public void Sim200ms(float dt)
	{
		this.UnsafeUpdate(dt);
	}

	// Token: 0x06002376 RID: 9078 RVA: 0x000C242C File Offset: 0x000C062C
	private unsafe void UnsafeUpdate(float dt)
	{
		if (!Sim.IsValidHandle(this.simHandle))
		{
			return;
		}
		this.UpdateSimState();
		int handleIndex = Sim.GetHandleIndex(this.simHandle);
		Sim.EmittedMassInfo emittedMassInfo = Game.Instance.simData.emittedMassEntries[handleIndex];
		if (emittedMassInfo.mass > 0f)
		{
			Game.Instance.accumulators.Accumulate(this.accumulator, emittedMassInfo.mass);
			if (this.element == SimHashes.Oxygen)
			{
				ReportManager.Instance.ReportValue(ReportManager.ReportType.OxygenCreated, emittedMassInfo.mass, base.gameObject.GetProperName(), null);
			}
		}
	}

	// Token: 0x06002377 RID: 9079 RVA: 0x000C24CC File Offset: 0x000C06CC
	private void UpdateSimState()
	{
		if (!this.dirty)
		{
			return;
		}
		this.dirty = false;
		if (this.simActive)
		{
			if (this.element != (SimHashes)0 && this.emitRate > 0f)
			{
				int game_cell = Grid.PosToCell(new Vector3(base.transform.GetPosition().x + this.modifierOffset.x, base.transform.GetPosition().y + this.modifierOffset.y, 0f));
				SimMessages.ModifyElementEmitter(this.simHandle, game_cell, (int)this.emitRange, this.element, 0.2f, this.emitRate * 0.2f, this.temperature, float.MaxValue, this.emitDiseaseIdx, this.emitDiseaseCount);
			}
			this.statusHandle = base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.EmittingElement, this);
			return;
		}
		SimMessages.ModifyElementEmitter(this.simHandle, 0, 0, SimHashes.Vacuum, 0f, 0f, 0f, 0f, byte.MaxValue, 0);
		this.statusHandle = base.GetComponent<KSelectable>().RemoveStatusItem(this.statusHandle, this);
	}

	// Token: 0x06002378 RID: 9080 RVA: 0x000C2604 File Offset: 0x000C0804
	private void SimRegister()
	{
		if (base.isSpawned && this.simHandle == -1)
		{
			this.simHandle = -2;
			SimMessages.AddElementEmitter(float.MaxValue, Game.Instance.simComponentCallbackManager.Add(new Action<int, object>(BuildingElementEmitter.OnSimRegisteredCallback), this, "BuildingElementEmitter").index, -1, -1);
		}
	}

	// Token: 0x06002379 RID: 9081 RVA: 0x000C265F File Offset: 0x000C085F
	private void SimUnregister()
	{
		if (this.simHandle != -1)
		{
			if (Sim.IsValidHandle(this.simHandle))
			{
				SimMessages.RemoveElementEmitter(-1, this.simHandle);
			}
			this.simHandle = -1;
		}
	}

	// Token: 0x0600237A RID: 9082 RVA: 0x000C268A File Offset: 0x000C088A
	private static void OnSimRegisteredCallback(int handle, object data)
	{
		((BuildingElementEmitter)data).OnSimRegistered(handle);
	}

	// Token: 0x0600237B RID: 9083 RVA: 0x000C2698 File Offset: 0x000C0898
	private void OnSimRegistered(int handle)
	{
		if (this != null)
		{
			this.simHandle = handle;
			return;
		}
		SimMessages.RemoveElementEmitter(-1, handle);
	}

	// Token: 0x0600237C RID: 9084 RVA: 0x000C26B4 File Offset: 0x000C08B4
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		string arg = ElementLoader.FindElementByHash(this.element).tag.ProperName();
		Descriptor item = default(Descriptor);
		item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTEMITTED_FIXEDTEMP, arg, GameUtil.GetFormattedMass(this.EmitRate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), GameUtil.GetFormattedTemperature(this.temperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTEMITTED_FIXEDTEMP, arg, GameUtil.GetFormattedMass(this.EmitRate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), GameUtil.GetFormattedTemperature(this.temperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), Descriptor.DescriptorType.Effect);
		list.Add(item);
		return list;
	}

	// Token: 0x04001436 RID: 5174
	[SerializeField]
	public float emitRate = 0.3f;

	// Token: 0x04001437 RID: 5175
	[SerializeField]
	[Serialize]
	public float temperature = 293f;

	// Token: 0x04001438 RID: 5176
	[SerializeField]
	[HashedEnum]
	public SimHashes element = SimHashes.Oxygen;

	// Token: 0x04001439 RID: 5177
	[SerializeField]
	public Vector2 modifierOffset;

	// Token: 0x0400143A RID: 5178
	[SerializeField]
	public byte emitRange = 1;

	// Token: 0x0400143B RID: 5179
	[SerializeField]
	public byte emitDiseaseIdx = byte.MaxValue;

	// Token: 0x0400143C RID: 5180
	[SerializeField]
	public int emitDiseaseCount;

	// Token: 0x0400143D RID: 5181
	private HandleVector<int>.Handle accumulator = HandleVector<int>.InvalidHandle;

	// Token: 0x0400143E RID: 5182
	private int simHandle = -1;

	// Token: 0x0400143F RID: 5183
	private bool simActive;

	// Token: 0x04001440 RID: 5184
	private bool dirty = true;

	// Token: 0x04001441 RID: 5185
	private Guid statusHandle;

	// Token: 0x04001442 RID: 5186
	private static readonly EventSystem.IntraObjectHandler<BuildingElementEmitter> OnActiveChangedDelegate = new EventSystem.IntraObjectHandler<BuildingElementEmitter>(delegate(BuildingElementEmitter component, object data)
	{
		component.OnActiveChanged(data);
	});
}
