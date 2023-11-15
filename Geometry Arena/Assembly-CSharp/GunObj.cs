using System;
using UnityEngine;

// Token: 0x020000F8 RID: 248
public class GunObj : MonoBehaviour
{
	// Token: 0x170000FC RID: 252
	// (get) Token: 0x060008E4 RID: 2276 RVA: 0x000342F8 File Offset: 0x000324F8
	// (set) Token: 0x060008E5 RID: 2277 RVA: 0x00034314 File Offset: 0x00032514
	[SerializeField]
	private float PosX
	{
		get
		{
			return this.spr.gameObject.transform.localPosition.x;
		}
		set
		{
			this.spr.gameObject.transform.localPosition = new Vector2(value, this.spr.gameObject.transform.localPosition.y);
		}
	}

	// Token: 0x060008E6 RID: 2278 RVA: 0x00034350 File Offset: 0x00032550
	private void Awake()
	{
		this.posXonOrigin = this.PosX;
	}

	// Token: 0x060008E7 RID: 2279 RVA: 0x0003435E File Offset: 0x0003255E
	private void FixedUpdate()
	{
		this.GunShiftRecover();
	}

	// Token: 0x060008E8 RID: 2280 RVA: 0x00034368 File Offset: 0x00032568
	public void GunShiftOnceOnShoot()
	{
		this.PosX = this.posXonFire;
		float gunFireBack_SmoothTime = GameParameters.Inst.GunFireBack_SmoothTime;
		BasicUnit basicUnit = base.transform.root.gameObject.GetComponent<BasicUnit>();
		if (basicUnit == null)
		{
			basicUnit = Player.inst.unit;
		}
		float num = basicUnit.FactorTotalNew.fireSpd;
		if (num == 0f)
		{
			num = 9f;
		}
		this.smoothTimeTotal = gunFireBack_SmoothTime / Mathf.Sqrt(num);
		if (basicUnit.objType == EnumObjType.ENEMY)
		{
			this.smoothTimeTotal /= 3f;
		}
	}

	// Token: 0x060008E9 RID: 2281 RVA: 0x000343FC File Offset: 0x000325FC
	public void GunShiftRecover()
	{
		if (this.PosX == this.posXonOrigin)
		{
			return;
		}
		float posX = Mathf.SmoothDamp(this.PosX, this.posXonOrigin, ref this.refVelo, this.smoothTimeTotal);
		this.PosX = posX;
	}

	// Token: 0x060008EA RID: 2282 RVA: 0x0003443D File Offset: 0x0003263D
	public Vector2 GetDirection()
	{
		return MyTool.AngleToVec2(base.transform.eulerAngles.z);
	}

	// Token: 0x04000765 RID: 1893
	public Transform posEmit;

	// Token: 0x04000766 RID: 1894
	public SpriteRenderer spr;

	// Token: 0x04000767 RID: 1895
	[SerializeField]
	private float posXonOrigin;

	// Token: 0x04000768 RID: 1896
	[SerializeField]
	private float posXonFire;

	// Token: 0x04000769 RID: 1897
	private float refVelo;

	// Token: 0x0400076A RID: 1898
	[SerializeField]
	private float smoothTimeTotal = 0.5f;
}
