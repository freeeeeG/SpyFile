using System;
using System.Collections.Generic;
using UnityEngine;

namespace flanne
{
	// Token: 0x020000C4 RID: 196
	public class LineCollider : MonoBehaviour
	{
		// Token: 0x06000643 RID: 1603 RVA: 0x0001CB34 File Offset: 0x0001AD34
		private void LateUpdate()
		{
			Vector3[] positions = this.lineController.GetPositions();
			if (positions.Length >= 2)
			{
				int num = positions.Length - 1;
				if (this.loop)
				{
					this.polygonCollider2D.pathCount = num + 1;
				}
				else
				{
					this.polygonCollider2D.pathCount = num;
				}
				for (int i = 0; i < num; i++)
				{
					List<Vector2> positions2 = new List<Vector2>
					{
						positions[i],
						positions[i + 1]
					};
					List<Vector2> list = this.CalculateColliderPoints(positions2);
					this.polygonCollider2D.SetPath(i, list.ConvertAll<Vector2>((Vector2 p) => base.transform.InverseTransformPoint(p)));
				}
				if (this.loop)
				{
					List<Vector2> positions3 = new List<Vector2>
					{
						positions[num],
						positions[0]
					};
					List<Vector2> list2 = this.CalculateColliderPoints(positions3);
					this.polygonCollider2D.SetPath(num, list2.ConvertAll<Vector2>((Vector2 p) => base.transform.InverseTransformPoint(p)));
					return;
				}
			}
			else
			{
				this.polygonCollider2D.pathCount = 0;
			}
		}

		// Token: 0x06000644 RID: 1604 RVA: 0x0001CC4C File Offset: 0x0001AE4C
		private List<Vector2> CalculateColliderPoints(List<Vector2> positions)
		{
			float width = this.lineController.GetWidth();
			float num = (positions[1].y - positions[0].y) / (positions[1].x - positions[0].x);
			float num2 = width / 2f * (num / Mathf.Pow(num * num + 1f, 0.5f));
			float num3 = width / 2f * (1f / Mathf.Pow(1f + num * num, 0.5f));
			Vector2[] array = new Vector2[]
			{
				new Vector2(-num2, num3),
				new Vector2(num2, -num3)
			};
			return new List<Vector2>
			{
				positions[0] + array[0],
				positions[1] + array[0],
				positions[1] + array[1],
				positions[0] + array[1]
			};
		}

		// Token: 0x04000408 RID: 1032
		[SerializeField]
		private LineController lineController;

		// Token: 0x04000409 RID: 1033
		[SerializeField]
		private PolygonCollider2D polygonCollider2D;

		// Token: 0x0400040A RID: 1034
		[SerializeField]
		private bool loop;

		// Token: 0x0400040B RID: 1035
		private List<Vector2> colliderPoints = new List<Vector2>();
	}
}
