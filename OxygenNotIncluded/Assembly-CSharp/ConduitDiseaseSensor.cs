using System;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020005DF RID: 1503
[SerializationConfig(MemberSerialization.OptIn)]
public class ConduitDiseaseSensor : ConduitThresholdSensor, IThresholdSwitch
{
	// Token: 0x06002554 RID: 9556 RVA: 0x000CB71C File Offset: 0x000C991C
	protected override void UpdateVisualState(bool force = false)
	{
		if (this.wasOn != this.switchedOn || force)
		{
			this.wasOn = this.switchedOn;
			if (this.switchedOn)
			{
				this.animController.Play(ConduitSensor.ON_ANIMS, KAnim.PlayMode.Loop);
				int num;
				int num2;
				bool flag;
				this.GetContentsDisease(out num, out num2, out flag);
				Color32 c = Color.white;
				if (num != 255)
				{
					Disease disease = Db.Get().Diseases[num];
					c = GlobalAssets.Instance.colorSet.GetColorByName(disease.overlayColourName);
				}
				this.animController.SetSymbolTint(ConduitDiseaseSensor.TINT_SYMBOL, c);
				return;
			}
			this.animController.Play(ConduitSensor.OFF_ANIMS, KAnim.PlayMode.Once);
		}
	}

	// Token: 0x06002555 RID: 9557 RVA: 0x000CB7DC File Offset: 0x000C99DC
	private void GetContentsDisease(out int diseaseIdx, out int diseaseCount, out bool hasMass)
	{
		int cell = Grid.PosToCell(this);
		if (this.conduitType == ConduitType.Liquid || this.conduitType == ConduitType.Gas)
		{
			ConduitFlow.ConduitContents contents = Conduit.GetFlowManager(this.conduitType).GetContents(cell);
			diseaseIdx = (int)contents.diseaseIdx;
			diseaseCount = contents.diseaseCount;
			hasMass = (contents.mass > 0f);
			return;
		}
		SolidConduitFlow flowManager = SolidConduit.GetFlowManager();
		SolidConduitFlow.ConduitContents contents2 = flowManager.GetContents(cell);
		Pickupable pickupable = flowManager.GetPickupable(contents2.pickupableHandle);
		if (pickupable != null && pickupable.PrimaryElement.Mass > 0f)
		{
			diseaseIdx = (int)pickupable.PrimaryElement.DiseaseIdx;
			diseaseCount = pickupable.PrimaryElement.DiseaseCount;
			hasMass = true;
			return;
		}
		diseaseIdx = 0;
		diseaseCount = 0;
		hasMass = false;
	}

	// Token: 0x170001E3 RID: 483
	// (get) Token: 0x06002556 RID: 9558 RVA: 0x000CB890 File Offset: 0x000C9A90
	public override float CurrentValue
	{
		get
		{
			int num;
			int num2;
			bool flag;
			this.GetContentsDisease(out num, out num2, out flag);
			if (flag)
			{
				this.lastValue = (float)num2;
			}
			return this.lastValue;
		}
	}

	// Token: 0x170001E4 RID: 484
	// (get) Token: 0x06002557 RID: 9559 RVA: 0x000CB8BA File Offset: 0x000C9ABA
	public float RangeMin
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170001E5 RID: 485
	// (get) Token: 0x06002558 RID: 9560 RVA: 0x000CB8C1 File Offset: 0x000C9AC1
	public float RangeMax
	{
		get
		{
			return 100000f;
		}
	}

	// Token: 0x06002559 RID: 9561 RVA: 0x000CB8C8 File Offset: 0x000C9AC8
	public float GetRangeMinInputField()
	{
		return 0f;
	}

	// Token: 0x0600255A RID: 9562 RVA: 0x000CB8CF File Offset: 0x000C9ACF
	public float GetRangeMaxInputField()
	{
		return 100000f;
	}

	// Token: 0x170001E6 RID: 486
	// (get) Token: 0x0600255B RID: 9563 RVA: 0x000CB8D6 File Offset: 0x000C9AD6
	public LocString Title
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.DISEASE_TITLE;
		}
	}

	// Token: 0x170001E7 RID: 487
	// (get) Token: 0x0600255C RID: 9564 RVA: 0x000CB8DD File Offset: 0x000C9ADD
	public LocString ThresholdValueName
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.CONTENT_DISEASE;
		}
	}

	// Token: 0x170001E8 RID: 488
	// (get) Token: 0x0600255D RID: 9565 RVA: 0x000CB8E4 File Offset: 0x000C9AE4
	public string AboveToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.DISEASE_TOOLTIP_ABOVE;
		}
	}

	// Token: 0x170001E9 RID: 489
	// (get) Token: 0x0600255E RID: 9566 RVA: 0x000CB8F0 File Offset: 0x000C9AF0
	public string BelowToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.DISEASE_TOOLTIP_BELOW;
		}
	}

	// Token: 0x0600255F RID: 9567 RVA: 0x000CB8FC File Offset: 0x000C9AFC
	public string Format(float value, bool units)
	{
		return GameUtil.GetFormattedInt((float)((int)value), GameUtil.TimeSlice.None);
	}

	// Token: 0x06002560 RID: 9568 RVA: 0x000CB907 File Offset: 0x000C9B07
	public float ProcessedSliderValue(float input)
	{
		return input;
	}

	// Token: 0x06002561 RID: 9569 RVA: 0x000CB90A File Offset: 0x000C9B0A
	public float ProcessedInputValue(float input)
	{
		return input;
	}

	// Token: 0x06002562 RID: 9570 RVA: 0x000CB90D File Offset: 0x000C9B0D
	public LocString ThresholdValueUnits()
	{
		return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.DISEASE_UNITS;
	}

	// Token: 0x170001EA RID: 490
	// (get) Token: 0x06002563 RID: 9571 RVA: 0x000CB914 File Offset: 0x000C9B14
	public ThresholdScreenLayoutType LayoutType
	{
		get
		{
			return ThresholdScreenLayoutType.SliderBar;
		}
	}

	// Token: 0x170001EB RID: 491
	// (get) Token: 0x06002564 RID: 9572 RVA: 0x000CB917 File Offset: 0x000C9B17
	public int IncrementScale
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x170001EC RID: 492
	// (get) Token: 0x06002565 RID: 9573 RVA: 0x000CB91A File Offset: 0x000C9B1A
	public NonLinearSlider.Range[] GetRanges
	{
		get
		{
			return NonLinearSlider.GetDefaultRange(this.RangeMax);
		}
	}

	// Token: 0x04001564 RID: 5476
	private const float rangeMin = 0f;

	// Token: 0x04001565 RID: 5477
	private const float rangeMax = 100000f;

	// Token: 0x04001566 RID: 5478
	[Serialize]
	private float lastValue;

	// Token: 0x04001567 RID: 5479
	private static readonly HashedString TINT_SYMBOL = "germs";
}
