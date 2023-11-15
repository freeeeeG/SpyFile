using System;
using STRINGS;

// Token: 0x020005ED RID: 1517
public class DevLightGenerator : Light2D, IMultiSliderControl
{
	// Token: 0x060025DB RID: 9691 RVA: 0x000CDC4B File Offset: 0x000CBE4B
	public DevLightGenerator()
	{
		this.sliderControls = new ISliderControl[]
		{
			new DevLightGenerator.LuxController(this),
			new DevLightGenerator.RangeController(this),
			new DevLightGenerator.FalloffController(this)
		};
	}

	// Token: 0x17000201 RID: 513
	// (get) Token: 0x060025DC RID: 9692 RVA: 0x000CDC7A File Offset: 0x000CBE7A
	string IMultiSliderControl.SidescreenTitleKey
	{
		get
		{
			return "STRINGS.BUILDINGS.PREFABS.DEVLIGHTGENERATOR.NAME";
		}
	}

	// Token: 0x17000202 RID: 514
	// (get) Token: 0x060025DD RID: 9693 RVA: 0x000CDC81 File Offset: 0x000CBE81
	ISliderControl[] IMultiSliderControl.sliderControls
	{
		get
		{
			return this.sliderControls;
		}
	}

	// Token: 0x060025DE RID: 9694 RVA: 0x000CDC89 File Offset: 0x000CBE89
	bool IMultiSliderControl.SidescreenEnabled()
	{
		return true;
	}

	// Token: 0x0400159F RID: 5535
	protected ISliderControl[] sliderControls;

	// Token: 0x0200128E RID: 4750
	protected class LuxController : ISingleSliderControl, ISliderControl
	{
		// Token: 0x06007DC2 RID: 32194 RVA: 0x002E5938 File Offset: 0x002E3B38
		public LuxController(Light2D t)
		{
			this.target = t;
		}

		// Token: 0x1700085C RID: 2140
		// (get) Token: 0x06007DC3 RID: 32195 RVA: 0x002E5947 File Offset: 0x002E3B47
		public string SliderTitleKey
		{
			get
			{
				return "STRINGS.BUILDINGS.PREFABS.DEVLIGHTGENERATOR.BRIGHTNESS_LABEL";
			}
		}

		// Token: 0x1700085D RID: 2141
		// (get) Token: 0x06007DC4 RID: 32196 RVA: 0x002E594E File Offset: 0x002E3B4E
		public string SliderUnits
		{
			get
			{
				return UI.UNITSUFFIXES.LIGHT.LUX;
			}
		}

		// Token: 0x06007DC5 RID: 32197 RVA: 0x002E595A File Offset: 0x002E3B5A
		public float GetSliderMax(int index)
		{
			return 100000f;
		}

		// Token: 0x06007DC6 RID: 32198 RVA: 0x002E5961 File Offset: 0x002E3B61
		public float GetSliderMin(int index)
		{
			return 0f;
		}

		// Token: 0x06007DC7 RID: 32199 RVA: 0x002E5968 File Offset: 0x002E3B68
		public string GetSliderTooltip(int index)
		{
			return string.Format(UI.GAMEOBJECTEFFECTS.EMITS_LIGHT_LUX, this.target.Lux);
		}

		// Token: 0x06007DC8 RID: 32200 RVA: 0x002E5989 File Offset: 0x002E3B89
		public string GetSliderTooltipKey(int index)
		{
			return "<unused>";
		}

		// Token: 0x06007DC9 RID: 32201 RVA: 0x002E5990 File Offset: 0x002E3B90
		public float GetSliderValue(int index)
		{
			return (float)this.target.Lux;
		}

		// Token: 0x06007DCA RID: 32202 RVA: 0x002E599E File Offset: 0x002E3B9E
		public void SetSliderValue(float value, int index)
		{
			this.target.Lux = (int)value;
			this.target.FullRefresh();
		}

		// Token: 0x06007DCB RID: 32203 RVA: 0x002E59B8 File Offset: 0x002E3BB8
		public int SliderDecimalPlaces(int index)
		{
			return 0;
		}

		// Token: 0x04005FFF RID: 24575
		protected Light2D target;
	}

	// Token: 0x0200128F RID: 4751
	protected class RangeController : ISingleSliderControl, ISliderControl
	{
		// Token: 0x06007DCC RID: 32204 RVA: 0x002E59BB File Offset: 0x002E3BBB
		public RangeController(Light2D t)
		{
			this.target = t;
		}

		// Token: 0x1700085E RID: 2142
		// (get) Token: 0x06007DCD RID: 32205 RVA: 0x002E59CA File Offset: 0x002E3BCA
		public string SliderTitleKey
		{
			get
			{
				return "STRINGS.BUILDINGS.PREFABS.DEVLIGHTGENERATOR.RANGE_LABEL";
			}
		}

		// Token: 0x1700085F RID: 2143
		// (get) Token: 0x06007DCE RID: 32206 RVA: 0x002E59D1 File Offset: 0x002E3BD1
		public string SliderUnits
		{
			get
			{
				return UI.UNITSUFFIXES.TILES;
			}
		}

		// Token: 0x06007DCF RID: 32207 RVA: 0x002E59DD File Offset: 0x002E3BDD
		public float GetSliderMax(int index)
		{
			return 20f;
		}

		// Token: 0x06007DD0 RID: 32208 RVA: 0x002E59E4 File Offset: 0x002E3BE4
		public float GetSliderMin(int index)
		{
			return 1f;
		}

		// Token: 0x06007DD1 RID: 32209 RVA: 0x002E59EB File Offset: 0x002E3BEB
		public string GetSliderTooltip(int index)
		{
			return string.Format(UI.GAMEOBJECTEFFECTS.EMITS_LIGHT, this.target.Range);
		}

		// Token: 0x06007DD2 RID: 32210 RVA: 0x002E5A0C File Offset: 0x002E3C0C
		public string GetSliderTooltipKey(int index)
		{
			return "";
		}

		// Token: 0x06007DD3 RID: 32211 RVA: 0x002E5A13 File Offset: 0x002E3C13
		public float GetSliderValue(int index)
		{
			return this.target.Range;
		}

		// Token: 0x06007DD4 RID: 32212 RVA: 0x002E5A21 File Offset: 0x002E3C21
		public void SetSliderValue(float value, int index)
		{
			this.target.Range = (float)((int)value);
			this.target.FullRefresh();
		}

		// Token: 0x06007DD5 RID: 32213 RVA: 0x002E5A3C File Offset: 0x002E3C3C
		public int SliderDecimalPlaces(int index)
		{
			return 0;
		}

		// Token: 0x04006000 RID: 24576
		protected Light2D target;
	}

	// Token: 0x02001290 RID: 4752
	protected class FalloffController : ISingleSliderControl, ISliderControl
	{
		// Token: 0x06007DD6 RID: 32214 RVA: 0x002E5A3F File Offset: 0x002E3C3F
		public FalloffController(Light2D t)
		{
			this.target = t;
		}

		// Token: 0x17000860 RID: 2144
		// (get) Token: 0x06007DD7 RID: 32215 RVA: 0x002E5A4E File Offset: 0x002E3C4E
		public string SliderTitleKey
		{
			get
			{
				return "STRINGS.BUILDINGS.PREFABS.DEVLIGHTGENERATOR.FALLOFF_LABEL";
			}
		}

		// Token: 0x17000861 RID: 2145
		// (get) Token: 0x06007DD8 RID: 32216 RVA: 0x002E5A55 File Offset: 0x002E3C55
		public string SliderUnits
		{
			get
			{
				return UI.UNITSUFFIXES.PERCENT;
			}
		}

		// Token: 0x06007DD9 RID: 32217 RVA: 0x002E5A61 File Offset: 0x002E3C61
		public float GetSliderMax(int index)
		{
			return 100f;
		}

		// Token: 0x06007DDA RID: 32218 RVA: 0x002E5A68 File Offset: 0x002E3C68
		public float GetSliderMin(int index)
		{
			return 1f;
		}

		// Token: 0x06007DDB RID: 32219 RVA: 0x002E5A6F File Offset: 0x002E3C6F
		public string GetSliderTooltip(int index)
		{
			return string.Format("{0}", this.target.FalloffRate * 100f);
		}

		// Token: 0x06007DDC RID: 32220 RVA: 0x002E5A91 File Offset: 0x002E3C91
		public string GetSliderTooltipKey(int index)
		{
			return "";
		}

		// Token: 0x06007DDD RID: 32221 RVA: 0x002E5A98 File Offset: 0x002E3C98
		public float GetSliderValue(int index)
		{
			return this.target.FalloffRate * 100f;
		}

		// Token: 0x06007DDE RID: 32222 RVA: 0x002E5AAC File Offset: 0x002E3CAC
		public void SetSliderValue(float value, int index)
		{
			this.target.FalloffRate = value / 100f;
			this.target.FullRefresh();
		}

		// Token: 0x06007DDF RID: 32223 RVA: 0x002E5ACB File Offset: 0x002E3CCB
		public int SliderDecimalPlaces(int index)
		{
			return 0;
		}

		// Token: 0x04006001 RID: 24577
		protected Light2D target;
	}
}
