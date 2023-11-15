using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

// Token: 0x02000ADB RID: 2779
public class ColorSet : ScriptableObject
{
	// Token: 0x060055A4 RID: 21924 RVA: 0x001F2BE4 File Offset: 0x001F0DE4
	private void Init()
	{
		if (this.namedLookup == null)
		{
			this.namedLookup = new Dictionary<string, Color32>();
			foreach (FieldInfo fieldInfo in typeof(ColorSet).GetFields())
			{
				if (fieldInfo.FieldType == typeof(Color32))
				{
					this.namedLookup[fieldInfo.Name] = (Color32)fieldInfo.GetValue(this);
				}
			}
		}
	}

	// Token: 0x060055A5 RID: 21925 RVA: 0x001F2C5A File Offset: 0x001F0E5A
	public Color32 GetColorByName(string name)
	{
		this.Init();
		return this.namedLookup[name];
	}

	// Token: 0x060055A6 RID: 21926 RVA: 0x001F2C6E File Offset: 0x001F0E6E
	public void RefreshLookup()
	{
		this.namedLookup = null;
		this.Init();
	}

	// Token: 0x060055A7 RID: 21927 RVA: 0x001F2C7D File Offset: 0x001F0E7D
	public bool IsDefaultColorSet()
	{
		return Array.IndexOf<ColorSet>(GlobalAssets.Instance.colorSetOptions, this) == 0;
	}

	// Token: 0x04003928 RID: 14632
	public string settingName;

	// Token: 0x04003929 RID: 14633
	[Header("Logic")]
	public Color32 logicOn;

	// Token: 0x0400392A RID: 14634
	public Color32 logicOff;

	// Token: 0x0400392B RID: 14635
	public Color32 logicDisconnected;

	// Token: 0x0400392C RID: 14636
	public Color32 logicOnText;

	// Token: 0x0400392D RID: 14637
	public Color32 logicOffText;

	// Token: 0x0400392E RID: 14638
	public Color32 logicOnSidescreen;

	// Token: 0x0400392F RID: 14639
	public Color32 logicOffSidescreen;

	// Token: 0x04003930 RID: 14640
	[Header("Decor")]
	public Color32 decorPositive;

	// Token: 0x04003931 RID: 14641
	public Color32 decorNegative;

	// Token: 0x04003932 RID: 14642
	public Color32 decorBaseline;

	// Token: 0x04003933 RID: 14643
	public Color32 decorHighlightPositive;

	// Token: 0x04003934 RID: 14644
	public Color32 decorHighlightNegative;

	// Token: 0x04003935 RID: 14645
	[Header("Crop Overlay")]
	public Color32 cropHalted;

	// Token: 0x04003936 RID: 14646
	public Color32 cropGrowing;

	// Token: 0x04003937 RID: 14647
	public Color32 cropGrown;

	// Token: 0x04003938 RID: 14648
	[Header("Harvest Overlay")]
	public Color32 harvestEnabled;

	// Token: 0x04003939 RID: 14649
	public Color32 harvestDisabled;

	// Token: 0x0400393A RID: 14650
	[Header("Gameplay Events")]
	public Color32 eventPositive;

	// Token: 0x0400393B RID: 14651
	public Color32 eventNegative;

	// Token: 0x0400393C RID: 14652
	public Color32 eventNeutral;

	// Token: 0x0400393D RID: 14653
	[Header("Notifications")]
	public Color32 NotificationBad;

	// Token: 0x0400393E RID: 14654
	public Color32 NotificationEvent;

	// Token: 0x0400393F RID: 14655
	[Header("PrioritiesScreen")]
	public Color32 PrioritiesNeutralColor;

	// Token: 0x04003940 RID: 14656
	public Color32 PrioritiesLowColor;

	// Token: 0x04003941 RID: 14657
	public Color32 PrioritiesHighColor;

	// Token: 0x04003942 RID: 14658
	[Header("Info Screen Status Items")]
	public Color32 statusItemBad;

	// Token: 0x04003943 RID: 14659
	public Color32 statusItemEvent;

	// Token: 0x04003944 RID: 14660
	[Header("Germ Overlay")]
	public Color32 germFoodPoisoning;

	// Token: 0x04003945 RID: 14661
	public Color32 germPollenGerms;

	// Token: 0x04003946 RID: 14662
	public Color32 germSlimeLung;

	// Token: 0x04003947 RID: 14663
	public Color32 germZombieSpores;

	// Token: 0x04003948 RID: 14664
	public Color32 germRadiationSickness;

	// Token: 0x04003949 RID: 14665
	[Header("Room Overlay")]
	public Color32 roomNone;

	// Token: 0x0400394A RID: 14666
	public Color32 roomFood;

	// Token: 0x0400394B RID: 14667
	public Color32 roomSleep;

	// Token: 0x0400394C RID: 14668
	public Color32 roomRecreation;

	// Token: 0x0400394D RID: 14669
	public Color32 roomBathroom;

	// Token: 0x0400394E RID: 14670
	public Color32 roomHospital;

	// Token: 0x0400394F RID: 14671
	public Color32 roomIndustrial;

	// Token: 0x04003950 RID: 14672
	public Color32 roomAgricultural;

	// Token: 0x04003951 RID: 14673
	public Color32 roomScience;

	// Token: 0x04003952 RID: 14674
	public Color32 roomPark;

	// Token: 0x04003953 RID: 14675
	[Header("Power Overlay")]
	public Color32 powerConsumer;

	// Token: 0x04003954 RID: 14676
	public Color32 powerGenerator;

	// Token: 0x04003955 RID: 14677
	public Color32 powerBuildingDisabled;

	// Token: 0x04003956 RID: 14678
	public Color32 powerCircuitUnpowered;

	// Token: 0x04003957 RID: 14679
	public Color32 powerCircuitSafe;

	// Token: 0x04003958 RID: 14680
	public Color32 powerCircuitStraining;

	// Token: 0x04003959 RID: 14681
	public Color32 powerCircuitOverloading;

	// Token: 0x0400395A RID: 14682
	[Header("Light Overlay")]
	public Color32 lightOverlay;

	// Token: 0x0400395B RID: 14683
	[Header("Conduit Overlay")]
	public Color32 conduitNormal;

	// Token: 0x0400395C RID: 14684
	public Color32 conduitInsulated;

	// Token: 0x0400395D RID: 14685
	public Color32 conduitRadiant;

	// Token: 0x0400395E RID: 14686
	[Header("Temperature Overlay")]
	public Color32 temperatureThreshold0;

	// Token: 0x0400395F RID: 14687
	public Color32 temperatureThreshold1;

	// Token: 0x04003960 RID: 14688
	public Color32 temperatureThreshold2;

	// Token: 0x04003961 RID: 14689
	public Color32 temperatureThreshold3;

	// Token: 0x04003962 RID: 14690
	public Color32 temperatureThreshold4;

	// Token: 0x04003963 RID: 14691
	public Color32 temperatureThreshold5;

	// Token: 0x04003964 RID: 14692
	public Color32 temperatureThreshold6;

	// Token: 0x04003965 RID: 14693
	public Color32 temperatureThreshold7;

	// Token: 0x04003966 RID: 14694
	public Color32 heatflowThreshold0;

	// Token: 0x04003967 RID: 14695
	public Color32 heatflowThreshold1;

	// Token: 0x04003968 RID: 14696
	public Color32 heatflowThreshold2;

	// Token: 0x04003969 RID: 14697
	private Dictionary<string, Color32> namedLookup;
}
