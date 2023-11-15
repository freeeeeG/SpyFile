using System;
using System.Collections.Generic;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x0200000B RID: 11
	public class RichPath
	{
		// Token: 0x06000154 RID: 340 RVA: 0x00006613 File Offset: 0x00004813
		public RichPath()
		{
			this.Clear();
		}

		// Token: 0x06000155 RID: 341 RVA: 0x0000662C File Offset: 0x0000482C
		public void Clear()
		{
			this.parts.Clear();
			this.currentPart = 0;
			this.Endpoint = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
		}

		// Token: 0x06000156 RID: 342 RVA: 0x0000665C File Offset: 0x0000485C
		public void Initialize(Seeker seeker, Path path, bool mergePartEndpoints, bool simplificationMode)
		{
			if (path.error)
			{
				throw new ArgumentException("Path has an error");
			}
			List<GraphNode> path2 = path.path;
			if (path2.Count == 0)
			{
				throw new ArgumentException("Path traverses no nodes");
			}
			this.seeker = seeker;
			for (int i = 0; i < this.parts.Count; i++)
			{
				RichFunnel richFunnel = this.parts[i] as RichFunnel;
				RichSpecial richSpecial = this.parts[i] as RichSpecial;
				if (richFunnel != null)
				{
					ObjectPool<RichFunnel>.Release(ref richFunnel);
				}
				else if (richSpecial != null)
				{
					ObjectPool<RichSpecial>.Release(ref richSpecial);
				}
			}
			this.Clear();
			this.Endpoint = path.vectorPath[path.vectorPath.Count - 1];
			for (int j = 0; j < path2.Count; j++)
			{
				if (path2[j] is TriangleMeshNode)
				{
					NavmeshBase navmeshBase = AstarData.GetGraph(path2[j]) as NavmeshBase;
					if (navmeshBase == null)
					{
						throw new Exception("Found a TriangleMeshNode that was not in a NavmeshBase graph");
					}
					RichFunnel richFunnel2 = ObjectPool<RichFunnel>.Claim().Initialize(this, navmeshBase);
					richFunnel2.funnelSimplification = simplificationMode;
					int num = j;
					uint graphIndex = path2[num].GraphIndex;
					while (j < path2.Count && (path2[j].GraphIndex == graphIndex || path2[j] is NodeLink3Node))
					{
						j++;
					}
					j--;
					if (num == 0)
					{
						richFunnel2.exactStart = path.vectorPath[0];
					}
					else
					{
						richFunnel2.exactStart = (Vector3)path2[mergePartEndpoints ? (num - 1) : num].position;
					}
					if (j == path2.Count - 1)
					{
						richFunnel2.exactEnd = path.vectorPath[path.vectorPath.Count - 1];
					}
					else
					{
						richFunnel2.exactEnd = (Vector3)path2[mergePartEndpoints ? (j + 1) : j].position;
					}
					richFunnel2.BuildFunnelCorridor(path2, num, j);
					this.parts.Add(richFunnel2);
				}
				else if (NodeLink2.GetNodeLink(path2[j]) != null)
				{
					NodeLink2 nodeLink = NodeLink2.GetNodeLink(path2[j]);
					int num2 = j;
					uint graphIndex2 = path2[num2].GraphIndex;
					j++;
					while (j < path2.Count && path2[j].GraphIndex == graphIndex2)
					{
						j++;
					}
					j--;
					if (j - num2 > 1)
					{
						throw new Exception("NodeLink2 path length greater than two (2) nodes. " + (j - num2).ToString());
					}
					if (j - num2 != 0)
					{
						RichSpecial item = ObjectPool<RichSpecial>.Claim().Initialize(nodeLink, path2[num2]);
						this.parts.Add(item);
					}
				}
				else if (!(path2[j] is PointNode))
				{
					throw new InvalidOperationException("The RichAI movment script can only be used on recast/navmesh graphs. A node of type " + path2[j].GetType().Name + " was in the path.");
				}
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000157 RID: 343 RVA: 0x0000695D File Offset: 0x00004B5D
		// (set) Token: 0x06000158 RID: 344 RVA: 0x00006965 File Offset: 0x00004B65
		public Vector3 Endpoint { get; private set; }

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000159 RID: 345 RVA: 0x0000696E File Offset: 0x00004B6E
		public bool CompletedAllParts
		{
			get
			{
				return this.currentPart >= this.parts.Count;
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x0600015A RID: 346 RVA: 0x00006986 File Offset: 0x00004B86
		public bool IsLastPart
		{
			get
			{
				return this.currentPart >= this.parts.Count - 1;
			}
		}

		// Token: 0x0600015B RID: 347 RVA: 0x000069A0 File Offset: 0x00004BA0
		public void NextPart()
		{
			this.currentPart = Mathf.Min(this.currentPart + 1, this.parts.Count);
		}

		// Token: 0x0600015C RID: 348 RVA: 0x000069C0 File Offset: 0x00004BC0
		public RichPathPart GetCurrentPart()
		{
			if (this.parts.Count == 0)
			{
				return null;
			}
			if (this.currentPart >= this.parts.Count)
			{
				return this.parts[this.parts.Count - 1];
			}
			return this.parts[this.currentPart];
		}

		// Token: 0x0600015D RID: 349 RVA: 0x00006A1C File Offset: 0x00004C1C
		public void GetRemainingPath(List<Vector3> buffer, Vector3 currentPosition, out bool requiresRepath)
		{
			buffer.Clear();
			buffer.Add(currentPosition);
			requiresRepath = false;
			for (int i = this.currentPart; i < this.parts.Count; i++)
			{
				RichPathPart richPathPart = this.parts[i];
				RichFunnel richFunnel = richPathPart as RichFunnel;
				if (richFunnel != null)
				{
					if (i != 0)
					{
						buffer.Add(richFunnel.exactStart);
					}
					bool flag;
					richFunnel.Update((i == 0) ? currentPosition : richFunnel.exactStart, buffer, int.MaxValue, out flag, out requiresRepath);
					if (requiresRepath)
					{
						return;
					}
				}
				else
				{
					RichSpecial richSpecial = richPathPart as RichSpecial;
				}
			}
		}

		// Token: 0x040000A9 RID: 169
		private int currentPart;

		// Token: 0x040000AA RID: 170
		private readonly List<RichPathPart> parts = new List<RichPathPart>();

		// Token: 0x040000AB RID: 171
		public Seeker seeker;

		// Token: 0x040000AC RID: 172
		public ITransform transform;
	}
}
