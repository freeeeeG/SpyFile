using System;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000010 RID: 16
	[Serializable]
	public class AstarColor
	{
		// Token: 0x0600018E RID: 398 RVA: 0x000082A0 File Offset: 0x000064A0
		public static int ColorHash()
		{
			int num = AstarColor.SolidColor.GetHashCode() ^ AstarColor.UnwalkableNode.GetHashCode() ^ AstarColor.BoundsHandles.GetHashCode() ^ AstarColor.ConnectionLowLerp.GetHashCode() ^ AstarColor.ConnectionHighLerp.GetHashCode() ^ AstarColor.MeshEdgeColor.GetHashCode();
			for (int i = 0; i < AstarColor.AreaColors.Length; i++)
			{
				num = (7 * num ^ AstarColor.AreaColors[i].GetHashCode());
			}
			return num;
		}

		// Token: 0x0600018F RID: 399 RVA: 0x00008341 File Offset: 0x00006541
		public static Color GetAreaColor(uint area)
		{
			if ((ulong)area >= (ulong)((long)AstarColor.AreaColors.Length))
			{
				return AstarMath.IntToColor((int)area, 1f);
			}
			return AstarColor.AreaColors[(int)area];
		}

		// Token: 0x06000190 RID: 400 RVA: 0x00008366 File Offset: 0x00006566
		public static Color GetTagColor(uint tag)
		{
			if ((ulong)tag >= (ulong)((long)AstarColor.AreaColors.Length))
			{
				return AstarMath.IntToColor((int)tag, 1f);
			}
			return AstarColor.AreaColors[(int)tag];
		}

		// Token: 0x06000191 RID: 401 RVA: 0x0000838C File Offset: 0x0000658C
		public void PushToStatic(AstarPath astar)
		{
			this._AreaColors = (this._AreaColors ?? new Color[1]);
			AstarColor.SolidColor = this._SolidColor;
			AstarColor.UnwalkableNode = this._UnwalkableNode;
			AstarColor.BoundsHandles = this._BoundsHandles;
			AstarColor.ConnectionLowLerp = this._ConnectionLowLerp;
			AstarColor.ConnectionHighLerp = this._ConnectionHighLerp;
			AstarColor.MeshEdgeColor = this._MeshEdgeColor;
			AstarColor.AreaColors = this._AreaColors;
		}

		// Token: 0x06000192 RID: 402 RVA: 0x000083FC File Offset: 0x000065FC
		public AstarColor()
		{
			this._SolidColor = new Color(0.11764706f, 0.4f, 0.7882353f, 0.9f);
			this._UnwalkableNode = new Color(1f, 0f, 0f, 0.5f);
			this._BoundsHandles = new Color(0.29f, 0.454f, 0.741f, 0.9f);
			this._ConnectionLowLerp = new Color(0f, 1f, 0f, 0.5f);
			this._ConnectionHighLerp = new Color(1f, 0f, 0f, 0.5f);
			this._MeshEdgeColor = new Color(0f, 0f, 0f, 0.5f);
		}

		// Token: 0x040000D4 RID: 212
		public Color _SolidColor;

		// Token: 0x040000D5 RID: 213
		public Color _UnwalkableNode;

		// Token: 0x040000D6 RID: 214
		public Color _BoundsHandles;

		// Token: 0x040000D7 RID: 215
		public Color _ConnectionLowLerp;

		// Token: 0x040000D8 RID: 216
		public Color _ConnectionHighLerp;

		// Token: 0x040000D9 RID: 217
		public Color _MeshEdgeColor;

		// Token: 0x040000DA RID: 218
		public Color[] _AreaColors;

		// Token: 0x040000DB RID: 219
		public static Color SolidColor = new Color(0.11764706f, 0.4f, 0.7882353f, 0.9f);

		// Token: 0x040000DC RID: 220
		public static Color UnwalkableNode = new Color(1f, 0f, 0f, 0.5f);

		// Token: 0x040000DD RID: 221
		public static Color BoundsHandles = new Color(0.29f, 0.454f, 0.741f, 0.9f);

		// Token: 0x040000DE RID: 222
		public static Color ConnectionLowLerp = new Color(0f, 1f, 0f, 0.5f);

		// Token: 0x040000DF RID: 223
		public static Color ConnectionHighLerp = new Color(1f, 0f, 0f, 0.5f);

		// Token: 0x040000E0 RID: 224
		public static Color MeshEdgeColor = new Color(0f, 0f, 0f, 0.5f);

		// Token: 0x040000E1 RID: 225
		private static Color[] AreaColors = new Color[1];
	}
}
