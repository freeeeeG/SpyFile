using System;
using UnityEngine;

// Token: 0x020001E4 RID: 484
public class ImitateTrap : TrapContent
{
	// Token: 0x17000476 RID: 1142
	// (get) Token: 0x06000C72 RID: 3186 RVA: 0x000207A4 File Offset: 0x0001E9A4
	// (set) Token: 0x06000C73 RID: 3187 RVA: 0x000207A8 File Offset: 0x0001E9A8
	public override long DamageAnalysis
	{
		get
		{
			return 0L;
		}
		set
		{
			base.DamageAnalysis = value;
		}
	}

	// Token: 0x17000477 RID: 1143
	// (get) Token: 0x06000C74 RID: 3188 RVA: 0x000207B1 File Offset: 0x0001E9B1
	// (set) Token: 0x06000C75 RID: 3189 RVA: 0x000207B4 File Offset: 0x0001E9B4
	public override int CoinAnalysis
	{
		get
		{
			return 0;
		}
		set
		{
			base.CoinAnalysis = value;
		}
	}

	// Token: 0x06000C76 RID: 3190 RVA: 0x000207C0 File Offset: 0x0001E9C0
	public override void OnContentPass(Enemy enemy, GameTileContent content = null, int index = 0)
	{
		if (content == null)
		{
			content = this;
		}
		if (enemy.PassedTraps.Count > 0)
		{
			index++;
			if (index > enemy.PassedTraps.Count)
			{
				Debug.Log("复制陷阱溢出");
				return;
			}
			TrapContent trapContent = enemy.PassedTraps[enemy.PassedTraps.Count - index];
			if (trapContent != content)
			{
				trapContent.OnContentPass(enemy, content, index);
			}
		}
		if (content.GetComponent<BlinkTrap>() && !this.BlinkedEnemy.Contains(enemy))
		{
			this.BlinkedEnemy.Add(enemy);
		}
	}

	// Token: 0x06000C77 RID: 3191 RVA: 0x00020857 File Offset: 0x0001EA57
	public override void ClearTurnData()
	{
		base.ClearTurnData();
	}
}
