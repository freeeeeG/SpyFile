using System;
using UnityEngine;

// Token: 0x02000041 RID: 65
public class Particle : MonoBehaviour
{
	// Token: 0x170000C2 RID: 194
	// (get) Token: 0x0600028F RID: 655 RVA: 0x0000F420 File Offset: 0x0000D620
	// (set) Token: 0x06000290 RID: 656 RVA: 0x0000F46C File Offset: 0x0000D66C
	[SerializeField]
	private ParticleSystem Part
	{
		get
		{
			if (this.compParticleSystem == null)
			{
				this.compParticleSystem = base.gameObject.GetComponent<ParticleSystem>();
			}
			if (this.compParticleSystem == null)
			{
				Debug.LogError("part==Null");
				return null;
			}
			return this.compParticleSystem;
		}
		set
		{
			this.compParticleSystem = value;
		}
	}

	// Token: 0x06000291 RID: 657 RVA: 0x0000F475 File Offset: 0x0000D675
	private void Awake()
	{
		this.compParticleSystem = base.gameObject.GetComponent<ParticleSystem>();
		base.gameObject.name = this.partName;
	}

	// Token: 0x06000292 RID: 658 RVA: 0x000051D0 File Offset: 0x000033D0
	private void Update()
	{
	}

	// Token: 0x06000293 RID: 659 RVA: 0x0000F49C File Offset: 0x0000D69C
	public void OnParticleSystemStopped()
	{
		if (this.inPool)
		{
			Debug.LogWarning("HasInPool");
			return;
		}
		this.inPool = true;
		this.Part.main.loop = this.ifLoop;
		ObjectPool.inst.Particle_GoPool(base.gameObject);
	}

	// Token: 0x06000294 RID: 660 RVA: 0x0000F4EC File Offset: 0x0000D6EC
	private void InitParticle(EnumShapeType shape, Color color)
	{
		this.inPool = false;
		ParticleSystem.MainModule main = this.Part.main;
		ParticleSystem.ShapeModule shape2 = this.Part.shape;
		this.ifLoop = main.loop;
		Sprite sprite_Shape = ResourceLibrary.Inst.GetSprite_Shape(shape, true);
		this.Part.textureSheetAnimation.SetSprite(0, sprite_Shape);
		float num = color.Saturation();
		float num2 = color.Value();
		Color min = color.SetSaturation(this.colorSatFacMin * num).SetValue(this.colorValFacMin * num2);
		Color max = color.SetSaturation(this.colorSatFacMax * num).SetValue(this.colorValFacMax * num2);
		main.startColor = new ParticleSystem.MinMaxGradient(min, max);
	}

	// Token: 0x06000295 RID: 661 RVA: 0x0000F5A0 File Offset: 0x0000D7A0
	public void Blast_Init(EnumShapeType shape, Color color, float scale, bool bloom_IfBullet = false)
	{
		this.InitParticle(shape, color);
		this.Blast_SetSize(scale);
		ParticleSystemRenderer component = base.gameObject.GetComponent<ParticleSystemRenderer>();
		if (bloom_IfBullet)
		{
			this.bloom.InitMat(component, ResourceLibrary.Inst.GlowIntensity_Bullet, true);
			return;
		}
		this.bloom.InitMat(component, ResourceLibrary.Inst.GlowIntensity_OtherParticle, false);
	}

	// Token: 0x06000296 RID: 662 RVA: 0x0000F5FC File Offset: 0x0000D7FC
	public void Skill_Destroy_Init(EnumShapeType shape, Color color, float scale)
	{
		ParticleSystemRenderer component = base.gameObject.GetComponent<ParticleSystemRenderer>();
		this.bloom.InitMat(component, ResourceLibrary.Inst.GlowIntensity_OtherParticle, true);
		this.InitParticle(shape, color);
		ParticleSystem.MainModule main = this.Part.main;
		ParticleSystem.ShapeModule shape2 = this.Part.shape;
		main.startSize = new ParticleSystem.MinMaxCurve(scale * this.sizeFacMin, scale * this.sizeFacMax);
		shape2.radius = scale / 2f;
		shape2.radiusThickness = 1f;
	}

	// Token: 0x06000297 RID: 663 RVA: 0x0000F684 File Offset: 0x0000D884
	public void Trail_Fragment_Init(EnumShapeType shape, Color color, float scale)
	{
		this.InitParticle(shape, color);
		this.BulletTrail_UpdateSizeAndRotate(scale, 0f);
		ParticleSystemRenderer component = base.gameObject.GetComponent<ParticleSystemRenderer>();
		this.bloom.InitMat(component, ResourceLibrary.Inst.GlowIntensity_OtherParticle, false);
	}

	// Token: 0x06000298 RID: 664 RVA: 0x0000F6C8 File Offset: 0x0000D8C8
	public void Trail_Bullet_Init(EnumShapeType shape, Color color, float scale)
	{
		this.InitParticle(shape, color);
		this.BulletTrail_UpdateSizeAndRotate(scale, 0f);
		ParticleSystemRenderer component = base.gameObject.GetComponent<ParticleSystemRenderer>();
		this.bloom.InitMat(component, ResourceLibrary.Inst.GlowIntensity_Bullet, true);
	}

	// Token: 0x06000299 RID: 665 RVA: 0x0000F70C File Offset: 0x0000D90C
	public void EnmeyPreGene_Init(EnumShapeType shape, Color color, float scale)
	{
		this.InitParticle(shape, color);
		this.Blast_SetSize(scale);
		ParticleSystemRenderer component = base.gameObject.GetComponent<ParticleSystemRenderer>();
		this.bloom.InitMat(component, ResourceLibrary.Inst.GlowIntensity_OtherParticle, false);
	}

	// Token: 0x0600029A RID: 666 RVA: 0x0000F74C File Offset: 0x0000D94C
	public void Close()
	{
		this.Part.main.loop = false;
	}

	// Token: 0x0600029B RID: 667 RVA: 0x0000F770 File Offset: 0x0000D970
	private void Blast_SetSize(float scale)
	{
		ParticleSystem.MainModule main = this.Part.main;
		ParticleSystem.ShapeModule shape = this.Part.shape;
		shape.radius = 0.5f * scale;
		scale = Mathf.Min(scale, this.blast_MaxSize);
		main.startSize = new ParticleSystem.MinMaxCurve(scale * this.sizeFacMin, scale * this.sizeFacMax);
		shape.radiusThickness = 1f;
	}

	// Token: 0x0600029C RID: 668 RVA: 0x0000F7DC File Offset: 0x0000D9DC
	public void BulletTrail_UpdateSizeAndRotate(float scale, float angleZ)
	{
		ParticleSystem.MainModule main = this.Part.main;
		ParticleSystem.ShapeModule shape = this.Part.shape;
		main.startSize = new ParticleSystem.MinMaxCurve(scale * this.sizeFacMin, scale * this.sizeFacMax);
		shape.radius = 0.2f;
	}

	// Token: 0x0600029D RID: 669 RVA: 0x0000F82C File Offset: 0x0000DA2C
	public void WantoDestroy()
	{
		this.Part.main.loop = false;
		this.Part.gameObject.transform.parent = null;
		this.Part.gameObject.transform.localScale = Vector2.one;
	}

	// Token: 0x0400025B RID: 603
	public string partName = "Part";

	// Token: 0x0400025C RID: 604
	[SerializeField]
	private float colorSatFacMin = 1f;

	// Token: 0x0400025D RID: 605
	[SerializeField]
	private float colorSatFacMax = 1f;

	// Token: 0x0400025E RID: 606
	[SerializeField]
	private float colorValFacMin = 1f;

	// Token: 0x0400025F RID: 607
	[SerializeField]
	private float colorValFacMax = 1f;

	// Token: 0x04000260 RID: 608
	[SerializeField]
	[Range(0f, 1f)]
	private float sizeFacMin = 0.5f;

	// Token: 0x04000261 RID: 609
	[SerializeField]
	[Range(0f, 1f)]
	private float sizeFacMax = 0.6f;

	// Token: 0x04000262 RID: 610
	[SerializeField]
	private float blast_MaxSize = 5f;

	// Token: 0x04000263 RID: 611
	[SerializeField]
	private bool ifLoop;

	// Token: 0x04000264 RID: 612
	[SerializeField]
	private bool inPool;

	// Token: 0x04000265 RID: 613
	[SerializeField]
	private ParticleSystem compParticleSystem;

	// Token: 0x04000266 RID: 614
	[SerializeField]
	private BloomMaterialControl bloom;
}
