using System;
using UnityEngine;

// Token: 0x02000A42 RID: 2626
internal abstract class SafeArea : IQuantizedOption, OptionsData.IUnloadable, IOption
{
	// Token: 0x170003A4 RID: 932
	// (get) Token: 0x060033E9 RID: 13289
	public abstract string Label { get; }

	// Token: 0x170003A5 RID: 933
	// (get) Token: 0x060033EA RID: 13290 RVA: 0x000F3906 File Offset: 0x000F1D06
	public OptionsData.Categories Category
	{
		get
		{
			return OptionsData.Categories.SafeArea;
		}
	}

	// Token: 0x060033EB RID: 13291 RVA: 0x000F3909 File Offset: 0x000F1D09
	public void Unload()
	{
		this.SafeAreaAxis = 1f;
	}

	// Token: 0x060033EC RID: 13292 RVA: 0x000F3916 File Offset: 0x000F1D16
	public void SetOption(int _value)
	{
		this.m_area = (float)_value;
		this.SafeAreaAxis = MathUtils.ClampedRemap(this.m_area, 0f, (float)this.Quanta, 0.9f, 1f);
	}

	// Token: 0x170003A6 RID: 934
	// (get) Token: 0x060033ED RID: 13293
	// (set) Token: 0x060033EE RID: 13294
	public abstract float SafeAreaAxis { get; set; }

	// Token: 0x060033EF RID: 13295 RVA: 0x000F3948 File Offset: 0x000F1D48
	public int GetOption()
	{
		float f = MathUtils.ClampedRemap(this.SafeAreaAxis, 0.9f, 1f, 0f, (float)this.Quanta);
		return Mathf.RoundToInt(f);
	}

	// Token: 0x060033F0 RID: 13296 RVA: 0x000F397D File Offset: 0x000F1D7D
	public void Commit()
	{
		this.SafeAreaAxis = MathUtils.ClampedRemap(this.m_area, 0f, (float)this.Quanta, 0.9f, 1f);
	}

	// Token: 0x170003A7 RID: 935
	// (get) Token: 0x060033F1 RID: 13297 RVA: 0x000F39A6 File Offset: 0x000F1DA6
	public int Quanta
	{
		get
		{
			return 10;
		}
	}

	// Token: 0x040029AF RID: 10671
	private float m_area;
}
