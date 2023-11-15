using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020008A0 RID: 2208
public class TubeTraveller : GameStateMachine<TubeTraveller, TubeTraveller.Instance>
{
	// Token: 0x06004004 RID: 16388 RVA: 0x001665DC File Offset: 0x001647DC
	public void InitModifiers()
	{
		this.modifiers.Add(new AttributeModifier(Db.Get().Attributes.Insulation.Id, (float)TUNING.EQUIPMENT.SUITS.ATMOSUIT_INSULATION, STRINGS.BUILDINGS.PREFABS.TRAVELTUBE.NAME, false, false, true));
		this.modifiers.Add(new AttributeModifier(Db.Get().Attributes.ThermalConductivityBarrier.Id, TUNING.EQUIPMENT.SUITS.ATMOSUIT_THERMAL_CONDUCTIVITY_BARRIER, STRINGS.BUILDINGS.PREFABS.TRAVELTUBE.NAME, false, false, true));
		this.modifiers.Add(new AttributeModifier(Db.Get().Amounts.Bladder.deltaAttribute.Id, TUNING.EQUIPMENT.SUITS.ATMOSUIT_BLADDER, STRINGS.BUILDINGS.PREFABS.TRAVELTUBE.NAME, false, false, true));
		this.modifiers.Add(new AttributeModifier(Db.Get().Attributes.ScaldingThreshold.Id, (float)TUNING.EQUIPMENT.SUITS.ATMOSUIT_SCALDING, STRINGS.BUILDINGS.PREFABS.TRAVELTUBE.NAME, false, false, true));
		this.waxSpeedBoostModifier = new AttributeModifier(Db.Get().Attributes.TransitTubeTravelSpeed.Id, 4.5f, STRINGS.BUILDINGS.PREFABS.TRAVELTUBE.NAME, false, false, true);
		this.immunities.Add(Db.Get().effects.Get("SoakingWet"));
		this.immunities.Add(Db.Get().effects.Get("WetFeet"));
		this.immunities.Add(Db.Get().effects.Get("PoppedEarDrums"));
		this.immunities.Add(Db.Get().effects.Get("MinorIrritation"));
		this.immunities.Add(Db.Get().effects.Get("MajorIrritation"));
	}

	// Token: 0x06004005 RID: 16389 RVA: 0x00166794 File Offset: 0x00164994
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		this.InitModifiers();
		default_state = this.root;
		this.root.DoNothing();
	}

	// Token: 0x06004006 RID: 16390 RVA: 0x001667B0 File Offset: 0x001649B0
	public void OnSetOxygenBreather(OxygenBreather oxygen_breather)
	{
	}

	// Token: 0x06004007 RID: 16391 RVA: 0x001667B2 File Offset: 0x001649B2
	public void OnClearOxygenBreather(OxygenBreather oxygen_breather)
	{
	}

	// Token: 0x06004008 RID: 16392 RVA: 0x001667B4 File Offset: 0x001649B4
	public bool ConsumeGas(OxygenBreather oxygen_breather, float amount)
	{
		return false;
	}

	// Token: 0x06004009 RID: 16393 RVA: 0x001667B7 File Offset: 0x001649B7
	public bool ShouldEmitCO2()
	{
		return false;
	}

	// Token: 0x0600400A RID: 16394 RVA: 0x001667BA File Offset: 0x001649BA
	public bool ShouldStoreCO2()
	{
		return false;
	}

	// Token: 0x040029B2 RID: 10674
	private List<Effect> immunities = new List<Effect>();

	// Token: 0x040029B3 RID: 10675
	private List<AttributeModifier> modifiers = new List<AttributeModifier>();

	// Token: 0x040029B4 RID: 10676
	private AttributeModifier waxSpeedBoostModifier;

	// Token: 0x040029B5 RID: 10677
	private const float WaxSpeedBoost = 0.25f;

	// Token: 0x020016D2 RID: 5842
	public new class Instance : GameStateMachine<TubeTraveller, TubeTraveller.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x17000967 RID: 2407
		// (get) Token: 0x06008C8F RID: 35983 RVA: 0x003191FA File Offset: 0x003173FA
		public int prefabInstanceID
		{
			get
			{
				return base.GetComponent<Navigator>().gameObject.GetComponent<KPrefabID>().InstanceID;
			}
		}

		// Token: 0x06008C90 RID: 35984 RVA: 0x00319211 File Offset: 0x00317411
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x06008C91 RID: 35985 RVA: 0x00319225 File Offset: 0x00317425
		public void OnPathAdvanced(object data)
		{
			this.UnreserveEntrances();
			this.ReserveEntrances();
		}

		// Token: 0x06008C92 RID: 35986 RVA: 0x00319234 File Offset: 0x00317434
		public void ReserveEntrances()
		{
			PathFinder.Path path = base.GetComponent<Navigator>().path;
			if (path.nodes == null)
			{
				return;
			}
			for (int i = 0; i < path.nodes.Count - 1; i++)
			{
				if (path.nodes[i].navType == NavType.Floor && path.nodes[i + 1].navType == NavType.Tube)
				{
					int cell = path.nodes[i].cell;
					if (Grid.HasUsableTubeEntrance(cell, this.prefabInstanceID))
					{
						GameObject gameObject = Grid.Objects[cell, 1];
						if (gameObject)
						{
							TravelTubeEntrance component = gameObject.GetComponent<TravelTubeEntrance>();
							if (component)
							{
								component.Reserve(this, this.prefabInstanceID);
								this.reservations.Add(component);
							}
						}
					}
				}
			}
		}

		// Token: 0x06008C93 RID: 35987 RVA: 0x00319300 File Offset: 0x00317500
		public void UnreserveEntrances()
		{
			foreach (TravelTubeEntrance travelTubeEntrance in this.reservations)
			{
				if (!(travelTubeEntrance == null))
				{
					travelTubeEntrance.Unreserve(this, this.prefabInstanceID);
				}
			}
			this.reservations.Clear();
		}

		// Token: 0x06008C94 RID: 35988 RVA: 0x00319370 File Offset: 0x00317570
		public void ApplyEnteringTubeEffects()
		{
			Effects component = base.GetComponent<Effects>();
			Attributes attributes = base.gameObject.GetAttributes();
			base.gameObject.AddTag(GameTags.InTransitTube);
			string name = GameTags.InTransitTube.Name;
			foreach (Effect effect in base.sm.immunities)
			{
				component.AddImmunity(effect, name, true);
			}
			foreach (AttributeModifier modifier in base.sm.modifiers)
			{
				attributes.Add(modifier);
			}
			if (this.isWaxed)
			{
				attributes.Add(base.sm.waxSpeedBoostModifier);
			}
			CreatureSimTemperatureTransfer component2 = base.gameObject.GetComponent<CreatureSimTemperatureTransfer>();
			if (component2 != null)
			{
				component2.RefreshRegistration();
			}
		}

		// Token: 0x06008C95 RID: 35989 RVA: 0x00319480 File Offset: 0x00317680
		public void ClearAllEffects()
		{
			Effects component = base.GetComponent<Effects>();
			Attributes attributes = base.gameObject.GetAttributes();
			base.gameObject.RemoveTag(GameTags.InTransitTube);
			string name = GameTags.InTransitTube.Name;
			foreach (Effect effect in base.sm.immunities)
			{
				component.RemoveImmunity(effect, name);
			}
			foreach (AttributeModifier modifier in base.sm.modifiers)
			{
				attributes.Remove(modifier);
			}
			this.SetWaxState(false);
			attributes.Remove(base.sm.waxSpeedBoostModifier);
			CreatureSimTemperatureTransfer component2 = base.gameObject.GetComponent<CreatureSimTemperatureTransfer>();
			if (component2 != null)
			{
				component2.RefreshRegistration();
			}
		}

		// Token: 0x06008C96 RID: 35990 RVA: 0x0031958C File Offset: 0x0031778C
		public void SetWaxState(bool isWaxed)
		{
			this.isWaxed = isWaxed;
			KSelectable component = base.GetComponent<KSelectable>();
			if (component != null)
			{
				if (isWaxed)
				{
					component.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().DuplicantStatusItems.WaxedForTransitTube, 0.25f);
					return;
				}
				component.RemoveStatusItem(Db.Get().DuplicantStatusItems.WaxedForTransitTube, false);
			}
		}

		// Token: 0x06008C97 RID: 35991 RVA: 0x003195FA File Offset: 0x003177FA
		public void OnTubeTransition(bool nowInTube)
		{
			if (nowInTube != this.inTube)
			{
				this.inTube = nowInTube;
				base.GetComponent<Effects>();
				base.gameObject.GetAttributes();
				if (nowInTube)
				{
					this.ApplyEnteringTubeEffects();
					return;
				}
				this.ClearAllEffects();
			}
		}

		// Token: 0x04006D04 RID: 27908
		private List<TravelTubeEntrance> reservations = new List<TravelTubeEntrance>();

		// Token: 0x04006D05 RID: 27909
		public bool inTube;

		// Token: 0x04006D06 RID: 27910
		public bool isWaxed;
	}
}
