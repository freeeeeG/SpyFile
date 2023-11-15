using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200053A RID: 1338
public class UserMenu
{
	// Token: 0x06002001 RID: 8193 RVA: 0x000AB7E7 File Offset: 0x000A99E7
	public void Refresh(GameObject go)
	{
		Game.Instance.Trigger(1980521255, go);
	}

	// Token: 0x06002002 RID: 8194 RVA: 0x000AB7FC File Offset: 0x000A99FC
	public void AddButton(GameObject go, KIconButtonMenu.ButtonInfo button, float sort_order = 1f)
	{
		if (button.onClick != null)
		{
			System.Action callback = button.onClick;
			button.onClick = delegate()
			{
				callback();
				Game.Instance.Trigger(1980521255, go);
			};
		}
		this.buttons.Add(new KeyValuePair<KIconButtonMenu.ButtonInfo, float>(button, sort_order));
	}

	// Token: 0x06002003 RID: 8195 RVA: 0x000AB84E File Offset: 0x000A9A4E
	public void AddSlider(GameObject go, UserMenu.SliderInfo slider)
	{
		this.sliders.Add(slider);
	}

	// Token: 0x06002004 RID: 8196 RVA: 0x000AB85C File Offset: 0x000A9A5C
	public void AppendToScreen(GameObject go, UserMenuScreen screen)
	{
		this.buttons.Clear();
		this.sliders.Clear();
		go.Trigger(493375141, null);
		if (this.buttons.Count > 0)
		{
			this.buttons.Sort(delegate(KeyValuePair<KIconButtonMenu.ButtonInfo, float> x, KeyValuePair<KIconButtonMenu.ButtonInfo, float> y)
			{
				if (x.Value == y.Value)
				{
					return 0;
				}
				if (x.Value > y.Value)
				{
					return 1;
				}
				return -1;
			});
			for (int i = 0; i < this.buttons.Count; i++)
			{
				this.sortedButtons.Add(this.buttons[i].Key);
			}
			screen.AddButtons(this.sortedButtons);
			this.sortedButtons.Clear();
		}
		if (this.sliders.Count > 0)
		{
			screen.AddSliders(this.sliders);
		}
	}

	// Token: 0x040011DA RID: 4570
	public const float DECONSTRUCT_PRIORITY = 0f;

	// Token: 0x040011DB RID: 4571
	public const float DRAWPATHS_PRIORITY = 0.1f;

	// Token: 0x040011DC RID: 4572
	public const float FOLLOWCAM_PRIORITY = 0.3f;

	// Token: 0x040011DD RID: 4573
	public const float SETDIRECTION_PRIORITY = 0.4f;

	// Token: 0x040011DE RID: 4574
	public const float AUTOBOTTLE_PRIORITY = 0.4f;

	// Token: 0x040011DF RID: 4575
	public const float AUTOREPAIR_PRIORITY = 0.5f;

	// Token: 0x040011E0 RID: 4576
	public const float DEFAULT_PRIORITY = 1f;

	// Token: 0x040011E1 RID: 4577
	public const float SUITEQUIP_PRIORITY = 2f;

	// Token: 0x040011E2 RID: 4578
	public const float AUTODISINFECT_PRIORITY = 10f;

	// Token: 0x040011E3 RID: 4579
	public const float ROCKETUSAGERESTRICTION_PRIORITY = 11f;

	// Token: 0x040011E4 RID: 4580
	private List<KeyValuePair<KIconButtonMenu.ButtonInfo, float>> buttons = new List<KeyValuePair<KIconButtonMenu.ButtonInfo, float>>();

	// Token: 0x040011E5 RID: 4581
	private List<UserMenu.SliderInfo> sliders = new List<UserMenu.SliderInfo>();

	// Token: 0x040011E6 RID: 4582
	private List<KIconButtonMenu.ButtonInfo> sortedButtons = new List<KIconButtonMenu.ButtonInfo>();

	// Token: 0x020011D2 RID: 4562
	public class SliderInfo
	{
		// Token: 0x04005DB4 RID: 23988
		public MinMaxSlider.LockingType lockType = MinMaxSlider.LockingType.Drag;

		// Token: 0x04005DB5 RID: 23989
		public MinMaxSlider.Mode mode;

		// Token: 0x04005DB6 RID: 23990
		public Slider.Direction direction;

		// Token: 0x04005DB7 RID: 23991
		public bool interactable = true;

		// Token: 0x04005DB8 RID: 23992
		public bool lockRange;

		// Token: 0x04005DB9 RID: 23993
		public string toolTip;

		// Token: 0x04005DBA RID: 23994
		public string toolTipMin;

		// Token: 0x04005DBB RID: 23995
		public string toolTipMax;

		// Token: 0x04005DBC RID: 23996
		public float minLimit;

		// Token: 0x04005DBD RID: 23997
		public float maxLimit = 100f;

		// Token: 0x04005DBE RID: 23998
		public float currentMinValue = 10f;

		// Token: 0x04005DBF RID: 23999
		public float currentMaxValue = 90f;

		// Token: 0x04005DC0 RID: 24000
		public GameObject sliderGO;

		// Token: 0x04005DC1 RID: 24001
		public Action<MinMaxSlider> onMinChange;

		// Token: 0x04005DC2 RID: 24002
		public Action<MinMaxSlider> onMaxChange;
	}
}
