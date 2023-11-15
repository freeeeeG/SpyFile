using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000974 RID: 2420
[AddComponentMenu("KMonoBehaviour/scripts/SituationalAnim")]
public class SituationalAnim : KMonoBehaviour
{
	// Token: 0x06004748 RID: 18248 RVA: 0x00192CD8 File Offset: 0x00190ED8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		SituationalAnim.Situation situation = this.GetSituation();
		DebugUtil.LogArgs(new object[]
		{
			"Situation is",
			situation
		});
		this.SetAnimForSituation(situation);
	}

	// Token: 0x06004749 RID: 18249 RVA: 0x00192D18 File Offset: 0x00190F18
	private void SetAnimForSituation(SituationalAnim.Situation situation)
	{
		foreach (global::Tuple<SituationalAnim.Situation, string> tuple in this.anims)
		{
			if ((tuple.first & situation) == tuple.first)
			{
				DebugUtil.LogArgs(new object[]
				{
					"Chose Anim",
					tuple.first,
					tuple.second
				});
				this.SetAnim(tuple.second);
				break;
			}
		}
	}

	// Token: 0x0600474A RID: 18250 RVA: 0x00192DAC File Offset: 0x00190FAC
	private void SetAnim(string animName)
	{
		base.GetComponent<KBatchedAnimController>().Play(animName, KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x0600474B RID: 18251 RVA: 0x00192DCC File Offset: 0x00190FCC
	private SituationalAnim.Situation GetSituation()
	{
		SituationalAnim.Situation situation = (SituationalAnim.Situation)0;
		Extents extents = base.GetComponent<Building>().GetExtents();
		int x = extents.x;
		int num = extents.x + extents.width - 1;
		int y = extents.y;
		int num2 = extents.y + extents.height - 1;
		if (this.DoesSatisfy(this.GetSatisfactionForEdge(x, num, y - 1, y - 1), this.mustSatisfy))
		{
			situation |= SituationalAnim.Situation.Bottom;
		}
		if (this.DoesSatisfy(this.GetSatisfactionForEdge(x - 1, x - 1, y, num2), this.mustSatisfy))
		{
			situation |= SituationalAnim.Situation.Left;
		}
		if (this.DoesSatisfy(this.GetSatisfactionForEdge(x, num, num2 + 1, num2 + 1), this.mustSatisfy))
		{
			situation |= SituationalAnim.Situation.Top;
		}
		if (this.DoesSatisfy(this.GetSatisfactionForEdge(num + 1, num + 1, y, num2), this.mustSatisfy))
		{
			situation |= SituationalAnim.Situation.Right;
		}
		return situation;
	}

	// Token: 0x0600474C RID: 18252 RVA: 0x00192EA0 File Offset: 0x001910A0
	private bool DoesSatisfy(SituationalAnim.MustSatisfy result, SituationalAnim.MustSatisfy requirement)
	{
		if (requirement == SituationalAnim.MustSatisfy.All)
		{
			return result == SituationalAnim.MustSatisfy.All;
		}
		if (requirement == SituationalAnim.MustSatisfy.Any)
		{
			return result > SituationalAnim.MustSatisfy.None;
		}
		return result == SituationalAnim.MustSatisfy.None;
	}

	// Token: 0x0600474D RID: 18253 RVA: 0x00192EB8 File Offset: 0x001910B8
	private SituationalAnim.MustSatisfy GetSatisfactionForEdge(int minx, int maxx, int miny, int maxy)
	{
		bool flag = false;
		bool flag2 = true;
		for (int i = minx; i <= maxx; i++)
		{
			for (int j = miny; j <= maxy; j++)
			{
				int arg = Grid.XYToCell(i, j);
				if (this.test(arg))
				{
					flag = true;
				}
				else
				{
					flag2 = false;
				}
			}
		}
		if (flag2)
		{
			return SituationalAnim.MustSatisfy.All;
		}
		if (flag)
		{
			return SituationalAnim.MustSatisfy.Any;
		}
		return SituationalAnim.MustSatisfy.None;
	}

	// Token: 0x04002F37 RID: 12087
	public List<global::Tuple<SituationalAnim.Situation, string>> anims;

	// Token: 0x04002F38 RID: 12088
	public Func<int, bool> test;

	// Token: 0x04002F39 RID: 12089
	public SituationalAnim.MustSatisfy mustSatisfy;

	// Token: 0x020017DA RID: 6106
	[Flags]
	public enum Situation
	{
		// Token: 0x0400702F RID: 28719
		Left = 1,
		// Token: 0x04007030 RID: 28720
		Right = 2,
		// Token: 0x04007031 RID: 28721
		Top = 4,
		// Token: 0x04007032 RID: 28722
		Bottom = 8
	}

	// Token: 0x020017DB RID: 6107
	public enum MustSatisfy
	{
		// Token: 0x04007034 RID: 28724
		None,
		// Token: 0x04007035 RID: 28725
		Any,
		// Token: 0x04007036 RID: 28726
		All
	}
}
