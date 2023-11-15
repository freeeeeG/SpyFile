using System;
using UnityEngine;

// Token: 0x020007B4 RID: 1972
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/Exhaust")]
public class Exhaust : KMonoBehaviour, ISim200ms
{
	// Token: 0x060036A2 RID: 13986 RVA: 0x001270EA File Offset: 0x001252EA
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<Exhaust>(-592767678, Exhaust.OnConduitStateChangedDelegate);
		base.Subscribe<Exhaust>(-111137758, Exhaust.OnConduitStateChangedDelegate);
		base.GetComponent<RequireInputs>().visualizeRequirements = RequireInputs.Requirements.NoWire;
		this.simRenderLoadBalance = true;
	}

	// Token: 0x060036A3 RID: 13987 RVA: 0x00127127 File Offset: 0x00125327
	protected override void OnSpawn()
	{
		this.OnConduitStateChanged(null);
	}

	// Token: 0x060036A4 RID: 13988 RVA: 0x00127130 File Offset: 0x00125330
	private void OnConduitStateChanged(object data)
	{
		this.operational.SetActive(this.operational.IsOperational && !this.vent.IsBlocked, false);
	}

	// Token: 0x060036A5 RID: 13989 RVA: 0x0012715C File Offset: 0x0012535C
	private void CalculateDiseaseTransfer(PrimaryElement item1, PrimaryElement item2, float transfer_rate, out int disease_to_item1, out int disease_to_item2)
	{
		disease_to_item1 = (int)((float)item2.DiseaseCount * transfer_rate);
		disease_to_item2 = (int)((float)item1.DiseaseCount * transfer_rate);
	}

	// Token: 0x060036A6 RID: 13990 RVA: 0x00127178 File Offset: 0x00125378
	public void Sim200ms(float dt)
	{
		this.operational.SetFlag(Exhaust.canExhaust, !this.vent.IsBlocked);
		if (!this.operational.IsOperational)
		{
			if (this.isAnimating)
			{
				this.isAnimating = false;
				this.recentlyExhausted = false;
				base.Trigger(-793429877, null);
			}
			return;
		}
		this.UpdateEmission();
		this.elapsedSwitchTime -= dt;
		if (this.elapsedSwitchTime <= 0f)
		{
			this.elapsedSwitchTime = 1f;
			if (this.recentlyExhausted != this.isAnimating)
			{
				this.isAnimating = this.recentlyExhausted;
				base.Trigger(-793429877, null);
			}
			this.recentlyExhausted = false;
		}
	}

	// Token: 0x060036A7 RID: 13991 RVA: 0x0012722C File Offset: 0x0012542C
	public bool IsAnimating()
	{
		return this.isAnimating;
	}

	// Token: 0x060036A8 RID: 13992 RVA: 0x00127234 File Offset: 0x00125434
	private void UpdateEmission()
	{
		if (this.consumer.ConsumptionRate == 0f)
		{
			return;
		}
		if (this.storage.items.Count == 0)
		{
			return;
		}
		int num = Grid.PosToCell(base.transform.GetPosition());
		if (Grid.Solid[num])
		{
			return;
		}
		ConduitType typeOfConduit = this.consumer.TypeOfConduit;
		if (typeOfConduit != ConduitType.Gas)
		{
			if (typeOfConduit == ConduitType.Liquid)
			{
				this.EmitLiquid(num);
				return;
			}
		}
		else
		{
			this.EmitGas(num);
		}
	}

	// Token: 0x060036A9 RID: 13993 RVA: 0x001272AC File Offset: 0x001254AC
	private bool EmitCommon(int cell, PrimaryElement primary_element, Exhaust.EmitDelegate emit)
	{
		if (primary_element.Mass <= 0f)
		{
			return false;
		}
		int num;
		int num2;
		this.CalculateDiseaseTransfer(this.exhaustPE, primary_element, 0.05f, out num, out num2);
		primary_element.ModifyDiseaseCount(-num, "Exhaust transfer");
		primary_element.AddDisease(this.exhaustPE.DiseaseIdx, num2, "Exhaust transfer");
		this.exhaustPE.ModifyDiseaseCount(-num2, "Exhaust transfer");
		this.exhaustPE.AddDisease(primary_element.DiseaseIdx, num, "Exhaust transfer");
		emit(cell, primary_element);
		if (this.vent != null)
		{
			this.vent.UpdateVentedMass(primary_element.ElementID, primary_element.Mass);
		}
		primary_element.KeepZeroMassObject = true;
		primary_element.Mass = 0f;
		primary_element.ModifyDiseaseCount(int.MinValue, "Exhaust.SimUpdate");
		this.recentlyExhausted = true;
		return true;
	}

	// Token: 0x060036AA RID: 13994 RVA: 0x00127384 File Offset: 0x00125584
	private void EmitLiquid(int cell)
	{
		int num = Grid.CellBelow(cell);
		Exhaust.EmitDelegate emit = (Grid.IsValidCell(num) && !Grid.Solid[num]) ? Exhaust.emit_particle : Exhaust.emit_element;
		foreach (GameObject gameObject in this.storage.items)
		{
			PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
			if (component.Element.IsLiquid && this.EmitCommon(cell, component, emit))
			{
				break;
			}
		}
	}

	// Token: 0x060036AB RID: 13995 RVA: 0x00127420 File Offset: 0x00125620
	private void EmitGas(int cell)
	{
		foreach (GameObject gameObject in this.storage.items)
		{
			PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
			if (component.Element.IsGas && this.EmitCommon(cell, component, Exhaust.emit_element))
			{
				break;
			}
		}
	}

	// Token: 0x0400218D RID: 8589
	[MyCmpGet]
	private Vent vent;

	// Token: 0x0400218E RID: 8590
	[MyCmpGet]
	private Storage storage;

	// Token: 0x0400218F RID: 8591
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04002190 RID: 8592
	[MyCmpGet]
	private ConduitConsumer consumer;

	// Token: 0x04002191 RID: 8593
	[MyCmpGet]
	private PrimaryElement exhaustPE;

	// Token: 0x04002192 RID: 8594
	private static readonly Operational.Flag canExhaust = new Operational.Flag("canExhaust", Operational.Flag.Type.Requirement);

	// Token: 0x04002193 RID: 8595
	private bool isAnimating;

	// Token: 0x04002194 RID: 8596
	private bool recentlyExhausted;

	// Token: 0x04002195 RID: 8597
	private const float MinSwitchTime = 1f;

	// Token: 0x04002196 RID: 8598
	private float elapsedSwitchTime;

	// Token: 0x04002197 RID: 8599
	private static readonly EventSystem.IntraObjectHandler<Exhaust> OnConduitStateChangedDelegate = new EventSystem.IntraObjectHandler<Exhaust>(delegate(Exhaust component, object data)
	{
		component.OnConduitStateChanged(data);
	});

	// Token: 0x04002198 RID: 8600
	private static Exhaust.EmitDelegate emit_element = delegate(int cell, PrimaryElement primary_element)
	{
		SimMessages.AddRemoveSubstance(cell, primary_element.ElementID, CellEventLogger.Instance.ExhaustSimUpdate, primary_element.Mass, primary_element.Temperature, primary_element.DiseaseIdx, primary_element.DiseaseCount, true, -1);
	};

	// Token: 0x04002199 RID: 8601
	private static Exhaust.EmitDelegate emit_particle = delegate(int cell, PrimaryElement primary_element)
	{
		FallingWater.instance.AddParticle(cell, primary_element.Element.idx, primary_element.Mass, primary_element.Temperature, primary_element.DiseaseIdx, primary_element.DiseaseCount, true, false, true, false);
	};

	// Token: 0x02001524 RID: 5412
	// (Invoke) Token: 0x060086F6 RID: 34550
	private delegate void EmitDelegate(int cell, PrimaryElement primary_element);
}
