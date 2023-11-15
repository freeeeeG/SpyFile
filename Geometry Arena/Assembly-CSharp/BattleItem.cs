using System;
using UnityEngine;

// Token: 0x02000043 RID: 67
public class BattleItem : MonoBehaviour
{
	// Token: 0x060002A3 RID: 675 RVA: 0x0000F940 File Offset: 0x0000DB40
	public void Init(int typeID)
	{
		this.targetScale = this.setScale;
		base.transform.localScale = Vector2.zero;
		this.typeID = typeID;
		this.spr_Icon.sprite = ResourceLibrary.Inst.SpList_Icon_BattleItem.GetSpriteWithId(typeID);
		int rank = (int)DataBase.Inst.Data_BattleBuffs[typeID].rank;
		this.spr_Outline.color = UI_Setting.Inst.rankColors[rank];
		BattleManager.inst.listBattleItems.Add(this);
		SpriteRenderer[] sprs = new SpriteRenderer[]
		{
			this.spr_Icon,
			this.spr_Outline
		};
		this.bloom.InitMat(sprs, ResourceLibrary.Inst.GlowIntensityItems, false);
	}

	// Token: 0x060002A4 RID: 676 RVA: 0x0000F9FD File Offset: 0x0000DBFD
	private void Update()
	{
		this.UpdateAnim();
		this.moveSpd = Mathf.Max(Player.inst.unit.playerFactorTotal.moveSpd * 1.2f, this.moveSpd);
	}

	// Token: 0x060002A5 RID: 677 RVA: 0x0000FA30 File Offset: 0x0000DC30
	private void FixedUpdate()
	{
		if (!this.inited)
		{
			return;
		}
		if (Player.inst == null)
		{
			return;
		}
		Vector2 vector = Player.inst.gameObject.transform.position - base.transform.position;
		float magnitude = vector.magnitude;
		this.Antibug_MoveRestriction();
		if (magnitude < Player.inst.unit.lastScale / 2f + this.distToActivate)
		{
			this.Activate();
		}
		else if (!this.active && magnitude <= this.distToMove)
		{
			this.active = true;
		}
		if (BattleManager.inst.timeStage != EnumTimeStage.REST && Battle.inst.specialEffect[71] >= 1)
		{
			Vector2 a = Player.inst.transform.position - base.transform.position;
			base.transform.position += a * this.special_SlowMoveSpd * Time.fixedDeltaTime;
		}
		if (this.active)
		{
			Vector2 normalized = vector.normalized;
			base.transform.position += normalized * this.moveSpd * Time.fixedDeltaTime;
			if (magnitude < Player.inst.unit.lastScale / 2f + this.distToActivate + this.moveSpd * Time.fixedDeltaTime)
			{
				this.Activate();
			}
		}
	}

	// Token: 0x060002A6 RID: 678 RVA: 0x0000FBB0 File Offset: 0x0000DDB0
	private void Activate()
	{
		new BattleBuff().Init(this.typeID);
		SpecialEffects.BattleItemActivate();
		Particle component = Object.Instantiate<GameObject>(ResourceLibrary.Inst.Prefab_Particle_UnitBlastTop).GetComponent<Particle>();
		component.transform.position = base.transform.position;
		component.Blast_Init(EnumShapeType.SQUARE, this.spr_Outline.color, this.targetScale, false);
		SoundEffects.Inst.getCoin.PlayRandom();
		BattleManager.inst.listBattleItems.Remove(this);
		Object.Destroy(base.gameObject);
	}

	// Token: 0x060002A7 RID: 679 RVA: 0x0000FC40 File Offset: 0x0000DE40
	public void EndLife()
	{
		Particle component = Object.Instantiate<GameObject>(ResourceLibrary.Inst.Prefab_Particle_UnitBlastTop).GetComponent<Particle>();
		component.transform.position = base.transform.position;
		component.Blast_Init(EnumShapeType.SQUARE, this.spr_Outline.color, this.targetScale, false);
		BattleManager.inst.listBattleItems.Remove(this);
		Object.Destroy(base.gameObject);
	}

	// Token: 0x060002A8 RID: 680 RVA: 0x0000FCAC File Offset: 0x0000DEAC
	private void UpdateAnim()
	{
		if (this.animTimeNow > this.animTimeMax * 1.1f)
		{
			this.inited = true;
			return;
		}
		this.animTimeNow += Time.deltaTime;
		float num = this.animTimeNow / this.animTimeMax;
		num = Mathf.Min(num, 1f);
		float num2 = Mathf.Sqrt(1f - (1f - num) * (1f - num)) * this.targetScale;
		base.transform.localScale = new Vector2(num2, num2);
	}

	// Token: 0x060002A9 RID: 681 RVA: 0x0000FD3C File Offset: 0x0000DF3C
	private void Antibug_MoveRestriction()
	{
		float num = Mathf.Max(0f, 23.1f * SceneObj.inst.SceneSize - base.transform.localScale.x * 0.2f);
		Vector2 vector = base.transform.position;
		float num2 = vector.x;
		float num3 = vector.y;
		if (Mathf.Abs(num2) > num || Mathf.Abs(num3) > num)
		{
			num2 = Mathf.Clamp(num2, -num, num);
			num3 = Mathf.Clamp(num3, -num, num);
			base.transform.position = new Vector2(num2, num3);
		}
	}

	// Token: 0x04000269 RID: 617
	public SpriteRenderer spr_Icon;

	// Token: 0x0400026A RID: 618
	public SpriteRenderer spr_Outline;

	// Token: 0x0400026B RID: 619
	[SerializeField]
	private bool active;

	// Token: 0x0400026C RID: 620
	[SerializeField]
	private float distToMove = 2f;

	// Token: 0x0400026D RID: 621
	[SerializeField]
	private float distToActivate = 0.1f;

	// Token: 0x0400026E RID: 622
	[SerializeField]
	private float moveSpd = 20f;

	// Token: 0x0400026F RID: 623
	[Header("Animation")]
	[SerializeField]
	private float animTimeMax = 1f;

	// Token: 0x04000270 RID: 624
	[SerializeField]
	private float animTimeNow;

	// Token: 0x04000271 RID: 625
	[SerializeField]
	private float targetScale = 3f;

	// Token: 0x04000272 RID: 626
	[SerializeField]
	public int typeID;

	// Token: 0x04000273 RID: 627
	[SerializeField]
	private bool inited;

	// Token: 0x04000274 RID: 628
	[SerializeField]
	private float setScale = 2f;

	// Token: 0x04000275 RID: 629
	[SerializeField]
	private float special_SlowMoveSpd = 3f;

	// Token: 0x04000276 RID: 630
	[SerializeField]
	private BloomMaterialControl bloom;
}
