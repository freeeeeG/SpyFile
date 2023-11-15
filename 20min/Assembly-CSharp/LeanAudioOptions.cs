using System;
using UnityEngine;

// Token: 0x0200001E RID: 30
public class LeanAudioOptions
{
	// Token: 0x06000228 RID: 552 RVA: 0x0000E633 File Offset: 0x0000C833
	public LeanAudioOptions setFrequency(int frequencyRate)
	{
		this.frequencyRate = frequencyRate;
		return this;
	}

	// Token: 0x06000229 RID: 553 RVA: 0x0000E63D File Offset: 0x0000C83D
	public LeanAudioOptions setVibrato(Vector3[] vibrato)
	{
		this.vibrato = vibrato;
		return this;
	}

	// Token: 0x0600022A RID: 554 RVA: 0x0000E647 File Offset: 0x0000C847
	public LeanAudioOptions setWaveSine()
	{
		this.waveStyle = LeanAudioOptions.LeanAudioWaveStyle.Sine;
		return this;
	}

	// Token: 0x0600022B RID: 555 RVA: 0x0000E651 File Offset: 0x0000C851
	public LeanAudioOptions setWaveSquare()
	{
		this.waveStyle = LeanAudioOptions.LeanAudioWaveStyle.Square;
		return this;
	}

	// Token: 0x0600022C RID: 556 RVA: 0x0000E65B File Offset: 0x0000C85B
	public LeanAudioOptions setWaveSawtooth()
	{
		this.waveStyle = LeanAudioOptions.LeanAudioWaveStyle.Sawtooth;
		return this;
	}

	// Token: 0x0600022D RID: 557 RVA: 0x0000E665 File Offset: 0x0000C865
	public LeanAudioOptions setWaveNoise()
	{
		this.waveStyle = LeanAudioOptions.LeanAudioWaveStyle.Noise;
		return this;
	}

	// Token: 0x0600022E RID: 558 RVA: 0x0000E66F File Offset: 0x0000C86F
	public LeanAudioOptions setWaveStyle(LeanAudioOptions.LeanAudioWaveStyle style)
	{
		this.waveStyle = style;
		return this;
	}

	// Token: 0x0600022F RID: 559 RVA: 0x0000E679 File Offset: 0x0000C879
	public LeanAudioOptions setWaveNoiseScale(float waveScale)
	{
		this.waveNoiseScale = waveScale;
		return this;
	}

	// Token: 0x06000230 RID: 560 RVA: 0x0000E683 File Offset: 0x0000C883
	public LeanAudioOptions setWaveNoiseInfluence(float influence)
	{
		this.waveNoiseInfluence = influence;
		return this;
	}

	// Token: 0x040000F8 RID: 248
	public LeanAudioOptions.LeanAudioWaveStyle waveStyle;

	// Token: 0x040000F9 RID: 249
	public Vector3[] vibrato;

	// Token: 0x040000FA RID: 250
	public Vector3[] modulation;

	// Token: 0x040000FB RID: 251
	public int frequencyRate = 44100;

	// Token: 0x040000FC RID: 252
	public float waveNoiseScale = 1000f;

	// Token: 0x040000FD RID: 253
	public float waveNoiseInfluence = 1f;

	// Token: 0x040000FE RID: 254
	public bool useSetData = true;

	// Token: 0x040000FF RID: 255
	public LeanAudioStream stream;

	// Token: 0x02000283 RID: 643
	public enum LeanAudioWaveStyle
	{
		// Token: 0x040009F7 RID: 2551
		Sine,
		// Token: 0x040009F8 RID: 2552
		Square,
		// Token: 0x040009F9 RID: 2553
		Sawtooth,
		// Token: 0x040009FA RID: 2554
		Noise
	}
}
