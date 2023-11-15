using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020005DE RID: 1502
public class TriggerIgniteArea : MonoBehaviour
{
	// Token: 0x06001CAD RID: 7341 RVA: 0x0008BFA6 File Offset: 0x0008A3A6
	private void OnTrigger(string _trigger)
	{
		if (this.m_ignitionTrigger == _trigger)
		{
			this.IgniteArea();
		}
	}

	// Token: 0x06001CAE RID: 7342 RVA: 0x0008BFC0 File Offset: 0x0008A3C0
	public void IgniteArea()
	{
		foreach (GameObject obj in this.GridObjectsInRadius(this.m_radius))
		{
			if (obj.RequestComponent<ServerFlammable>() != null)
			{
				ServerFlammable serverFlammable = obj.RequireComponent<ServerFlammable>();
				serverFlammable.Ignite();
			}
		}
	}

	// Token: 0x06001CAF RID: 7343 RVA: 0x0008C038 File Offset: 0x0008A438
	private IEnumerable<GameObject> GridObjectsInRadius(float _radius)
	{
		int gridCount = GridManager.GetActiveCount();
		for (int g = 0; g < gridCount; g++)
		{
			GridManager gridManager = GridManager.GetActive(g);
			GridIndex gridIndex = gridManager.GetUnclampedGridLocationFromPos(base.transform.position);
			Vector3 referencePos = gridManager.GetPosFromGridLocation(gridIndex);
			int x = gridIndex.X;
			int y = gridIndex.Y;
			int z = gridIndex.Z;
			int total = 1;
			bool bContinue = false;
			do
			{
				bContinue = false;
				for (int i = -total; i <= total; i++)
				{
					int jPve = total - Mathf.Abs(i);
					GridIndex testIndex = new GridIndex(x + i, y, z + jPve);
					GridIndex testIndex2 = new GridIndex(x + i, y, z - jPve);
					GridIndex[] testIndices = new GridIndex[]
					{
						testIndex,
						testIndex2
					};
					for (int j = 0; j < testIndices.Length; j++)
					{
						Vector3 position = gridManager.GetPosFromGridLocation(testIndices[j]);
						if ((referencePos - position).sqrMagnitude < this.m_radius * this.m_radius)
						{
							bContinue = true;
							GameObject occupier = gridManager.GetGridOccupant(testIndices[j]);
							if (occupier != null)
							{
								yield return occupier;
							}
						}
					}
				}
				total++;
			}
			while (bContinue);
		}
		yield break;
	}

	// Token: 0x0400165D RID: 5725
	[SerializeField]
	private float m_radius;

	// Token: 0x0400165E RID: 5726
	[SerializeField]
	private string m_ignitionTrigger;
}
