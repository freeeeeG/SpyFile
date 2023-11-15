using System;
using UnityEngine;

// Token: 0x0200004A RID: 74
public class VaultSkill : BuildingSkill
{
	// Token: 0x170000AB RID: 171
	// (get) Token: 0x060001D6 RID: 470 RVA: 0x0000754F File Offset: 0x0000574F
	public override BuildingSkillName BuildingSkillName
	{
		get
		{
			return BuildingSkillName.VAULTSKILL;
		}
	}

	// Token: 0x170000AC RID: 172
	// (get) Token: 0x060001D7 RID: 471 RVA: 0x00007552 File Offset: 0x00005752
	public override ElementType IntensifyElement
	{
		get
		{
			return ElementType.None;
		}
	}

	// Token: 0x170000AD RID: 173
	// (get) Token: 0x060001D8 RID: 472 RVA: 0x00007555 File Offset: 0x00005755
	public override float KeyValue
	{
		get
		{
			return (float)(20 + this.strategy.TotalElementCount);
		}
	}

	// Token: 0x170000AE RID: 174
	// (get) Token: 0x060001D9 RID: 473 RVA: 0x00007566 File Offset: 0x00005766
	public override float KeyValue2
	{
		get
		{
			return (float)(5 + this.strategy.TotalElementCount / 3);
		}
	}

	// Token: 0x170000AF RID: 175
	// (get) Token: 0x060001DA RID: 474 RVA: 0x00007578 File Offset: 0x00005778
	public override string DisplayValue
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized(((int)this.KeyValue).ToString());
		}
	}

	// Token: 0x170000B0 RID: 176
	// (get) Token: 0x060001DB RID: 475 RVA: 0x000075A4 File Offset: 0x000057A4
	public override string DisplayValue2
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized(((int)this.KeyValue2).ToString());
		}
	}

	// Token: 0x170000B1 RID: 177
	// (get) Token: 0x060001DC RID: 476 RVA: 0x000075D0 File Offset: 0x000057D0
	public override string DisplayValue3
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized(this.gainedAmount.ToString());
		}
	}

	// Token: 0x060001DD RID: 477 RVA: 0x000075ED File Offset: 0x000057ED
	public override void EndTurn()
	{
		base.EndTurn();
		this.gainedAmount += (int)this.KeyValue + (int)this.KeyValue2 * Mathf.Min(5, this.successiveTurn);
		this.successiveTurn++;
	}

	// Token: 0x060001DE RID: 478 RVA: 0x0000762C File Offset: 0x0000582C
	public override void MainFuncCallBack()
	{
		GameRes.Coin += this.gainedAmount;
		this.successiveTurn = 0;
	}

	// Token: 0x04000129 RID: 297
	private int gainedAmount;

	// Token: 0x0400012A RID: 298
	private int successiveTurn;
}
