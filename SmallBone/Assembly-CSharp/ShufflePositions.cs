using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000089 RID: 137
public class ShufflePositions : MonoBehaviour
{
	// Token: 0x0600029F RID: 671 RVA: 0x0000A772 File Offset: 0x00008972
	private void Awake()
	{
		this.Initialize();
		if (this._shuffleOnAwake)
		{
			this.Shuffle();
		}
	}

	// Token: 0x060002A0 RID: 672 RVA: 0x0000A788 File Offset: 0x00008988
	private void Initialize()
	{
		this._childs.Clear();
		this._positions.Clear();
		for (int i = 0; i < base.transform.childCount; i++)
		{
			Transform child = base.transform.GetChild(i);
			this._childs.Add(child);
			this._positions.Add(child.position);
		}
	}

	// Token: 0x060002A1 RID: 673 RVA: 0x0000A7EC File Offset: 0x000089EC
	public void Shuffle()
	{
		this._positions.Shuffle<Vector3>();
		for (int i = 0; i < this._childs.Count; i++)
		{
			this._childs[i].transform.position = this._positions[i];
		}
	}

	// Token: 0x0400022E RID: 558
	[SerializeField]
	private bool _shuffleOnAwake = true;

	// Token: 0x0400022F RID: 559
	private List<Transform> _childs = new List<Transform>();

	// Token: 0x04000230 RID: 560
	private List<Vector3> _positions = new List<Vector3>();
}
