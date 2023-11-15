using System;
using Characters;
using UnityEngine;

namespace Utils
{
	// Token: 0x02000469 RID: 1129
	[Serializable]
	public class PositionInfo
	{
		// Token: 0x17000436 RID: 1078
		// (get) Token: 0x0600158B RID: 5515 RVA: 0x00043BC7 File Offset: 0x00041DC7
		public PositionInfo.Pivot pivot
		{
			get
			{
				return this._pivot;
			}
		}

		// Token: 0x17000437 RID: 1079
		// (get) Token: 0x0600158C RID: 5516 RVA: 0x00043BCF File Offset: 0x00041DCF
		public Vector2 pivotValue
		{
			get
			{
				return this._pivotValue;
			}
		}

		// Token: 0x0600158D RID: 5517 RVA: 0x00043BD7 File Offset: 0x00041DD7
		public PositionInfo()
		{
			this._pivot = PositionInfo.Pivot.Center;
			this._pivotValue = Vector2.zero;
		}

		// Token: 0x0600158E RID: 5518 RVA: 0x00043BF1 File Offset: 0x00041DF1
		public PositionInfo(bool attach, bool layerOnly, int layerOrderOffset, PositionInfo.Pivot pivot)
		{
			this._pivot = pivot;
			this._pivotValue = PositionInfo._pivotValues[pivot];
		}

		// Token: 0x0600158F RID: 5519 RVA: 0x00043C14 File Offset: 0x00041E14
		public void Attach(ITarget target, Transform obj)
		{
			if (obj == null)
			{
				return;
			}
			Vector3 center = target.collider.bounds.center;
			Vector3 size = target.collider.bounds.size;
			size.x *= this.pivotValue.x;
			size.y *= this.pivotValue.y;
			Vector3 position = center + size;
			obj.position = position;
		}

		// Token: 0x06001590 RID: 5520 RVA: 0x00043C8C File Offset: 0x00041E8C
		public void Attach(Character target, Transform obj)
		{
			if (obj == null)
			{
				return;
			}
			Vector3 center = target.collider.bounds.center;
			Vector3 size = target.collider.bounds.size;
			size.x *= this.pivotValue.x;
			size.y *= this.pivotValue.y;
			Vector3 position = center + size;
			obj.position = position;
		}

		// Token: 0x06001591 RID: 5521 RVA: 0x00043D04 File Offset: 0x00041F04
		public Vector2 GetPosition(Character target)
		{
			Vector3 center = target.collider.bounds.center;
			Vector3 size = target.collider.bounds.size;
			size.x *= this.pivotValue.x;
			size.y *= this.pivotValue.y;
			return center + size;
		}

		// Token: 0x040012D0 RID: 4816
		private static readonly EnumArray<PositionInfo.Pivot, Vector2> _pivotValues = new EnumArray<PositionInfo.Pivot, Vector2>(new Vector2[]
		{
			new Vector2(0f, 0f),
			new Vector2(-0.5f, 0.5f),
			new Vector2(0f, 0.5f),
			new Vector2(0.5f, 0.5f),
			new Vector2(-0.5f, 0f),
			new Vector2(0f, 0.5f),
			new Vector2(-0.5f, -0.5f),
			new Vector2(0f, -0.5f),
			new Vector2(0.5f, -0.5f),
			new Vector2(0f, 0f)
		});

		// Token: 0x040012D1 RID: 4817
		[SerializeField]
		private PositionInfo.Pivot _pivot;

		// Token: 0x040012D2 RID: 4818
		[SerializeField]
		[HideInInspector]
		private Vector2 _pivotValue;

		// Token: 0x0200046A RID: 1130
		public enum Pivot
		{
			// Token: 0x040012D4 RID: 4820
			Center,
			// Token: 0x040012D5 RID: 4821
			TopLeft,
			// Token: 0x040012D6 RID: 4822
			Top,
			// Token: 0x040012D7 RID: 4823
			TopRight,
			// Token: 0x040012D8 RID: 4824
			Left,
			// Token: 0x040012D9 RID: 4825
			Right,
			// Token: 0x040012DA RID: 4826
			BottomLeft,
			// Token: 0x040012DB RID: 4827
			Bottom,
			// Token: 0x040012DC RID: 4828
			BottomRight,
			// Token: 0x040012DD RID: 4829
			Custom
		}
	}
}
