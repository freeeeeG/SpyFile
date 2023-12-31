﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000074 RID: 116
	public class RecastBBTree
	{
		// Token: 0x06000622 RID: 1570 RVA: 0x000241B4 File Offset: 0x000223B4
		public void QueryInBounds(Rect bounds, List<RecastMeshObj> buffer)
		{
			if (this.root == null)
			{
				return;
			}
			this.QueryBoxInBounds(this.root, bounds, buffer);
		}

		// Token: 0x06000623 RID: 1571 RVA: 0x000241D0 File Offset: 0x000223D0
		private void QueryBoxInBounds(RecastBBTreeBox box, Rect bounds, List<RecastMeshObj> boxes)
		{
			if (box.mesh != null)
			{
				if (RecastBBTree.RectIntersectsRect(box.rect, bounds))
				{
					boxes.Add(box.mesh);
					return;
				}
			}
			else
			{
				if (RecastBBTree.RectIntersectsRect(box.c1.rect, bounds))
				{
					this.QueryBoxInBounds(box.c1, bounds, boxes);
				}
				if (RecastBBTree.RectIntersectsRect(box.c2.rect, bounds))
				{
					this.QueryBoxInBounds(box.c2, bounds, boxes);
				}
			}
		}

		// Token: 0x06000624 RID: 1572 RVA: 0x00024248 File Offset: 0x00022448
		public bool Remove(RecastMeshObj mesh)
		{
			if (mesh == null)
			{
				throw new ArgumentNullException("mesh");
			}
			if (this.root == null)
			{
				return false;
			}
			bool result = false;
			Bounds bounds = mesh.GetBounds();
			Rect bounds2 = Rect.MinMaxRect(bounds.min.x, bounds.min.z, bounds.max.x, bounds.max.z);
			this.root = this.RemoveBox(this.root, mesh, bounds2, ref result);
			return result;
		}

		// Token: 0x06000625 RID: 1573 RVA: 0x000242CC File Offset: 0x000224CC
		private RecastBBTreeBox RemoveBox(RecastBBTreeBox c, RecastMeshObj mesh, Rect bounds, ref bool found)
		{
			if (!RecastBBTree.RectIntersectsRect(c.rect, bounds))
			{
				return c;
			}
			if (c.mesh == mesh)
			{
				found = true;
				return null;
			}
			if (c.mesh == null && !found)
			{
				c.c1 = this.RemoveBox(c.c1, mesh, bounds, ref found);
				if (c.c1 == null)
				{
					return c.c2;
				}
				if (!found)
				{
					c.c2 = this.RemoveBox(c.c2, mesh, bounds, ref found);
					if (c.c2 == null)
					{
						return c.c1;
					}
				}
				if (found)
				{
					c.rect = RecastBBTree.ExpandToContain(c.c1.rect, c.c2.rect);
				}
			}
			return c;
		}

		// Token: 0x06000626 RID: 1574 RVA: 0x00024388 File Offset: 0x00022588
		public void Insert(RecastMeshObj mesh)
		{
			RecastBBTreeBox recastBBTreeBox = new RecastBBTreeBox(mesh);
			if (this.root == null)
			{
				this.root = recastBBTreeBox;
				return;
			}
			RecastBBTreeBox recastBBTreeBox2 = this.root;
			for (;;)
			{
				recastBBTreeBox2.rect = RecastBBTree.ExpandToContain(recastBBTreeBox2.rect, recastBBTreeBox.rect);
				if (recastBBTreeBox2.mesh != null)
				{
					break;
				}
				float num = RecastBBTree.ExpansionRequired(recastBBTreeBox2.c1.rect, recastBBTreeBox.rect);
				float num2 = RecastBBTree.ExpansionRequired(recastBBTreeBox2.c2.rect, recastBBTreeBox.rect);
				if (num < num2)
				{
					recastBBTreeBox2 = recastBBTreeBox2.c1;
				}
				else if (num2 < num)
				{
					recastBBTreeBox2 = recastBBTreeBox2.c2;
				}
				else
				{
					recastBBTreeBox2 = ((RecastBBTree.RectArea(recastBBTreeBox2.c1.rect) < RecastBBTree.RectArea(recastBBTreeBox2.c2.rect)) ? recastBBTreeBox2.c1 : recastBBTreeBox2.c2);
				}
			}
			recastBBTreeBox2.c1 = recastBBTreeBox;
			RecastBBTreeBox c = new RecastBBTreeBox(recastBBTreeBox2.mesh);
			recastBBTreeBox2.c2 = c;
			recastBBTreeBox2.mesh = null;
		}

		// Token: 0x06000627 RID: 1575 RVA: 0x00024480 File Offset: 0x00022680
		private static bool RectIntersectsRect(Rect r, Rect r2)
		{
			return r.xMax > r2.xMin && r.yMax > r2.yMin && r2.xMax > r.xMin && r2.yMax > r.yMin;
		}

		// Token: 0x06000628 RID: 1576 RVA: 0x000244D0 File Offset: 0x000226D0
		private static float ExpansionRequired(Rect r, Rect r2)
		{
			float num = Mathf.Min(r.xMin, r2.xMin);
			float num2 = Mathf.Max(r.xMax, r2.xMax);
			float num3 = Mathf.Min(r.yMin, r2.yMin);
			float num4 = Mathf.Max(r.yMax, r2.yMax);
			return (num2 - num) * (num4 - num3) - RecastBBTree.RectArea(r);
		}

		// Token: 0x06000629 RID: 1577 RVA: 0x0002453C File Offset: 0x0002273C
		private static Rect ExpandToContain(Rect r, Rect r2)
		{
			float xmin = Mathf.Min(r.xMin, r2.xMin);
			float xmax = Mathf.Max(r.xMax, r2.xMax);
			float ymin = Mathf.Min(r.yMin, r2.yMin);
			float ymax = Mathf.Max(r.yMax, r2.yMax);
			return Rect.MinMaxRect(xmin, ymin, xmax, ymax);
		}

		// Token: 0x0600062A RID: 1578 RVA: 0x000245A0 File Offset: 0x000227A0
		private static float RectArea(Rect r)
		{
			return r.width * r.height;
		}

		// Token: 0x04000375 RID: 885
		private RecastBBTreeBox root;
	}
}
