using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020003DF RID: 991
public class EventInfoData
{
	// Token: 0x060014D2 RID: 5330 RVA: 0x0006E200 File Offset: 0x0006C400
	public EventInfoData(string title, string description, HashedString animFileName)
	{
		this.title = title;
		this.description = description;
		this.animFileName = animFileName;
	}

	// Token: 0x060014D3 RID: 5331 RVA: 0x0006E24E File Offset: 0x0006C44E
	public List<EventInfoData.Option> GetOptions()
	{
		this.FinalizeText();
		return this.options;
	}

	// Token: 0x060014D4 RID: 5332 RVA: 0x0006E25C File Offset: 0x0006C45C
	public EventInfoData.Option AddOption(string mainText, string description = null)
	{
		EventInfoData.Option option = new EventInfoData.Option
		{
			mainText = mainText,
			description = description
		};
		this.options.Add(option);
		this.dirty = true;
		return option;
	}

	// Token: 0x060014D5 RID: 5333 RVA: 0x0006E294 File Offset: 0x0006C494
	public EventInfoData.Option SimpleOption(string mainText, System.Action callback)
	{
		EventInfoData.Option option = new EventInfoData.Option
		{
			mainText = mainText,
			callback = callback
		};
		this.options.Add(option);
		this.dirty = true;
		return option;
	}

	// Token: 0x060014D6 RID: 5334 RVA: 0x0006E2C9 File Offset: 0x0006C4C9
	public EventInfoData.Option AddDefaultOption(System.Action callback = null)
	{
		return this.SimpleOption(GAMEPLAY_EVENTS.DEFAULT_OPTION_NAME, callback);
	}

	// Token: 0x060014D7 RID: 5335 RVA: 0x0006E2DC File Offset: 0x0006C4DC
	public EventInfoData.Option AddDefaultConsiderLaterOption(System.Action callback = null)
	{
		return this.SimpleOption(GAMEPLAY_EVENTS.DEFAULT_OPTION_CONSIDER_NAME, callback);
	}

	// Token: 0x060014D8 RID: 5336 RVA: 0x0006E2EF File Offset: 0x0006C4EF
	public void SetTextParameter(string key, string value)
	{
		this.textParameters[key] = value;
		this.dirty = true;
	}

	// Token: 0x060014D9 RID: 5337 RVA: 0x0006E308 File Offset: 0x0006C508
	public void FinalizeText()
	{
		if (!this.dirty)
		{
			return;
		}
		this.dirty = false;
		foreach (KeyValuePair<string, string> keyValuePair in this.textParameters)
		{
			string oldValue = "{" + keyValuePair.Key + "}";
			if (this.title != null)
			{
				this.title = this.title.Replace(oldValue, keyValuePair.Value);
			}
			if (this.description != null)
			{
				this.description = this.description.Replace(oldValue, keyValuePair.Value);
			}
			if (this.location != null)
			{
				this.location = this.location.Replace(oldValue, keyValuePair.Value);
			}
			if (this.whenDescription != null)
			{
				this.whenDescription = this.whenDescription.Replace(oldValue, keyValuePair.Value);
			}
			foreach (EventInfoData.Option option in this.options)
			{
				if (option.mainText != null)
				{
					option.mainText = option.mainText.Replace(oldValue, keyValuePair.Value);
				}
				if (option.description != null)
				{
					option.description = option.description.Replace(oldValue, keyValuePair.Value);
				}
				if (option.tooltip != null)
				{
					option.tooltip = option.tooltip.Replace(oldValue, keyValuePair.Value);
				}
				foreach (EventInfoData.OptionIcon optionIcon in option.informationIcons)
				{
					if (optionIcon.tooltip != null)
					{
						optionIcon.tooltip = optionIcon.tooltip.Replace(oldValue, keyValuePair.Value);
					}
				}
				foreach (EventInfoData.OptionIcon optionIcon2 in option.consequenceIcons)
				{
					if (optionIcon2.tooltip != null)
					{
						optionIcon2.tooltip = optionIcon2.tooltip.Replace(oldValue, keyValuePair.Value);
					}
				}
			}
		}
	}

	// Token: 0x04000B3B RID: 2875
	public string title;

	// Token: 0x04000B3C RID: 2876
	public string description;

	// Token: 0x04000B3D RID: 2877
	public string location;

	// Token: 0x04000B3E RID: 2878
	public string whenDescription;

	// Token: 0x04000B3F RID: 2879
	public Transform clickFocus;

	// Token: 0x04000B40 RID: 2880
	public GameObject[] minions;

	// Token: 0x04000B41 RID: 2881
	public GameObject artifact;

	// Token: 0x04000B42 RID: 2882
	public HashedString animFileName;

	// Token: 0x04000B43 RID: 2883
	public HashedString mainAnim = "event";

	// Token: 0x04000B44 RID: 2884
	public Dictionary<string, string> textParameters = new Dictionary<string, string>();

	// Token: 0x04000B45 RID: 2885
	public List<EventInfoData.Option> options = new List<EventInfoData.Option>();

	// Token: 0x04000B46 RID: 2886
	public System.Action showCallback;

	// Token: 0x04000B47 RID: 2887
	private bool dirty;

	// Token: 0x02001050 RID: 4176
	public class OptionIcon
	{
		// Token: 0x0600754D RID: 30029 RVA: 0x002CCCEB File Offset: 0x002CAEEB
		public OptionIcon(Sprite sprite, EventInfoData.OptionIcon.ContainerType containerType, string tooltip, float scale = 1f)
		{
			this.sprite = sprite;
			this.containerType = containerType;
			this.tooltip = tooltip;
			this.scale = scale;
		}

		// Token: 0x040058C7 RID: 22727
		public EventInfoData.OptionIcon.ContainerType containerType;

		// Token: 0x040058C8 RID: 22728
		public Sprite sprite;

		// Token: 0x040058C9 RID: 22729
		public string tooltip;

		// Token: 0x040058CA RID: 22730
		public float scale;

		// Token: 0x02002001 RID: 8193
		public enum ContainerType
		{
			// Token: 0x04009012 RID: 36882
			Neutral,
			// Token: 0x04009013 RID: 36883
			Positive,
			// Token: 0x04009014 RID: 36884
			Negative,
			// Token: 0x04009015 RID: 36885
			Information
		}
	}

	// Token: 0x02001051 RID: 4177
	public class Option
	{
		// Token: 0x0600754E RID: 30030 RVA: 0x002CCD10 File Offset: 0x002CAF10
		public void AddInformationIcon(string tooltip, float scale = 1f)
		{
			this.informationIcons.Add(new EventInfoData.OptionIcon(null, EventInfoData.OptionIcon.ContainerType.Information, tooltip, scale));
		}

		// Token: 0x0600754F RID: 30031 RVA: 0x002CCD26 File Offset: 0x002CAF26
		public void AddPositiveIcon(Sprite sprite, string tooltip, float scale = 1f)
		{
			this.consequenceIcons.Add(new EventInfoData.OptionIcon(sprite, EventInfoData.OptionIcon.ContainerType.Positive, tooltip, scale));
		}

		// Token: 0x06007550 RID: 30032 RVA: 0x002CCD3C File Offset: 0x002CAF3C
		public void AddNeutralIcon(Sprite sprite, string tooltip, float scale = 1f)
		{
			this.consequenceIcons.Add(new EventInfoData.OptionIcon(sprite, EventInfoData.OptionIcon.ContainerType.Neutral, tooltip, scale));
		}

		// Token: 0x06007551 RID: 30033 RVA: 0x002CCD52 File Offset: 0x002CAF52
		public void AddNegativeIcon(Sprite sprite, string tooltip, float scale = 1f)
		{
			this.consequenceIcons.Add(new EventInfoData.OptionIcon(sprite, EventInfoData.OptionIcon.ContainerType.Negative, tooltip, scale));
		}

		// Token: 0x040058CB RID: 22731
		public string mainText;

		// Token: 0x040058CC RID: 22732
		public string description;

		// Token: 0x040058CD RID: 22733
		public string tooltip;

		// Token: 0x040058CE RID: 22734
		public System.Action callback;

		// Token: 0x040058CF RID: 22735
		public List<EventInfoData.OptionIcon> informationIcons = new List<EventInfoData.OptionIcon>();

		// Token: 0x040058D0 RID: 22736
		public List<EventInfoData.OptionIcon> consequenceIcons = new List<EventInfoData.OptionIcon>();

		// Token: 0x040058D1 RID: 22737
		public bool allowed = true;
	}
}
