using System;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000DBB RID: 3515
	public class EnergyBombTakeAim : CharacterOperation
	{
		// Token: 0x060046B5 RID: 18101 RVA: 0x000CD308 File Offset: 0x000CB508
		public override void Run(Character owner)
		{
			Bounds bounds = this.GetBounds(owner);
			int num = this._centerAxisPositions.Length;
			float num2 = (bounds.size.x - this._term * (float)num) / (float)num;
			for (int i = 0; i < num; i++)
			{
				float minInclusive = (i == 0) ? 0f : (num2 * (float)i + this._term * (float)i);
				float maxInclusive = num2 * (float)(i + 1) + this._term * (float)i;
				float num3 = UnityEngine.Random.Range(minInclusive, maxInclusive);
				Vector3 vector = new Vector3(bounds.min.x + num3, bounds.max.y) - this._centerAxisPositions[i].transform.position;
				float z = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
				this._centerAxisPositions[i].rotation = Quaternion.Euler(0f, 0f, z);
			}
		}

		// Token: 0x060046B6 RID: 18102 RVA: 0x000CD3F8 File Offset: 0x000CB5F8
		private Bounds GetBounds(Character owner)
		{
			return Physics2D.Raycast(owner.transform.position, Vector2.down, 20f, 262144).collider.bounds;
		}

		// Token: 0x04003588 RID: 13704
		[SerializeField]
		private Transform[] _centerAxisPositions;

		// Token: 0x04003589 RID: 13705
		[SerializeField]
		private float _term = 2f;
	}
}
