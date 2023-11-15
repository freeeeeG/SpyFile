using System;
using UnityEngine;

// Token: 0x020000C9 RID: 201
public class Obj_RandomPlacement_Circle : AObj_RandomPlacement
{
	// Token: 0x060004A1 RID: 1185 RVA: 0x00012908 File Offset: 0x00010B08
	public override void TriggerRandomPlacement()
	{
		DebugManager.Log(eDebugKey.ENVIRONMENT, "觸發隨機放置物件 (" + base.gameObject.name + ")", base.gameObject);
		if (this.list_PlacedObjects != null && this.list_PlacedObjects.Count > 0)
		{
			for (int i = this.list_PlacedObjects.Count - 1; i >= 0; i--)
			{
				Object.Destroy(this.list_PlacedObjects[i]);
			}
			this.list_PlacedObjects.Clear();
		}
		for (int j = 0; j < this.list_RandomPlacementData.Count; j++)
		{
			RandomPlacementData randomPlacementData = this.list_RandomPlacementData[j];
			for (int k = 0; k < randomPlacementData.Count; k++)
			{
				Vector2 vector = Random.insideUnitCircle * (float)this.placementRange;
				int num = Mathf.CeilToInt(vector.x);
				int num2 = Mathf.CeilToInt(vector.y);
				Vector3 newPosition = base.transform.position + new Vector3((float)num, 0f, (float)num2);
				if (!this.list_PlacedObjects.Exists((GameObject obj) => obj.transform.position == newPosition))
				{
					int mask = LayerMask.GetMask(new string[]
					{
						"Tetris",
						"PathObstacle",
						"Default",
						"MouseRaycastOnly"
					});
					RaycastHit raycastHit;
					if (!Physics.Raycast(newPosition + Vector3.up * 1000f, Vector3.down, out raycastHit, 1000f, mask))
					{
						GameObject gameObject = Object.Instantiate<GameObject>(randomPlacementData.prefab, newPosition, Quaternion.identity);
						gameObject.transform.SetParent(base.transform);
						this.list_PlacedObjects.Add(gameObject);
						if (randomPlacementData.DoRandomRotate)
						{
							gameObject.transform.localRotation = Quaternion.Euler(0f, 90f * (float)Random.Range(0, 4), 0f);
						}
					}
					else
					{
						k--;
					}
				}
				else
				{
					k--;
				}
			}
		}
	}

	// Token: 0x0400047D RID: 1149
	[SerializeField]
	[Header("隨機放置的範圍限制(半徑)")]
	private int placementRange;
}
