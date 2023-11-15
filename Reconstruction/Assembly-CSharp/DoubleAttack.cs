using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200003E RID: 62
public class DoubleAttack : ElementSkill
{
	// Token: 0x17000079 RID: 121
	// (get) Token: 0x06000186 RID: 390 RVA: 0x00006D00 File Offset: 0x00004F00
	public override List<int> InitElements
	{
		get
		{
			return new List<int>
			{
				0,
				0,
				0
			};
		}
	}

	// Token: 0x1700007A RID: 122
	// (get) Token: 0x06000187 RID: 391 RVA: 0x00006D1C File Offset: 0x00004F1C
	public override float KeyValue
	{
		get
		{
			return (float)GameRes.CurrentWave;
		}
	}

	// Token: 0x1700007B RID: 123
	// (get) Token: 0x06000188 RID: 392 RVA: 0x00006D24 File Offset: 0x00004F24
	public override float KeyValue2
	{
		get
		{
			return 60f;
		}
	}

	// Token: 0x1700007C RID: 124
	// (get) Token: 0x06000189 RID: 393 RVA: 0x00006D2B File Offset: 0x00004F2B
	public override string DisplayValue
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized("X");
		}
	}

	// Token: 0x1700007D RID: 125
	// (get) Token: 0x0600018A RID: 394 RVA: 0x00006D42 File Offset: 0x00004F42
	public override string DisplayValue2
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized(GameMultiLang.GetTraduction("ATBATTLESTART"));
		}
	}

	// Token: 0x1700007E RID: 126
	// (get) Token: 0x0600018B RID: 395 RVA: 0x00006D60 File Offset: 0x00004F60
	public override string DisplayValue3
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized(this.KeyValue2.ToString());
		}
	}

	// Token: 0x0600018C RID: 396 RVA: 0x00006D8B File Offset: 0x00004F8B
	public override void StartTurn2()
	{
		if (!this.triggered)
		{
			base.StartTurn2();
			this.Duration = Mathf.Min(this.KeyValue, this.KeyValue2);
			this.triggered = true;
		}
	}

	// Token: 0x0600018D RID: 397 RVA: 0x00006DB9 File Offset: 0x00004FB9
	public override void TickEnd()
	{
		base.TickEnd();
		this.strategy.StartTurnSkill2();
		this.strategy.StartTurnSkill3();
	}

	// Token: 0x0600018E RID: 398 RVA: 0x00006DD7 File Offset: 0x00004FD7
	public override void EndTurn()
	{
		base.EndTurn();
		this.triggered = false;
	}

	// Token: 0x0400011C RID: 284
	private bool triggered;
}
