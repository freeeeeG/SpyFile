﻿using System;
using Pathfinding.RVO.Sampled;
using UnityEngine;

namespace Pathfinding.RVO
{
	// Token: 0x020000D9 RID: 217
	public class RVOQuadtree
	{
		// Token: 0x0600091E RID: 2334 RVA: 0x0003C471 File Offset: 0x0003A671
		public void Clear()
		{
			this.nodes[0] = default(RVOQuadtree.Node);
			this.filledNodes = 1;
			this.maxRadius = 0f;
		}

		// Token: 0x0600091F RID: 2335 RVA: 0x0003C497 File Offset: 0x0003A697
		public void SetBounds(Rect r)
		{
			this.bounds = r;
		}

		// Token: 0x06000920 RID: 2336 RVA: 0x0003C4A0 File Offset: 0x0003A6A0
		private int GetNodeIndex()
		{
			if (this.filledNodes + 4 >= this.nodes.Length)
			{
				RVOQuadtree.Node[] array = new RVOQuadtree.Node[this.nodes.Length * 2];
				for (int i = 0; i < this.nodes.Length; i++)
				{
					array[i] = this.nodes[i];
				}
				this.nodes = array;
			}
			this.nodes[this.filledNodes] = default(RVOQuadtree.Node);
			this.nodes[this.filledNodes].child00 = this.filledNodes;
			this.filledNodes++;
			this.nodes[this.filledNodes] = default(RVOQuadtree.Node);
			this.nodes[this.filledNodes].child00 = this.filledNodes;
			this.filledNodes++;
			this.nodes[this.filledNodes] = default(RVOQuadtree.Node);
			this.nodes[this.filledNodes].child00 = this.filledNodes;
			this.filledNodes++;
			this.nodes[this.filledNodes] = default(RVOQuadtree.Node);
			this.nodes[this.filledNodes].child00 = this.filledNodes;
			this.filledNodes++;
			return this.filledNodes - 4;
		}

		// Token: 0x06000921 RID: 2337 RVA: 0x0003C608 File Offset: 0x0003A808
		public void Insert(Agent agent)
		{
			int num = 0;
			Rect r = this.bounds;
			Vector2 vector = new Vector2(agent.position.x, agent.position.y);
			agent.next = null;
			this.maxRadius = Math.Max(agent.radius, this.maxRadius);
			int num2 = 0;
			for (;;)
			{
				num2++;
				if (this.nodes[num].child00 == num)
				{
					if (this.nodes[num].count < 15 || num2 > 10)
					{
						break;
					}
					this.nodes[num].child00 = this.GetNodeIndex();
					this.nodes[num].Distribute(this.nodes, r);
				}
				if (this.nodes[num].child00 != num)
				{
					Vector2 center = r.center;
					if (vector.x > center.x)
					{
						if (vector.y > center.y)
						{
							num = this.nodes[num].child00 + 3;
							r = Rect.MinMaxRect(center.x, center.y, r.xMax, r.yMax);
						}
						else
						{
							num = this.nodes[num].child00 + 2;
							r = Rect.MinMaxRect(center.x, r.yMin, r.xMax, center.y);
						}
					}
					else if (vector.y > center.y)
					{
						num = this.nodes[num].child00 + 1;
						r = Rect.MinMaxRect(r.xMin, center.y, center.x, r.yMax);
					}
					else
					{
						num = this.nodes[num].child00;
						r = Rect.MinMaxRect(r.xMin, r.yMin, center.x, center.y);
					}
				}
			}
			this.nodes[num].Add(agent);
			RVOQuadtree.Node[] array = this.nodes;
			int num3 = num;
			array[num3].count = array[num3].count + 1;
		}

		// Token: 0x06000922 RID: 2338 RVA: 0x0003C81F File Offset: 0x0003AA1F
		public void CalculateSpeeds()
		{
			this.nodes[0].CalculateMaxSpeed(this.nodes, 0);
		}

		// Token: 0x06000923 RID: 2339 RVA: 0x0003C83C File Offset: 0x0003AA3C
		public void Query(Vector2 p, float speed, float timeHorizon, float agentRadius, Agent agent)
		{
			RVOQuadtree.QuadtreeQuery quadtreeQuery = default(RVOQuadtree.QuadtreeQuery);
			quadtreeQuery.p = p;
			quadtreeQuery.speed = speed;
			quadtreeQuery.timeHorizon = timeHorizon;
			quadtreeQuery.maxRadius = float.PositiveInfinity;
			quadtreeQuery.agentRadius = agentRadius;
			quadtreeQuery.agent = agent;
			quadtreeQuery.nodes = this.nodes;
			quadtreeQuery.QueryRec(0, this.bounds);
		}

		// Token: 0x06000924 RID: 2340 RVA: 0x0003C8A2 File Offset: 0x0003AAA2
		public void DebugDraw()
		{
			this.DebugDrawRec(0, this.bounds);
		}

		// Token: 0x06000925 RID: 2341 RVA: 0x0003C8B4 File Offset: 0x0003AAB4
		private void DebugDrawRec(int i, Rect r)
		{
			Debug.DrawLine(new Vector3(r.xMin, 0f, r.yMin), new Vector3(r.xMax, 0f, r.yMin), Color.white);
			Debug.DrawLine(new Vector3(r.xMax, 0f, r.yMin), new Vector3(r.xMax, 0f, r.yMax), Color.white);
			Debug.DrawLine(new Vector3(r.xMax, 0f, r.yMax), new Vector3(r.xMin, 0f, r.yMax), Color.white);
			Debug.DrawLine(new Vector3(r.xMin, 0f, r.yMax), new Vector3(r.xMin, 0f, r.yMin), Color.white);
			if (this.nodes[i].child00 != i)
			{
				Vector2 center = r.center;
				this.DebugDrawRec(this.nodes[i].child00 + 3, Rect.MinMaxRect(center.x, center.y, r.xMax, r.yMax));
				this.DebugDrawRec(this.nodes[i].child00 + 2, Rect.MinMaxRect(center.x, r.yMin, r.xMax, center.y));
				this.DebugDrawRec(this.nodes[i].child00 + 1, Rect.MinMaxRect(r.xMin, center.y, center.x, r.yMax));
				this.DebugDrawRec(this.nodes[i].child00, Rect.MinMaxRect(r.xMin, r.yMin, center.x, center.y));
			}
			for (Agent agent = this.nodes[i].linkedList; agent != null; agent = agent.next)
			{
				Vector2 position = this.nodes[i].linkedList.position;
				Debug.DrawLine(new Vector3(position.x, 0f, position.y) + Vector3.up, new Vector3(agent.position.x, 0f, agent.position.y) + Vector3.up, new Color(1f, 1f, 0f, 0.5f));
			}
		}

		// Token: 0x0400055D RID: 1373
		private const int LeafSize = 15;

		// Token: 0x0400055E RID: 1374
		private float maxRadius;

		// Token: 0x0400055F RID: 1375
		private RVOQuadtree.Node[] nodes = new RVOQuadtree.Node[16];

		// Token: 0x04000560 RID: 1376
		private int filledNodes = 1;

		// Token: 0x04000561 RID: 1377
		private Rect bounds;

		// Token: 0x02000174 RID: 372
		private struct Node
		{
			// Token: 0x06000B9F RID: 2975 RVA: 0x00049C4C File Offset: 0x00047E4C
			public void Add(Agent agent)
			{
				agent.next = this.linkedList;
				this.linkedList = agent;
			}

			// Token: 0x06000BA0 RID: 2976 RVA: 0x00049C64 File Offset: 0x00047E64
			public void Distribute(RVOQuadtree.Node[] nodes, Rect r)
			{
				Vector2 center = r.center;
				while (this.linkedList != null)
				{
					Agent next = this.linkedList.next;
					int num = this.child00 + ((this.linkedList.position.x > center.x) ? 2 : 0) + ((this.linkedList.position.y > center.y) ? 1 : 0);
					nodes[num].Add(this.linkedList);
					this.linkedList = next;
				}
				this.count = 0;
			}

			// Token: 0x06000BA1 RID: 2977 RVA: 0x00049CF0 File Offset: 0x00047EF0
			public float CalculateMaxSpeed(RVOQuadtree.Node[] nodes, int index)
			{
				if (this.child00 == index)
				{
					for (Agent next = this.linkedList; next != null; next = next.next)
					{
						this.maxSpeed = Math.Max(this.maxSpeed, next.CalculatedSpeed);
					}
				}
				else
				{
					this.maxSpeed = Math.Max(nodes[this.child00].CalculateMaxSpeed(nodes, this.child00), nodes[this.child00 + 1].CalculateMaxSpeed(nodes, this.child00 + 1));
					this.maxSpeed = Math.Max(this.maxSpeed, nodes[this.child00 + 2].CalculateMaxSpeed(nodes, this.child00 + 2));
					this.maxSpeed = Math.Max(this.maxSpeed, nodes[this.child00 + 3].CalculateMaxSpeed(nodes, this.child00 + 3));
				}
				return this.maxSpeed;
			}

			// Token: 0x0400084B RID: 2123
			public int child00;

			// Token: 0x0400084C RID: 2124
			public Agent linkedList;

			// Token: 0x0400084D RID: 2125
			public byte count;

			// Token: 0x0400084E RID: 2126
			public float maxSpeed;
		}

		// Token: 0x02000175 RID: 373
		private struct QuadtreeQuery
		{
			// Token: 0x06000BA2 RID: 2978 RVA: 0x00049DD4 File Offset: 0x00047FD4
			public void QueryRec(int i, Rect r)
			{
				float num = Math.Min(Math.Max((this.nodes[i].maxSpeed + this.speed) * this.timeHorizon, this.agentRadius) + this.agentRadius, this.maxRadius);
				if (this.nodes[i].child00 == i)
				{
					for (Agent agent = this.nodes[i].linkedList; agent != null; agent = agent.next)
					{
						float num2 = this.agent.InsertAgentNeighbour(agent, num * num);
						if (num2 < this.maxRadius * this.maxRadius)
						{
							this.maxRadius = Mathf.Sqrt(num2);
						}
					}
					return;
				}
				Vector2 center = r.center;
				if (this.p.x - num < center.x)
				{
					if (this.p.y - num < center.y)
					{
						this.QueryRec(this.nodes[i].child00, Rect.MinMaxRect(r.xMin, r.yMin, center.x, center.y));
						num = Math.Min(num, this.maxRadius);
					}
					if (this.p.y + num > center.y)
					{
						this.QueryRec(this.nodes[i].child00 + 1, Rect.MinMaxRect(r.xMin, center.y, center.x, r.yMax));
						num = Math.Min(num, this.maxRadius);
					}
				}
				if (this.p.x + num > center.x)
				{
					if (this.p.y - num < center.y)
					{
						this.QueryRec(this.nodes[i].child00 + 2, Rect.MinMaxRect(center.x, r.yMin, r.xMax, center.y));
						num = Math.Min(num, this.maxRadius);
					}
					if (this.p.y + num > center.y)
					{
						this.QueryRec(this.nodes[i].child00 + 3, Rect.MinMaxRect(center.x, center.y, r.xMax, r.yMax));
					}
				}
			}

			// Token: 0x0400084F RID: 2127
			public Vector2 p;

			// Token: 0x04000850 RID: 2128
			public float speed;

			// Token: 0x04000851 RID: 2129
			public float timeHorizon;

			// Token: 0x04000852 RID: 2130
			public float agentRadius;

			// Token: 0x04000853 RID: 2131
			public float maxRadius;

			// Token: 0x04000854 RID: 2132
			public Agent agent;

			// Token: 0x04000855 RID: 2133
			public RVOQuadtree.Node[] nodes;
		}
	}
}
