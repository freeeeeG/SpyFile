using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020005BE RID: 1470
public class WindAccumulator : MonoBehaviour, IWindReceiver
{
	// Token: 0x06001BDF RID: 7135 RVA: 0x000874FC File Offset: 0x000858FC
	public static List<WindAccumulator> GetAllWindReceivers()
	{
		return WindAccumulator.s_allWindReceivers;
	}

	// Token: 0x06001BE0 RID: 7136 RVA: 0x00087503 File Offset: 0x00085903
	private void OnEnable()
	{
		WindAccumulator.s_allWindReceivers.Add(this);
	}

	// Token: 0x06001BE1 RID: 7137 RVA: 0x00087510 File Offset: 0x00085910
	private void OnDisable()
	{
		this.m_sources.Clear();
		WindAccumulator.s_allWindReceivers.Remove(this);
	}

	// Token: 0x06001BE2 RID: 7138 RVA: 0x00087529 File Offset: 0x00085929
	protected virtual void Start()
	{
	}

	// Token: 0x06001BE3 RID: 7139 RVA: 0x0008752C File Offset: 0x0008592C
	protected virtual void Update()
	{
		this.m_totalForce = Vector3.zero;
		for (int i = 0; i < this.m_sources.Count; i++)
		{
			this.m_totalForce += this.m_sources[i].GetVelocity();
		}
	}

	// Token: 0x06001BE4 RID: 7140 RVA: 0x00087582 File Offset: 0x00085982
	public virtual void AddWindSource(IWindSource _source)
	{
		if (!this.m_sources.Contains(_source))
		{
			this.m_sources.Add(_source);
		}
	}

	// Token: 0x06001BE5 RID: 7141 RVA: 0x000875A1 File Offset: 0x000859A1
	public virtual void RemoveWindSource(IWindSource _source)
	{
		this.m_sources.Remove(_source);
	}

	// Token: 0x06001BE6 RID: 7142 RVA: 0x000875B0 File Offset: 0x000859B0
	public void Reset()
	{
		this.m_sources.Clear();
	}

	// Token: 0x06001BE7 RID: 7143 RVA: 0x000875BD File Offset: 0x000859BD
	public Vector3 GetVelocity()
	{
		return this.m_totalForce;
	}

	// Token: 0x040015E1 RID: 5601
	private static List<WindAccumulator> s_allWindReceivers = new List<WindAccumulator>();

	// Token: 0x040015E2 RID: 5602
	private List<IWindSource> m_sources = new List<IWindSource>();

	// Token: 0x040015E3 RID: 5603
	private Vector3 m_totalForce = Vector3.zero;
}
