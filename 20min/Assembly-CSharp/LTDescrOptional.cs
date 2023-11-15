using System;
using UnityEngine;

// Token: 0x0200001A RID: 26
public class LTDescrOptional
{
	// Token: 0x17000009 RID: 9
	// (get) Token: 0x060001D8 RID: 472 RVA: 0x0000DA72 File Offset: 0x0000BC72
	// (set) Token: 0x060001D9 RID: 473 RVA: 0x0000DA7A File Offset: 0x0000BC7A
	public Transform toTrans { get; set; }

	// Token: 0x1700000A RID: 10
	// (get) Token: 0x060001DA RID: 474 RVA: 0x0000DA83 File Offset: 0x0000BC83
	// (set) Token: 0x060001DB RID: 475 RVA: 0x0000DA8B File Offset: 0x0000BC8B
	public Vector3 point { get; set; }

	// Token: 0x1700000B RID: 11
	// (get) Token: 0x060001DC RID: 476 RVA: 0x0000DA94 File Offset: 0x0000BC94
	// (set) Token: 0x060001DD RID: 477 RVA: 0x0000DA9C File Offset: 0x0000BC9C
	public Vector3 axis { get; set; }

	// Token: 0x1700000C RID: 12
	// (get) Token: 0x060001DE RID: 478 RVA: 0x0000DAA5 File Offset: 0x0000BCA5
	// (set) Token: 0x060001DF RID: 479 RVA: 0x0000DAAD File Offset: 0x0000BCAD
	public float lastVal { get; set; }

	// Token: 0x1700000D RID: 13
	// (get) Token: 0x060001E0 RID: 480 RVA: 0x0000DAB6 File Offset: 0x0000BCB6
	// (set) Token: 0x060001E1 RID: 481 RVA: 0x0000DABE File Offset: 0x0000BCBE
	public Quaternion origRotation { get; set; }

	// Token: 0x1700000E RID: 14
	// (get) Token: 0x060001E2 RID: 482 RVA: 0x0000DAC7 File Offset: 0x0000BCC7
	// (set) Token: 0x060001E3 RID: 483 RVA: 0x0000DACF File Offset: 0x0000BCCF
	public LTBezierPath path { get; set; }

	// Token: 0x1700000F RID: 15
	// (get) Token: 0x060001E4 RID: 484 RVA: 0x0000DAD8 File Offset: 0x0000BCD8
	// (set) Token: 0x060001E5 RID: 485 RVA: 0x0000DAE0 File Offset: 0x0000BCE0
	public LTSpline spline { get; set; }

	// Token: 0x17000010 RID: 16
	// (get) Token: 0x060001E6 RID: 486 RVA: 0x0000DAE9 File Offset: 0x0000BCE9
	// (set) Token: 0x060001E7 RID: 487 RVA: 0x0000DAF1 File Offset: 0x0000BCF1
	public LTRect ltRect { get; set; }

	// Token: 0x17000011 RID: 17
	// (get) Token: 0x060001E8 RID: 488 RVA: 0x0000DAFA File Offset: 0x0000BCFA
	// (set) Token: 0x060001E9 RID: 489 RVA: 0x0000DB02 File Offset: 0x0000BD02
	public Action<float> onUpdateFloat { get; set; }

	// Token: 0x17000012 RID: 18
	// (get) Token: 0x060001EA RID: 490 RVA: 0x0000DB0B File Offset: 0x0000BD0B
	// (set) Token: 0x060001EB RID: 491 RVA: 0x0000DB13 File Offset: 0x0000BD13
	public Action<float, float> onUpdateFloatRatio { get; set; }

	// Token: 0x17000013 RID: 19
	// (get) Token: 0x060001EC RID: 492 RVA: 0x0000DB1C File Offset: 0x0000BD1C
	// (set) Token: 0x060001ED RID: 493 RVA: 0x0000DB24 File Offset: 0x0000BD24
	public Action<float, object> onUpdateFloatObject { get; set; }

	// Token: 0x17000014 RID: 20
	// (get) Token: 0x060001EE RID: 494 RVA: 0x0000DB2D File Offset: 0x0000BD2D
	// (set) Token: 0x060001EF RID: 495 RVA: 0x0000DB35 File Offset: 0x0000BD35
	public Action<Vector2> onUpdateVector2 { get; set; }

	// Token: 0x17000015 RID: 21
	// (get) Token: 0x060001F0 RID: 496 RVA: 0x0000DB3E File Offset: 0x0000BD3E
	// (set) Token: 0x060001F1 RID: 497 RVA: 0x0000DB46 File Offset: 0x0000BD46
	public Action<Vector3> onUpdateVector3 { get; set; }

	// Token: 0x17000016 RID: 22
	// (get) Token: 0x060001F2 RID: 498 RVA: 0x0000DB4F File Offset: 0x0000BD4F
	// (set) Token: 0x060001F3 RID: 499 RVA: 0x0000DB57 File Offset: 0x0000BD57
	public Action<Vector3, object> onUpdateVector3Object { get; set; }

	// Token: 0x17000017 RID: 23
	// (get) Token: 0x060001F4 RID: 500 RVA: 0x0000DB60 File Offset: 0x0000BD60
	// (set) Token: 0x060001F5 RID: 501 RVA: 0x0000DB68 File Offset: 0x0000BD68
	public Action<Color> onUpdateColor { get; set; }

	// Token: 0x17000018 RID: 24
	// (get) Token: 0x060001F6 RID: 502 RVA: 0x0000DB71 File Offset: 0x0000BD71
	// (set) Token: 0x060001F7 RID: 503 RVA: 0x0000DB79 File Offset: 0x0000BD79
	public Action<Color, object> onUpdateColorObject { get; set; }

	// Token: 0x17000019 RID: 25
	// (get) Token: 0x060001F8 RID: 504 RVA: 0x0000DB82 File Offset: 0x0000BD82
	// (set) Token: 0x060001F9 RID: 505 RVA: 0x0000DB8A File Offset: 0x0000BD8A
	public Action onComplete { get; set; }

	// Token: 0x1700001A RID: 26
	// (get) Token: 0x060001FA RID: 506 RVA: 0x0000DB93 File Offset: 0x0000BD93
	// (set) Token: 0x060001FB RID: 507 RVA: 0x0000DB9B File Offset: 0x0000BD9B
	public Action<object> onCompleteObject { get; set; }

	// Token: 0x1700001B RID: 27
	// (get) Token: 0x060001FC RID: 508 RVA: 0x0000DBA4 File Offset: 0x0000BDA4
	// (set) Token: 0x060001FD RID: 509 RVA: 0x0000DBAC File Offset: 0x0000BDAC
	public object onCompleteParam { get; set; }

	// Token: 0x1700001C RID: 28
	// (get) Token: 0x060001FE RID: 510 RVA: 0x0000DBB5 File Offset: 0x0000BDB5
	// (set) Token: 0x060001FF RID: 511 RVA: 0x0000DBBD File Offset: 0x0000BDBD
	public object onUpdateParam { get; set; }

	// Token: 0x1700001D RID: 29
	// (get) Token: 0x06000200 RID: 512 RVA: 0x0000DBC6 File Offset: 0x0000BDC6
	// (set) Token: 0x06000201 RID: 513 RVA: 0x0000DBCE File Offset: 0x0000BDCE
	public Action onStart { get; set; }

	// Token: 0x06000202 RID: 514 RVA: 0x0000DBD8 File Offset: 0x0000BDD8
	public void reset()
	{
		this.animationCurve = null;
		this.onUpdateFloat = null;
		this.onUpdateFloatRatio = null;
		this.onUpdateVector2 = null;
		this.onUpdateVector3 = null;
		this.onUpdateFloatObject = null;
		this.onUpdateVector3Object = null;
		this.onUpdateColor = null;
		this.onComplete = null;
		this.onCompleteObject = null;
		this.onCompleteParam = null;
		this.onStart = null;
		this.point = Vector3.zero;
		this.initFrameCount = 0;
	}

	// Token: 0x06000203 RID: 515 RVA: 0x0000DC4C File Offset: 0x0000BE4C
	public void callOnUpdate(float val, float ratioPassed)
	{
		if (this.onUpdateFloat != null)
		{
			this.onUpdateFloat(val);
		}
		if (this.onUpdateFloatRatio != null)
		{
			this.onUpdateFloatRatio(val, ratioPassed);
			return;
		}
		if (this.onUpdateFloatObject != null)
		{
			this.onUpdateFloatObject(val, this.onUpdateParam);
			return;
		}
		if (this.onUpdateVector3Object != null)
		{
			this.onUpdateVector3Object(LTDescr.newVect, this.onUpdateParam);
			return;
		}
		if (this.onUpdateVector3 != null)
		{
			this.onUpdateVector3(LTDescr.newVect);
			return;
		}
		if (this.onUpdateVector2 != null)
		{
			this.onUpdateVector2(new Vector2(LTDescr.newVect.x, LTDescr.newVect.y));
		}
	}

	// Token: 0x040000D6 RID: 214
	public AnimationCurve animationCurve;

	// Token: 0x040000D7 RID: 215
	public int initFrameCount;

	// Token: 0x040000D8 RID: 216
	public Color color;
}
