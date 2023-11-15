using System;
using UnityEngine;
using UnityEngine.Audio;

// Token: 0x02000A3F RID: 2623
internal abstract class AudioVolume : IQuantizedOption, OptionsData.IUnloadable, OptionsData.IUpdatable, OptionsData.IInit, IOption
{
	// Token: 0x060033D5 RID: 13269 RVA: 0x000F3714 File Offset: 0x000F1B14
	public AudioVolume()
	{
	}

	// Token: 0x1700039A RID: 922
	// (get) Token: 0x060033D6 RID: 13270
	public abstract string Label { get; }

	// Token: 0x1700039B RID: 923
	// (get) Token: 0x060033D7 RID: 13271 RVA: 0x000F371C File Offset: 0x000F1B1C
	public OptionsData.Categories Category
	{
		get
		{
			return OptionsData.Categories.Audio;
		}
	}

	// Token: 0x1700039C RID: 924
	// (get) Token: 0x060033D8 RID: 13272 RVA: 0x000F371F File Offset: 0x000F1B1F
	public int Quanta
	{
		get
		{
			return 10;
		}
	}

	// Token: 0x1700039D RID: 925
	// (get) Token: 0x060033D9 RID: 13273
	protected abstract string MixerControlName { get; }

	// Token: 0x1700039E RID: 926
	// (get) Token: 0x060033DA RID: 13274 RVA: 0x000F3723 File Offset: 0x000F1B23
	protected virtual float MaxAudioVolume
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x060033DB RID: 13275 RVA: 0x000F372A File Offset: 0x000F1B2A
	public void Init()
	{
		this.m_mixer = GameUtils.RequireManager<AudioManager>().m_audioMixer;
	}

	// Token: 0x060033DC RID: 13276 RVA: 0x000F373C File Offset: 0x000F1B3C
	public void Update()
	{
		if (this.m_volume != null)
		{
			this.m_mixer.SetFloat(this.MixerControlName, this.m_volume.Value);
		}
	}

	// Token: 0x060033DD RID: 13277 RVA: 0x000F376B File Offset: 0x000F1B6B
	public void Unload()
	{
		this.m_mixer.SetFloat(this.MixerControlName, 1f);
	}

	// Token: 0x060033DE RID: 13278 RVA: 0x000F3784 File Offset: 0x000F1B84
	public void SetOption(int _value)
	{
		float value;
		if (_value == 0)
		{
			value = -80f;
		}
		else
		{
			value = MathUtils.ClampedRemap((float)_value, 0f, (float)this.Quanta, -40f, this.MaxAudioVolume);
		}
		this.m_volume = new float?(value);
	}

	// Token: 0x060033DF RID: 13279 RVA: 0x000F37D0 File Offset: 0x000F1BD0
	public int GetOption()
	{
		float value = 0f;
		if (this.m_volume != null)
		{
			value = this.m_volume.Value;
		}
		else if (this.m_mixer != null)
		{
			this.m_mixer.GetFloat(this.MixerControlName, out value);
		}
		float f = MathUtils.ClampedRemap(value, -40f, this.MaxAudioVolume, 0f, (float)this.Quanta);
		return Mathf.RoundToInt(f);
	}

	// Token: 0x060033E0 RID: 13280 RVA: 0x000F3850 File Offset: 0x000F1C50
	public void Commit()
	{
		if (this.m_mixer != null && this.m_volume != null)
		{
			float b = 0f;
			this.m_mixer.GetFloat(this.MixerControlName, out b);
			if (!Mathf.Approximately(this.m_volume.Value, b))
			{
				this.m_mixer.SetFloat(this.MixerControlName, this.m_volume.Value);
			}
		}
	}

	// Token: 0x040029AB RID: 10667
	private AudioMixer m_mixer;

	// Token: 0x040029AC RID: 10668
	private const float MinAudioVolume = -40f;

	// Token: 0x040029AD RID: 10669
	private const float MuteAudioVolume = -80f;

	// Token: 0x040029AE RID: 10670
	private float? m_volume;
}
