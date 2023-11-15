using System;
using UnityEngine;

namespace Pathfinding.Voxels
{
	// Token: 0x020000A5 RID: 165
	public class VoxelArea
	{
		// Token: 0x0600077F RID: 1919 RVA: 0x0002E3FC File Offset: 0x0002C5FC
		public void Reset()
		{
			this.ResetLinkedVoxelSpans();
			for (int i = 0; i < this.compactCells.Length; i++)
			{
				this.compactCells[i].count = 0U;
				this.compactCells[i].index = 0U;
			}
		}

		// Token: 0x06000780 RID: 1920 RVA: 0x0002E448 File Offset: 0x0002C648
		private void ResetLinkedVoxelSpans()
		{
			int num = this.linkedSpans.Length;
			this.linkedSpanCount = this.width * this.depth;
			LinkedVoxelSpan linkedVoxelSpan = new LinkedVoxelSpan(uint.MaxValue, uint.MaxValue, -1, -1);
			for (int i = 0; i < num; i++)
			{
				this.linkedSpans[i] = linkedVoxelSpan;
				i++;
				this.linkedSpans[i] = linkedVoxelSpan;
				i++;
				this.linkedSpans[i] = linkedVoxelSpan;
				i++;
				this.linkedSpans[i] = linkedVoxelSpan;
				i++;
				this.linkedSpans[i] = linkedVoxelSpan;
				i++;
				this.linkedSpans[i] = linkedVoxelSpan;
				i++;
				this.linkedSpans[i] = linkedVoxelSpan;
				i++;
				this.linkedSpans[i] = linkedVoxelSpan;
				i++;
				this.linkedSpans[i] = linkedVoxelSpan;
				i++;
				this.linkedSpans[i] = linkedVoxelSpan;
				i++;
				this.linkedSpans[i] = linkedVoxelSpan;
				i++;
				this.linkedSpans[i] = linkedVoxelSpan;
				i++;
				this.linkedSpans[i] = linkedVoxelSpan;
				i++;
				this.linkedSpans[i] = linkedVoxelSpan;
				i++;
				this.linkedSpans[i] = linkedVoxelSpan;
				i++;
				this.linkedSpans[i] = linkedVoxelSpan;
			}
			this.removedStackCount = 0;
		}

		// Token: 0x06000781 RID: 1921 RVA: 0x0002E5A4 File Offset: 0x0002C7A4
		public VoxelArea(int width, int depth)
		{
			this.width = width;
			this.depth = depth;
			int num = width * depth;
			this.compactCells = new CompactVoxelCell[num];
			this.linkedSpans = new LinkedVoxelSpan[(int)((float)num * 8f) + 15 & -16];
			this.ResetLinkedVoxelSpans();
			int[] array = new int[4];
			array[0] = -1;
			array[2] = 1;
			this.DirectionX = array;
			this.DirectionZ = new int[]
			{
				0,
				width,
				0,
				-width
			};
			this.VectorDirection = new Vector3[]
			{
				Vector3.left,
				Vector3.forward,
				Vector3.right,
				Vector3.back
			};
		}

		// Token: 0x06000782 RID: 1922 RVA: 0x0002E66C File Offset: 0x0002C86C
		public int GetSpanCountAll()
		{
			int num = 0;
			int num2 = this.width * this.depth;
			for (int i = 0; i < num2; i++)
			{
				int num3 = i;
				while (num3 != -1 && this.linkedSpans[num3].bottom != 4294967295U)
				{
					num++;
					num3 = this.linkedSpans[num3].next;
				}
			}
			return num;
		}

		// Token: 0x06000783 RID: 1923 RVA: 0x0002E6C8 File Offset: 0x0002C8C8
		public int GetSpanCount()
		{
			int num = 0;
			int num2 = this.width * this.depth;
			for (int i = 0; i < num2; i++)
			{
				int num3 = i;
				while (num3 != -1 && this.linkedSpans[num3].bottom != 4294967295U)
				{
					if (this.linkedSpans[num3].area != 0)
					{
						num++;
					}
					num3 = this.linkedSpans[num3].next;
				}
			}
			return num;
		}

		// Token: 0x06000784 RID: 1924 RVA: 0x0002E738 File Offset: 0x0002C938
		private void PushToSpanRemovedStack(int index)
		{
			if (this.removedStackCount == this.removedStack.Length)
			{
				int[] dst = new int[this.removedStackCount * 4];
				Buffer.BlockCopy(this.removedStack, 0, dst, 0, this.removedStackCount * 4);
				this.removedStack = dst;
			}
			this.removedStack[this.removedStackCount] = index;
			this.removedStackCount++;
		}

		// Token: 0x06000785 RID: 1925 RVA: 0x0002E79C File Offset: 0x0002C99C
		public void AddLinkedSpan(int index, uint bottom, uint top, int area, int voxelWalkableClimb)
		{
			LinkedVoxelSpan[] array = this.linkedSpans;
			if (array[index].bottom == 4294967295U)
			{
				array[index] = new LinkedVoxelSpan(bottom, top, area);
				return;
			}
			int num = -1;
			int num2 = index;
			while (index != -1)
			{
				LinkedVoxelSpan linkedVoxelSpan = array[index];
				if (linkedVoxelSpan.bottom > top)
				{
					break;
				}
				if (linkedVoxelSpan.top < bottom)
				{
					num = index;
					index = linkedVoxelSpan.next;
				}
				else
				{
					bottom = Math.Min(linkedVoxelSpan.bottom, bottom);
					top = Math.Max(linkedVoxelSpan.top, top);
					if (Math.Abs((int)(top - linkedVoxelSpan.top)) <= voxelWalkableClimb)
					{
						area = Math.Max(area, linkedVoxelSpan.area);
					}
					int next = linkedVoxelSpan.next;
					if (num != -1)
					{
						array[num].next = next;
						this.PushToSpanRemovedStack(index);
						index = next;
					}
					else
					{
						if (next == -1)
						{
							array[num2] = new LinkedVoxelSpan(bottom, top, area);
							return;
						}
						array[num2] = array[next];
						this.PushToSpanRemovedStack(next);
					}
				}
			}
			if (this.linkedSpanCount >= array.Length)
			{
				LinkedVoxelSpan[] array2 = array;
				int num3 = this.linkedSpanCount;
				int num4 = this.removedStackCount;
				array = (this.linkedSpans = new LinkedVoxelSpan[array.Length * 2]);
				this.ResetLinkedVoxelSpans();
				this.linkedSpanCount = num3;
				this.removedStackCount = num4;
				for (int i = 0; i < this.linkedSpanCount; i++)
				{
					array[i] = array2[i];
				}
			}
			int num5;
			if (this.removedStackCount > 0)
			{
				this.removedStackCount--;
				num5 = this.removedStack[this.removedStackCount];
			}
			else
			{
				num5 = this.linkedSpanCount;
				this.linkedSpanCount++;
			}
			if (num != -1)
			{
				array[num5] = new LinkedVoxelSpan(bottom, top, area, array[num].next);
				array[num].next = num5;
				return;
			}
			array[num5] = array[num2];
			array[num2] = new LinkedVoxelSpan(bottom, top, area, num5);
		}

		// Token: 0x0400044A RID: 1098
		public const uint MaxHeight = 65536U;

		// Token: 0x0400044B RID: 1099
		public const int MaxHeightInt = 65536;

		// Token: 0x0400044C RID: 1100
		public const uint InvalidSpanValue = 4294967295U;

		// Token: 0x0400044D RID: 1101
		public const float AvgSpanLayerCountEstimate = 8f;

		// Token: 0x0400044E RID: 1102
		public readonly int width;

		// Token: 0x0400044F RID: 1103
		public readonly int depth;

		// Token: 0x04000450 RID: 1104
		public CompactVoxelSpan[] compactSpans;

		// Token: 0x04000451 RID: 1105
		public CompactVoxelCell[] compactCells;

		// Token: 0x04000452 RID: 1106
		public int compactSpanCount;

		// Token: 0x04000453 RID: 1107
		public ushort[] tmpUShortArr;

		// Token: 0x04000454 RID: 1108
		public int[] areaTypes;

		// Token: 0x04000455 RID: 1109
		public ushort[] dist;

		// Token: 0x04000456 RID: 1110
		public ushort maxDistance;

		// Token: 0x04000457 RID: 1111
		public int maxRegions;

		// Token: 0x04000458 RID: 1112
		public int[] DirectionX;

		// Token: 0x04000459 RID: 1113
		public int[] DirectionZ;

		// Token: 0x0400045A RID: 1114
		public Vector3[] VectorDirection;

		// Token: 0x0400045B RID: 1115
		private int linkedSpanCount;

		// Token: 0x0400045C RID: 1116
		public LinkedVoxelSpan[] linkedSpans;

		// Token: 0x0400045D RID: 1117
		private int[] removedStack = new int[128];

		// Token: 0x0400045E RID: 1118
		private int removedStackCount;
	}
}
