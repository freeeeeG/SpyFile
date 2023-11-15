using System;
using Klei;
using UnityEngine;

// Token: 0x02000625 RID: 1573
[AddComponentMenu("KMonoBehaviour/Workable/LiquidPumpingStation")]
public class LiquidPumpingStation : Workable, ISim200ms
{
	// Token: 0x060027C5 RID: 10181 RVA: 0x000D7ED5 File Offset: 0x000D60D5
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.resetProgressOnStop = true;
		this.showProgressBar = false;
	}

	// Token: 0x060027C6 RID: 10182 RVA: 0x000D7EEC File Offset: 0x000D60EC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.infos = new LiquidPumpingStation.LiquidInfo[LiquidPumpingStation.liquidOffsets.Length * 2];
		this.RefreshStatusItem();
		this.Sim200ms(0f);
		base.SetWorkTime(10f);
		this.RefreshDepthAvailable();
		this.RegisterListenersToCellChanges();
		this.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Behind, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_target",
			"meter_arrow",
			"meter_scale"
		});
		foreach (GameObject gameObject in base.GetComponent<Storage>().items)
		{
			if (!(gameObject == null) && gameObject != null)
			{
				gameObject.DeleteObject();
			}
		}
	}

	// Token: 0x060027C7 RID: 10183 RVA: 0x000D7FD4 File Offset: 0x000D61D4
	private void RegisterListenersToCellChanges()
	{
		int widthInCells = base.GetComponent<BuildingComplete>().Def.WidthInCells;
		CellOffset[] array = new CellOffset[widthInCells * 4];
		for (int i = 0; i < 4; i++)
		{
			int y = -(i + 1);
			for (int j = 0; j < widthInCells; j++)
			{
				array[i * widthInCells + j] = new CellOffset(j, y);
			}
		}
		Extents extents = new Extents(Grid.PosToCell(base.transform.GetPosition()), array);
		this.partitionerEntry_solids = GameScenePartitioner.Instance.Add("LiquidPumpingStation", base.gameObject, extents, GameScenePartitioner.Instance.solidChangedLayer, new Action<object>(this.OnLowerCellChanged));
		this.partitionerEntry_buildings = GameScenePartitioner.Instance.Add("LiquidPumpingStation", base.gameObject, extents, GameScenePartitioner.Instance.objectLayers[1], new Action<object>(this.OnLowerCellChanged));
	}

	// Token: 0x060027C8 RID: 10184 RVA: 0x000D80B0 File Offset: 0x000D62B0
	private void UnregisterListenersToCellChanges()
	{
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry_solids);
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry_buildings);
	}

	// Token: 0x060027C9 RID: 10185 RVA: 0x000D80D2 File Offset: 0x000D62D2
	private void OnLowerCellChanged(object o)
	{
		this.RefreshDepthAvailable();
	}

	// Token: 0x060027CA RID: 10186 RVA: 0x000D80DC File Offset: 0x000D62DC
	private void RefreshDepthAvailable()
	{
		int num = PumpingStationGuide.GetDepthAvailable(Grid.PosToCell(this), base.gameObject);
		int num2 = 4;
		if (num != this.depthAvailable)
		{
			KAnimControllerBase component = base.GetComponent<KAnimControllerBase>();
			for (int i = 1; i <= num2; i++)
			{
				component.SetSymbolVisiblity("pipe" + i.ToString(), i <= num);
			}
			PumpingStationGuide.OccupyArea(base.gameObject, num);
			this.depthAvailable = num;
		}
	}

	// Token: 0x060027CB RID: 10187 RVA: 0x000D8150 File Offset: 0x000D6350
	public void Sim200ms(float dt)
	{
		if (this.session != null)
		{
			return;
		}
		int num = this.infoCount;
		for (int i = 0; i < this.infoCount; i++)
		{
			this.infos[i].amount = 0f;
		}
		if (base.GetComponent<Operational>().IsOperational)
		{
			int cell = Grid.PosToCell(this);
			for (int j = 0; j < LiquidPumpingStation.liquidOffsets.Length; j++)
			{
				if (this.depthAvailable >= Math.Abs(LiquidPumpingStation.liquidOffsets[j].y))
				{
					int num2 = Grid.OffsetCell(cell, LiquidPumpingStation.liquidOffsets[j]);
					bool flag = false;
					Element element = Grid.Element[num2];
					if (element.IsLiquid)
					{
						float num3 = Grid.Mass[num2];
						for (int k = 0; k < this.infoCount; k++)
						{
							if (this.infos[k].element == element)
							{
								LiquidPumpingStation.LiquidInfo[] array = this.infos;
								int num4 = k;
								array[num4].amount = array[num4].amount + num3;
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							this.infos[this.infoCount].amount = num3;
							this.infos[this.infoCount].element = element;
							this.infoCount++;
						}
					}
				}
			}
		}
		int l = 0;
		while (l < this.infoCount)
		{
			LiquidPumpingStation.LiquidInfo liquidInfo = this.infos[l];
			if (liquidInfo.amount <= 1f)
			{
				if (liquidInfo.source != null)
				{
					liquidInfo.source.DeleteObject();
				}
				this.infos[l] = this.infos[this.infoCount - 1];
				this.infoCount--;
			}
			else
			{
				if (liquidInfo.source == null)
				{
					liquidInfo.source = base.GetComponent<Storage>().AddLiquid(liquidInfo.element.id, liquidInfo.amount, liquidInfo.element.defaultValues.temperature, byte.MaxValue, 0, false, true).GetComponent<SubstanceChunk>();
					Pickupable component = liquidInfo.source.GetComponent<Pickupable>();
					component.GetComponent<KPrefabID>().AddTag(GameTags.LiquidSource, false);
					component.SetOffsets(new CellOffset[]
					{
						new CellOffset(0, 1)
					});
					component.targetWorkable = this;
					Pickupable pickupable = component;
					pickupable.OnReservationsChanged = (System.Action)Delegate.Combine(pickupable.OnReservationsChanged, new System.Action(this.OnReservationsChanged));
				}
				liquidInfo.source.GetComponent<Pickupable>().TotalAmount = liquidInfo.amount;
				this.infos[l] = liquidInfo;
				l++;
			}
		}
		if (num != this.infoCount)
		{
			this.RefreshStatusItem();
		}
	}

	// Token: 0x060027CC RID: 10188 RVA: 0x000D8420 File Offset: 0x000D6620
	private void RefreshStatusItem()
	{
		if (this.infoCount > 0)
		{
			base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.PumpingStation, this);
			return;
		}
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.EmptyPumpingStation, this);
	}

	// Token: 0x060027CD RID: 10189 RVA: 0x000D8490 File Offset: 0x000D6690
	public string ResolveString(string base_string)
	{
		string text = "";
		for (int i = 0; i < this.infoCount; i++)
		{
			if (this.infos[i].source != null)
			{
				text = string.Concat(new string[]
				{
					text,
					"\n",
					this.infos[i].element.name,
					": ",
					GameUtil.GetFormattedMass(this.infos[i].amount, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")
				});
			}
		}
		return base_string.Replace("{Liquids}", text);
	}

	// Token: 0x060027CE RID: 10190 RVA: 0x000D8533 File Offset: 0x000D6733
	public static bool IsLiquidAccessible(Element element)
	{
		return true;
	}

	// Token: 0x060027CF RID: 10191 RVA: 0x000D8536 File Offset: 0x000D6736
	public override float GetPercentComplete()
	{
		if (this.session != null)
		{
			return this.session.GetPercentComplete();
		}
		return 0f;
	}

	// Token: 0x060027D0 RID: 10192 RVA: 0x000D8554 File Offset: 0x000D6754
	protected override void OnStartWork(Worker worker)
	{
		base.OnStartWork(worker);
		Pickupable.PickupableStartWorkInfo pickupableStartWorkInfo = (Pickupable.PickupableStartWorkInfo)worker.startWorkInfo;
		float amount = pickupableStartWorkInfo.amount;
		Element element = pickupableStartWorkInfo.originalPickupable.GetComponent<PrimaryElement>().Element;
		this.session = new LiquidPumpingStation.WorkSession(Grid.PosToCell(this), element.id, pickupableStartWorkInfo.originalPickupable.GetComponent<SubstanceChunk>(), amount, base.gameObject);
		this.meter.SetPositionPercent(0f);
		this.meter.SetSymbolTint(new KAnimHashedString("meter_target"), element.substance.colour);
	}

	// Token: 0x060027D1 RID: 10193 RVA: 0x000D85E8 File Offset: 0x000D67E8
	protected override void OnStopWork(Worker worker)
	{
		base.OnStopWork(worker);
		if (this.session != null)
		{
			Storage component = worker.GetComponent<Storage>();
			float consumedAmount = this.session.GetConsumedAmount();
			if (consumedAmount > 0f)
			{
				SubstanceChunk source = this.session.GetSource();
				SimUtil.DiseaseInfo diseaseInfo = (this.session != null) ? this.session.GetDiseaseInfo() : SimUtil.DiseaseInfo.Invalid;
				PrimaryElement component2 = source.GetComponent<PrimaryElement>();
				Pickupable component3 = LiquidSourceManager.Instance.CreateChunk(component2.Element, consumedAmount, this.session.GetTemperature(), diseaseInfo.idx, diseaseInfo.count, base.transform.GetPosition()).GetComponent<Pickupable>();
				component3.TotalAmount = consumedAmount;
				component3.Trigger(1335436905, source.GetComponent<Pickupable>());
				worker.workCompleteData = component3;
				this.Sim200ms(0f);
				if (component3 != null)
				{
					component.Store(component3.gameObject, false, false, true, false);
				}
			}
			this.session.Cleanup();
			this.session = null;
		}
		base.GetComponent<KAnimControllerBase>().Play("on", KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x060027D2 RID: 10194 RVA: 0x000D870C File Offset: 0x000D690C
	private void OnReservationsChanged()
	{
		bool forceUnfetchable = false;
		for (int i = 0; i < this.infoCount; i++)
		{
			if (this.infos[i].source != null && this.infos[i].source.GetComponent<Pickupable>().ReservedAmount > 0f)
			{
				forceUnfetchable = true;
				break;
			}
		}
		for (int j = 0; j < this.infoCount; j++)
		{
			if (this.infos[j].source != null)
			{
				FetchableMonitor.Instance smi = this.infos[j].source.GetSMI<FetchableMonitor.Instance>();
				if (smi != null)
				{
					smi.SetForceUnfetchable(forceUnfetchable);
				}
			}
		}
	}

	// Token: 0x060027D3 RID: 10195 RVA: 0x000D87B6 File Offset: 0x000D69B6
	protected override bool OnWorkTick(Worker worker, float dt)
	{
		if (this.session != null)
		{
			this.meter.SetPositionPercent(this.session.GetPercentComplete());
			if (this.session.GetLastTickAmount() <= 0f)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060027D4 RID: 10196 RVA: 0x000D87EC File Offset: 0x000D69EC
	protected override void OnCleanUp()
	{
		this.UnregisterListenersToCellChanges();
		base.OnCleanUp();
		if (this.session != null)
		{
			this.session.Cleanup();
			this.session = null;
		}
		for (int i = 0; i < this.infoCount; i++)
		{
			if (this.infos[i].source != null)
			{
				this.infos[i].source.DeleteObject();
			}
		}
	}

	// Token: 0x04001713 RID: 5907
	private static readonly CellOffset[] liquidOffsets = new CellOffset[]
	{
		new CellOffset(0, 0),
		new CellOffset(1, 0),
		new CellOffset(0, -1),
		new CellOffset(1, -1),
		new CellOffset(0, -2),
		new CellOffset(1, -2),
		new CellOffset(0, -3),
		new CellOffset(1, -3),
		new CellOffset(0, -4),
		new CellOffset(1, -4)
	};

	// Token: 0x04001714 RID: 5908
	private LiquidPumpingStation.LiquidInfo[] infos;

	// Token: 0x04001715 RID: 5909
	private int infoCount;

	// Token: 0x04001716 RID: 5910
	private int depthAvailable = -1;

	// Token: 0x04001717 RID: 5911
	private HandleVector<int>.Handle partitionerEntry_buildings;

	// Token: 0x04001718 RID: 5912
	private HandleVector<int>.Handle partitionerEntry_solids;

	// Token: 0x04001719 RID: 5913
	private LiquidPumpingStation.WorkSession session;

	// Token: 0x0400171A RID: 5914
	private MeterController meter;

	// Token: 0x020012F0 RID: 4848
	private class WorkSession
	{
		// Token: 0x06007F0C RID: 32524 RVA: 0x002EB688 File Offset: 0x002E9888
		public WorkSession(int cell, SimHashes element, SubstanceChunk source, float amount_to_pickup, GameObject pump)
		{
			this.cell = cell;
			this.element = element;
			this.source = source;
			this.amountToPickup = amount_to_pickup;
			this.temperature = ElementLoader.FindElementByHash(element).defaultValues.temperature;
			this.diseaseInfo = SimUtil.DiseaseInfo.Invalid;
			this.amountPerTick = 40f;
			this.pump = pump;
			this.lastTickAmount = this.amountPerTick;
			this.ConsumeMass();
		}

		// Token: 0x06007F0D RID: 32525 RVA: 0x002EB6FE File Offset: 0x002E98FE
		private void OnSimConsumeCallback(Sim.MassConsumedCallback mass_cb_info, object data)
		{
			((LiquidPumpingStation.WorkSession)data).OnSimConsume(mass_cb_info);
		}

		// Token: 0x06007F0E RID: 32526 RVA: 0x002EB70C File Offset: 0x002E990C
		private void OnSimConsume(Sim.MassConsumedCallback mass_cb_info)
		{
			if (this.consumedAmount == 0f)
			{
				this.temperature = mass_cb_info.temperature;
			}
			else
			{
				this.temperature = GameUtil.GetFinalTemperature(this.temperature, this.consumedAmount, mass_cb_info.temperature, mass_cb_info.mass);
			}
			this.consumedAmount += mass_cb_info.mass;
			this.lastTickAmount = mass_cb_info.mass;
			this.diseaseInfo = SimUtil.CalculateFinalDiseaseInfo(this.diseaseInfo.idx, this.diseaseInfo.count, mass_cb_info.diseaseIdx, mass_cb_info.diseaseCount);
			if (this.consumedAmount >= this.amountToPickup)
			{
				this.amountPerTick = 0f;
				this.lastTickAmount = 0f;
			}
			this.ConsumeMass();
		}

		// Token: 0x06007F0F RID: 32527 RVA: 0x002EB7D0 File Offset: 0x002E99D0
		private void ConsumeMass()
		{
			if (this.amountPerTick > 0f)
			{
				float num = Mathf.Min(this.amountPerTick, this.amountToPickup - this.consumedAmount);
				num = Mathf.Max(num, 1f);
				HandleVector<Game.ComplexCallbackInfo<Sim.MassConsumedCallback>>.Handle handle = Game.Instance.massConsumedCallbackManager.Add(new Action<Sim.MassConsumedCallback, object>(this.OnSimConsumeCallback), this, "LiquidPumpingStation");
				int depthAvailable = PumpingStationGuide.GetDepthAvailable(this.cell, this.pump);
				SimMessages.ConsumeMass(Grid.OffsetCell(this.cell, new CellOffset(0, -depthAvailable)), this.element, num, (byte)(depthAvailable + 1), handle.index);
			}
		}

		// Token: 0x06007F10 RID: 32528 RVA: 0x002EB870 File Offset: 0x002E9A70
		public float GetPercentComplete()
		{
			return this.consumedAmount / this.amountToPickup;
		}

		// Token: 0x06007F11 RID: 32529 RVA: 0x002EB87F File Offset: 0x002E9A7F
		public float GetLastTickAmount()
		{
			return this.lastTickAmount;
		}

		// Token: 0x06007F12 RID: 32530 RVA: 0x002EB887 File Offset: 0x002E9A87
		public SimUtil.DiseaseInfo GetDiseaseInfo()
		{
			return this.diseaseInfo;
		}

		// Token: 0x06007F13 RID: 32531 RVA: 0x002EB88F File Offset: 0x002E9A8F
		public SubstanceChunk GetSource()
		{
			return this.source;
		}

		// Token: 0x06007F14 RID: 32532 RVA: 0x002EB897 File Offset: 0x002E9A97
		public float GetConsumedAmount()
		{
			return this.consumedAmount;
		}

		// Token: 0x06007F15 RID: 32533 RVA: 0x002EB89F File Offset: 0x002E9A9F
		public float GetTemperature()
		{
			if (this.temperature <= 0f)
			{
				global::Debug.LogWarning("TODO(YOG): Fix bad temperature in liquid pumping station.");
				return ElementLoader.FindElementByHash(this.element).defaultValues.temperature;
			}
			return this.temperature;
		}

		// Token: 0x06007F16 RID: 32534 RVA: 0x002EB8D4 File Offset: 0x002E9AD4
		public void Cleanup()
		{
			this.amountPerTick = 0f;
			this.diseaseInfo = SimUtil.DiseaseInfo.Invalid;
		}

		// Token: 0x04006115 RID: 24853
		private int cell;

		// Token: 0x04006116 RID: 24854
		private float amountToPickup;

		// Token: 0x04006117 RID: 24855
		private float consumedAmount;

		// Token: 0x04006118 RID: 24856
		private float temperature;

		// Token: 0x04006119 RID: 24857
		private float amountPerTick;

		// Token: 0x0400611A RID: 24858
		private SimHashes element;

		// Token: 0x0400611B RID: 24859
		private float lastTickAmount;

		// Token: 0x0400611C RID: 24860
		private SubstanceChunk source;

		// Token: 0x0400611D RID: 24861
		private SimUtil.DiseaseInfo diseaseInfo;

		// Token: 0x0400611E RID: 24862
		private GameObject pump;
	}

	// Token: 0x020012F1 RID: 4849
	private struct LiquidInfo
	{
		// Token: 0x0400611F RID: 24863
		public float amount;

		// Token: 0x04006120 RID: 24864
		public Element element;

		// Token: 0x04006121 RID: 24865
		public SubstanceChunk source;
	}
}
