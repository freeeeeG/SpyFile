using System;
using Unity.Mathematics;
using UnityEngine;

// Token: 0x02000044 RID: 68
public class Fragment : MonoBehaviour
{
	// Token: 0x170000C3 RID: 195
	// (get) Token: 0x060002AB RID: 683 RVA: 0x0000FE38 File Offset: 0x0000E038
	private float Scale
	{
		get
		{
			return base.gameObject.transform.localScale.x;
		}
	}

	// Token: 0x060002AC RID: 684 RVA: 0x0000FE50 File Offset: 0x0000E050
	private void Awake()
	{
		this.particle = base.gameObject.GetComponentInChildren<Particle>();
		this.spr = base.gameObject.GetComponentInChildren<SpriteRenderer>();
		this.rb = base.gameObject.GetComponentInChildren<Rigidbody2D>();
		if (!this.colliderCircle2D.enabled)
		{
			this.colliderCircle2D.enabled = true;
		}
		BattleManager.inst.listFragments.Add(this);
	}

	// Token: 0x060002AD RID: 685 RVA: 0x0000FEB9 File Offset: 0x0000E0B9
	private void Start()
	{
		this.particle.Trail_Fragment_Init(this.shapeType, this.spr.color, this.Scale);
		this.bloom.InitMat(this.spr, ResourceLibrary.Inst.GlowIntensityFragments, false);
	}

	// Token: 0x060002AE RID: 686 RVA: 0x0000FEF9 File Offset: 0x0000E0F9
	private void Update()
	{
		this.UpdateDetectVelocity();
	}

	// Token: 0x060002AF RID: 687 RVA: 0x0000FF04 File Offset: 0x0000E104
	private void FixedUpdate()
	{
		if (Player.inst == null)
		{
			return;
		}
		this.MoveTowardPlayer();
		Vector2 vector = Player.inst.gameObject.transform.position - base.transform.position;
		if (!this.canMove)
		{
			if (BattleManager.inst.timeStage == EnumTimeStage.REST)
			{
				this.canMove = true;
			}
			else if (TempData.inst.modeType == EnumModeType.WANDER)
			{
				this.canMove = true;
			}
			if (this.canMove)
			{
				this.particle.gameObject.SetActive(true);
				this.particle.GetComponent<ParticleSystem>().Play();
			}
		}
		if (this.canMove && vector.magnitude < Player.inst.unit.lastScale / 2f + this.distActivate)
		{
			this.Activate();
		}
	}

	// Token: 0x060002B0 RID: 688 RVA: 0x0000FFDC File Offset: 0x0000E1DC
	private void UpdateDetectVelocity()
	{
		if (this.colliderCircle2D.enabled && this.rb.velocity.magnitude < 0.2f)
		{
			this.rb.velocity = Vector2.zero;
			this.colliderCircle2D.enabled = false;
			Object.Destroy(this.rb);
			this.particle.gameObject.SetActive(false);
		}
	}

	// Token: 0x060002B1 RID: 689 RVA: 0x00010048 File Offset: 0x0000E248
	private void MoveTowardPlayer()
	{
		if (!this.canMove)
		{
			return;
		}
		Vector2 vector = Player.inst.gameObject.transform.position - base.transform.position;
		float d = math.max(vector.magnitude, this.speed);
		Vector2 a = vector.normalized;
		if (TempData.inst.modeType == EnumModeType.WANDER)
		{
			a *= 2f;
		}
		base.transform.position += a * d * Time.fixedDeltaTime;
	}

	// Token: 0x060002B2 RID: 690 RVA: 0x000100E8 File Offset: 0x0000E2E8
	private void Activate()
	{
		TempData.inst.battle.Fragment_Gain(this.goldValue);
		this.particle.WantoDestroy();
		Particle pool_Particle_GetOrNew = ObjectPool.inst.GetPool_Particle_GetOrNew(ResourceLibrary.Inst.Prefab_Particle_UnitBlastTop);
		pool_Particle_GetOrNew.transform.position = base.transform.position;
		pool_Particle_GetOrNew.Blast_Init(EnumShapeType.CIRCLE, this.spr.color, this.Scale, false);
		SoundEffects.Inst.getCoin.PlayRandom();
		BattleManager.inst.listFragments.Remove(this);
		Object.Destroy(base.gameObject);
	}

	// Token: 0x060002B3 RID: 691 RVA: 0x00010184 File Offset: 0x0000E384
	public void Init(EnumShapeType shape, Color color, float scale)
	{
		scale = Mathf.Clamp(scale, 0f, 1f);
		this.saveScale = scale;
		this.spr.sprite = ResourceLibrary.Inst.GetSprite_Shape(shape, false);
		this.shapeType = shape;
		float num = color.Saturation();
		float num2 = color.Value();
		this.spr.color = color.SetValue(num2 * this.facValue).SetSaturation(num * this.facSaturation);
		scale *= this.facScale;
		scale = Mathf.Min(scale, this.maxScale);
		base.transform.localScale = new Vector2(scale, scale);
		this.rb.velocity = UnityEngine.Random.insideUnitCircle * UnityEngine.Random.Range(this.originVeloMin, this.originVeloMax);
		base.transform.Rotate(0f, 0f, UnityEngine.Random.Range(0f, 180f));
	}

	// Token: 0x060002B4 RID: 692 RVA: 0x00010278 File Offset: 0x0000E478
	public static void InstantiateFragments(Vector2 pos, int value, EnumShapeType shape, Color color, float scale)
	{
		float x = UnityEngine.Random.Range(-0.1f, 0.1f);
		float y = UnityEngine.Random.Range(-0.1f, 0.1f);
		Fragment component = Object.Instantiate<GameObject>(ResourceLibrary.Inst.Prefab_Fragment, pos + new Vector2(x, y), Quaternion.identity).GetComponent<Fragment>();
		component.Init(shape, color, scale);
		component.goldValue = value;
	}

	// Token: 0x04000277 RID: 631
	[SerializeField]
	public int goldValue = 1;

	// Token: 0x04000278 RID: 632
	[SerializeField]
	private float speed = 15f;

	// Token: 0x04000279 RID: 633
	[SerializeField]
	private float distActivate = 0.5f;

	// Token: 0x0400027A RID: 634
	private SpriteRenderer spr;

	// Token: 0x0400027B RID: 635
	[SerializeField]
	private bool canMove;

	// Token: 0x0400027C RID: 636
	private Rigidbody2D rb;

	// Token: 0x0400027D RID: 637
	[SerializeField]
	private float originVeloMin = 2f;

	// Token: 0x0400027E RID: 638
	[SerializeField]
	private float originVeloMax = 5f;

	// Token: 0x0400027F RID: 639
	private Particle particle;

	// Token: 0x04000280 RID: 640
	[Header("Surface")]
	[SerializeField]
	[Range(0f, 1f)]
	private float facSaturation = 0.5f;

	// Token: 0x04000281 RID: 641
	[SerializeField]
	[Range(0f, 1f)]
	private float facValue = 0.5f;

	// Token: 0x04000282 RID: 642
	[SerializeField]
	[Range(0f, 1f)]
	private float facScale = 0.5f;

	// Token: 0x04000283 RID: 643
	private EnumShapeType shapeType = EnumShapeType.UNINITED;

	// Token: 0x04000284 RID: 644
	[SerializeField]
	[Range(0f, 1f)]
	private float maxScale = 0.6f;

	// Token: 0x04000285 RID: 645
	[SerializeField]
	private Collider2D colliderCircle2D;

	// Token: 0x04000286 RID: 646
	[SerializeField]
	private BloomMaterialControl bloom;

	// Token: 0x04000287 RID: 647
	public float saveScale = 1f;
}
