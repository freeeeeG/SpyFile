using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x020009F9 RID: 2553
public class SubstanceTable : ScriptableObject, ISerializationCallbackReceiver
{
	// Token: 0x06004C5A RID: 19546 RVA: 0x001AC63E File Offset: 0x001AA83E
	public List<Substance> GetList()
	{
		return this.list;
	}

	// Token: 0x06004C5B RID: 19547 RVA: 0x001AC648 File Offset: 0x001AA848
	public Substance GetSubstance(SimHashes substance)
	{
		int count = this.list.Count;
		for (int i = 0; i < count; i++)
		{
			if (this.list[i].elementID == substance)
			{
				return this.list[i];
			}
		}
		return null;
	}

	// Token: 0x06004C5C RID: 19548 RVA: 0x001AC68F File Offset: 0x001AA88F
	public void OnBeforeSerialize()
	{
		this.BindAnimList();
	}

	// Token: 0x06004C5D RID: 19549 RVA: 0x001AC697 File Offset: 0x001AA897
	public void OnAfterDeserialize()
	{
		this.BindAnimList();
	}

	// Token: 0x06004C5E RID: 19550 RVA: 0x001AC6A0 File Offset: 0x001AA8A0
	private void BindAnimList()
	{
		foreach (Substance substance in this.list)
		{
			if (substance.anim != null && (substance.anims == null || substance.anims.Length == 0))
			{
				substance.anims = new KAnimFile[1];
				substance.anims[0] = substance.anim;
			}
		}
	}

	// Token: 0x06004C5F RID: 19551 RVA: 0x001AC728 File Offset: 0x001AA928
	public void RemoveDuplicates()
	{
		this.list = this.list.Distinct(new SubstanceTable.SubstanceEqualityComparer()).ToList<Substance>();
	}

	// Token: 0x040031D3 RID: 12755
	[SerializeField]
	private List<Substance> list;

	// Token: 0x040031D4 RID: 12756
	public Material solidMaterial;

	// Token: 0x040031D5 RID: 12757
	public Material liquidMaterial;

	// Token: 0x02001882 RID: 6274
	private class SubstanceEqualityComparer : IEqualityComparer<Substance>
	{
		// Token: 0x060091E3 RID: 37347 RVA: 0x0032A9FE File Offset: 0x00328BFE
		public bool Equals(Substance x, Substance y)
		{
			return x.elementID.Equals(y.elementID);
		}

		// Token: 0x060091E4 RID: 37348 RVA: 0x0032AA1C File Offset: 0x00328C1C
		public int GetHashCode(Substance obj)
		{
			return obj.elementID.GetHashCode();
		}
	}
}
