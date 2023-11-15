using System;
using UnityEngine;

// Token: 0x020000E8 RID: 232
public class BloomMaterialControl : MonoBehaviour
{
	// Token: 0x06000841 RID: 2113 RVA: 0x0002F5C5 File Offset: 0x0002D7C5
	public void InitMat(SpriteRenderer[] sprs, float intensity, bool ifBullet = false)
	{
		this.ifBullet = ifBullet;
		this.sprs = sprs;
		this.intensity = intensity;
		this.ifParticleMode = false;
		this.lastAlpha_Float = -1f;
		if (!this.ifInited)
		{
			this.UpdateMat();
			this.ifInited = true;
		}
	}

	// Token: 0x06000842 RID: 2114 RVA: 0x0002F604 File Offset: 0x0002D804
	public void InitMat(SpriteRenderer spr, float intensity, bool ifBullet = false)
	{
		this.ifBullet = ifBullet;
		this.sprs = new SpriteRenderer[1];
		this.sprs[0] = spr;
		this.intensity = intensity;
		this.ifParticleMode = false;
		this.lastAlpha_Float = -1f;
		if (!this.ifInited)
		{
			this.UpdateMat();
			this.ifInited = true;
		}
	}

	// Token: 0x06000843 RID: 2115 RVA: 0x0002F65B File Offset: 0x0002D85B
	public void InitMat(ParticleSystemRenderer partRenderer, float intensity, bool ifAlpha = false)
	{
		this.ifBullet = ifAlpha;
		this.partRenderer = partRenderer;
		this.intensity = intensity;
		this.ifParticleMode = true;
		this.lastAlpha_Float = -1f;
		if (!this.ifInited)
		{
			this.UpdateMat();
			this.ifInited = true;
		}
	}

	// Token: 0x06000844 RID: 2116 RVA: 0x0002F69C File Offset: 0x0002D89C
	private void UpdateMat()
	{
		this.lastGlobalIntensity = ResourceLibrary.Inst.GetFloat_GlowMulti();
		this.lastAlpha_Float = Battle.inst.DIY_BulletAlpha_Float;
		this.lastAlpha_OpenFlag = Battle.inst.DIY_BulletAlpha_BoolFlag;
		if (Setting.Inst.setInts[0] == 0)
		{
			if (!this.ifParticleMode)
			{
				foreach (SpriteRenderer spriteRenderer in this.sprs)
				{
					spriteRenderer.material = ResourceLibrary.Inst.matNormalUnlit;
					if (Battle.inst.DIY_BulletAlpha_BoolFlag && this.ifBullet)
					{
						spriteRenderer.color = spriteRenderer.color.SetAlpha(Battle.inst.DIY_BulletAlpha_Float);
					}
				}
				return;
			}
			if (this.ifBullet && Battle.inst.DIY_BulletAlpha_BoolFlag)
			{
				this.partRenderer.material = ResourceLibrary.Inst.matAlphaUnlit;
				MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
				float diy_BulletAlpha_Float = Battle.inst.DIY_BulletAlpha_Float;
				materialPropertyBlock.SetFloat("_Alpha", diy_BulletAlpha_Float);
				this.partRenderer.SetPropertyBlock(materialPropertyBlock);
				return;
			}
			this.partRenderer.material = ResourceLibrary.Inst.matNormalUnlit;
			return;
		}
		else
		{
			if (this.ifParticleMode)
			{
				this.partRenderer.SetMaterialAndIntensity(this.intensity, this.ifBullet, null);
				return;
			}
			SpriteRenderer[] array = this.sprs;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetMaterialAndIntensity(this.intensity, this.ifBullet, null);
			}
			return;
		}
	}

	// Token: 0x06000845 RID: 2117 RVA: 0x0002F808 File Offset: 0x0002DA08
	public void CopyBlock(MaterialPropertyBlock block)
	{
		if (this.ifParticleMode)
		{
			this.partRenderer.SetPropertyBlock(block);
			return;
		}
		SpriteRenderer[] array = this.sprs;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetPropertyBlock(block);
		}
	}

	// Token: 0x06000846 RID: 2118 RVA: 0x0002F848 File Offset: 0x0002DA48
	public MaterialPropertyBlock GetBlock()
	{
		MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
		if (this.ifParticleMode)
		{
			this.partRenderer.GetPropertyBlock(materialPropertyBlock);
		}
		else
		{
			this.sprs[0].GetPropertyBlock(materialPropertyBlock);
		}
		return materialPropertyBlock;
	}

	// Token: 0x06000847 RID: 2119 RVA: 0x0002F880 File Offset: 0x0002DA80
	private void Update()
	{
		if (!this.ifInited)
		{
			return;
		}
		if (ResourceLibrary.Inst.GetFloat_GlowMulti() != this.lastGlobalIntensity || this.lastAlpha_Float != Battle.inst.DIY_BulletAlpha_Float || this.lastAlpha_OpenFlag != Battle.inst.DIY_BulletAlpha_BoolFlag)
		{
			this.UpdateMat();
		}
	}

	// Token: 0x040006C3 RID: 1731
	[SerializeField]
	private bool ifInited;

	// Token: 0x040006C4 RID: 1732
	[SerializeField]
	private SpriteRenderer[] sprs;

	// Token: 0x040006C5 RID: 1733
	[SerializeField]
	private float intensity;

	// Token: 0x040006C6 RID: 1734
	[SerializeField]
	private float lastGlobalIntensity;

	// Token: 0x040006C7 RID: 1735
	[Header("ParticleMode")]
	[SerializeField]
	private bool ifParticleMode;

	// Token: 0x040006C8 RID: 1736
	[SerializeField]
	private ParticleSystemRenderer partRenderer;

	// Token: 0x040006C9 RID: 1737
	[Header("BulletAlpha")]
	[SerializeField]
	private bool ifBullet;

	// Token: 0x040006CA RID: 1738
	[SerializeField]
	private float lastAlpha_Float = 1f;

	// Token: 0x040006CB RID: 1739
	[SerializeField]
	private bool lastAlpha_OpenFlag;
}
