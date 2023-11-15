using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000907 RID: 2311
[AddComponentMenu("KMonoBehaviour/Workable/RelaxationPoint")]
public class RelaxationPoint : Workable, IGameObjectEffectDescriptor
{
	// Token: 0x0600430E RID: 17166 RVA: 0x00176FFE File Offset: 0x001751FE
	public RelaxationPoint()
	{
		base.SetReportType(ReportManager.ReportType.PersonalTime);
		this.showProgressBar = false;
	}

	// Token: 0x0600430F RID: 17167 RVA: 0x00177018 File Offset: 0x00175218
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.lightEfficiencyBonus = false;
		base.GetComponent<KPrefabID>().AddTag(TagManager.Create("RelaxationPoint", MISC.TAGS.RELAXATION_POINT), false);
		if (RelaxationPoint.stressReductionEffect == null)
		{
			RelaxationPoint.stressReductionEffect = this.CreateEffect();
			RelaxationPoint.roomStressReductionEffect = this.CreateRoomEffect();
		}
	}

	// Token: 0x06004310 RID: 17168 RVA: 0x00177070 File Offset: 0x00175270
	public Effect CreateEffect()
	{
		Effect effect = new Effect("StressReduction", DUPLICANTS.MODIFIERS.STRESSREDUCTION.NAME, DUPLICANTS.MODIFIERS.STRESSREDUCTION.TOOLTIP, 0f, true, false, false, null, -1f, 0f, null, "");
		AttributeModifier modifier = new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, this.stressModificationValue / 600f, DUPLICANTS.MODIFIERS.STRESSREDUCTION.NAME, false, false, true);
		effect.Add(modifier);
		return effect;
	}

	// Token: 0x06004311 RID: 17169 RVA: 0x001770F4 File Offset: 0x001752F4
	public Effect CreateRoomEffect()
	{
		Effect effect = new Effect("RoomRelaxationEffect", DUPLICANTS.MODIFIERS.STRESSREDUCTION_CLINIC.NAME, DUPLICANTS.MODIFIERS.STRESSREDUCTION_CLINIC.TOOLTIP, 0f, true, false, false, null, -1f, 0f, null, "");
		AttributeModifier modifier = new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, this.roomStressModificationValue / 600f, DUPLICANTS.MODIFIERS.STRESSREDUCTION_CLINIC.NAME, false, false, true);
		effect.Add(modifier);
		return effect;
	}

	// Token: 0x06004312 RID: 17170 RVA: 0x00177177 File Offset: 0x00175377
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.smi = new RelaxationPoint.RelaxationPointSM.Instance(this);
		this.smi.StartSM();
		base.SetWorkTime(float.PositiveInfinity);
	}

	// Token: 0x06004313 RID: 17171 RVA: 0x001771A4 File Offset: 0x001753A4
	protected override void OnStartWork(Worker worker)
	{
		base.OnStartWork(worker);
		if (this.roomTracker != null && this.roomTracker.room != null && this.roomTracker.room.roomType == Db.Get().RoomTypes.MassageClinic)
		{
			worker.GetComponent<Effects>().Add(RelaxationPoint.roomStressReductionEffect, false);
		}
		else
		{
			worker.GetComponent<Effects>().Add(RelaxationPoint.stressReductionEffect, false);
		}
		base.GetComponent<Operational>().SetActive(true, false);
	}

	// Token: 0x06004314 RID: 17172 RVA: 0x00177227 File Offset: 0x00175427
	protected override bool OnWorkTick(Worker worker, float dt)
	{
		if (Db.Get().Amounts.Stress.Lookup(worker.gameObject).value <= this.stopStressingValue)
		{
			return true;
		}
		base.OnWorkTick(worker, dt);
		return false;
	}

	// Token: 0x06004315 RID: 17173 RVA: 0x0017725C File Offset: 0x0017545C
	protected override void OnStopWork(Worker worker)
	{
		worker.GetComponent<Effects>().Remove(RelaxationPoint.stressReductionEffect);
		worker.GetComponent<Effects>().Remove(RelaxationPoint.roomStressReductionEffect);
		base.GetComponent<Operational>().SetActive(false, false);
		base.OnStopWork(worker);
	}

	// Token: 0x06004316 RID: 17174 RVA: 0x00177292 File Offset: 0x00175492
	protected override void OnCompleteWork(Worker worker)
	{
		base.OnCompleteWork(worker);
	}

	// Token: 0x06004317 RID: 17175 RVA: 0x0017729B File Offset: 0x0017549B
	public override bool InstantlyFinish(Worker worker)
	{
		return false;
	}

	// Token: 0x06004318 RID: 17176 RVA: 0x001772A0 File Offset: 0x001754A0
	protected virtual WorkChore<RelaxationPoint> CreateWorkChore()
	{
		return new WorkChore<RelaxationPoint>(Db.Get().ChoreTypes.Relax, this, null, false, null, null, null, false, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
	}

	// Token: 0x06004319 RID: 17177 RVA: 0x001772D4 File Offset: 0x001754D4
	public override List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> descriptors = base.GetDescriptors(go);
		Descriptor item = default(Descriptor);
		item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.STRESSREDUCEDPERMINUTE, GameUtil.GetFormattedPercent(this.stressModificationValue / 600f * 60f, GameUtil.TimeSlice.None)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.STRESSREDUCEDPERMINUTE, GameUtil.GetFormattedPercent(this.stressModificationValue / 600f * 60f, GameUtil.TimeSlice.None)), Descriptor.DescriptorType.Effect);
		descriptors.Add(item);
		return descriptors;
	}

	// Token: 0x04002BB8 RID: 11192
	[MyCmpGet]
	private RoomTracker roomTracker;

	// Token: 0x04002BB9 RID: 11193
	[Serialize]
	protected float stopStressingValue;

	// Token: 0x04002BBA RID: 11194
	public float stressModificationValue;

	// Token: 0x04002BBB RID: 11195
	public float roomStressModificationValue;

	// Token: 0x04002BBC RID: 11196
	private RelaxationPoint.RelaxationPointSM.Instance smi;

	// Token: 0x04002BBD RID: 11197
	private static Effect stressReductionEffect;

	// Token: 0x04002BBE RID: 11198
	private static Effect roomStressReductionEffect;

	// Token: 0x0200175B RID: 5979
	public class RelaxationPointSM : GameStateMachine<RelaxationPoint.RelaxationPointSM, RelaxationPoint.RelaxationPointSM.Instance, RelaxationPoint>
	{
		// Token: 0x06008E28 RID: 36392 RVA: 0x0031EBBC File Offset: 0x0031CDBC
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.unoperational;
			this.unoperational.EventTransition(GameHashes.OperationalChanged, this.operational, (RelaxationPoint.RelaxationPointSM.Instance smi) => smi.GetComponent<Operational>().IsOperational).PlayAnim("off");
			this.operational.ToggleChore((RelaxationPoint.RelaxationPointSM.Instance smi) => smi.master.CreateWorkChore(), this.unoperational);
		}

		// Token: 0x04006E77 RID: 28279
		public GameStateMachine<RelaxationPoint.RelaxationPointSM, RelaxationPoint.RelaxationPointSM.Instance, RelaxationPoint, object>.State unoperational;

		// Token: 0x04006E78 RID: 28280
		public GameStateMachine<RelaxationPoint.RelaxationPointSM, RelaxationPoint.RelaxationPointSM.Instance, RelaxationPoint, object>.State operational;

		// Token: 0x020021D3 RID: 8659
		public new class Instance : GameStateMachine<RelaxationPoint.RelaxationPointSM, RelaxationPoint.RelaxationPointSM.Instance, RelaxationPoint, object>.GameInstance
		{
			// Token: 0x0600ABD7 RID: 43991 RVA: 0x00376205 File Offset: 0x00374405
			public Instance(RelaxationPoint master) : base(master)
			{
			}
		}
	}
}
