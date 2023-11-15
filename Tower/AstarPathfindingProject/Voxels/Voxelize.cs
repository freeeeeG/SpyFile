﻿using System;
using System.Collections.Generic;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding.Voxels
{
	// Token: 0x020000AF RID: 175
	public class Voxelize
	{
		// Token: 0x06000794 RID: 1940 RVA: 0x0002ECAC File Offset: 0x0002CEAC
		public void BuildContours(float maxError, int maxEdgeLength, VoxelContourSet cset, int buildFlags)
		{
			int num = this.voxelArea.width;
			int num2 = this.voxelArea.depth;
			int num3 = num * num2;
			List<VoxelContour> list = new List<VoxelContour>(Mathf.Max(8, 8));
			ushort[] array = this.voxelArea.tmpUShortArr;
			if (array.Length < this.voxelArea.compactSpanCount)
			{
				array = (this.voxelArea.tmpUShortArr = new ushort[this.voxelArea.compactSpanCount]);
			}
			for (int i = 0; i < num3; i += this.voxelArea.width)
			{
				for (int j = 0; j < this.voxelArea.width; j++)
				{
					CompactVoxelCell compactVoxelCell = this.voxelArea.compactCells[j + i];
					int k = (int)compactVoxelCell.index;
					int num4 = (int)(compactVoxelCell.index + compactVoxelCell.count);
					while (k < num4)
					{
						ushort num5 = 0;
						CompactVoxelSpan compactVoxelSpan = this.voxelArea.compactSpans[k];
						if (compactVoxelSpan.reg == 0 || (compactVoxelSpan.reg & 32768) == 32768)
						{
							array[k] = 0;
						}
						else
						{
							for (int l = 0; l < 4; l++)
							{
								int num6 = 0;
								if ((long)compactVoxelSpan.GetConnection(l) != 63L)
								{
									int num7 = j + this.voxelArea.DirectionX[l];
									int num8 = i + this.voxelArea.DirectionZ[l];
									int num9 = (int)(this.voxelArea.compactCells[num7 + num8].index + (uint)compactVoxelSpan.GetConnection(l));
									num6 = this.voxelArea.compactSpans[num9].reg;
								}
								if (num6 == compactVoxelSpan.reg)
								{
									num5 |= (ushort)(1 << l);
								}
							}
							array[k] = (num5 ^ 15);
						}
						k++;
					}
				}
			}
			List<int> list2 = ListPool<int>.Claim(256);
			List<int> list3 = ListPool<int>.Claim(64);
			for (int m = 0; m < num3; m += this.voxelArea.width)
			{
				for (int n = 0; n < this.voxelArea.width; n++)
				{
					CompactVoxelCell compactVoxelCell2 = this.voxelArea.compactCells[n + m];
					int num10 = (int)compactVoxelCell2.index;
					int num11 = (int)(compactVoxelCell2.index + compactVoxelCell2.count);
					while (num10 < num11)
					{
						if (array[num10] == 0 || array[num10] == 15)
						{
							array[num10] = 0;
						}
						else
						{
							int reg = this.voxelArea.compactSpans[num10].reg;
							if (reg != 0 && (reg & 32768) != 32768)
							{
								int area = this.voxelArea.areaTypes[num10];
								list2.Clear();
								list3.Clear();
								this.WalkContour(n, m, num10, array, list2);
								this.SimplifyContour(list2, list3, maxError, maxEdgeLength, buildFlags);
								this.RemoveDegenerateSegments(list3);
								VoxelContour voxelContour = default(VoxelContour);
								voxelContour.verts = ArrayPool<int>.Claim(list3.Count);
								for (int num12 = 0; num12 < list3.Count; num12++)
								{
									voxelContour.verts[num12] = list3[num12];
								}
								voxelContour.nverts = list3.Count / 4;
								voxelContour.reg = reg;
								voxelContour.area = area;
								list.Add(voxelContour);
							}
						}
						num10++;
					}
				}
			}
			ListPool<int>.Release(ref list2);
			ListPool<int>.Release(ref list3);
			for (int num13 = 0; num13 < list.Count; num13++)
			{
				VoxelContour voxelContour2 = list[num13];
				if (this.CalcAreaOfPolygon2D(voxelContour2.verts, voxelContour2.nverts) < 0)
				{
					int num14 = -1;
					for (int num15 = 0; num15 < list.Count; num15++)
					{
						if (num13 != num15 && list[num15].nverts > 0 && list[num15].reg == voxelContour2.reg && this.CalcAreaOfPolygon2D(list[num15].verts, list[num15].nverts) > 0)
						{
							num14 = num15;
							break;
						}
					}
					if (num14 == -1)
					{
						Debug.LogError("rcBuildContours: Could not find merge target for bad contour " + num13.ToString() + ".");
					}
					else
					{
						VoxelContour voxelContour3 = list[num14];
						int num16 = 0;
						int num17 = 0;
						this.GetClosestIndices(voxelContour3.verts, voxelContour3.nverts, voxelContour2.verts, voxelContour2.nverts, ref num16, ref num17);
						if (num16 == -1 || num17 == -1)
						{
							Debug.LogWarning(string.Concat(new string[]
							{
								"rcBuildContours: Failed to find merge points for ",
								num13.ToString(),
								" and ",
								num14.ToString(),
								"."
							}));
						}
						else if (!Voxelize.MergeContours(ref voxelContour3, ref voxelContour2, num16, num17))
						{
							Debug.LogWarning(string.Concat(new string[]
							{
								"rcBuildContours: Failed to merge contours ",
								num13.ToString(),
								" and ",
								num14.ToString(),
								"."
							}));
						}
						else
						{
							list[num14] = voxelContour3;
							list[num13] = voxelContour2;
						}
					}
				}
			}
			cset.conts = list;
		}

		// Token: 0x06000795 RID: 1941 RVA: 0x0002F1EC File Offset: 0x0002D3EC
		private void GetClosestIndices(int[] vertsa, int nvertsa, int[] vertsb, int nvertsb, ref int ia, ref int ib)
		{
			int num = 268435455;
			ia = -1;
			ib = -1;
			for (int i = 0; i < nvertsa; i++)
			{
				int num2 = (i + 1) % nvertsa;
				int num3 = (i + nvertsa - 1) % nvertsa;
				int num4 = i * 4;
				int b = num2 * 4;
				int a = num3 * 4;
				for (int j = 0; j < nvertsb; j++)
				{
					int num5 = j * 4;
					if (Voxelize.Ileft(a, num4, num5, vertsa, vertsa, vertsb) && Voxelize.Ileft(num4, b, num5, vertsa, vertsa, vertsb))
					{
						int num6 = vertsb[num5] - vertsa[num4];
						int num7 = vertsb[num5 + 2] / this.voxelArea.width - vertsa[num4 + 2] / this.voxelArea.width;
						int num8 = num6 * num6 + num7 * num7;
						if (num8 < num)
						{
							ia = i;
							ib = j;
							num = num8;
						}
					}
				}
			}
		}

		// Token: 0x06000796 RID: 1942 RVA: 0x0002F2B4 File Offset: 0x0002D4B4
		private static void ReleaseContours(VoxelContourSet cset)
		{
			for (int i = 0; i < cset.conts.Count; i++)
			{
				VoxelContour voxelContour = cset.conts[i];
				ArrayPool<int>.Release(ref voxelContour.verts, false);
				ArrayPool<int>.Release(ref voxelContour.rverts, false);
			}
			cset.conts = null;
		}

		// Token: 0x06000797 RID: 1943 RVA: 0x0002F308 File Offset: 0x0002D508
		public static bool MergeContours(ref VoxelContour ca, ref VoxelContour cb, int ia, int ib)
		{
			int[] array = ArrayPool<int>.Claim((ca.nverts + cb.nverts + 2) * 4);
			int num = 0;
			for (int i = 0; i <= ca.nverts; i++)
			{
				int num2 = num * 4;
				int num3 = (ia + i) % ca.nverts * 4;
				array[num2] = ca.verts[num3];
				array[num2 + 1] = ca.verts[num3 + 1];
				array[num2 + 2] = ca.verts[num3 + 2];
				array[num2 + 3] = ca.verts[num3 + 3];
				num++;
			}
			for (int j = 0; j <= cb.nverts; j++)
			{
				int num4 = num * 4;
				int num5 = (ib + j) % cb.nverts * 4;
				array[num4] = cb.verts[num5];
				array[num4 + 1] = cb.verts[num5 + 1];
				array[num4 + 2] = cb.verts[num5 + 2];
				array[num4 + 3] = cb.verts[num5 + 3];
				num++;
			}
			ArrayPool<int>.Release(ref ca.verts, false);
			ArrayPool<int>.Release(ref cb.verts, false);
			ca.verts = array;
			ca.nverts = num;
			cb.verts = ArrayPool<int>.Claim(0);
			cb.nverts = 0;
			return true;
		}

		// Token: 0x06000798 RID: 1944 RVA: 0x0002F438 File Offset: 0x0002D638
		public void SimplifyContour(List<int> verts, List<int> simplified, float maxError, int maxEdgeLenght, int buildFlags)
		{
			bool flag = false;
			for (int i = 0; i < verts.Count; i += 4)
			{
				if ((verts[i + 3] & 65535) != 0)
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				int j = 0;
				int num = verts.Count / 4;
				while (j < num)
				{
					int num2 = (j + 1) % num;
					bool flag2 = (verts[j * 4 + 3] & 65535) != (verts[num2 * 4 + 3] & 65535);
					bool flag3 = (verts[j * 4 + 3] & 131072) != (verts[num2 * 4 + 3] & 131072);
					if (flag2 || flag3)
					{
						simplified.Add(verts[j * 4]);
						simplified.Add(verts[j * 4 + 1]);
						simplified.Add(verts[j * 4 + 2]);
						simplified.Add(j);
					}
					j++;
				}
			}
			if (simplified.Count == 0)
			{
				int num3 = verts[0];
				int item = verts[1];
				int num4 = verts[2];
				int item2 = 0;
				int num5 = verts[0];
				int item3 = verts[1];
				int num6 = verts[2];
				int item4 = 0;
				for (int k = 0; k < verts.Count; k += 4)
				{
					int num7 = verts[k];
					int num8 = verts[k + 1];
					int num9 = verts[k + 2];
					if (num7 < num3 || (num7 == num3 && num9 < num4))
					{
						num3 = num7;
						item = num8;
						num4 = num9;
						item2 = k / 4;
					}
					if (num7 > num5 || (num7 == num5 && num9 > num6))
					{
						num5 = num7;
						item3 = num8;
						num6 = num9;
						item4 = k / 4;
					}
				}
				simplified.Add(num3);
				simplified.Add(item);
				simplified.Add(num4);
				simplified.Add(item2);
				simplified.Add(num5);
				simplified.Add(item3);
				simplified.Add(num6);
				simplified.Add(item4);
			}
			int num10 = verts.Count / 4;
			maxError *= maxError;
			int l = 0;
			while (l < simplified.Count / 4)
			{
				int num11 = (l + 1) % (simplified.Count / 4);
				int num12 = simplified[l * 4];
				int num13 = simplified[l * 4 + 2];
				int num14 = simplified[l * 4 + 3];
				int num15 = simplified[num11 * 4];
				int num16 = simplified[num11 * 4 + 2];
				int num17 = simplified[num11 * 4 + 3];
				float num18 = 0f;
				int num19 = -1;
				int num20;
				int num21;
				int num22;
				if (num15 > num12 || (num15 == num12 && num16 > num13))
				{
					num20 = 1;
					num21 = (num14 + num20) % num10;
					num22 = num17;
				}
				else
				{
					num20 = num10 - 1;
					num21 = (num17 + num20) % num10;
					num22 = num14;
					Memory.Swap<int>(ref num12, ref num15);
					Memory.Swap<int>(ref num13, ref num16);
				}
				if ((verts[num21 * 4 + 3] & 65535) != 0)
				{
					if ((verts[num21 * 4 + 3] & 131072) != 131072)
					{
						goto IL_34F;
					}
				}
				while (num21 != num22)
				{
					float num23 = VectorMath.SqrDistancePointSegmentApproximate(verts[num21 * 4], verts[num21 * 4 + 2] / this.voxelArea.width, num12, num13 / this.voxelArea.width, num15, num16 / this.voxelArea.width);
					if (num23 > num18)
					{
						num18 = num23;
						num19 = num21;
					}
					num21 = (num21 + num20) % num10;
				}
				IL_34F:
				if (num19 != -1 && num18 > maxError)
				{
					simplified.Add(0);
					simplified.Add(0);
					simplified.Add(0);
					simplified.Add(0);
					for (int m = simplified.Count / 4 - 1; m > l; m--)
					{
						simplified[m * 4] = simplified[(m - 1) * 4];
						simplified[m * 4 + 1] = simplified[(m - 1) * 4 + 1];
						simplified[m * 4 + 2] = simplified[(m - 1) * 4 + 2];
						simplified[m * 4 + 3] = simplified[(m - 1) * 4 + 3];
					}
					simplified[(l + 1) * 4] = verts[num19 * 4];
					simplified[(l + 1) * 4 + 1] = verts[num19 * 4 + 1];
					simplified[(l + 1) * 4 + 2] = verts[num19 * 4 + 2];
					simplified[(l + 1) * 4 + 3] = num19;
				}
				else
				{
					l++;
				}
			}
			float num24 = this.maxEdgeLength / this.cellSize;
			if (num24 > 0f && (buildFlags & 7) != 0)
			{
				int num25 = 0;
				while (num25 < simplified.Count / 4 && simplified.Count / 4 <= 200)
				{
					int num26 = (num25 + 1) % (simplified.Count / 4);
					int num27 = simplified[num25 * 4];
					int num28 = simplified[num25 * 4 + 2];
					int num29 = simplified[num25 * 4 + 3];
					int num30 = simplified[num26 * 4];
					int num31 = simplified[num26 * 4 + 2];
					int num32 = simplified[num26 * 4 + 3];
					int num33 = -1;
					int num34 = (num29 + 1) % num10;
					bool flag4 = false;
					if ((buildFlags & 1) != 0 && (verts[num34 * 4 + 3] & 65535) == 0)
					{
						flag4 = true;
					}
					if ((buildFlags & 2) != 0 && (verts[num34 * 4 + 3] & 131072) == 131072)
					{
						flag4 = true;
					}
					if ((buildFlags & 4) != 0 && (verts[num34 * 4 + 3] & 32768) == 32768)
					{
						flag4 = true;
					}
					if (flag4)
					{
						int num35 = num30 - num27;
						int num36 = num31 / this.voxelArea.width - num28 / this.voxelArea.width;
						if ((float)(num35 * num35 + num36 * num36) > num24 * num24)
						{
							int num37 = (num32 < num29) ? (num32 + num10 - num29) : (num32 - num29);
							if (num37 > 1)
							{
								if (num30 > num27 || (num30 == num27 && num31 > num28))
								{
									num33 = (num29 + num37 / 2) % num10;
								}
								else
								{
									num33 = (num29 + (num37 + 1) / 2) % num10;
								}
							}
						}
					}
					if (num33 != -1)
					{
						simplified.AddRange(new int[4]);
						for (int n = simplified.Count / 4 - 1; n > num25; n--)
						{
							simplified[n * 4] = simplified[(n - 1) * 4];
							simplified[n * 4 + 1] = simplified[(n - 1) * 4 + 1];
							simplified[n * 4 + 2] = simplified[(n - 1) * 4 + 2];
							simplified[n * 4 + 3] = simplified[(n - 1) * 4 + 3];
						}
						simplified[(num25 + 1) * 4] = verts[num33 * 4];
						simplified[(num25 + 1) * 4 + 1] = verts[num33 * 4 + 1];
						simplified[(num25 + 1) * 4 + 2] = verts[num33 * 4 + 2];
						simplified[(num25 + 1) * 4 + 3] = num33;
					}
					else
					{
						num25++;
					}
				}
			}
			for (int num38 = 0; num38 < simplified.Count / 4; num38++)
			{
				int num39 = (simplified[num38 * 4 + 3] + 1) % num10;
				int num40 = simplified[num38 * 4 + 3];
				simplified[num38 * 4 + 3] = ((verts[num39 * 4 + 3] & 65535) | (verts[num40 * 4 + 3] & 65536));
			}
		}

		// Token: 0x06000799 RID: 1945 RVA: 0x0002FBA4 File Offset: 0x0002DDA4
		public void WalkContour(int x, int z, int i, ushort[] flags, List<int> verts)
		{
			int num = 0;
			while ((flags[i] & (ushort)(1 << num)) == 0)
			{
				num++;
			}
			int num2 = num;
			int num3 = i;
			int num4 = this.voxelArea.areaTypes[i];
			int num5 = 0;
			while (num5++ < 40000)
			{
				if ((flags[i] & (ushort)(1 << num)) != 0)
				{
					bool flag = false;
					bool flag2 = false;
					int num6 = x;
					int cornerHeight = this.GetCornerHeight(x, z, i, num, ref flag);
					int num7 = z;
					switch (num)
					{
					case 0:
						num7 += this.voxelArea.width;
						break;
					case 1:
						num6++;
						num7 += this.voxelArea.width;
						break;
					case 2:
						num6++;
						break;
					}
					int num8 = 0;
					CompactVoxelSpan compactVoxelSpan = this.voxelArea.compactSpans[i];
					if ((long)compactVoxelSpan.GetConnection(num) != 63L)
					{
						int num9 = x + this.voxelArea.DirectionX[num];
						int num10 = z + this.voxelArea.DirectionZ[num];
						int num11 = (int)(this.voxelArea.compactCells[num9 + num10].index + (uint)compactVoxelSpan.GetConnection(num));
						num8 = this.voxelArea.compactSpans[num11].reg;
						if (num4 != this.voxelArea.areaTypes[num11])
						{
							flag2 = true;
						}
					}
					if (flag)
					{
						num8 |= 65536;
					}
					if (flag2)
					{
						num8 |= 131072;
					}
					verts.Add(num6);
					verts.Add(cornerHeight);
					verts.Add(num7);
					verts.Add(num8);
					flags[i] = (ushort)((int)flags[i] & ~(1 << num));
					num = (num + 1 & 3);
				}
				else
				{
					int num12 = -1;
					int num13 = x + this.voxelArea.DirectionX[num];
					int num14 = z + this.voxelArea.DirectionZ[num];
					CompactVoxelSpan compactVoxelSpan2 = this.voxelArea.compactSpans[i];
					if ((long)compactVoxelSpan2.GetConnection(num) != 63L)
					{
						num12 = (int)(this.voxelArea.compactCells[num13 + num14].index + (uint)compactVoxelSpan2.GetConnection(num));
					}
					if (num12 == -1)
					{
						Debug.LogWarning("Degenerate triangles might have been generated.\nUsually this is not a problem, but if you have a static level, try to modify the graph settings slightly to avoid this edge case.");
						return;
					}
					x = num13;
					z = num14;
					i = num12;
					num = (num + 3 & 3);
				}
				if (num3 == i && num2 == num)
				{
					break;
				}
			}
		}

		// Token: 0x0600079A RID: 1946 RVA: 0x0002FDEC File Offset: 0x0002DFEC
		public int GetCornerHeight(int x, int z, int i, int dir, ref bool isBorderVertex)
		{
			CompactVoxelSpan compactVoxelSpan = this.voxelArea.compactSpans[i];
			int num = (int)compactVoxelSpan.y;
			int num2 = dir + 1 & 3;
			uint[] array = new uint[4];
			array[0] = (uint)(this.voxelArea.compactSpans[i].reg | this.voxelArea.areaTypes[i] << 16);
			if ((long)compactVoxelSpan.GetConnection(dir) != 63L)
			{
				int num3 = x + this.voxelArea.DirectionX[dir];
				int num4 = z + this.voxelArea.DirectionZ[dir];
				int num5 = (int)(this.voxelArea.compactCells[num3 + num4].index + (uint)compactVoxelSpan.GetConnection(dir));
				CompactVoxelSpan compactVoxelSpan2 = this.voxelArea.compactSpans[num5];
				num = Math.Max(num, (int)compactVoxelSpan2.y);
				array[1] = (uint)(compactVoxelSpan2.reg | this.voxelArea.areaTypes[num5] << 16);
				if ((long)compactVoxelSpan2.GetConnection(num2) != 63L)
				{
					int num6 = num3 + this.voxelArea.DirectionX[num2];
					int num7 = num4 + this.voxelArea.DirectionZ[num2];
					int num8 = (int)(this.voxelArea.compactCells[num6 + num7].index + (uint)compactVoxelSpan2.GetConnection(num2));
					CompactVoxelSpan compactVoxelSpan3 = this.voxelArea.compactSpans[num8];
					num = Math.Max(num, (int)compactVoxelSpan3.y);
					array[2] = (uint)(compactVoxelSpan3.reg | this.voxelArea.areaTypes[num8] << 16);
				}
			}
			if ((long)compactVoxelSpan.GetConnection(num2) != 63L)
			{
				int num9 = x + this.voxelArea.DirectionX[num2];
				int num10 = z + this.voxelArea.DirectionZ[num2];
				int num11 = (int)(this.voxelArea.compactCells[num9 + num10].index + (uint)compactVoxelSpan.GetConnection(num2));
				CompactVoxelSpan compactVoxelSpan4 = this.voxelArea.compactSpans[num11];
				num = Math.Max(num, (int)compactVoxelSpan4.y);
				array[3] = (uint)(compactVoxelSpan4.reg | this.voxelArea.areaTypes[num11] << 16);
				if ((long)compactVoxelSpan4.GetConnection(dir) != 63L)
				{
					int num12 = num9 + this.voxelArea.DirectionX[dir];
					int num13 = num10 + this.voxelArea.DirectionZ[dir];
					int num14 = (int)(this.voxelArea.compactCells[num12 + num13].index + (uint)compactVoxelSpan4.GetConnection(dir));
					CompactVoxelSpan compactVoxelSpan5 = this.voxelArea.compactSpans[num14];
					num = Math.Max(num, (int)compactVoxelSpan5.y);
					array[2] = (uint)(compactVoxelSpan5.reg | this.voxelArea.areaTypes[num14] << 16);
				}
			}
			for (int j = 0; j < 4; j++)
			{
				int num15 = j;
				int num16 = j + 1 & 3;
				int num17 = j + 2 & 3;
				int num18 = j + 3 & 3;
				bool flag = (array[num15] & array[num16] & 32768U) != 0U && array[num15] == array[num16];
				bool flag2 = ((array[num17] | array[num18]) & 32768U) == 0U;
				bool flag3 = array[num17] >> 16 == array[num18] >> 16;
				bool flag4 = array[num15] != 0U && array[num16] != 0U && array[num17] != 0U && array[num18] > 0U;
				if (flag && flag2 && flag3 && flag4)
				{
					isBorderVertex = true;
					break;
				}
			}
			return num;
		}

		// Token: 0x0600079B RID: 1947 RVA: 0x00030154 File Offset: 0x0002E354
		public void RemoveDegenerateSegments(List<int> simplified)
		{
			for (int i = 0; i < simplified.Count / 4; i++)
			{
				int num = i + 1;
				if (num >= simplified.Count / 4)
				{
					num = 0;
				}
				if (simplified[i * 4] == simplified[num * 4] && simplified[i * 4 + 2] == simplified[num * 4 + 2])
				{
					simplified.RemoveRange(i, 4);
				}
			}
		}

		// Token: 0x0600079C RID: 1948 RVA: 0x000301BC File Offset: 0x0002E3BC
		public int CalcAreaOfPolygon2D(int[] verts, int nverts)
		{
			int num = 0;
			int i = 0;
			int num2 = nverts - 1;
			while (i < nverts)
			{
				int num3 = i * 4;
				int num4 = num2 * 4;
				num += verts[num3] * (verts[num4 + 2] / this.voxelArea.width) - verts[num4] * (verts[num3 + 2] / this.voxelArea.width);
				num2 = i++;
			}
			return (num + 1) / 2;
		}

		// Token: 0x0600079D RID: 1949 RVA: 0x0003021B File Offset: 0x0002E41B
		public static bool Ileft(int a, int b, int c, int[] va, int[] vb, int[] vc)
		{
			return (vb[b] - va[a]) * (vc[c + 2] - va[a + 2]) - (vc[c] - va[a]) * (vb[b + 2] - va[a + 2]) <= 0;
		}

		// Token: 0x0600079E RID: 1950 RVA: 0x0003024E File Offset: 0x0002E44E
		public static bool Diagonal(int i, int j, int n, int[] verts, int[] indices)
		{
			return Voxelize.InCone(i, j, n, verts, indices) && Voxelize.Diagonalie(i, j, n, verts, indices);
		}

		// Token: 0x0600079F RID: 1951 RVA: 0x0003026C File Offset: 0x0002E46C
		public static bool InCone(int i, int j, int n, int[] verts, int[] indices)
		{
			int num = (indices[i] & 268435455) * 4;
			int num2 = (indices[j] & 268435455) * 4;
			int c = (indices[Voxelize.Next(i, n)] & 268435455) * 4;
			int num3 = (indices[Voxelize.Prev(i, n)] & 268435455) * 4;
			if (Voxelize.LeftOn(num3, num, c, verts))
			{
				return Voxelize.Left(num, num2, num3, verts) && Voxelize.Left(num2, num, c, verts);
			}
			return !Voxelize.LeftOn(num, num2, c, verts) || !Voxelize.LeftOn(num2, num, num3, verts);
		}

		// Token: 0x060007A0 RID: 1952 RVA: 0x000302F4 File Offset: 0x0002E4F4
		public static bool Left(int a, int b, int c, int[] verts)
		{
			return Voxelize.Area2(a, b, c, verts) < 0;
		}

		// Token: 0x060007A1 RID: 1953 RVA: 0x00030302 File Offset: 0x0002E502
		public static bool LeftOn(int a, int b, int c, int[] verts)
		{
			return Voxelize.Area2(a, b, c, verts) <= 0;
		}

		// Token: 0x060007A2 RID: 1954 RVA: 0x00030313 File Offset: 0x0002E513
		public static bool Collinear(int a, int b, int c, int[] verts)
		{
			return Voxelize.Area2(a, b, c, verts) == 0;
		}

		// Token: 0x060007A3 RID: 1955 RVA: 0x00030321 File Offset: 0x0002E521
		public static int Area2(int a, int b, int c, int[] verts)
		{
			return (verts[b] - verts[a]) * (verts[c + 2] - verts[a + 2]) - (verts[c] - verts[a]) * (verts[b + 2] - verts[a + 2]);
		}

		// Token: 0x060007A4 RID: 1956 RVA: 0x0003034C File Offset: 0x0002E54C
		private static bool Diagonalie(int i, int j, int n, int[] verts, int[] indices)
		{
			int a = (indices[i] & 268435455) * 4;
			int num = (indices[j] & 268435455) * 4;
			for (int k = 0; k < n; k++)
			{
				int num2 = Voxelize.Next(k, n);
				if (k != i && num2 != i && k != j && num2 != j)
				{
					int num3 = (indices[k] & 268435455) * 4;
					int num4 = (indices[num2] & 268435455) * 4;
					if (!Voxelize.Vequal(a, num3, verts) && !Voxelize.Vequal(num, num3, verts) && !Voxelize.Vequal(a, num4, verts) && !Voxelize.Vequal(num, num4, verts) && Voxelize.Intersect(a, num, num3, num4, verts))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x060007A5 RID: 1957 RVA: 0x000303F0 File Offset: 0x0002E5F0
		public static bool Xorb(bool x, bool y)
		{
			return !x ^ !y;
		}

		// Token: 0x060007A6 RID: 1958 RVA: 0x000303FC File Offset: 0x0002E5FC
		public static bool IntersectProp(int a, int b, int c, int d, int[] verts)
		{
			return !Voxelize.Collinear(a, b, c, verts) && !Voxelize.Collinear(a, b, d, verts) && !Voxelize.Collinear(c, d, a, verts) && !Voxelize.Collinear(c, d, b, verts) && Voxelize.Xorb(Voxelize.Left(a, b, c, verts), Voxelize.Left(a, b, d, verts)) && Voxelize.Xorb(Voxelize.Left(c, d, a, verts), Voxelize.Left(c, d, b, verts));
		}

		// Token: 0x060007A7 RID: 1959 RVA: 0x00030474 File Offset: 0x0002E674
		private static bool Between(int a, int b, int c, int[] verts)
		{
			if (!Voxelize.Collinear(a, b, c, verts))
			{
				return false;
			}
			if (verts[a] != verts[b])
			{
				return (verts[a] <= verts[c] && verts[c] <= verts[b]) || (verts[a] >= verts[c] && verts[c] >= verts[b]);
			}
			return (verts[a + 2] <= verts[c + 2] && verts[c + 2] <= verts[b + 2]) || (verts[a + 2] >= verts[c + 2] && verts[c + 2] >= verts[b + 2]);
		}

		// Token: 0x060007A8 RID: 1960 RVA: 0x000304F8 File Offset: 0x0002E6F8
		private static bool Intersect(int a, int b, int c, int d, int[] verts)
		{
			return Voxelize.IntersectProp(a, b, c, d, verts) || (Voxelize.Between(a, b, c, verts) || Voxelize.Between(a, b, d, verts) || Voxelize.Between(c, d, a, verts) || Voxelize.Between(c, d, b, verts));
		}

		// Token: 0x060007A9 RID: 1961 RVA: 0x00030547 File Offset: 0x0002E747
		private static bool Vequal(int a, int b, int[] verts)
		{
			return verts[a] == verts[b] && verts[a + 2] == verts[b + 2];
		}

		// Token: 0x060007AA RID: 1962 RVA: 0x0003055F File Offset: 0x0002E75F
		public static int Prev(int i, int n)
		{
			if (i - 1 < 0)
			{
				return n - 1;
			}
			return i - 1;
		}

		// Token: 0x060007AB RID: 1963 RVA: 0x0003056E File Offset: 0x0002E76E
		public static int Next(int i, int n)
		{
			if (i + 1 >= n)
			{
				return 0;
			}
			return i + 1;
		}

		// Token: 0x060007AC RID: 1964 RVA: 0x0003057C File Offset: 0x0002E77C
		public void BuildPolyMesh(VoxelContourSet cset, int nvp, out VoxelMesh mesh)
		{
			nvp = 3;
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			for (int i = 0; i < cset.conts.Count; i++)
			{
				if (cset.conts[i].nverts >= 3)
				{
					num += cset.conts[i].nverts;
					num2 += cset.conts[i].nverts - 2;
					num3 = Math.Max(num3, cset.conts[i].nverts);
				}
			}
			Int3[] array = ArrayPool<Int3>.Claim(num);
			int[] array2 = ArrayPool<int>.Claim(num2 * nvp);
			int[] array3 = ArrayPool<int>.Claim(num2);
			Memory.MemSet<int>(array2, 255, 4);
			int[] array4 = ArrayPool<int>.Claim(num3);
			int[] array5 = ArrayPool<int>.Claim(num3 * 3);
			int num4 = 0;
			int num5 = 0;
			int num6 = 0;
			for (int j = 0; j < cset.conts.Count; j++)
			{
				VoxelContour voxelContour = cset.conts[j];
				if (voxelContour.nverts >= 3)
				{
					for (int k = 0; k < voxelContour.nverts; k++)
					{
						array4[k] = k;
						voxelContour.verts[k * 4 + 2] /= this.voxelArea.width;
					}
					int num7 = this.Triangulate(voxelContour.nverts, voxelContour.verts, ref array4, ref array5);
					int num8 = num4;
					for (int l = 0; l < num7 * 3; l++)
					{
						array2[num5] = array5[l] + num8;
						num5++;
					}
					for (int m = 0; m < num7; m++)
					{
						array3[num6] = voxelContour.area;
						num6++;
					}
					for (int n = 0; n < voxelContour.nverts; n++)
					{
						array[num4] = new Int3(voxelContour.verts[n * 4], voxelContour.verts[n * 4 + 1], voxelContour.verts[n * 4 + 2]);
						num4++;
					}
				}
			}
			mesh = new VoxelMesh
			{
				verts = Memory.ShrinkArray<Int3>(array, num4),
				tris = Memory.ShrinkArray<int>(array2, num5),
				areas = Memory.ShrinkArray<int>(array3, num6)
			};
			ArrayPool<Int3>.Release(ref array, false);
			ArrayPool<int>.Release(ref array2, false);
			ArrayPool<int>.Release(ref array3, false);
			ArrayPool<int>.Release(ref array4, false);
			ArrayPool<int>.Release(ref array5, false);
		}

		// Token: 0x060007AD RID: 1965 RVA: 0x000307E0 File Offset: 0x0002E9E0
		private int Triangulate(int n, int[] verts, ref int[] indices, ref int[] tris)
		{
			int num = 0;
			int[] array = tris;
			int num2 = 0;
			for (int i = 0; i < n; i++)
			{
				int num3 = Voxelize.Next(i, n);
				int j = Voxelize.Next(num3, n);
				if (Voxelize.Diagonal(i, j, n, verts, indices))
				{
					indices[num3] |= 1073741824;
				}
			}
			while (n > 3)
			{
				int num4 = -1;
				int num5 = -1;
				for (int k = 0; k < n; k++)
				{
					int num6 = Voxelize.Next(k, n);
					if ((indices[num6] & 1073741824) != 0)
					{
						int num7 = (indices[k] & 268435455) * 4;
						int num8 = (indices[Voxelize.Next(num6, n)] & 268435455) * 4;
						int num9 = verts[num8] - verts[num7];
						int num10 = verts[num8 + 2] - verts[num7 + 2];
						int num11 = num9 * num9 + num10 * num10;
						if (num4 < 0 || num11 < num4)
						{
							num4 = num11;
							num5 = k;
						}
					}
				}
				if (num5 == -1)
				{
					Debug.LogWarning("Degenerate triangles might have been generated.\nUsually this is not a problem, but if you have a static level, try to modify the graph settings slightly to avoid this edge case.");
					return -num;
				}
				int num12 = num5;
				int num13 = Voxelize.Next(num12, n);
				int num14 = Voxelize.Next(num13, n);
				array[num2] = (indices[num12] & 268435455);
				num2++;
				array[num2] = (indices[num13] & 268435455);
				num2++;
				array[num2] = (indices[num14] & 268435455);
				num2++;
				num++;
				n--;
				for (int l = num13; l < n; l++)
				{
					indices[l] = indices[l + 1];
				}
				if (num13 >= n)
				{
					num13 = 0;
				}
				num12 = Voxelize.Prev(num13, n);
				if (Voxelize.Diagonal(Voxelize.Prev(num12, n), num13, n, verts, indices))
				{
					indices[num12] |= 1073741824;
				}
				else
				{
					indices[num12] &= 268435455;
				}
				if (Voxelize.Diagonal(num12, Voxelize.Next(num13, n), n, verts, indices))
				{
					indices[num13] |= 1073741824;
				}
				else
				{
					indices[num13] &= 268435455;
				}
			}
			array[num2] = (indices[0] & 268435455);
			num2++;
			array[num2] = (indices[1] & 268435455);
			num2++;
			array[num2] = (indices[2] & 268435455);
			num2++;
			return num + 1;
		}

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x060007AE RID: 1966 RVA: 0x00030A0C File Offset: 0x0002EC0C
		// (set) Token: 0x060007AF RID: 1967 RVA: 0x00030A14 File Offset: 0x0002EC14
		public GraphTransform transformVoxel2Graph { get; private set; }

		// Token: 0x060007B0 RID: 1968 RVA: 0x00030A20 File Offset: 0x0002EC20
		public Vector3 CompactSpanToVector(int x, int z, int i)
		{
			return this.voxelOffset + new Vector3(((float)x + 0.5f) * this.cellSize, (float)this.voxelArea.compactSpans[i].y * this.cellHeight, ((float)z + 0.5f) * this.cellSize);
		}

		// Token: 0x060007B1 RID: 1969 RVA: 0x00030A7C File Offset: 0x0002EC7C
		public void VectorToIndex(Vector3 p, out int x, out int z)
		{
			p -= this.voxelOffset;
			x = Mathf.RoundToInt(p.x / this.cellSize - 0.5f);
			z = Mathf.RoundToInt(p.z / this.cellSize - 0.5f);
		}

		// Token: 0x060007B2 RID: 1970 RVA: 0x00030ACC File Offset: 0x0002ECCC
		public Voxelize(float ch, float cs, float walkableClimb, float walkableHeight, float maxSlope, float maxEdgeLength)
		{
			this.cellSize = cs;
			this.cellHeight = ch;
			this.maxSlope = maxSlope;
			this.cellScale = new Vector3(this.cellSize, this.cellHeight, this.cellSize);
			this.voxelWalkableHeight = (uint)(walkableHeight / this.cellHeight);
			this.voxelWalkableClimb = Mathf.RoundToInt(walkableClimb / this.cellHeight);
			this.maxEdgeLength = maxEdgeLength;
		}

		// Token: 0x060007B3 RID: 1971 RVA: 0x00030B7C File Offset: 0x0002ED7C
		public void Init()
		{
			if (this.voxelArea == null || this.voxelArea.width != this.width || this.voxelArea.depth != this.depth)
			{
				this.voxelArea = new VoxelArea(this.width, this.depth);
				return;
			}
			this.voxelArea.Reset();
		}

		// Token: 0x060007B4 RID: 1972 RVA: 0x00030BDC File Offset: 0x0002EDDC
		public void VoxelizeInput(GraphTransform graphTransform, Bounds graphSpaceBounds)
		{
			Matrix4x4 matrix4x = Matrix4x4.TRS(graphSpaceBounds.min, Quaternion.identity, Vector3.one) * Matrix4x4.Scale(new Vector3(this.cellSize, this.cellHeight, this.cellSize));
			this.transformVoxel2Graph = new GraphTransform(matrix4x);
			this.transform = graphTransform * matrix4x * Matrix4x4.TRS(new Vector3(0.5f, 0f, 0.5f), Quaternion.identity, Vector3.one);
			int num = (int)(graphSpaceBounds.size.y / this.cellHeight);
			float num2 = Mathf.Cos(Mathf.Atan(Mathf.Tan(this.maxSlope * 0.017453292f) * (this.cellSize / this.cellHeight)));
			VoxelPolygonClipper voxelPolygonClipper = new VoxelPolygonClipper(3);
			VoxelPolygonClipper voxelPolygonClipper2 = new VoxelPolygonClipper(7);
			VoxelPolygonClipper voxelPolygonClipper3 = new VoxelPolygonClipper(7);
			VoxelPolygonClipper voxelPolygonClipper4 = new VoxelPolygonClipper(7);
			VoxelPolygonClipper voxelPolygonClipper5 = new VoxelPolygonClipper(7);
			if (this.inputMeshes == null)
			{
				throw new NullReferenceException("inputMeshes not set");
			}
			int num3 = 0;
			for (int i = 0; i < this.inputMeshes.Count; i++)
			{
				num3 = Math.Max(this.inputMeshes[i].vertices.Length, num3);
			}
			Vector3[] array = new Vector3[num3];
			for (int j = 0; j < this.inputMeshes.Count; j++)
			{
				RasterizationMesh rasterizationMesh = this.inputMeshes[j];
				Matrix4x4 matrix = rasterizationMesh.matrix;
				bool flag = VectorMath.ReversesFaceOrientations(matrix);
				Vector3[] vertices = rasterizationMesh.vertices;
				int[] triangles = rasterizationMesh.triangles;
				int numTriangles = rasterizationMesh.numTriangles;
				for (int k = 0; k < vertices.Length; k++)
				{
					array[k] = this.transform.InverseTransform(matrix.MultiplyPoint3x4(vertices[k]));
				}
				int area = rasterizationMesh.area;
				for (int l = 0; l < numTriangles; l += 3)
				{
					Vector3 vector = array[triangles[l]];
					Vector3 vector2 = array[triangles[l + 1]];
					Vector3 vector3 = array[triangles[l + 2]];
					if (flag)
					{
						Vector3 vector4 = vector;
						vector = vector3;
						vector3 = vector4;
					}
					int num4 = (int)Utility.Min(vector.x, vector2.x, vector3.x);
					int num5 = (int)Utility.Min(vector.z, vector2.z, vector3.z);
					int num6 = (int)Math.Ceiling((double)Utility.Max(vector.x, vector2.x, vector3.x));
					int num7 = (int)Math.Ceiling((double)Utility.Max(vector.z, vector2.z, vector3.z));
					num4 = Mathf.Clamp(num4, 0, this.voxelArea.width - 1);
					num6 = Mathf.Clamp(num6, 0, this.voxelArea.width - 1);
					num5 = Mathf.Clamp(num5, 0, this.voxelArea.depth - 1);
					num7 = Mathf.Clamp(num7, 0, this.voxelArea.depth - 1);
					if (num4 < this.voxelArea.width && num5 < this.voxelArea.depth && num6 > 0 && num7 > 0)
					{
						int area2;
						if (Vector3.Dot(Vector3.Cross(vector2 - vector, vector3 - vector).normalized, Vector3.up) < num2)
						{
							area2 = 0;
						}
						else
						{
							area2 = 1 + area;
						}
						voxelPolygonClipper[0] = vector;
						voxelPolygonClipper[1] = vector2;
						voxelPolygonClipper[2] = vector3;
						voxelPolygonClipper.n = 3;
						for (int m = num4; m <= num6; m++)
						{
							voxelPolygonClipper.ClipPolygonAlongX(ref voxelPolygonClipper2, 1f, (float)(-(float)m) + 0.5f);
							if (voxelPolygonClipper2.n >= 3)
							{
								voxelPolygonClipper2.ClipPolygonAlongX(ref voxelPolygonClipper3, -1f, (float)m + 0.5f);
								if (voxelPolygonClipper3.n >= 3)
								{
									float num8 = voxelPolygonClipper3.z[0];
									float num9 = voxelPolygonClipper3.z[0];
									for (int n = 1; n < voxelPolygonClipper3.n; n++)
									{
										float val = voxelPolygonClipper3.z[n];
										num8 = Math.Min(num8, val);
										num9 = Math.Max(num9, val);
									}
									int num10 = Mathf.Clamp((int)Math.Round((double)num8), 0, this.voxelArea.depth - 1);
									int num11 = Mathf.Clamp((int)Math.Round((double)num9), 0, this.voxelArea.depth - 1);
									for (int num12 = num10; num12 <= num11; num12++)
									{
										voxelPolygonClipper3.ClipPolygonAlongZWithYZ(ref voxelPolygonClipper4, 1f, (float)(-(float)num12) + 0.5f);
										if (voxelPolygonClipper4.n >= 3)
										{
											voxelPolygonClipper4.ClipPolygonAlongZWithY(ref voxelPolygonClipper5, -1f, (float)num12 + 0.5f);
											if (voxelPolygonClipper5.n >= 3)
											{
												float num13 = voxelPolygonClipper5.y[0];
												float num14 = voxelPolygonClipper5.y[0];
												for (int num15 = 1; num15 < voxelPolygonClipper5.n; num15++)
												{
													float val2 = voxelPolygonClipper5.y[num15];
													num13 = Math.Min(num13, val2);
													num14 = Math.Max(num14, val2);
												}
												int num16 = (int)Math.Ceiling((double)num14);
												if (num16 >= 0 && num13 <= (float)num)
												{
													int num17 = Math.Max(0, (int)num13);
													num16 = Math.Max(num17 + 1, num16);
													this.voxelArea.AddLinkedSpan(num12 * this.voxelArea.width + m, (uint)num17, (uint)num16, area2, this.voxelWalkableClimb);
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x060007B5 RID: 1973 RVA: 0x00031168 File Offset: 0x0002F368
		public void DebugDrawSpans()
		{
			int num = this.voxelArea.width * this.voxelArea.depth;
			Vector3 min = this.forcedBounds.min;
			LinkedVoxelSpan[] linkedSpans = this.voxelArea.linkedSpans;
			int i = 0;
			int num2 = 0;
			while (i < num)
			{
				for (int j = 0; j < this.voxelArea.width; j++)
				{
					int num3 = i + j;
					while (num3 != -1 && linkedSpans[num3].bottom != 4294967295U)
					{
						uint top = linkedSpans[num3].top;
						uint num4 = (linkedSpans[num3].next != -1) ? linkedSpans[linkedSpans[num3].next].bottom : 65536U;
						if (top > num4)
						{
							Debug.Log(top.ToString() + " " + num4.ToString());
							Debug.DrawLine(new Vector3((float)j * this.cellSize, top * this.cellHeight, (float)num2 * this.cellSize) + min, new Vector3((float)j * this.cellSize, num4 * this.cellHeight, (float)num2 * this.cellSize) + min, Color.yellow, 1f);
						}
						uint num5 = num4 - top;
						uint num6 = this.voxelWalkableHeight;
						num3 = linkedSpans[num3].next;
					}
				}
				i += this.voxelArea.width;
				num2++;
			}
		}

		// Token: 0x060007B6 RID: 1974 RVA: 0x000312F4 File Offset: 0x0002F4F4
		public void BuildCompactField()
		{
			int spanCount = this.voxelArea.GetSpanCount();
			this.voxelArea.compactSpanCount = spanCount;
			if (this.voxelArea.compactSpans == null || this.voxelArea.compactSpans.Length < spanCount)
			{
				this.voxelArea.compactSpans = new CompactVoxelSpan[spanCount];
				this.voxelArea.areaTypes = new int[spanCount];
			}
			uint num = 0U;
			int num2 = this.voxelArea.width;
			int num3 = this.voxelArea.depth;
			int num4 = num2 * num3;
			if (this.voxelWalkableHeight >= 65535U)
			{
				Debug.LogWarning("Too high walkable height to guarantee correctness. Increase voxel height or lower walkable height.");
			}
			LinkedVoxelSpan[] linkedSpans = this.voxelArea.linkedSpans;
			int i = 0;
			int num5 = 0;
			while (i < num4)
			{
				for (int j = 0; j < num2; j++)
				{
					int num6 = j + i;
					if (linkedSpans[num6].bottom == 4294967295U)
					{
						this.voxelArea.compactCells[j + i] = new CompactVoxelCell(0U, 0U);
					}
					else
					{
						uint i2 = num;
						uint num7 = 0U;
						while (num6 != -1)
						{
							if (linkedSpans[num6].area != 0)
							{
								int top = (int)linkedSpans[num6].top;
								int next = linkedSpans[num6].next;
								int num8 = (int)((next != -1) ? linkedSpans[next].bottom : 65536U);
								this.voxelArea.compactSpans[(int)num] = new CompactVoxelSpan((ushort)((top > 65535) ? 65535 : top), (uint)((num8 - top > 65535) ? 65535 : (num8 - top)));
								this.voxelArea.areaTypes[(int)num] = linkedSpans[num6].area;
								num += 1U;
								num7 += 1U;
							}
							num6 = linkedSpans[num6].next;
						}
						this.voxelArea.compactCells[j + i] = new CompactVoxelCell(i2, num7);
					}
				}
				i += num2;
				num5++;
			}
		}

		// Token: 0x060007B7 RID: 1975 RVA: 0x00031500 File Offset: 0x0002F700
		public void BuildVoxelConnections()
		{
			int num = this.voxelArea.width * this.voxelArea.depth;
			CompactVoxelSpan[] compactSpans = this.voxelArea.compactSpans;
			CompactVoxelCell[] compactCells = this.voxelArea.compactCells;
			int i = 0;
			int num2 = 0;
			while (i < num)
			{
				for (int j = 0; j < this.voxelArea.width; j++)
				{
					CompactVoxelCell compactVoxelCell = compactCells[j + i];
					int k = (int)compactVoxelCell.index;
					int num3 = (int)(compactVoxelCell.index + compactVoxelCell.count);
					while (k < num3)
					{
						CompactVoxelSpan compactVoxelSpan = compactSpans[k];
						compactSpans[k].con = uint.MaxValue;
						for (int l = 0; l < 4; l++)
						{
							int num4 = j + this.voxelArea.DirectionX[l];
							int num5 = i + this.voxelArea.DirectionZ[l];
							if (num4 >= 0 && num5 >= 0 && num5 < num && num4 < this.voxelArea.width)
							{
								CompactVoxelCell compactVoxelCell2 = compactCells[num4 + num5];
								int m = (int)compactVoxelCell2.index;
								int num6 = (int)(compactVoxelCell2.index + compactVoxelCell2.count);
								while (m < num6)
								{
									CompactVoxelSpan compactVoxelSpan2 = compactSpans[m];
									int num7 = (int)Math.Max(compactVoxelSpan.y, compactVoxelSpan2.y);
									if ((long)(Math.Min((int)((uint)compactVoxelSpan.y + compactVoxelSpan.h), (int)((uint)compactVoxelSpan2.y + compactVoxelSpan2.h)) - num7) >= (long)((ulong)this.voxelWalkableHeight) && Math.Abs((int)(compactVoxelSpan2.y - compactVoxelSpan.y)) <= this.voxelWalkableClimb)
									{
										uint num8 = (uint)(m - (int)compactVoxelCell2.index);
										if (num8 <= 65535U)
										{
											compactSpans[k].SetConnection(l, num8);
											break;
										}
										Debug.LogError("Too many layers");
									}
									m++;
								}
							}
						}
						k++;
					}
				}
				i += this.voxelArea.width;
				num2++;
			}
		}

		// Token: 0x060007B8 RID: 1976 RVA: 0x00031714 File Offset: 0x0002F914
		private void DrawLine(int a, int b, int[] indices, int[] verts, Color color)
		{
			int num = (indices[a] & 268435455) * 4;
			int num2 = (indices[b] & 268435455) * 4;
			Debug.DrawLine(this.VoxelToWorld(verts[num], verts[num + 1], verts[num + 2]), this.VoxelToWorld(verts[num2], verts[num2 + 1], verts[num2 + 2]), color);
		}

		// Token: 0x060007B9 RID: 1977 RVA: 0x0003176C File Offset: 0x0002F96C
		public Vector3 VoxelToWorld(int x, int y, int z)
		{
			return Vector3.Scale(new Vector3((float)x, (float)y, (float)z), this.cellScale) + this.voxelOffset;
		}

		// Token: 0x060007BA RID: 1978 RVA: 0x00031790 File Offset: 0x0002F990
		public Int3 VoxelToWorldInt3(Int3 voxelPosition)
		{
			Int3 @int = voxelPosition * 1000;
			@int = new Int3(Mathf.RoundToInt((float)@int.x * this.cellScale.x), Mathf.RoundToInt((float)@int.y * this.cellScale.y), Mathf.RoundToInt((float)@int.z * this.cellScale.z));
			return @int + (Int3)this.voxelOffset;
		}

		// Token: 0x060007BB RID: 1979 RVA: 0x00031809 File Offset: 0x0002FA09
		private Vector3 ConvertPosWithoutOffset(int x, int y, int z)
		{
			return Vector3.Scale(new Vector3((float)x, (float)y, (float)z / (float)this.voxelArea.width), this.cellScale) + this.voxelOffset;
		}

		// Token: 0x060007BC RID: 1980 RVA: 0x0003183C File Offset: 0x0002FA3C
		private Vector3 ConvertPosition(int x, int z, int i)
		{
			CompactVoxelSpan compactVoxelSpan = this.voxelArea.compactSpans[i];
			return new Vector3((float)x * this.cellSize, (float)compactVoxelSpan.y * this.cellHeight, (float)z / (float)this.voxelArea.width * this.cellSize) + this.voxelOffset;
		}

		// Token: 0x060007BD RID: 1981 RVA: 0x00031898 File Offset: 0x0002FA98
		public void ErodeWalkableArea(int radius)
		{
			ushort[] array = this.voxelArea.tmpUShortArr;
			if (array == null || array.Length < this.voxelArea.compactSpanCount)
			{
				array = (this.voxelArea.tmpUShortArr = new ushort[this.voxelArea.compactSpanCount]);
			}
			Memory.MemSet<ushort>(array, ushort.MaxValue, 2);
			this.CalculateDistanceField(array);
			for (int i = 0; i < array.Length; i++)
			{
				if ((int)array[i] < radius * 2)
				{
					this.voxelArea.areaTypes[i] = 0;
				}
			}
		}

		// Token: 0x060007BE RID: 1982 RVA: 0x0003191C File Offset: 0x0002FB1C
		public void BuildDistanceField()
		{
			ushort[] array = this.voxelArea.tmpUShortArr;
			if (array == null || array.Length < this.voxelArea.compactSpanCount)
			{
				array = (this.voxelArea.tmpUShortArr = new ushort[this.voxelArea.compactSpanCount]);
			}
			Memory.MemSet<ushort>(array, ushort.MaxValue, 2);
			this.voxelArea.maxDistance = this.CalculateDistanceField(array);
			ushort[] array2 = this.voxelArea.dist;
			if (array2 == null || array2.Length < this.voxelArea.compactSpanCount)
			{
				array2 = new ushort[this.voxelArea.compactSpanCount];
			}
			array2 = this.BoxBlur(array, array2);
			this.voxelArea.dist = array2;
		}

		// Token: 0x060007BF RID: 1983 RVA: 0x000319CC File Offset: 0x0002FBCC
		[Obsolete("This function is not complete and should not be used")]
		public void ErodeVoxels(int radius)
		{
			if (radius > 255)
			{
				Debug.LogError("Max Erode Radius is 255");
				radius = 255;
			}
			int num = this.voxelArea.width * this.voxelArea.depth;
			int[] array = new int[this.voxelArea.compactSpanCount];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = 255;
			}
			for (int j = 0; j < num; j += this.voxelArea.width)
			{
				for (int k = 0; k < this.voxelArea.width; k++)
				{
					CompactVoxelCell compactVoxelCell = this.voxelArea.compactCells[k + j];
					int l = (int)compactVoxelCell.index;
					int num2 = (int)(compactVoxelCell.index + compactVoxelCell.count);
					while (l < num2)
					{
						if (this.voxelArea.areaTypes[l] != 0)
						{
							CompactVoxelSpan compactVoxelSpan = this.voxelArea.compactSpans[l];
							int num3 = 0;
							for (int m = 0; m < 4; m++)
							{
								if ((long)compactVoxelSpan.GetConnection(m) != 63L)
								{
									num3++;
								}
							}
							if (num3 != 4)
							{
								array[l] = 0;
							}
						}
						l++;
					}
				}
			}
		}

		// Token: 0x060007C0 RID: 1984 RVA: 0x00031B04 File Offset: 0x0002FD04
		public void FilterLowHeightSpans(uint voxelWalkableHeight, float cs, float ch)
		{
			int num = this.voxelArea.width * this.voxelArea.depth;
			LinkedVoxelSpan[] linkedSpans = this.voxelArea.linkedSpans;
			int i = 0;
			int num2 = 0;
			while (i < num)
			{
				for (int j = 0; j < this.voxelArea.width; j++)
				{
					int num3 = i + j;
					while (num3 != -1 && linkedSpans[num3].bottom != 4294967295U)
					{
						uint top = linkedSpans[num3].top;
						if (((linkedSpans[num3].next != -1) ? linkedSpans[linkedSpans[num3].next].bottom : 65536U) - top < voxelWalkableHeight)
						{
							linkedSpans[num3].area = 0;
						}
						num3 = linkedSpans[num3].next;
					}
				}
				i += this.voxelArea.width;
				num2++;
			}
		}

		// Token: 0x060007C1 RID: 1985 RVA: 0x00031BF8 File Offset: 0x0002FDF8
		public void FilterLedges(uint voxelWalkableHeight, int voxelWalkableClimb, float cs, float ch)
		{
			int num = this.voxelArea.width * this.voxelArea.depth;
			LinkedVoxelSpan[] linkedSpans = this.voxelArea.linkedSpans;
			int[] directionX = this.voxelArea.DirectionX;
			int[] directionZ = this.voxelArea.DirectionZ;
			int num2 = this.voxelArea.width;
			int i = 0;
			int num3 = 0;
			while (i < num)
			{
				for (int j = 0; j < num2; j++)
				{
					if (linkedSpans[j + i].bottom != 4294967295U)
					{
						for (int num4 = j + i; num4 != -1; num4 = linkedSpans[num4].next)
						{
							if (linkedSpans[num4].area != 0)
							{
								int top = (int)linkedSpans[num4].top;
								int val = (int)((linkedSpans[num4].next != -1) ? linkedSpans[linkedSpans[num4].next].bottom : 65536U);
								int num5 = 65536;
								int num6 = (int)linkedSpans[num4].top;
								int num7 = num6;
								for (int k = 0; k < 4; k++)
								{
									int num8 = j + directionX[k];
									int num9 = i + directionZ[k];
									if (num8 < 0 || num9 < 0 || num9 >= num || num8 >= num2)
									{
										linkedSpans[num4].area = 0;
										break;
									}
									int num10 = num8 + num9;
									int num11 = -voxelWalkableClimb;
									int val2 = (int)((linkedSpans[num10].bottom != uint.MaxValue) ? linkedSpans[num10].bottom : 65536U);
									if ((long)(Math.Min(val, val2) - Math.Max(top, num11)) > (long)((ulong)voxelWalkableHeight))
									{
										num5 = Math.Min(num5, num11 - top);
									}
									if (linkedSpans[num10].bottom != 4294967295U)
									{
										for (int num12 = num10; num12 != -1; num12 = linkedSpans[num12].next)
										{
											num11 = (int)linkedSpans[num12].top;
											val2 = (int)((linkedSpans[num12].next != -1) ? linkedSpans[linkedSpans[num12].next].bottom : 65536U);
											if ((long)(Math.Min(val, val2) - Math.Max(top, num11)) > (long)((ulong)voxelWalkableHeight))
											{
												num5 = Math.Min(num5, num11 - top);
												if (Math.Abs(num11 - top) <= voxelWalkableClimb)
												{
													if (num11 < num6)
													{
														num6 = num11;
													}
													if (num11 > num7)
													{
														num7 = num11;
													}
												}
											}
										}
									}
								}
								if (num5 < -voxelWalkableClimb || num7 - num6 > voxelWalkableClimb)
								{
									linkedSpans[num4].area = 0;
								}
							}
						}
					}
				}
				i += num2;
				num3++;
			}
		}

		// Token: 0x060007C2 RID: 1986 RVA: 0x00031EA8 File Offset: 0x000300A8
		public bool FloodRegion(int x, int z, int i, uint level, ushort r, ushort[] srcReg, ushort[] srcDist, Int3[] stack, int[] flags = null, bool[] closed = null)
		{
			int num = this.voxelArea.areaTypes[i];
			int j = 1;
			stack[0] = new Int3
			{
				x = x,
				y = i,
				z = z
			};
			srcReg[i] = r;
			srcDist[i] = 0;
			int num2 = (int)((level >= 2U) ? (level - 2U) : 0U);
			int num3 = 0;
			int[] directionX = this.voxelArea.DirectionX;
			int[] directionZ = this.voxelArea.DirectionZ;
			CompactVoxelCell[] compactCells = this.voxelArea.compactCells;
			CompactVoxelSpan[] compactSpans = this.voxelArea.compactSpans;
			int[] areaTypes = this.voxelArea.areaTypes;
			ushort[] dist = this.voxelArea.dist;
			while (j > 0)
			{
				j--;
				Int3 @int = stack[j];
				int y = @int.y;
				int x2 = @int.x;
				int z2 = @int.z;
				CompactVoxelSpan compactVoxelSpan = compactSpans[y];
				ushort num4 = 0;
				for (int k = 0; k < 4; k++)
				{
					if ((long)compactVoxelSpan.GetConnection(k) != 63L)
					{
						int num5 = x2 + directionX[k];
						int num6 = z2 + directionZ[k];
						int num7 = (int)(compactCells[num5 + num6].index + (uint)compactVoxelSpan.GetConnection(k));
						if (areaTypes[num7] == num)
						{
							ushort num8 = srcReg[num7];
							if ((num8 & 32768) != 32768)
							{
								if (num8 != 0 && num8 != r)
								{
									num4 = num8;
									break;
								}
								int num9 = k + 1 & 3;
								int connection = compactSpans[num7].GetConnection(num9);
								if ((long)connection != 63L)
								{
									int num10 = num5 + directionX[num9];
									int num11 = num6 + directionZ[num9];
									int num12 = (int)(compactCells[num10 + num11].index + (uint)connection);
									if (areaTypes[num12] == num)
									{
										ushort num13 = srcReg[num12];
										if ((num13 & 32768) != 32768 && num13 != 0 && num13 != r)
										{
											num4 = num13;
											break;
										}
									}
								}
							}
						}
					}
				}
				if (num4 != 0)
				{
					srcReg[y] = 0;
					srcDist[y] = ushort.MaxValue;
				}
				else
				{
					num3++;
					if (closed != null)
					{
						closed[y] = true;
					}
					for (int l = 0; l < 4; l++)
					{
						if ((long)compactVoxelSpan.GetConnection(l) != 63L)
						{
							int num14 = x2 + directionX[l];
							int num15 = z2 + directionZ[l];
							int num16 = (int)(compactCells[num14 + num15].index + (uint)compactVoxelSpan.GetConnection(l));
							if (areaTypes[num16] == num && srcReg[num16] == 0)
							{
								if ((int)dist[num16] >= num2 && flags[num16] == 0)
								{
									srcReg[num16] = r;
									srcDist[num16] = 0;
									stack[j] = new Int3
									{
										x = num14,
										y = num16,
										z = num15
									};
									j++;
								}
								else if (flags != null)
								{
									flags[num16] = (int)r;
									srcDist[num16] = 2;
								}
							}
						}
					}
				}
			}
			return num3 > 0;
		}

		// Token: 0x060007C3 RID: 1987 RVA: 0x00032194 File Offset: 0x00030394
		public void MarkRectWithRegion(int minx, int maxx, int minz, int maxz, ushort region, ushort[] srcReg)
		{
			int num = maxz * this.voxelArea.width;
			for (int i = minz * this.voxelArea.width; i < num; i += this.voxelArea.width)
			{
				for (int j = minx; j < maxx; j++)
				{
					CompactVoxelCell compactVoxelCell = this.voxelArea.compactCells[i + j];
					int k = (int)compactVoxelCell.index;
					int num2 = (int)(compactVoxelCell.index + compactVoxelCell.count);
					while (k < num2)
					{
						if (this.voxelArea.areaTypes[k] != 0)
						{
							srcReg[k] = region;
						}
						k++;
					}
				}
			}
		}

		// Token: 0x060007C4 RID: 1988 RVA: 0x00032230 File Offset: 0x00030430
		public ushort CalculateDistanceField(ushort[] src)
		{
			int num = this.voxelArea.width * this.voxelArea.depth;
			for (int i = 0; i < num; i += this.voxelArea.width)
			{
				for (int j = 0; j < this.voxelArea.width; j++)
				{
					CompactVoxelCell compactVoxelCell = this.voxelArea.compactCells[j + i];
					int k = (int)compactVoxelCell.index;
					int num2 = (int)(compactVoxelCell.index + compactVoxelCell.count);
					while (k < num2)
					{
						CompactVoxelSpan compactVoxelSpan = this.voxelArea.compactSpans[k];
						int num3 = 0;
						int num4 = 0;
						while (num4 < 4 && (long)compactVoxelSpan.GetConnection(num4) != 63L)
						{
							num3++;
							num4++;
						}
						if (num3 != 4)
						{
							src[k] = 0;
						}
						k++;
					}
				}
			}
			for (int l = 0; l < num; l += this.voxelArea.width)
			{
				for (int m = 0; m < this.voxelArea.width; m++)
				{
					CompactVoxelCell compactVoxelCell2 = this.voxelArea.compactCells[m + l];
					int n = (int)compactVoxelCell2.index;
					int num5 = (int)(compactVoxelCell2.index + compactVoxelCell2.count);
					while (n < num5)
					{
						CompactVoxelSpan compactVoxelSpan2 = this.voxelArea.compactSpans[n];
						if ((long)compactVoxelSpan2.GetConnection(0) != 63L)
						{
							int num6 = m + this.voxelArea.DirectionX[0];
							int num7 = l + this.voxelArea.DirectionZ[0];
							int num8 = (int)((ulong)this.voxelArea.compactCells[num6 + num7].index + (ulong)((long)compactVoxelSpan2.GetConnection(0)));
							if (src[num8] + 2 < src[n])
							{
								src[n] = src[num8] + 2;
							}
							CompactVoxelSpan compactVoxelSpan3 = this.voxelArea.compactSpans[num8];
							if ((long)compactVoxelSpan3.GetConnection(3) != 63L)
							{
								int num9 = num6 + this.voxelArea.DirectionX[3];
								int num10 = num7 + this.voxelArea.DirectionZ[3];
								int num11 = (int)((ulong)this.voxelArea.compactCells[num9 + num10].index + (ulong)((long)compactVoxelSpan3.GetConnection(3)));
								if (src[num11] + 3 < src[n])
								{
									src[n] = src[num11] + 3;
								}
							}
						}
						if ((long)compactVoxelSpan2.GetConnection(3) != 63L)
						{
							int num12 = m + this.voxelArea.DirectionX[3];
							int num13 = l + this.voxelArea.DirectionZ[3];
							int num14 = (int)((ulong)this.voxelArea.compactCells[num12 + num13].index + (ulong)((long)compactVoxelSpan2.GetConnection(3)));
							if (src[num14] + 2 < src[n])
							{
								src[n] = src[num14] + 2;
							}
							CompactVoxelSpan compactVoxelSpan4 = this.voxelArea.compactSpans[num14];
							if ((long)compactVoxelSpan4.GetConnection(2) != 63L)
							{
								int num15 = num12 + this.voxelArea.DirectionX[2];
								int num16 = num13 + this.voxelArea.DirectionZ[2];
								int num17 = (int)((ulong)this.voxelArea.compactCells[num15 + num16].index + (ulong)((long)compactVoxelSpan4.GetConnection(2)));
								if (src[num17] + 3 < src[n])
								{
									src[n] = src[num17] + 3;
								}
							}
						}
						n++;
					}
				}
			}
			for (int num18 = num - this.voxelArea.width; num18 >= 0; num18 -= this.voxelArea.width)
			{
				for (int num19 = this.voxelArea.width - 1; num19 >= 0; num19--)
				{
					CompactVoxelCell compactVoxelCell3 = this.voxelArea.compactCells[num19 + num18];
					int num20 = (int)compactVoxelCell3.index;
					int num21 = (int)(compactVoxelCell3.index + compactVoxelCell3.count);
					while (num20 < num21)
					{
						CompactVoxelSpan compactVoxelSpan5 = this.voxelArea.compactSpans[num20];
						if ((long)compactVoxelSpan5.GetConnection(2) != 63L)
						{
							int num22 = num19 + this.voxelArea.DirectionX[2];
							int num23 = num18 + this.voxelArea.DirectionZ[2];
							int num24 = (int)((ulong)this.voxelArea.compactCells[num22 + num23].index + (ulong)((long)compactVoxelSpan5.GetConnection(2)));
							if (src[num24] + 2 < src[num20])
							{
								src[num20] = src[num24] + 2;
							}
							CompactVoxelSpan compactVoxelSpan6 = this.voxelArea.compactSpans[num24];
							if ((long)compactVoxelSpan6.GetConnection(1) != 63L)
							{
								int num25 = num22 + this.voxelArea.DirectionX[1];
								int num26 = num23 + this.voxelArea.DirectionZ[1];
								int num27 = (int)((ulong)this.voxelArea.compactCells[num25 + num26].index + (ulong)((long)compactVoxelSpan6.GetConnection(1)));
								if (src[num27] + 3 < src[num20])
								{
									src[num20] = src[num27] + 3;
								}
							}
						}
						if ((long)compactVoxelSpan5.GetConnection(1) != 63L)
						{
							int num28 = num19 + this.voxelArea.DirectionX[1];
							int num29 = num18 + this.voxelArea.DirectionZ[1];
							int num30 = (int)((ulong)this.voxelArea.compactCells[num28 + num29].index + (ulong)((long)compactVoxelSpan5.GetConnection(1)));
							if (src[num30] + 2 < src[num20])
							{
								src[num20] = src[num30] + 2;
							}
							CompactVoxelSpan compactVoxelSpan7 = this.voxelArea.compactSpans[num30];
							if ((long)compactVoxelSpan7.GetConnection(0) != 63L)
							{
								int num31 = num28 + this.voxelArea.DirectionX[0];
								int num32 = num29 + this.voxelArea.DirectionZ[0];
								int num33 = (int)((ulong)this.voxelArea.compactCells[num31 + num32].index + (ulong)((long)compactVoxelSpan7.GetConnection(0)));
								if (src[num33] + 3 < src[num20])
								{
									src[num20] = src[num33] + 3;
								}
							}
						}
						num20++;
					}
				}
			}
			ushort num34 = 0;
			for (int num35 = 0; num35 < this.voxelArea.compactSpanCount; num35++)
			{
				num34 = Math.Max(src[num35], num34);
			}
			return num34;
		}

		// Token: 0x060007C5 RID: 1989 RVA: 0x0003285C File Offset: 0x00030A5C
		public ushort[] BoxBlur(ushort[] src, ushort[] dst)
		{
			ushort num = 20;
			for (int i = this.voxelArea.width * this.voxelArea.depth - this.voxelArea.width; i >= 0; i -= this.voxelArea.width)
			{
				for (int j = this.voxelArea.width - 1; j >= 0; j--)
				{
					CompactVoxelCell compactVoxelCell = this.voxelArea.compactCells[j + i];
					int k = (int)compactVoxelCell.index;
					int num2 = (int)(compactVoxelCell.index + compactVoxelCell.count);
					while (k < num2)
					{
						CompactVoxelSpan compactVoxelSpan = this.voxelArea.compactSpans[k];
						ushort num3 = src[k];
						if (num3 < num)
						{
							dst[k] = num3;
						}
						else
						{
							int num4 = (int)num3;
							for (int l = 0; l < 4; l++)
							{
								if ((long)compactVoxelSpan.GetConnection(l) != 63L)
								{
									int num5 = j + this.voxelArea.DirectionX[l];
									int num6 = i + this.voxelArea.DirectionZ[l];
									int num7 = (int)((ulong)this.voxelArea.compactCells[num5 + num6].index + (ulong)((long)compactVoxelSpan.GetConnection(l)));
									num4 += (int)src[num7];
									CompactVoxelSpan compactVoxelSpan2 = this.voxelArea.compactSpans[num7];
									int num8 = l + 1 & 3;
									if ((long)compactVoxelSpan2.GetConnection(num8) != 63L)
									{
										int num9 = num5 + this.voxelArea.DirectionX[num8];
										int num10 = num6 + this.voxelArea.DirectionZ[num8];
										int num11 = (int)((ulong)this.voxelArea.compactCells[num9 + num10].index + (ulong)((long)compactVoxelSpan2.GetConnection(num8)));
										num4 += (int)src[num11];
									}
									else
									{
										num4 += (int)num3;
									}
								}
								else
								{
									num4 += (int)(num3 * 2);
								}
							}
							dst[k] = (ushort)((float)(num4 + 5) / 9f);
						}
						k++;
					}
				}
			}
			return dst;
		}

		// Token: 0x060007C6 RID: 1990 RVA: 0x00032A5C File Offset: 0x00030C5C
		public void BuildRegions()
		{
			int num = this.voxelArea.width;
			int num2 = this.voxelArea.depth;
			int num3 = num * num2;
			int compactSpanCount = this.voxelArea.compactSpanCount;
			int num4 = 8;
			ushort[] array = ArrayPool<ushort>.Claim(compactSpanCount);
			ushort[] array2 = ArrayPool<ushort>.Claim(compactSpanCount);
			bool[] array3 = ArrayPool<bool>.Claim(compactSpanCount);
			int[] array4 = ArrayPool<int>.Claim(compactSpanCount);
			Int3[] stack = ArrayPool<Int3>.Claim(compactSpanCount);
			Memory.MemSet<ushort>(array, 0, 2);
			Memory.MemSet<ushort>(array2, ushort.MaxValue, 2);
			Memory.MemSet<bool>(array3, false, 1);
			Memory.MemSet<int>(array4, 0, 4);
			int[] directionX = this.voxelArea.DirectionX;
			int[] directionZ = this.voxelArea.DirectionZ;
			ushort[] dist = this.voxelArea.dist;
			int[] areaTypes = this.voxelArea.areaTypes;
			CompactVoxelCell[] compactCells = this.voxelArea.compactCells;
			ushort num5 = 2;
			this.MarkRectWithRegion(0, this.borderSize, 0, num2, num5 | 32768, array);
			num5 += 1;
			this.MarkRectWithRegion(num - this.borderSize, num, 0, num2, num5 | 32768, array);
			num5 += 1;
			this.MarkRectWithRegion(0, num, 0, this.borderSize, num5 | 32768, array);
			num5 += 1;
			this.MarkRectWithRegion(0, num, num2 - this.borderSize, num2, num5 | 32768, array);
			num5 += 1;
			Int3[][] array5 = new Int3[(int)(this.voxelArea.maxDistance / 2 + 1)][];
			int[] array6 = new int[array5.Length];
			for (int i = 0; i < array5.Length; i++)
			{
				array5[i] = new Int3[16];
			}
			int j = 0;
			int num6 = 0;
			while (j < num3)
			{
				for (int k = 0; k < this.voxelArea.width; k++)
				{
					CompactVoxelCell compactVoxelCell = compactCells[j + k];
					int l = (int)compactVoxelCell.index;
					int num7 = (int)(compactVoxelCell.index + compactVoxelCell.count);
					while (l < num7)
					{
						if ((array[l] & 32768) != 32768 && areaTypes[l] != 0)
						{
							int num8 = (int)(this.voxelArea.dist[l] / 2);
							if (array6[num8] >= array5[num8].Length)
							{
								Int3[] array7 = new Int3[array6[num8] * 2];
								array5[num8].CopyTo(array7, 0);
								array5[num8] = array7;
							}
							Int3[] array8 = array5[num8];
							int[] array9 = array6;
							int num9 = num8;
							int num10 = array9[num9];
							array9[num9] = num10 + 1;
							array8[num10] = new Int3(k, l, j);
						}
						l++;
					}
				}
				j += num;
				num6++;
			}
			Queue<Int3> queue = new Queue<Int3>();
			Queue<Int3> queue2 = new Queue<Int3>();
			for (int m = array5.Length - 1; m >= 0; m--)
			{
				uint num11 = (uint)(m * 2);
				Int3[] array10 = array5[m];
				int num12 = array6[m];
				for (int n = 0; n < num12; n++)
				{
					int y = array10[n].y;
					if (array4[y] != 0 && array[y] == 0)
					{
						array[y] = (ushort)array4[y];
						queue.Enqueue(array10[n]);
						array3[y] = true;
					}
				}
				int num13 = 0;
				while (num13 < num4 && queue.Count > 0)
				{
					while (queue.Count > 0)
					{
						Int3 @int = queue.Dequeue();
						int num14 = areaTypes[@int.y];
						CompactVoxelSpan compactVoxelSpan = this.voxelArea.compactSpans[@int.y];
						ushort num15 = array[@int.y];
						array3[@int.y] = true;
						ushort num16 = array2[@int.y] + 2;
						for (int num17 = 0; num17 < 4; num17++)
						{
							int connection = compactVoxelSpan.GetConnection(num17);
							if ((long)connection != 63L)
							{
								int num18 = @int.x + directionX[num17];
								int num19 = @int.z + directionZ[num17];
								int num20 = (int)(compactCells[num18 + num19].index + (uint)connection);
								if ((array[num20] & 32768) != 32768 && num14 == areaTypes[num20] && num16 < array2[num20])
								{
									if ((uint)dist[num20] < num11)
									{
										array2[num20] = num16;
										array4[num20] = (int)num15;
									}
									else if (!array3[num20])
									{
										array2[num20] = num16;
										if (array[num20] == 0)
										{
											queue2.Enqueue(new Int3(num18, num20, num19));
										}
										array[num20] = num15;
									}
								}
							}
						}
					}
					Memory.Swap<Queue<Int3>>(ref queue, ref queue2);
					num13++;
				}
				for (int num21 = 0; num21 < num12; num21++)
				{
					Int3 int2 = array10[num21];
					if (array[int2.y] == 0 && this.FloodRegion(int2.x, int2.z, int2.y, num11, num5, array, array2, stack, array4, array3))
					{
						num5 += 1;
					}
				}
			}
			this.voxelArea.maxRegions = (int)num5;
			this.FilterSmallRegions(array, this.minRegionSize, this.voxelArea.maxRegions);
			CompactVoxelSpan[] compactSpans = this.voxelArea.compactSpans;
			for (int num22 = 0; num22 < compactSpanCount; num22++)
			{
				compactSpans[num22].reg = (int)array[num22];
			}
			ArrayPool<ushort>.Release(ref array, false);
			ArrayPool<ushort>.Release(ref array2, false);
			ArrayPool<bool>.Release(ref array3, false);
			ArrayPool<int>.Release(ref array4, false);
			ArrayPool<Int3>.Release(ref stack, false);
		}

		// Token: 0x060007C7 RID: 1991 RVA: 0x00032FA8 File Offset: 0x000311A8
		private static int union_find_find(int[] arr, int x)
		{
			if (arr[x] < 0)
			{
				return x;
			}
			return arr[x] = Voxelize.union_find_find(arr, arr[x]);
		}

		// Token: 0x060007C8 RID: 1992 RVA: 0x00032FCC File Offset: 0x000311CC
		private static void union_find_union(int[] arr, int a, int b)
		{
			a = Voxelize.union_find_find(arr, a);
			b = Voxelize.union_find_find(arr, b);
			if (a == b)
			{
				return;
			}
			if (arr[a] > arr[b])
			{
				int num = a;
				a = b;
				b = num;
			}
			arr[a] += arr[b];
			arr[b] = a;
		}

		// Token: 0x060007C9 RID: 1993 RVA: 0x00033008 File Offset: 0x00031208
		public void FilterSmallRegions(ushort[] reg, int minRegionSize, int maxRegions)
		{
			RelevantGraphSurface relevantGraphSurface = RelevantGraphSurface.Root;
			bool flag = relevantGraphSurface != null && this.relevantGraphSurfaceMode > RecastGraph.RelevantGraphSurfaceMode.DoNotRequire;
			if (!flag && minRegionSize <= 0)
			{
				return;
			}
			int[] array = new int[maxRegions];
			ushort[] array2 = this.voxelArea.tmpUShortArr;
			if (array2 == null || array2.Length < maxRegions)
			{
				array2 = (this.voxelArea.tmpUShortArr = new ushort[maxRegions]);
			}
			Memory.MemSet<int>(array, -1, 4);
			Memory.MemSet<ushort>(array2, 0, maxRegions, 2);
			int num = array.Length;
			int num2 = this.voxelArea.width * this.voxelArea.depth;
			int num3 = 2 | ((this.relevantGraphSurfaceMode == RecastGraph.RelevantGraphSurfaceMode.OnlyForCompletelyInsideTile) ? 1 : 0);
			if (flag)
			{
				while (relevantGraphSurface != null)
				{
					int num4;
					int num5;
					this.VectorToIndex(relevantGraphSurface.Position, out num4, out num5);
					if (num4 >= 0 && num5 >= 0 && num4 < this.voxelArea.width && num5 < this.voxelArea.depth)
					{
						int num6 = (int)((relevantGraphSurface.Position.y - this.voxelOffset.y) / this.cellHeight);
						int num7 = (int)(relevantGraphSurface.maxRange / this.cellHeight);
						CompactVoxelCell compactVoxelCell = this.voxelArea.compactCells[num4 + num5 * this.voxelArea.width];
						int num8 = (int)compactVoxelCell.index;
						while ((long)num8 < (long)((ulong)(compactVoxelCell.index + compactVoxelCell.count)))
						{
							if (Math.Abs((int)this.voxelArea.compactSpans[num8].y - num6) <= num7 && reg[num8] != 0)
							{
								ushort[] array3 = array2;
								int num9 = Voxelize.union_find_find(array, (int)reg[num8] & -32769);
								array3[num9] |= 2;
							}
							num8++;
						}
					}
					relevantGraphSurface = relevantGraphSurface.Next;
				}
			}
			int i = 0;
			int num10 = 0;
			while (i < num2)
			{
				for (int j = 0; j < this.voxelArea.width; j++)
				{
					CompactVoxelCell compactVoxelCell2 = this.voxelArea.compactCells[j + i];
					int num11 = (int)compactVoxelCell2.index;
					while ((long)num11 < (long)((ulong)(compactVoxelCell2.index + compactVoxelCell2.count)))
					{
						CompactVoxelSpan compactVoxelSpan = this.voxelArea.compactSpans[num11];
						int num12 = (int)reg[num11];
						if ((num12 & -32769) != 0)
						{
							if (num12 >= num)
							{
								ushort[] array4 = array2;
								int num13 = Voxelize.union_find_find(array, num12 & -32769);
								array4[num13] |= 1;
							}
							else
							{
								int num14 = Voxelize.union_find_find(array, num12);
								array[num14]--;
								for (int k = 0; k < 4; k++)
								{
									if ((long)compactVoxelSpan.GetConnection(k) != 63L)
									{
										int num15 = j + this.voxelArea.DirectionX[k];
										int num16 = i + this.voxelArea.DirectionZ[k];
										int num17 = (int)(this.voxelArea.compactCells[num15 + num16].index + (uint)compactVoxelSpan.GetConnection(k));
										int num18 = (int)reg[num17];
										if (num12 != num18 && (num18 & -32769) != 0)
										{
											if ((num18 & 32768) != 0)
											{
												ushort[] array5 = array2;
												int num19 = num14;
												array5[num19] |= 1;
											}
											else
											{
												Voxelize.union_find_union(array, num14, num18);
											}
										}
									}
								}
							}
						}
						num11++;
					}
				}
				i += this.voxelArea.width;
				num10++;
			}
			for (int l = 0; l < array.Length; l++)
			{
				ushort[] array6 = array2;
				int num20 = Voxelize.union_find_find(array, l);
				array6[num20] |= array2[l];
			}
			for (int m = 0; m < array.Length; m++)
			{
				int num21 = Voxelize.union_find_find(array, m);
				if ((array2[num21] & 1) != 0)
				{
					array[num21] = -minRegionSize - 2;
				}
				if (flag && ((int)array2[num21] & num3) == 0)
				{
					array[num21] = -1;
				}
			}
			for (int n = 0; n < this.voxelArea.compactSpanCount; n++)
			{
				int num22 = (int)reg[n];
				if (num22 < num && array[Voxelize.union_find_find(array, num22)] >= -minRegionSize - 1)
				{
					reg[n] = 0;
				}
			}
		}

		// Token: 0x04000481 RID: 1153
		public List<RasterizationMesh> inputMeshes;

		// Token: 0x04000482 RID: 1154
		public readonly int voxelWalkableClimb;

		// Token: 0x04000483 RID: 1155
		public readonly uint voxelWalkableHeight;

		// Token: 0x04000484 RID: 1156
		public readonly float cellSize = 0.2f;

		// Token: 0x04000485 RID: 1157
		public readonly float cellHeight = 0.1f;

		// Token: 0x04000486 RID: 1158
		public int minRegionSize = 100;

		// Token: 0x04000487 RID: 1159
		public int borderSize;

		// Token: 0x04000488 RID: 1160
		public float maxEdgeLength = 20f;

		// Token: 0x04000489 RID: 1161
		public float maxSlope = 30f;

		// Token: 0x0400048A RID: 1162
		public RecastGraph.RelevantGraphSurfaceMode relevantGraphSurfaceMode;

		// Token: 0x0400048B RID: 1163
		public Bounds forcedBounds;

		// Token: 0x0400048C RID: 1164
		public VoxelArea voxelArea;

		// Token: 0x0400048D RID: 1165
		public VoxelContourSet countourSet;

		// Token: 0x0400048E RID: 1166
		private GraphTransform transform;

		// Token: 0x04000490 RID: 1168
		public int width;

		// Token: 0x04000491 RID: 1169
		public int depth;

		// Token: 0x04000492 RID: 1170
		private Vector3 voxelOffset = Vector3.zero;

		// Token: 0x04000493 RID: 1171
		public const uint NotConnected = 63U;

		// Token: 0x04000494 RID: 1172
		private const int MaxLayers = 65535;

		// Token: 0x04000495 RID: 1173
		private const int MaxRegions = 500;

		// Token: 0x04000496 RID: 1174
		private const int UnwalkableArea = 0;

		// Token: 0x04000497 RID: 1175
		private const ushort BorderReg = 32768;

		// Token: 0x04000498 RID: 1176
		private const int RC_BORDER_VERTEX = 65536;

		// Token: 0x04000499 RID: 1177
		private const int RC_AREA_BORDER = 131072;

		// Token: 0x0400049A RID: 1178
		private const int VERTEX_BUCKET_COUNT = 4096;

		// Token: 0x0400049B RID: 1179
		public const int RC_CONTOUR_TESS_WALL_EDGES = 1;

		// Token: 0x0400049C RID: 1180
		public const int RC_CONTOUR_TESS_AREA_EDGES = 2;

		// Token: 0x0400049D RID: 1181
		public const int RC_CONTOUR_TESS_TILE_EDGES = 4;

		// Token: 0x0400049E RID: 1182
		private const int ContourRegMask = 65535;

		// Token: 0x0400049F RID: 1183
		private readonly Vector3 cellScale;
	}
}
