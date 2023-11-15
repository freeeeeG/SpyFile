using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000A36 RID: 2614
internal class Resolution : INameListOption, OptionsData.IInit, IOption
{
	// Token: 0x17000392 RID: 914
	// (get) Token: 0x060033B0 RID: 13232 RVA: 0x000F3094 File Offset: 0x000F1494
	public OptionsData.Categories Category
	{
		get
		{
			return OptionsData.Categories.Graphics;
		}
	}

	// Token: 0x060033B1 RID: 13233 RVA: 0x000F3098 File Offset: 0x000F1498
	private UnityEngine.Resolution[] GetResolutions()
	{
		if (Application.isEditor)
		{
			return new UnityEngine.Resolution[]
			{
				this.GetWindowResolution()
			};
		}
		List<UnityEngine.Resolution> resolutions = new List<UnityEngine.Resolution>(Screen.resolutions);
		List<UnityEngine.Resolution> resolutions2 = resolutions;
		if (global::Resolution.<>f__mg$cache0 == null)
		{
			global::Resolution.<>f__mg$cache0 = new Comparison<UnityEngine.Resolution>(global::Resolution.CompareResolution);
		}
		resolutions2.Sort(global::Resolution.<>f__mg$cache0);
		Predicate<UnityEngine.Resolution> match = delegate(UnityEngine.Resolution res)
		{
			int num = resolutions.FindIndex((UnityEngine.Resolution x) => x.width == res.width && x.height == res.height && x.refreshRate == res.refreshRate);
			if (res.width * res.height <= 480000)
			{
				return true;
			}
			if (num < resolutions.Count - 1)
			{
				UnityEngine.Resolution resolution = resolutions[num + 1];
				if (res.width == resolution.width && res.height == resolution.height)
				{
					return true;
				}
			}
			return false;
		};
		resolutions.RemoveAll(match);
		if (resolutions.Count == 0)
		{
			return new UnityEngine.Resolution[]
			{
				this.GetWindowResolution()
			};
		}
		return resolutions.ToArray();
	}

	// Token: 0x060033B2 RID: 13234 RVA: 0x000F3154 File Offset: 0x000F1554
	private static int CompareResolution(UnityEngine.Resolution _a, UnityEngine.Resolution _b)
	{
		return (int)(((float)_a.width + 0.001f) * (float)_a.height - ((float)_b.width + 0.001f) * (float)_b.height);
	}

	// Token: 0x060033B3 RID: 13235 RVA: 0x000F3188 File Offset: 0x000F1588
	private UnityEngine.Resolution GetWindowResolution()
	{
		return new UnityEngine.Resolution
		{
			width = Screen.width,
			height = Screen.height,
			refreshRate = Screen.currentResolution.refreshRate
		};
	}

	// Token: 0x060033B4 RID: 13236 RVA: 0x000F31CA File Offset: 0x000F15CA
	public string[] GetNames()
	{
		return this.GetResolutions().ConvertAll((UnityEngine.Resolution x) => x.width.ToString() + " x " + x.height.ToString());
	}

	// Token: 0x060033B5 RID: 13237 RVA: 0x000F31F4 File Offset: 0x000F15F4
	public void SetOption(int _value)
	{
		int width = Screen.width;
		int height = Screen.height;
		UnityEngine.Resolution[] resolutions = this.GetResolutions();
		int @int = Prefs.GetInt("ResolutionHash", 0);
		int num = 0;
		for (int i = 0; i < resolutions.Length; i++)
		{
			num ^= resolutions[i].width.GetHashCode();
			num ^= resolutions[i].height.GetHashCode();
		}
		if (num == @int)
		{
			UnityEngine.Resolution resolution = resolutions[Mathf.Clamp(_value, 0, resolutions.Length - 1)];
			width = resolution.width;
			height = resolution.height;
		}
		Prefs.SetInt("ResolutionHash", num);
		Generic<float, UnityEngine.Resolution> scoreFunction = (UnityEngine.Resolution _r) => (float)Mathf.Abs(_r.width - width) + 0.01f * (float)Mathf.Abs(_r.height - height);
		this.m_resolution = resolutions.FindLowestScoring(scoreFunction).Value;
	}

	// Token: 0x060033B6 RID: 13238 RVA: 0x000F32F4 File Offset: 0x000F16F4
	public int GetOption()
	{
		UnityEngine.Resolution current = this.m_resolution;
		UnityEngine.Resolution[] resolutions = this.GetResolutions();
		Generic<float, UnityEngine.Resolution> scoreFunction = (UnityEngine.Resolution _r) => (float)Mathf.Abs(_r.width - current.width) + 0.01f * (float)Mathf.Abs(_r.height - current.height);
		return resolutions.FindLowestScoring(scoreFunction).Key;
	}

	// Token: 0x060033B7 RID: 13239 RVA: 0x000F3338 File Offset: 0x000F1738
	public void Commit()
	{
		if (this.m_resolution.width != Screen.width || this.m_resolution.height != Screen.height)
		{
			Screen.SetResolution(this.m_resolution.width, this.m_resolution.height, Screen.fullScreen);
		}
	}

	// Token: 0x060033B8 RID: 13240 RVA: 0x000F338F File Offset: 0x000F178F
	public void Init()
	{
		this.m_resolution.width = Screen.width;
		this.m_resolution.height = Screen.height;
	}

	// Token: 0x17000393 RID: 915
	// (get) Token: 0x060033B9 RID: 13241 RVA: 0x000F33B1 File Offset: 0x000F17B1
	public string Label
	{
		get
		{
			return "OptionType.Resolution";
		}
	}

	// Token: 0x04002994 RID: 10644
	private int m_resolutionIndex;

	// Token: 0x04002995 RID: 10645
	private UnityEngine.Resolution m_resolution;

	// Token: 0x04002996 RID: 10646
	[CompilerGenerated]
	private static Comparison<UnityEngine.Resolution> <>f__mg$cache0;
}
