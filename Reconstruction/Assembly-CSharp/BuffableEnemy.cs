using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000026 RID: 38
public class BuffableEnemy : MonoBehaviour
{
	// Token: 0x17000009 RID: 9
	// (get) Token: 0x060000BA RID: 186 RVA: 0x00005E10 File Offset: 0x00004010
	// (set) Token: 0x060000BB RID: 187 RVA: 0x00005E18 File Offset: 0x00004018
	public Enemy Enemy { get; set; }

	// Token: 0x060000BC RID: 188 RVA: 0x00005E21 File Offset: 0x00004021
	private void Awake()
	{
		this.Enemy = base.GetComponent<Enemy>();
	}

	// Token: 0x060000BD RID: 189 RVA: 0x00005E30 File Offset: 0x00004030
	public void TimeTick()
	{
		foreach (TimeBuff timeBuff in this.TimeBuffs.ToList<TimeBuff>())
		{
			timeBuff.Tick(Time.deltaTime);
			List<TimeBuff>.Enumerator enumerator;
			if (enumerator.Current.IsFinished)
			{
				this.TimeBuffs.Remove(enumerator.Current);
			}
		}
	}

	// Token: 0x060000BE RID: 190 RVA: 0x00005E8C File Offset: 0x0000408C
	public void TileTick()
	{
		foreach (TileBuff tileBuff in this.TileBuffs.ToList<TileBuff>())
		{
			tileBuff.Tick(1f);
			if (tileBuff.IsFinished)
			{
				this.TileBuffs.Remove(tileBuff);
			}
		}
	}

	// Token: 0x060000BF RID: 191 RVA: 0x00005F00 File Offset: 0x00004100
	public void OnHit()
	{
		foreach (TimeBuff timeBuff in this.TimeBuffs)
		{
			timeBuff.OnHit();
		}
	}

	// Token: 0x060000C0 RID: 192 RVA: 0x00005F50 File Offset: 0x00004150
	public void AddBuff(BuffInfo buffInfo)
	{
		EnemyBuff buff = EnemyBuffFactory.GetBuff((int)buffInfo.EnemyBuffName);
		if (buff.IsTimeBase)
		{
			foreach (TimeBuff timeBuff in this.TimeBuffs)
			{
				if (timeBuff.BuffName == buff.BuffName)
				{
					timeBuff.ApplyBuff(this.Enemy, buffInfo.Stacks, buffInfo.IsAbnormal);
					return;
				}
			}
			this.TimeBuffs.Add(buff as TimeBuff);
		}
		else
		{
			if (!buff.IsStackable)
			{
				using (List<TileBuff>.Enumerator enumerator2 = this.TileBuffs.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						if (enumerator2.Current.BuffName == buff.BuffName)
						{
							return;
						}
					}
				}
			}
			this.TileBuffs.Add(buff as TileBuff);
		}
		buff.ApplyBuff(this.Enemy, buffInfo.Stacks, buffInfo.IsAbnormal);
	}

	// Token: 0x060000C1 RID: 193 RVA: 0x00006068 File Offset: 0x00004268
	public void RemoveAllBuffs()
	{
		foreach (TimeBuff timeBuff in this.TimeBuffs)
		{
			timeBuff.End();
		}
		foreach (TileBuff tileBuff in this.TileBuffs)
		{
			tileBuff.End();
		}
		this.TimeBuffs.Clear();
		this.TileBuffs.Clear();
	}

	// Token: 0x060000C2 RID: 194 RVA: 0x00006110 File Offset: 0x00004310
	public void RemoveBuff(EnemyBuffName buffName)
	{
		foreach (TimeBuff timeBuff in this.TimeBuffs)
		{
			if (timeBuff.BuffName == buffName)
			{
				this.TimeBuffs.Remove(timeBuff);
				return;
			}
		}
		foreach (TileBuff tileBuff in this.TileBuffs)
		{
			if (tileBuff.BuffName == buffName)
			{
				this.TileBuffs.Remove(tileBuff);
				break;
			}
		}
	}

	// Token: 0x040000EC RID: 236
	public List<TileBuff> TileBuffs = new List<TileBuff>();

	// Token: 0x040000ED RID: 237
	public List<TimeBuff> TimeBuffs = new List<TimeBuff>();
}
