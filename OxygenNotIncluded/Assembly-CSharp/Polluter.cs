using System;
using UnityEngine;

// Token: 0x020008B9 RID: 2233
public class Polluter : IPolluter
{
	// Token: 0x17000479 RID: 1145
	// (get) Token: 0x06004095 RID: 16533 RVA: 0x001694F0 File Offset: 0x001676F0
	// (set) Token: 0x06004096 RID: 16534 RVA: 0x001694F8 File Offset: 0x001676F8
	public int radius
	{
		get
		{
			return this._radius;
		}
		private set
		{
			this._radius = value;
			if (this._radius == 0)
			{
				global::Debug.LogFormat("[{0}] has a 0 radius noise, this will disable it", new object[]
				{
					this.GetName()
				});
				return;
			}
		}
	}

	// Token: 0x06004097 RID: 16535 RVA: 0x00169523 File Offset: 0x00167723
	public void SetAttributes(Vector2 pos, int dB, GameObject go, string name)
	{
		this.position = pos;
		this.sourceName = name;
		this.decibels = dB;
		this.gameObject = go;
	}

	// Token: 0x06004098 RID: 16536 RVA: 0x00169542 File Offset: 0x00167742
	public string GetName()
	{
		return this.sourceName;
	}

	// Token: 0x06004099 RID: 16537 RVA: 0x0016954A File Offset: 0x0016774A
	public int GetRadius()
	{
		return this.radius;
	}

	// Token: 0x0600409A RID: 16538 RVA: 0x00169552 File Offset: 0x00167752
	public int GetNoise()
	{
		return this.decibels;
	}

	// Token: 0x0600409B RID: 16539 RVA: 0x0016955A File Offset: 0x0016775A
	public GameObject GetGameObject()
	{
		return this.gameObject;
	}

	// Token: 0x0600409C RID: 16540 RVA: 0x00169562 File Offset: 0x00167762
	public Polluter(int radius)
	{
		this.radius = radius;
	}

	// Token: 0x0600409D RID: 16541 RVA: 0x00169571 File Offset: 0x00167771
	public void SetSplat(NoiseSplat new_splat)
	{
		if (new_splat == null && this.splat != null)
		{
			this.Clear();
		}
		this.splat = new_splat;
		if (this.splat != null)
		{
			AudioEventManager.Get().AddSplat(this.splat);
		}
	}

	// Token: 0x0600409E RID: 16542 RVA: 0x001695A3 File Offset: 0x001677A3
	public void Clear()
	{
		if (this.splat != null)
		{
			AudioEventManager.Get().ClearNoiseSplat(this.splat);
			this.splat.Clear();
			this.splat = null;
		}
	}

	// Token: 0x0600409F RID: 16543 RVA: 0x001695CF File Offset: 0x001677CF
	public Vector2 GetPosition()
	{
		return this.position;
	}

	// Token: 0x04002A12 RID: 10770
	private int _radius;

	// Token: 0x04002A13 RID: 10771
	private int decibels;

	// Token: 0x04002A14 RID: 10772
	private Vector2 position;

	// Token: 0x04002A15 RID: 10773
	private string sourceName;

	// Token: 0x04002A16 RID: 10774
	private GameObject gameObject;

	// Token: 0x04002A17 RID: 10775
	private NoiseSplat splat;
}
