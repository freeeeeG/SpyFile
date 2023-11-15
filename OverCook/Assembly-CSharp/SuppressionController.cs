using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B9C RID: 2972
public class SuppressionController
{
	// Token: 0x06003CE9 RID: 15593 RVA: 0x001234C4 File Offset: 0x001218C4
	public bool IsSuppressed()
	{
		return this.m_suppressors.Count != 0;
	}

	// Token: 0x06003CEA RID: 15594 RVA: 0x001234D7 File Offset: 0x001218D7
	public void UpdateSuppressors()
	{
		this.m_suppressors.RemoveAll((Suppressor x) => x.IsReleased());
	}

	// Token: 0x06003CEB RID: 15595 RVA: 0x00123504 File Offset: 0x00121904
	public Suppressor AddSuppressor(UnityEngine.Object _owner)
	{
		if (_owner == null)
		{
			return null;
		}
		Suppressor suppressor = new Suppressor(_owner);
		this.m_suppressors.Add(suppressor);
		return suppressor;
	}

	// Token: 0x06003CEC RID: 15596 RVA: 0x00123534 File Offset: 0x00121934
	public void Reset()
	{
		for (int i = 0; i < this.m_suppressors.Count; i++)
		{
			this.m_suppressors[i].Release();
		}
		this.m_suppressors.Clear();
	}

	// Token: 0x06003CED RID: 15597 RVA: 0x00123579 File Offset: 0x00121979
	public void MoveSuppressors(SuppressionController suppressionController)
	{
		suppressionController.m_suppressors.AddRange(this.m_suppressors);
		this.m_suppressors.Clear();
	}

	// Token: 0x04003106 RID: 12550
	private List<Suppressor> m_suppressors = new List<Suppressor>();
}
