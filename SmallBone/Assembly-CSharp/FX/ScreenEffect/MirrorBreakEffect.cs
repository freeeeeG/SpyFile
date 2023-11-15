using System;
using System.Collections.Generic;
using System.Linq;
using Level;
using Unity.Mathematics;
using UnityEngine;

namespace FX.ScreenEffect
{
	// Token: 0x0200027B RID: 635
	public class MirrorBreakEffect : MonoBehaviour
	{
		// Token: 0x06000C5D RID: 3165 RVA: 0x00021FF4 File Offset: 0x000201F4
		public void Initialize()
		{
			if (this.rootObject != null)
			{
				UnityEngine.Object.Destroy(this.rootObject);
			}
			this.IsInitialized = true;
			this.camera = Camera.main;
			this._cameraSize = this.camera.orthographicSize;
			if (this._forceControlcamera)
			{
				this.camera.GetComponent<CameraController>().enabled = false;
			}
			this.halfScreenSizeX = this._screenSize.x * 0.5f;
			this.halfScreenSizeY = this._screenSize.y * 0.5f;
			this.vertices = new Vector2[this.verticesNum + 4];
			if (this.verticesPositionRatio.x + this.verticesPositionRatio.y + this.verticesPositionRatio.z + this.verticesPositionRatio.w == 0f)
			{
				this.verticesPositionRatio.x = (this.verticesPositionRatio.y = (this.verticesPositionRatio.z = (this.verticesPositionRatio.w = 2.5f)));
			}
			if (this.verticesNum <= 0)
			{
				this.verticesNum = 1;
			}
			this.innerVerticesRatio = this.verticesPositionRatio.x;
			this.circleVerticesRatio = this.verticesPositionRatio.y;
			this.outerVerticesRatio = this.verticesPositionRatio.z;
			this.edgeVerticesRatio = this.verticesPositionRatio.w;
			float num = this.innerVerticesRatio + this.circleVerticesRatio + this.outerVerticesRatio + this.edgeVerticesRatio;
			this.innerVerticesRatio = this.innerVerticesRatio / num * (float)this.verticesNum;
			this.circleVerticesRatio = this.circleVerticesRatio / num * (float)this.verticesNum + this.innerVerticesRatio;
			this.outerVerticesRatio = this.outerVerticesRatio / num * (float)this.verticesNum + this.circleVerticesRatio;
			this.edgeVerticesRatio = this.edgeVerticesRatio / num * (float)this.verticesNum + this.outerVerticesRatio;
			this.topLeft = new Vector2(-this.halfScreenSizeX - this._centerPoint.x, this.halfScreenSizeY - this._centerPoint.y);
			this.topRight = new Vector2(this.halfScreenSizeX + this._centerPoint.x, this.halfScreenSizeY - this._centerPoint.y);
			this.downLeft = new Vector2(-this.halfScreenSizeX - this._centerPoint.x, -this.halfScreenSizeY + this._centerPoint.y);
			this.downRight = new Vector2(this.halfScreenSizeX + this._centerPoint.x, -this.halfScreenSizeY + this._centerPoint.y);
			this.topLeftDegree = Vector2.SignedAngle(new Vector2(1f, 0f), this.topLeft);
			this.topRightDegree = Vector2.SignedAngle(new Vector2(1f, 0f), this.topRight);
			this.downLeftDegree = Vector2.SignedAngle(new Vector2(1f, 0f), this.downLeft);
			this.downRightDegree = Vector2.SignedAngle(new Vector2(1f, 0f), this.downRight);
			this.deadVertices = new List<int>();
			this.edges = new List<int2>();
			this.triangles = new List<MirrorBreakEffect.Triangle>();
			this.deadEdges = new List<int2>();
			this.convexHullPointsIndex = new List<int>();
			this.deadEdgeOutterCircle = new List<int>();
			this.IsVertexCreated = false;
			this.IsEdgeCreated = false;
			this.IsTriangleCreated = false;
			this.firstTriangles = 0;
			this.convexHullTriangles = 0;
			this.convexHullToDeadVerticeTriangles = 0;
			this.outterCircleLinkVerticeTriangles = 0;
			this.outterCircleLeftTriangles = 0;
			this.insideConvexHullEdgeNum = 100;
			this.convexHullEdgeToOutterCircleVertice = 1000;
			this.outterCircleBranchVerticeLink = 1000;
			this._material.SetTexture("_MainTex", this._endingSprite);
		}

		// Token: 0x06000C5E RID: 3166 RVA: 0x000223D2 File Offset: 0x000205D2
		private void OnDestroy()
		{
			if (this.camera != null)
			{
				this.camera.GetComponent<CameraController>().enabled = true;
			}
		}

		// Token: 0x06000C5F RID: 3167 RVA: 0x000223F4 File Offset: 0x000205F4
		public void CreateVertex()
		{
			this.Initialize();
			for (int i = 0; i < this.verticesNum; i++)
			{
				float f = UnityEngine.Random.Range(0f, 6.2831855f);
				if ((float)i < this.innerVerticesRatio)
				{
					float d = UnityEngine.Random.Range(0f, this._innerCircleRadius.x);
					this.vertices[i] = new Vector2(Mathf.Cos(f) + this._centerPoint.x, Mathf.Sin(f) + this._centerPoint.y) * d;
				}
				else if ((float)i < this.circleVerticesRatio)
				{
					float d = UnityEngine.Random.Range(this._innerCircleRadius.x, this._innerCircleRadius.y);
					this.vertices[i] = new Vector2(Mathf.Cos(f) + this._centerPoint.x, Mathf.Sin(f) + this._centerPoint.y) * d;
				}
				else if ((float)i < this.outerVerticesRatio)
				{
					float num = this._edgeThickness * 2f;
					Vector2 vector = new Vector2(UnityEngine.Random.Range(-this.halfScreenSizeX + num, this.halfScreenSizeX - num), UnityEngine.Random.Range(-this.halfScreenSizeY + num, this.halfScreenSizeY - num));
					int num2 = 0;
					while (vector.magnitude <= this._innerCircleRadius.y + 1f)
					{
						vector = new Vector2(UnityEngine.Random.Range(-this.halfScreenSizeX + num, this.halfScreenSizeX - num), UnityEngine.Random.Range(-this.halfScreenSizeY + num, this.halfScreenSizeY - num));
						num2++;
						if (num2 > 100)
						{
							break;
						}
					}
					this.vertices[i] = vector;
					this.CheckOutSidePoints(i);
				}
				else
				{
					float d = this.halfScreenSizeX * 3f;
					this.vertices[i] = new Vector2(Mathf.Cos(f), Mathf.Sin(f)) * d;
					this.CheckOutSidePoints(i);
				}
			}
			this.vertices[this.verticesNum - 1] = new Vector2(UnityEngine.Random.Range(this._centerPoint.x - this.halfScreenSizeX * 0.5f, this._centerPoint.x + this._edgeThickness), this.halfScreenSizeY);
			this.vertices[this.verticesNum - 2] = new Vector2(UnityEngine.Random.Range(this._centerPoint.x + this.halfScreenSizeX * 0.5f, this._centerPoint.x - this._edgeThickness), this.halfScreenSizeY);
			this.vertices[this.verticesNum - 3] = new Vector2(UnityEngine.Random.Range(this._centerPoint.x - this.halfScreenSizeX * 0.5f, this._centerPoint.x + this._edgeThickness), -this.halfScreenSizeY);
			this.vertices[this.verticesNum - 4] = new Vector2(UnityEngine.Random.Range(this._centerPoint.x + this.halfScreenSizeX * 0.5f, this._centerPoint.x - this._edgeThickness), -this.halfScreenSizeY);
			this.vertices[this.verticesNum - 5] = new Vector2(this.halfScreenSizeX, UnityEngine.Random.Range(this._centerPoint.y - this.halfScreenSizeY * 0.5f, this._centerPoint.y + this.halfScreenSizeY * 0.5f));
			this.vertices[this.verticesNum - 6] = new Vector2(-this.halfScreenSizeX, UnityEngine.Random.Range(this._centerPoint.y - this.halfScreenSizeY * 0.5f, this._centerPoint.y + this.halfScreenSizeY * 0.5f));
			this.vertices[this.verticesNum] = new Vector2(this.topLeft.x, this.topLeft.y);
			this.vertices[this.verticesNum + 1] = new Vector2(this.topRight.x, this.topRight.y);
			this.vertices[this.verticesNum + 2] = new Vector2(this.downLeft.x, this.downLeft.y);
			this.vertices[this.verticesNum + 3] = new Vector2(this.downRight.x, this.downRight.y);
			this.deadVertices.Add(this.verticesNum + 1);
			this.deadVertices.Add(this.verticesNum + 2);
			this.deadVertices.Add(this.verticesNum + 3);
			this.IsVertexCreated = true;
		}

		// Token: 0x06000C60 RID: 3168 RVA: 0x000228C0 File Offset: 0x00020AC0
		public void CheckOutSidePoints(int _currentIndex)
		{
			if (this.topLeft.x < this.vertices[_currentIndex].x && this.vertices[_currentIndex].x < this.topRight.x && this.vertices[_currentIndex].y > this.downRight.y && this.vertices[_currentIndex].y < this.topRight.y)
			{
				return;
			}
			float num = Vector2.SignedAngle(new Vector2(1f, 0f), this.vertices[_currentIndex]);
			if (this.topRightDegree <= num && num <= this.topLeftDegree)
			{
				this.GetTwoLineIntersectionPoint(this._centerPoint, this.vertices[_currentIndex], new Vector2(-this.halfScreenSizeX, this.halfScreenSizeY), new Vector2(this.halfScreenSizeX, this.halfScreenSizeY), MirrorBreakEffect.EdgeDirection.Up, _currentIndex, ref this.vertices[_currentIndex]);
				return;
			}
			if (this.downLeftDegree <= num && num <= this.downRightDegree)
			{
				this.GetTwoLineIntersectionPoint(this._centerPoint, this.vertices[_currentIndex], new Vector2(-this.halfScreenSizeX, -this.halfScreenSizeY), new Vector2(this.halfScreenSizeX, -this.halfScreenSizeY), MirrorBreakEffect.EdgeDirection.Down, _currentIndex, ref this.vertices[_currentIndex]);
				return;
			}
			if (this.downRightDegree <= num && num <= this.topRightDegree)
			{
				this.GetTwoLineIntersectionPoint(this._centerPoint, this.vertices[_currentIndex], new Vector2(this.halfScreenSizeX, -this.halfScreenSizeY), new Vector2(this.halfScreenSizeX, this.halfScreenSizeY), MirrorBreakEffect.EdgeDirection.Right, _currentIndex, ref this.vertices[_currentIndex]);
				return;
			}
			this.GetTwoLineIntersectionPoint(this._centerPoint, this.vertices[_currentIndex], new Vector2(-this.halfScreenSizeX, -this.halfScreenSizeY), new Vector2(-this.halfScreenSizeX, this.halfScreenSizeY), MirrorBreakEffect.EdgeDirection.Left, _currentIndex, ref this.vertices[_currentIndex]);
		}

		// Token: 0x06000C61 RID: 3169 RVA: 0x00022ACC File Offset: 0x00020CCC
		public bool GetTwoLineIntersectionPoint(Vector2 A1, Vector2 A2, Vector2 B1, Vector2 B2, MirrorBreakEffect.EdgeDirection edgeNum, int currentIndex, ref Vector2 originalVector)
		{
			float num = (B2.x - B1.x) * (A2.y - A1.y) - (B2.y - B1.y) * (A2.x - A1.x);
			if (num == 0f)
			{
				return false;
			}
			float num2 = ((A1.x - B1.x) * (A2.y - A1.y) - (A1.y - B1.y) * (A2.x - A1.x)) / num;
			float num3 = B1.x + (B2.x - B1.x) * num2;
			float num4 = B1.y + (B2.y - B1.y) * num2;
			switch (edgeNum)
			{
			case MirrorBreakEffect.EdgeDirection.Up:
				if (num3 < B1.x || num3 > B2.x)
				{
					return false;
				}
				break;
			case MirrorBreakEffect.EdgeDirection.Left:
				if (num4 < B1.y || num4 > B2.y)
				{
					return false;
				}
				break;
			case MirrorBreakEffect.EdgeDirection.Down:
				if (num3 < B1.x || num3 > B2.x)
				{
					return false;
				}
				break;
			case MirrorBreakEffect.EdgeDirection.Right:
				if (num4 < B1.y || num4 > B2.y)
				{
					return false;
				}
				break;
			case MirrorBreakEffect.EdgeDirection.ShortTwo:
				if (math.min(math.min(A1.x, A2.x), math.min(B1.x, B2.x)) < num3 && num3 < math.max(math.max(A1.x, A2.x), math.max(B1.x, B2.x)) && math.min(math.min(A1.y, A2.y), math.min(B1.y, B2.y)) < num4 && num4 < math.max(math.max(A1.y, A2.y), math.max(B1.y, B2.y)))
				{
					return true;
				}
				break;
			}
			originalVector = new Vector2(num3, num4);
			this.deadVertices.Add(currentIndex);
			return true;
		}

		// Token: 0x06000C62 RID: 3170 RVA: 0x00022CD4 File Offset: 0x00020ED4
		public bool CheckTwoLineIntersect(Vector2 A1, Vector2 A2, Vector2 B1, Vector2 B2)
		{
			float num = (B2.x - B1.x) * (A2.y - A1.y) - (B2.y - B1.y) * (A2.x - A1.x);
			if (num == 0f)
			{
				return false;
			}
			float num2 = ((A1.x - B1.x) * (A2.y - A1.y) - (A1.y - B1.y) * (A2.x - A1.x)) / num;
			float num3 = B1.x + (B2.x - B1.x) * num2;
			float num4 = B1.y + (B2.y - B1.y) * num2;
			return math.min(math.min(A1.x, A2.x), math.min(B1.x, B2.x)) < num3 && num3 < math.max(math.max(A1.x, A2.x), math.max(B1.x, B2.x)) && math.min(math.min(A1.y, A2.y), math.min(B1.y, B2.y)) < num4 && num4 < math.max(math.max(A1.y, A2.y), math.max(B1.y, B2.y));
		}

		// Token: 0x06000C63 RID: 3171 RVA: 0x00022E44 File Offset: 0x00021044
		public void SetEdges()
		{
			List<int> list = new List<int>(this.deadVertices);
			this.deadEdges = new List<int2>();
			this.edges = new List<int2>();
			this.triangles = new List<MirrorBreakEffect.Triangle>();
			this.deadVertices.Add(this.verticesNum);
			int num = this.verticesNum;
			MirrorBreakEffect.EdgeDirection edgeDirection = MirrorBreakEffect.EdgeDirection.Up;
			for (int i = 0; i < this.deadVertices.Count; i++)
			{
				float num2 = this.halfScreenSizeX * 2.5f;
				int index = 0;
				switch (edgeDirection)
				{
				case MirrorBreakEffect.EdgeDirection.Up:
				{
					for (int j = 0; j < list.Count; j++)
					{
						if (this.vertices[list[j]].y == this.topLeft.y)
						{
							float num3 = Mathf.Abs(this.vertices[num].x - this.vertices[list[j]].x);
							if (num3 < num2)
							{
								num2 = num3;
								index = j;
							}
						}
					}
					int num4 = list[index];
					this.edges.Add(new int2(num, num4));
					this.deadEdges.Add(new int2(num, num4));
					list.RemoveAt(index);
					num = num4;
					if (this.vertices[num].x == this.topRight.x)
					{
						edgeDirection = MirrorBreakEffect.EdgeDirection.Right;
						list.Add(this.verticesNum);
					}
					break;
				}
				case MirrorBreakEffect.EdgeDirection.Left:
				{
					for (int k = 0; k < list.Count; k++)
					{
						if (this.vertices[list[k]].x == this.topLeft.x)
						{
							float num3 = Mathf.Abs(this.vertices[num].y - this.vertices[list[k]].y);
							if (num3 < num2)
							{
								num2 = num3;
								index = k;
							}
						}
					}
					int num4 = list[index];
					this.edges.Add(new int2(num, num4));
					this.deadEdges.Add(new int2(num, num4));
					list.RemoveAt(index);
					num = num4;
					break;
				}
				case MirrorBreakEffect.EdgeDirection.Down:
				{
					for (int l = 0; l < list.Count; l++)
					{
						if (this.vertices[list[l]].y == this.downLeft.y)
						{
							float num3 = Mathf.Abs(this.vertices[num].x - this.vertices[list[l]].x);
							if (num3 < num2)
							{
								num2 = num3;
								index = l;
							}
						}
					}
					int num4 = list[index];
					this.edges.Add(new int2(num, num4));
					this.deadEdges.Add(new int2(num, num4));
					list.RemoveAt(index);
					num = num4;
					if (this.vertices[num].x == this.downLeft.x)
					{
						edgeDirection = MirrorBreakEffect.EdgeDirection.Left;
					}
					break;
				}
				case MirrorBreakEffect.EdgeDirection.Right:
				{
					for (int m = 0; m < list.Count; m++)
					{
						if (this.vertices[list[m]].x == this.topRight.x)
						{
							float num3 = Mathf.Abs(this.vertices[num].y - this.vertices[list[m]].y);
							if (num3 < num2)
							{
								num2 = num3;
								index = m;
							}
						}
					}
					int num4 = list[index];
					this.edges.Add(new int2(num, num4));
					this.deadEdges.Add(new int2(num, num4));
					list.RemoveAt(index);
					num = num4;
					if (this.vertices[num].y == this.downLeft.y)
					{
						edgeDirection = MirrorBreakEffect.EdgeDirection.Down;
					}
					break;
				}
				}
			}
			float num5 = this._innerCircleRadius.y;
			int num6 = 0;
			while ((float)num6 < this.circleVerticesRatio)
			{
				float num7 = Vector2.Distance(Vector2.zero, this.vertices[num6]);
				if (num7 < num5)
				{
					num5 = num7;
					this.closestCenterPointIndex = num6;
				}
				num6++;
			}
			List<MirrorBreakEffect.VerticesWithAngle> list2 = new List<MirrorBreakEffect.VerticesWithAngle>();
			int num8 = 0;
			while ((float)num8 < this.circleVerticesRatio)
			{
				if (num8 != this.closestCenterPointIndex)
				{
					MirrorBreakEffect.VerticesWithAngle item = default(MirrorBreakEffect.VerticesWithAngle);
					item.verticeIndex = num8;
					Vector2 vector = this.vertices[num8] - this.vertices[this.closestCenterPointIndex];
					item.verticeAngle = Mathf.Atan2(vector.y, vector.x);
					list2.Add(item);
				}
				num8++;
			}
			List<MirrorBreakEffect.VerticesWithAngle> list3 = (from x in list2
			orderby x.verticeAngle descending
			select x).ToList<MirrorBreakEffect.VerticesWithAngle>();
			for (int n = 0; n < list3.Count; n++)
			{
			}
			int count = this.deadEdges.Count;
			for (int num9 = 0; num9 < list3.Count; num9++)
			{
				MirrorBreakEffect.Triangle triangle = new MirrorBreakEffect.Triangle();
				int index2 = (num9 + 1) % list3.Count;
				int2 @int = new int2(this.closestCenterPointIndex, list3[num9].verticeIndex);
				this.EdgeContainsCheck(0, ref @int, ref triangle);
				triangle.vertices.x = this.closestCenterPointIndex;
				triangle.vertices.y = list3[num9].verticeIndex;
				@int = new int2(list3[num9].verticeIndex, list3[index2].verticeIndex);
				this.EdgeContainsCheck(1, ref @int, ref triangle);
				triangle.vertices.z = list3[index2].verticeIndex;
				@int = new int2(this.closestCenterPointIndex, list3[index2].verticeIndex);
				this.EdgeContainsCheck(2, ref @int, ref triangle);
				this.triangles.Add(triangle);
				this.firstTriangles = this.triangles.Count - 1;
			}
			List<MirrorBreakEffect.VerticesWithAngle> list4 = new List<MirrorBreakEffect.VerticesWithAngle>(list3);
			List<int> list5 = new List<int>();
			this.convexHullPointsIndex = new List<int>();
			for (int num10 = 0; num10 < list3.Count; num10++)
			{
				this.convexHullPointsIndex.Add(list3[num10].verticeIndex);
			}
			int num11 = 0;
			while (list4.Count > 0)
			{
				int firstEdgesFromTheLast = this.GetFirstEdgesFromTheLast(list4[0].verticeIndex, this.closestCenterPointIndex, list5);
				int secondEdgesFromTheLast = this.GetSecondEdgesFromTheLast(list4[0].verticeIndex, this.closestCenterPointIndex, list5);
				if (firstEdgesFromTheLast == -1 || secondEdgesFromTheLast == -1)
				{
					list4.RemoveAt(0);
				}
				else
				{
					Vector2 vector2;
					int num12;
					if (list4[0].verticeIndex == this.edges[firstEdgesFromTheLast].x)
					{
						vector2 = this.vertices[this.edges[firstEdgesFromTheLast].y] - this.vertices[this.edges[firstEdgesFromTheLast].x];
						num12 = this.edges[firstEdgesFromTheLast].y;
					}
					else
					{
						vector2 = this.vertices[this.edges[firstEdgesFromTheLast].x] - this.vertices[this.edges[firstEdgesFromTheLast].y];
						num12 = this.edges[firstEdgesFromTheLast].x;
					}
					Vector2 vector3;
					int num13;
					if (list4[0].verticeIndex == this.edges[secondEdgesFromTheLast].x)
					{
						vector3 = this.vertices[this.edges[secondEdgesFromTheLast].y] - this.vertices[this.edges[secondEdgesFromTheLast].x];
						num13 = this.edges[secondEdgesFromTheLast].y;
					}
					else
					{
						vector3 = this.vertices[this.edges[secondEdgesFromTheLast].x] - this.vertices[this.edges[secondEdgesFromTheLast].y];
						num13 = this.edges[secondEdgesFromTheLast].x;
					}
					Vector2 vector4 = this.vertices[list4[0].verticeIndex] - this.vertices[this.closestCenterPointIndex];
					float num14 = Vector2.Dot(vector4.normalized, vector2.normalized);
					float num15 = Vector2.Dot(vector4.normalized, vector3.normalized);
					if (num14 + num15 <= 0f)
					{
						list4.RemoveAt(0);
					}
					else
					{
						this.edges.Add(new int2(num12, num13));
						MirrorBreakEffect.Triangle triangle2 = new MirrorBreakEffect.Triangle();
						this.SetEdge(0, firstEdgesFromTheLast, ref triangle2);
						this.SetEdge(1, this.edges.Count - 1, ref triangle2);
						this.SetEdge(2, secondEdgesFromTheLast, ref triangle2);
						Vector2 first = this.vertices[num12] - this.vertices[list4[0].verticeIndex];
						Vector2 second = this.vertices[num13] - this.vertices[list4[0].verticeIndex];
						if (this.CrossProduct(first, second) < 0f)
						{
							triangle2.vertices.x = list4[0].verticeIndex;
							triangle2.vertices.y = num12;
							triangle2.vertices.z = num13;
						}
						else
						{
							triangle2.vertices.x = list4[0].verticeIndex;
							triangle2.vertices.y = num13;
							triangle2.vertices.z = num12;
						}
						this.triangles.Add(triangle2);
						this.convexHullTriangles = this.triangles.Count - 1;
						MirrorBreakEffect.VerticesWithAngle item2 = default(MirrorBreakEffect.VerticesWithAngle);
						MirrorBreakEffect.VerticesWithAngle item3;
						item3.verticeIndex = num12;
						item2.verticeIndex = num13;
						item3.verticeAngle = (item2.verticeAngle = 0f);
						list4.Add(item3);
						list4.Add(item2);
						list5.Add(firstEdgesFromTheLast);
						list5.Add(secondEdgesFromTheLast);
						this.convexHullPointsIndex.Remove(list4[0].verticeIndex);
						list4.RemoveAt(0);
					}
				}
			}
			List<MirrorBreakEffect.VerticesWithAngle> list6 = new List<MirrorBreakEffect.VerticesWithAngle>();
			List<MirrorBreakEffect.VerticesWithAngle> list7 = new List<MirrorBreakEffect.VerticesWithAngle>();
			for (int num16 = 0; num16 < this.convexHullPointsIndex.Count; num16++)
			{
				MirrorBreakEffect.VerticesWithAngle item4 = default(MirrorBreakEffect.VerticesWithAngle);
				item4.verticeIndex = this.convexHullPointsIndex[num16];
				Vector2 vector5 = this.vertices[this.convexHullPointsIndex[num16]] - this.vertices[this.closestCenterPointIndex];
				item4.verticeAngle = Mathf.Atan2(vector5.y, vector5.x);
				list6.Add(item4);
			}
			for (int num17 = (int)this.circleVerticesRatio; num17 < this.vertices.Length; num17++)
			{
				MirrorBreakEffect.VerticesWithAngle item5 = default(MirrorBreakEffect.VerticesWithAngle);
				item5.verticeIndex = num17;
				Vector2 vector6 = this.vertices[num17] - this.vertices[this.closestCenterPointIndex];
				item5.verticeAngle = Mathf.Atan2(vector6.y, vector6.x);
				list7.Add(item5);
			}
			List<MirrorBreakEffect.VerticesWithAngle> list8 = (from x in list6
			orderby x.verticeAngle descending
			select x).ToList<MirrorBreakEffect.VerticesWithAngle>();
			List<MirrorBreakEffect.VerticesWithAngle> list9 = (from x in list7
			orderby x.verticeAngle descending
			select x).ToList<MirrorBreakEffect.VerticesWithAngle>();
			this.insideConvexHullEdgeNum = this.edges.Count - 1;
			int count2 = this.triangles.Count;
			List<MirrorBreakEffect.ConvexHullLinkedVertices> list10 = new List<MirrorBreakEffect.ConvexHullLinkedVertices>();
			for (int num18 = 0; num18 < list8.Count; num18++)
			{
				float num20;
				if (num18 == 0)
				{
					float num19 = (3.1415927f + list8[list8.Count - 1].verticeAngle) * 0.5f + (3.1415927f - list8[num18].verticeAngle) * 0.5f;
					num20 = list8[list8.Count - 1].verticeAngle - num19;
					if (num20 <= -3.1415927f)
					{
						num20 = list8[num18].verticeAngle + num19;
					}
				}
				else
				{
					num20 = list8[num18 - 1].verticeAngle * 0.5f + list8[num18].verticeAngle * 0.5f;
				}
				float num22;
				if (num18 == list8.Count - 1)
				{
					float num21 = (3.1415927f + list8[num18].verticeAngle) * 0.5f + (3.1415927f - list8[0].verticeAngle) * 0.5f;
					num22 = list8[0].verticeAngle + num21;
					if (num22 >= 3.1415927f)
					{
						num22 = list8[num18].verticeAngle - num21;
					}
				}
				else
				{
					num22 = list8[num18].verticeAngle * 0.5f + list8[num18 + 1].verticeAngle * 0.5f;
				}
				MirrorBreakEffect.ConvexHullLinkedVertices convexHullLinkedVertices = new MirrorBreakEffect.ConvexHullLinkedVertices();
				convexHullLinkedVertices.convexHullverticeIndex = list8[num18].verticeIndex;
				convexHullLinkedVertices.linkedVerticesWithAngles = new List<MirrorBreakEffect.VerticesWithAngle>();
				MirrorBreakEffect.VerticesWithAngle item6 = default(MirrorBreakEffect.VerticesWithAngle);
				for (int num23 = 0; num23 < list9.Count; num23++)
				{
					if (num20 * num22 < 0f)
					{
						if (num18 == 0 && num20 < -1f)
						{
							if (num20 >= list9[num23].verticeAngle && list9[num23].verticeAngle > -3.1415927f)
							{
								this.edges.Add(new int2(list8[num18].verticeIndex, list9[num23].verticeIndex));
								item6.verticeIndex = list9[num23].verticeIndex;
								Vector2 vector7 = this.vertices[list9[num23].verticeIndex] - this.vertices[list8[num18].verticeIndex] - this.vertices[this.closestCenterPointIndex];
								item6.verticeAngle = Mathf.Atan2(vector7.y, vector7.x);
								convexHullLinkedVertices.linkedVerticesWithAngles.Add(item6);
							}
							if (3.1415927f >= list9[num23].verticeAngle && list9[num23].verticeAngle > num22)
							{
								this.edges.Add(new int2(list8[num18].verticeIndex, list9[num23].verticeIndex));
								item6.verticeIndex = list9[num23].verticeIndex;
								Vector2 vector8 = this.vertices[list9[num23].verticeIndex] - this.vertices[list8[num18].verticeIndex] - this.vertices[this.closestCenterPointIndex];
								item6.verticeAngle = Mathf.Atan2(vector8.y, vector8.x);
								convexHullLinkedVertices.linkedVerticesWithAngles.Add(item6);
							}
						}
						else if (num18 == list8.Count - 1 && num22 > 0f)
						{
							if (3.1415927f >= list9[num23].verticeAngle && list9[num23].verticeAngle > num22)
							{
								this.edges.Add(new int2(list8[num18].verticeIndex, list9[num23].verticeIndex));
								item6.verticeIndex = list9[num23].verticeIndex;
								Vector2 vector9 = this.vertices[list9[num23].verticeIndex] - this.vertices[list8[num18].verticeIndex] - this.vertices[this.closestCenterPointIndex];
								item6.verticeAngle = Mathf.Atan2(vector9.y, vector9.x);
								convexHullLinkedVertices.linkedVerticesWithAngles.Add(item6);
							}
							if (num20 >= list9[num23].verticeAngle && list9[num23].verticeAngle > -3.1415927f)
							{
								this.edges.Add(new int2(list8[num18].verticeIndex, list9[num23].verticeIndex));
								item6.verticeIndex = list9[num23].verticeIndex;
								Vector2 vector10 = this.vertices[list9[num23].verticeIndex] - this.vertices[list8[num18].verticeIndex] - this.vertices[this.closestCenterPointIndex];
								item6.verticeAngle = Mathf.Atan2(vector10.y, vector10.x);
								convexHullLinkedVertices.linkedVerticesWithAngles.Add(item6);
							}
						}
						else if (num22 < 0f)
						{
							if (num20 >= list9[num23].verticeAngle && list9[num23].verticeAngle > 0f)
							{
								this.edges.Add(new int2(list8[num18].verticeIndex, list9[num23].verticeIndex));
								item6.verticeIndex = list9[num23].verticeIndex;
								Vector2 vector11 = this.vertices[list9[num23].verticeIndex] - this.vertices[list8[num18].verticeIndex] - this.vertices[this.closestCenterPointIndex];
								item6.verticeAngle = Mathf.Atan2(vector11.y, vector11.x);
								convexHullLinkedVertices.linkedVerticesWithAngles.Add(item6);
							}
							if (0f >= list9[num23].verticeAngle && list9[num23].verticeAngle > num22)
							{
								this.edges.Add(new int2(list8[num18].verticeIndex, list9[num23].verticeIndex));
								item6.verticeIndex = list9[num23].verticeIndex;
								Vector2 vector12 = this.vertices[list9[num23].verticeIndex] - this.vertices[list8[num18].verticeIndex] - this.vertices[this.closestCenterPointIndex];
								item6.verticeAngle = Mathf.Atan2(vector12.y, vector12.x);
								convexHullLinkedVertices.linkedVerticesWithAngles.Add(item6);
							}
						}
					}
					else if (num20 >= list9[num23].verticeAngle && list9[num23].verticeAngle > num22)
					{
						this.edges.Add(new int2(list8[num18].verticeIndex, list9[num23].verticeIndex));
						item6.verticeIndex = list9[num23].verticeIndex;
						Vector2 vector13 = this.vertices[list9[num23].verticeIndex] - this.vertices[list8[num18].verticeIndex] - this.vertices[this.closestCenterPointIndex];
						item6.verticeAngle = Mathf.Atan2(vector13.y, vector13.x);
						convexHullLinkedVertices.linkedVerticesWithAngles.Add(item6);
					}
				}
				list10.Add(convexHullLinkedVertices);
			}
			this.convexHullEdgeToOutterCircleVertice = this.edges.Count - 1;
			for (int num24 = 0; num24 < list10.Count; num24++)
			{
				Vector2 vector14 = this.vertices[list8[num24].verticeIndex] - this.vertices[this.closestCenterPointIndex];
				float num25 = Mathf.Atan2(vector14.y, vector14.x);
				string.Concat(new string[]
				{
					"convexHullverticeIndex : [",
					list10[num24].convexHullverticeIndex.ToString(),
					"    angle : ",
					num25.ToString(),
					"]"
				});
				List<MirrorBreakEffect.VerticesWithAngle> list11 = (from x in list10[num24].linkedVerticesWithAngles
				orderby x.verticeAngle descending
				select x).ToList<MirrorBreakEffect.VerticesWithAngle>();
				for (int num26 = 0; num26 < list11.Count; num26++)
				{
					if (Mathf.Abs(list11[num26].verticeAngle - num25) > 1.5f)
					{
						MirrorBreakEffect.VerticesWithAngle item7;
						item7.verticeIndex = list11[num26].verticeIndex;
						item7.verticeAngle = list11[num26].verticeAngle;
						if (list11[num26].verticeAngle < 0f)
						{
							item7.verticeAngle = 3.1415927f + list11[num26].verticeAngle + 3.1415927f;
							list11.RemoveAt(num26);
							list11.Insert(0, item7);
							list11 = (from x in list11
							orderby x.verticeAngle descending
							select x).ToList<MirrorBreakEffect.VerticesWithAngle>();
						}
						else
						{
							item7.verticeAngle = -(3.1415927f - list11[num26].verticeAngle) - 3.1415927f;
							list11.RemoveAt(num26);
							list11.Add(item7);
							list11 = (from x in list11
							orderby x.verticeAngle descending
							select x).ToList<MirrorBreakEffect.VerticesWithAngle>();
						}
						num26--;
					}
				}
			}
			int num27 = -1;
			int num28 = -1;
			int num29 = -1;
			int num30 = -1;
			int yIndex = -1;
			bool flag = false;
			List<int> list12 = new List<int>();
			List<int> list13 = new List<int>();
			for (int num31 = 0; num31 < list10.Count; num31++)
			{
				Vector2 vector15 = this.vertices[list8[num31].verticeIndex] - this.vertices[this.closestCenterPointIndex];
				float num32 = Mathf.Atan2(vector15.y, vector15.x);
				List<MirrorBreakEffect.VerticesWithAngle> list14 = (from x in list10[num31].linkedVerticesWithAngles
				orderby x.verticeAngle descending
				select x).ToList<MirrorBreakEffect.VerticesWithAngle>();
				for (int num33 = 0; num33 < list14.Count; num33++)
				{
					if (Mathf.Abs(list14[num33].verticeAngle - num32) > 1.5f)
					{
						MirrorBreakEffect.VerticesWithAngle item8;
						item8.verticeIndex = list14[num33].verticeIndex;
						item8.verticeAngle = list14[num33].verticeAngle;
						if (list14[num33].verticeAngle < 0f)
						{
							item8.verticeAngle = 3.1415927f + list14[num33].verticeAngle + 3.1415927f;
							list14.RemoveAt(num33);
							list14.Insert(0, item8);
							list14 = (from x in list14
							orderby x.verticeAngle descending
							select x).ToList<MirrorBreakEffect.VerticesWithAngle>();
						}
						else
						{
							item8.verticeAngle = -(3.1415927f - list14[num33].verticeAngle) - 3.1415927f;
							list14.RemoveAt(num33);
							list14.Add(item8);
							list14 = (from x in list14
							orderby x.verticeAngle descending
							select x).ToList<MirrorBreakEffect.VerticesWithAngle>();
						}
						num33--;
					}
				}
				if (num30 == -1)
				{
					list12.Add(list10[num31].convexHullverticeIndex);
					list13.Add(list10[num31].convexHullverticeIndex);
				}
				bool flag2 = false;
				if (num28 != -1 && num28 != list10[num31].convexHullverticeIndex)
				{
					list13.Add(list10[num31].convexHullverticeIndex);
					if (list14.Count > 0 && num29 != -1)
					{
						Vector2 vector16 = this.vertices[num29] - this.vertices[list10[num31].convexHullverticeIndex] - this.vertices[this.closestCenterPointIndex];
						if (Mathf.Atan2(vector16.y, vector16.x) > list14[0].verticeAngle)
						{
							for (int num34 = 0; num34 < list13.Count; num34++)
							{
								this.EdgeContainsCheck(new int2(list13[num34], num29));
								if (num34 == 0)
								{
									this.AddTriangle(num29, list13[num34], num28);
								}
								else
								{
									this.AddTriangle(num29, list13[num34], list13[num34 - 1]);
								}
								this.convexHullToDeadVerticeTriangles = this.triangles.Count - 1;
							}
						}
						else
						{
							for (int num35 = list13.Count - 1; num35 >= 0; num35--)
							{
								this.EdgeContainsCheck(new int2(list13[num35], list14[0].verticeIndex));
								flag2 = true;
								if (num35 == list13.Count - 1)
								{
									this.AddTriangle(list14[0].verticeIndex, num28, list13[num35]);
								}
								else
								{
									this.AddTriangle(list14[0].verticeIndex, list13[num35], list13[num35 + 1]);
								}
								this.convexHullToDeadVerticeTriangles = this.triangles.Count - 1;
							}
						}
					}
					else if (list14.Count > 0 && num29 == -1 && num31 - 1 >= 0 && list10[num31 - 1].linkedVerticesWithAngles.Count > 0)
					{
						this.EdgeContainsCheck(new int2(list10[num31 - 1].linkedVerticesWithAngles[0].verticeIndex, list10[num31].convexHullverticeIndex));
						this.AddTriangle(list10[num31 - 1].convexHullverticeIndex, list10[num31 - 1].linkedVerticesWithAngles[0].verticeIndex, list10[num31].convexHullverticeIndex);
						this.convexHullToDeadVerticeTriangles = this.triangles.Count - 1;
					}
				}
				for (int num36 = 0; num36 < list14.Count; num36++)
				{
					if (num27 == -1)
					{
						num27 = list14[num36].verticeIndex;
						num28 = list14[num36].verticeIndex;
						if (num30 == -1)
						{
							num30 = list14[num36].verticeIndex;
							yIndex = list10[num31].convexHullverticeIndex;
						}
					}
					else if (num27 != list14[num36].verticeIndex)
					{
						this.EdgeContainsCheck(new int2(num27, list14[num36].verticeIndex));
						if (flag2 && num31 - 1 >= 0 && list13.Count > 0)
						{
							this.AddTriangle(list13[0], num27, list14[num36].verticeIndex);
							this.convexHullToDeadVerticeTriangles = this.triangles.Count - 1;
							flag2 = false;
						}
						if (this.triangles.Count == count2 && list13.Count > 0)
						{
							for (int num37 = 0; num37 < list13.Count; num37++)
							{
								this.EdgeContainsCheck(new int2(num27, list13[num37]));
								if (num37 == 0)
								{
									this.AddTriangle(num27, list10[num31 - (list13.Count - 1)].convexHullverticeIndex, list13[num37]);
								}
								else
								{
									this.AddTriangle(num27, list13[num37], list13[num37 - 1]);
								}
							}
						}
						this.AddTriangle(list10[num31].convexHullverticeIndex, num27, list14[num36].verticeIndex);
						this.convexHullToDeadVerticeTriangles = this.triangles.Count - 1;
						num27 = list14[num36].verticeIndex;
						num28 = list10[num31].convexHullverticeIndex;
						num29 = list14[num36].verticeIndex;
						list13.Clear();
						list13.Add(list10[num31].convexHullverticeIndex);
						if (!flag)
						{
							flag = true;
							for (int num38 = 0; num38 < list13.Count - 1; num38++)
							{
								this.EdgeContainsCheck(new int2(num29, list13[num38]));
								if (num36 == list13.Count - 1)
								{
									this.AddTriangle(num29, num28, list13[num36]);
								}
								else if (list13.Count >= 2 && num36 >= 1)
								{
									this.AddTriangle(num29, list13[num36], list13[num36 + 1]);
								}
								this.convexHullToDeadVerticeTriangles = this.triangles.Count - 1;
							}
						}
					}
				}
				if (num31 == 0 && num28 == -1 && list10[num31].linkedVerticesWithAngles.Count != 0)
				{
					num28 = list10[num31].convexHullverticeIndex;
					num27 = list10[num31].linkedVerticesWithAngles[0].verticeIndex;
				}
				if (num31 == list10.Count - 1)
				{
					for (int num39 = list12.Count - 1; num39 >= 0; num39--)
					{
						this.EdgeContainsCheck(new int2(num30, list12[num39]));
						if (num39 == list12.Count - 1)
						{
							this.AddTriangle(num30, yIndex, list12[num39]);
						}
						else if (list12.Count >= 2 && num39 < list12.Count - 1)
						{
							this.AddTriangle(num30, list12[num39 + 1], list12[num39]);
						}
						this.convexHullToDeadVerticeTriangles = this.triangles.Count - 1;
					}
					for (int num40 = list13.Count - 1; num40 >= 0; num40--)
					{
						this.EdgeContainsCheck(new int2(num30, list13[num40]));
						if (list12.Count > 0)
						{
							if (num40 == list13.Count - 1)
							{
								this.AddTriangle(num30, list12[0], list13[num40]);
							}
							else if (list13.Count >= 2)
							{
								this.AddTriangle(num30, list13[num40 + 1], list13[num40]);
							}
						}
						else
						{
							if (num40 == list13.Count - 1)
							{
								this.AddTriangle(num30, yIndex, list13[num40]);
							}
							else if (list13.Count >= 2)
							{
								this.AddTriangle(num30, list13[num40 + 1], list13[num40]);
							}
							this.convexHullToDeadVerticeTriangles = this.triangles.Count - 1;
						}
					}
					this.EdgeContainsCheck(new int2(num29, num30));
					if (list13.Count > 0)
					{
						this.AddTriangle(num29, num30, list13[0]);
					}
					else if (list10.Count > num31)
					{
						this.AddTriangle(num29, num30, list10[num31].convexHullverticeIndex);
					}
					this.convexHullToDeadVerticeTriangles = this.triangles.Count - 1;
				}
				num11++;
				if (num11 > 500)
				{
					break;
				}
			}
			this.outterCircleBranchVerticeLink = this.edges.Count - 1;
			this.FindOutterTriangleCandidateVertices();
			this.IsEdgeCreated = true;
		}

		// Token: 0x06000C64 RID: 3172 RVA: 0x00024EB8 File Offset: 0x000230B8
		private void EdgeContainsCheck(int edgeNumber, ref int2 edge, ref MirrorBreakEffect.Triangle triangle)
		{
			for (int i = 0; i < this.edges.Count; i++)
			{
				if (this.edges[i].x == edge.x && this.edges[i].y == edge.y)
				{
					this.SetEdge(edgeNumber, i, ref triangle);
					return;
				}
				if (this.edges[i].x == edge.y && this.edges[i].y == edge.x)
				{
					this.SetEdge(edgeNumber, i, ref triangle);
					return;
				}
			}
			this.edges.Add(edge);
			this.SetEdge(edgeNumber, this.edges.Count - 1, ref triangle);
		}

		// Token: 0x06000C65 RID: 3173 RVA: 0x00024F7C File Offset: 0x0002317C
		private bool EdgeContainsCheck(int2 edge)
		{
			for (int i = 0; i < this.edges.Count; i++)
			{
				if (this.edges[i].x == edge.x && this.edges[i].y == edge.y)
				{
					return true;
				}
				if (this.edges[i].x == edge.y && this.edges[i].y == edge.x)
				{
					return true;
				}
			}
			this.edges.Add(edge);
			return false;
		}

		// Token: 0x06000C66 RID: 3174 RVA: 0x00025014 File Offset: 0x00023214
		private bool EdgeContainsCheck(int2 edge, int x, int y, int z)
		{
			for (int i = 0; i < this.edges.Count; i++)
			{
				if (this.edges[i].x == edge.x && this.edges[i].y == edge.y)
				{
					return true;
				}
				if (this.edges[i].x == edge.y && this.edges[i].y == edge.x)
				{
					return true;
				}
			}
			this.edges.Add(edge);
			MirrorBreakEffect.Triangle triangle = new MirrorBreakEffect.Triangle();
			triangle.vertices.x = x;
			triangle.vertices.y = y;
			triangle.vertices.z = z;
			triangle.edges.x = this.GetEdgeIndex(x, y);
			triangle.edges.y = this.GetEdgeIndex(y, z);
			triangle.edges.z = this.GetEdgeIndex(z, x);
			this.triangles.Add(triangle);
			return false;
		}

		// Token: 0x06000C67 RID: 3175 RVA: 0x00025120 File Offset: 0x00023320
		private int GetEdgeIndex(int x, int y)
		{
			for (int i = 0; i < this.edges.Count; i++)
			{
				if (this.edges[i].x == x && this.edges[i].y == y)
				{
					return i;
				}
				if (this.edges[i].y == x && this.edges[i].x == y)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06000C68 RID: 3176 RVA: 0x00025198 File Offset: 0x00023398
		private void SetEdge(int edgeNumber, int edgeIndex, ref MirrorBreakEffect.Triangle triangle)
		{
			switch (edgeNumber)
			{
			case 0:
				triangle.edges.x = edgeIndex;
				return;
			case 1:
				triangle.edges.y = edgeIndex;
				return;
			case 2:
				triangle.edges.z = edgeIndex;
				return;
			default:
				return;
			}
		}

		// Token: 0x06000C69 RID: 3177 RVA: 0x000251D8 File Offset: 0x000233D8
		private int GetFirstEdgesFromTheLast(int pointIndex, int closestCenterPointIndex, List<int> deadEdgeInConvexHull)
		{
			for (int i = this.edges.Count - 1; i >= 0; i--)
			{
				bool flag = false;
				for (int j = 0; j < deadEdgeInConvexHull.Count; j++)
				{
					if (i == deadEdgeInConvexHull[j])
					{
						flag = true;
						break;
					}
				}
				if (!flag && this.edges[i].x != closestCenterPointIndex && this.edges[i].y != closestCenterPointIndex && (this.edges[i].x == pointIndex || this.edges[i].y == pointIndex))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06000C6A RID: 3178 RVA: 0x00025274 File Offset: 0x00023474
		private int GetSecondEdgesFromTheLast(int pointIndex, int closestCenterPointIndex, List<int> deadEdgeInConvexHull)
		{
			int num = 0;
			for (int i = this.edges.Count - 1; i >= 0; i--)
			{
				bool flag = false;
				for (int j = 0; j < deadEdgeInConvexHull.Count; j++)
				{
					if (i == deadEdgeInConvexHull[j])
					{
						flag = true;
						break;
					}
				}
				if (!flag && this.edges[i].x != closestCenterPointIndex && this.edges[i].y != closestCenterPointIndex && (this.edges[i].x == pointIndex || this.edges[i].y == pointIndex))
				{
					num++;
					if (num == 2)
					{
						return i;
					}
				}
			}
			return -1;
		}

		// Token: 0x06000C6B RID: 3179 RVA: 0x00025320 File Offset: 0x00023520
		private void FindOutterTriangleCandidateVertices()
		{
			List<MirrorBreakEffect.OutterVerticesCandidate> list = new List<MirrorBreakEffect.OutterVerticesCandidate>();
			int num = (int)this.circleVerticesRatio;
			while ((float)num < this.outerVerticesRatio)
			{
				list.Add(new MirrorBreakEffect.OutterVerticesCandidate
				{
					verticeIndex = num,
					relateEdge = new List<int>()
				});
				num++;
			}
			for (int i = 0; i < list.Count; i++)
			{
				List<int> allRelateEdges = this.GetAllRelateEdges(list[i].verticeIndex);
				if (allRelateEdges.Count <= 0)
				{
					list.RemoveAt(i);
					i--;
				}
				else
				{
					for (int j = 0; j < allRelateEdges.Count; j++)
					{
						list[i].relateEdge.Add(allRelateEdges[j]);
					}
				}
			}
			int num2 = 0;
			while (list.Count > 0)
			{
				if (list[0].relateEdge.Count == 2)
				{
					int num3 = 0;
					for (int k = 0; k < list[0].relateEdge.Count; k++)
					{
						if ((float)this.edges[list[0].relateEdge[k]].x >= this.outerVerticesRatio || (float)this.edges[list[0].relateEdge[k]].y >= this.outerVerticesRatio)
						{
							num3++;
						}
					}
					int num4 = list[0].relateEdge[0];
					int num5 = list[0].relateEdge[1];
					if (num3 == 1)
					{
						Vector2 vector;
						int num6;
						if (list[0].verticeIndex == this.edges[num4].x)
						{
							vector = this.vertices[this.edges[num4].y] - this.vertices[this.edges[num4].x];
							num6 = this.edges[num4].y;
						}
						else
						{
							vector = this.vertices[this.edges[num4].x] - this.vertices[this.edges[num4].y];
							num6 = this.edges[num4].x;
						}
						Vector2 vector2;
						int num7;
						if (list[0].verticeIndex == this.edges[num5].x)
						{
							vector2 = this.vertices[this.edges[num5].y] - this.vertices[this.edges[num5].x];
							num7 = this.edges[num5].y;
						}
						else
						{
							vector2 = this.vertices[this.edges[num5].x] - this.vertices[this.edges[num5].y];
							num7 = this.edges[num5].x;
						}
						Vector2 vector3 = this.vertices[list[0].verticeIndex] - this.vertices[this.closestCenterPointIndex];
						float num8 = Vector2.Dot(vector3.normalized, vector.normalized);
						float num9 = Vector2.Dot(vector3.normalized, vector2.normalized);
						if (num8 + num9 <= 0.099f)
						{
							list.RemoveAt(0);
						}
						else
						{
							this.edges.Add(new int2(num6, num7));
							Vector2 first = this.vertices[num6] - this.vertices[list[0].verticeIndex];
							Vector2 second = this.vertices[num7] - this.vertices[list[0].verticeIndex];
							if (this.CrossProduct(first, second) < 0f)
							{
								this.AddTriangle(list[0].verticeIndex, num6, num7);
							}
							else
							{
								this.AddTriangle(list[0].verticeIndex, num7, num6);
							}
							this.deadEdgeOutterCircle.Add(num4);
							this.deadEdgeOutterCircle.Add(num5);
							MirrorBreakEffect.OutterVerticesCandidate outterVerticesCandidate = default(MirrorBreakEffect.OutterVerticesCandidate);
							if ((float)num6 >= this.outerVerticesRatio)
							{
								outterVerticesCandidate.verticeIndex = num7;
							}
							else
							{
								outterVerticesCandidate.verticeIndex = num6;
							}
							outterVerticesCandidate.relateEdge = this.GetAllRelateEdges(outterVerticesCandidate.verticeIndex);
							for (int l = 0; l < list.Count; l++)
							{
								if (list[l].verticeIndex == outterVerticesCandidate.verticeIndex)
								{
									list.RemoveAt(l);
								}
							}
							list.Add(outterVerticesCandidate);
							list.RemoveAt(0);
						}
					}
					else
					{
						Vector2 vector4;
						int num10;
						if (list[0].verticeIndex == this.edges[num4].x)
						{
							vector4 = this.vertices[this.edges[num4].y] - this.vertices[this.edges[num4].x];
							num10 = this.edges[num4].y;
						}
						else
						{
							vector4 = this.vertices[this.edges[num4].x] - this.vertices[this.edges[num4].y];
							num10 = this.edges[num4].x;
						}
						Vector2 vector5;
						int num11;
						if (list[0].verticeIndex == this.edges[num5].x)
						{
							vector5 = this.vertices[this.edges[num5].y] - this.vertices[this.edges[num5].x];
							num11 = this.edges[num5].y;
						}
						else
						{
							vector5 = this.vertices[this.edges[num5].x] - this.vertices[this.edges[num5].y];
							num11 = this.edges[num5].x;
						}
						Vector2 vector6 = this.vertices[list[0].verticeIndex] - this.vertices[this.closestCenterPointIndex];
						float num12 = Vector2.Dot(vector6.normalized, vector4.normalized);
						float num13 = Vector2.Dot(vector6.normalized, vector5.normalized);
						if (num12 + num13 <= 0.099f)
						{
							list.RemoveAt(0);
						}
						else
						{
							this.edges.Add(new int2(num10, num11));
							Vector2 first2 = this.vertices[num10] - this.vertices[list[0].verticeIndex];
							Vector2 second2 = this.vertices[num11] - this.vertices[list[0].verticeIndex];
							if (this.CrossProduct(first2, second2) < 0f)
							{
								this.AddTriangle(list[0].verticeIndex, num10, num11);
							}
							else
							{
								this.AddTriangle(list[0].verticeIndex, num11, num10);
							}
							this.deadEdgeOutterCircle.Add(num4);
							this.deadEdgeOutterCircle.Add(num5);
							MirrorBreakEffect.OutterVerticesCandidate outterVerticesCandidate2 = default(MirrorBreakEffect.OutterVerticesCandidate);
							outterVerticesCandidate2.verticeIndex = num10;
							outterVerticesCandidate2.relateEdge = this.GetAllRelateEdges(outterVerticesCandidate2.verticeIndex);
							for (int m = 0; m < list.Count; m++)
							{
								if (list[m].verticeIndex == outterVerticesCandidate2.verticeIndex)
								{
									list.RemoveAt(m);
								}
							}
							list.Add(outterVerticesCandidate2);
							outterVerticesCandidate2 = default(MirrorBreakEffect.OutterVerticesCandidate);
							outterVerticesCandidate2.verticeIndex = num11;
							outterVerticesCandidate2.relateEdge = this.GetAllRelateEdges(outterVerticesCandidate2.verticeIndex);
							for (int n = 0; n < list.Count; n++)
							{
								if (list[n].verticeIndex == outterVerticesCandidate2.verticeIndex)
								{
									list.RemoveAt(n);
								}
							}
							list.Add(outterVerticesCandidate2);
							list.RemoveAt(0);
						}
					}
				}
				else
				{
					list.RemoveAt(0);
				}
				num2++;
				if (num2 > 100)
				{
					break;
				}
			}
		}

		// Token: 0x06000C6C RID: 3180 RVA: 0x00025BB8 File Offset: 0x00023DB8
		private List<int> GetAllRelateEdges(int verticeIndex)
		{
			List<int> list = new List<int>();
			List<int> list2 = new List<int>();
			for (int i = 0; i < this.edges.Count; i++)
			{
				bool flag = false;
				for (int j = 0; j < this.deadEdgeOutterCircle.Count; j++)
				{
					if (i == this.deadEdgeOutterCircle[j])
					{
						flag = true;
						break;
					}
				}
				if (!flag && (this.edges[i].x == verticeIndex || this.edges[i].y == verticeIndex))
				{
					bool flag2 = false;
					if ((float)this.edges[i].x >= this.outerVerticesRatio)
					{
						list2.Add(this.edges[i].x);
					}
					if ((float)this.edges[i].y >= this.outerVerticesRatio)
					{
						list2.Add(this.edges[i].y);
					}
					for (int k = 0; k < this.convexHullPointsIndex.Count; k++)
					{
						if (this.edges[i].x == this.convexHullPointsIndex[k] || this.edges[i].y == this.convexHullPointsIndex[k])
						{
							flag2 = true;
						}
					}
					if (!flag2)
					{
						list.Add(i);
					}
				}
			}
			if (list2.Count == 2)
			{
				int num = list2[0];
				int num2 = list2[1];
				Vector2 first = this.vertices[num] - this.vertices[verticeIndex];
				Vector2 second = this.vertices[num2] - this.vertices[verticeIndex];
				if (this.CrossProduct(first, second) < 0f)
				{
					this.AddTriangle(verticeIndex, num, num2);
				}
				else
				{
					this.AddTriangle(verticeIndex, num2, num);
				}
				list.Clear();
			}
			return list;
		}

		// Token: 0x06000C6D RID: 3181 RVA: 0x00025DAC File Offset: 0x00023FAC
		public void SetTriangles()
		{
			this.IsEdgeCreated = false;
			this.IsTriangleCreated = true;
			for (int i = 0; i < this.triangles.Count; i++)
			{
			}
		}

		// Token: 0x06000C6E RID: 3182 RVA: 0x00025DDD File Offset: 0x00023FDD
		private float CrossProduct(Vector2 first, Vector2 second)
		{
			return first.x * second.y - first.y * second.x;
		}

		// Token: 0x06000C6F RID: 3183 RVA: 0x00025DFC File Offset: 0x00023FFC
		private void AddTriangle(int xIndex, int yIndex, int zIndex)
		{
			MirrorBreakEffect.Triangle triangle = new MirrorBreakEffect.Triangle();
			int edgeIndex = this.GetEdgeIndex(xIndex, yIndex);
			int edgeIndex2 = this.GetEdgeIndex(yIndex, zIndex);
			int edgeIndex3 = this.GetEdgeIndex(zIndex, xIndex);
			if (edgeIndex == -1 || edgeIndex2 == -1 || edgeIndex3 == -1)
			{
				return;
			}
			new List<int>();
			for (int i = 0; i < this.triangles.Count; i++)
			{
				if ((xIndex == this.triangles[i].vertices.x || xIndex == this.triangles[i].vertices.y || xIndex == this.triangles[i].vertices.z) && (yIndex == this.triangles[i].vertices.x || yIndex == this.triangles[i].vertices.y || yIndex == this.triangles[i].vertices.z) && (zIndex == this.triangles[i].vertices.x || zIndex == this.triangles[i].vertices.y || zIndex == this.triangles[i].vertices.z))
				{
					return;
				}
			}
			triangle.vertices.x = xIndex;
			triangle.vertices.y = yIndex;
			triangle.vertices.z = zIndex;
			this.SetEdge(0, edgeIndex, ref triangle);
			this.SetEdge(1, edgeIndex2, ref triangle);
			this.SetEdge(2, edgeIndex3, ref triangle);
			this.triangles.Add(triangle);
		}

		// Token: 0x06000C70 RID: 3184 RVA: 0x00025F9C File Offset: 0x0002419C
		private int FindPairTriangleWithEdge(int edgeIndex, int triangleIndex)
		{
			int result = -1;
			for (int i = 0; i < this.triangles.Count; i++)
			{
				if (triangleIndex != i && (this.triangles[i].edges.x == edgeIndex || this.triangles[i].edges.y == edgeIndex || this.triangles[i].edges.z == edgeIndex))
				{
					return i;
				}
			}
			return result;
		}

		// Token: 0x06000C71 RID: 3185 RVA: 0x00026014 File Offset: 0x00024214
		public void CreatePairTriangles()
		{
			for (int i = 0; i < this.triangles.Count; i++)
			{
				this.triangles[i].edgePairedTriangle.x = this.FindPairTriangleWithEdge(this.triangles[i].edges.x, i);
				this.triangles[i].edgePairedTriangle.y = this.FindPairTriangleWithEdge(this.triangles[i].edges.y, i);
				this.triangles[i].edgePairedTriangle.z = this.FindPairTriangleWithEdge(this.triangles[i].edges.z, i);
			}
			this.IsTriangleCreated = false;
			this.IsVertexCreated = false;
			this.IsPairedTriangleCreated = true;
		}

		// Token: 0x06000C72 RID: 3186 RVA: 0x000260EC File Offset: 0x000242EC
		public void SettingMeshData()
		{
			this.verticesUV = new float2[this.vertices.Length];
			for (int i = 0; i < this.verticesUV.Length; i++)
			{
				this.verticesUV[i].x = (this.vertices[i].x + this.halfScreenSizeX) / this._screenSize.x;
				this.verticesUV[i].y = (this.vertices[i].y + this.halfScreenSizeY) / this._screenSize.y;
			}
			for (int j = 0; j < this.triangles.Count; j++)
			{
				this.triangles[j].normal = new Vector3(0f, 0f, 1f);
			}
			for (int k = 0; k < this.triangles.Count; k++)
			{
				Vector2 first = this.vertices[this.triangles[k].vertices.y] - this.vertices[this.triangles[k].vertices.x];
				Vector2 second = this.vertices[this.triangles[k].vertices.z] - this.vertices[this.triangles[k].vertices.x];
				this.triangles[k].crossProduct = math.abs(this.CrossProduct(first, second));
				if (k < this.convexHullToDeadVerticeTriangles)
				{
					this.triangles[k].randomProbabilities = this._InnerCircleTriangleRandomProbabilities;
					this.triangles[k].decreaseAmount = this._InnerCircleTriangleRPDecreaseAmount;
				}
				else
				{
					this.triangles[k].randomProbabilities = this._outterCircleTriangleRandomProbabilities;
					this.triangles[k].decreaseAmount = this._outterCircleTriangleRPDecreaseAmount;
				}
			}
			for (int l = 0; l < this.triangles.Count; l++)
			{
				this.triangles[l].triangleIndex = l;
			}
		}

		// Token: 0x06000C73 RID: 3187 RVA: 0x00026328 File Offset: 0x00024528
		public void CreateChunk()
		{
			List<MirrorBreakEffect.Triangle> list = new List<MirrorBreakEffect.Triangle>(this.triangles);
			this.chunks = new List<MirrorBreakEffect.ChunkData>();
			int num = 0;
			while (list.Count > 0)
			{
				MirrorBreakEffect.ChunkData chunkData = new MirrorBreakEffect.ChunkData();
				chunkData.baseTriangleIndex = list[0].triangleIndex;
				chunkData.chunkedTriangleIndexes = new List<int>();
				chunkData.chunkedTriangleIndexes.Add(list[0].triangleIndex);
				bool inside = true;
				if (list[0].triangleIndex > this.convexHullTriangles)
				{
					inside = false;
				}
				list[0].MakeChunk(num, inside, (float)this.convexHullTriangles, this._innerCircleTriangleAreaLevel, this._outterCircleTriangleAreaLevel, this.triangles, ref list, ref chunkData.chunkedTriangleIndexes);
				this.chunks.Add(chunkData);
				num++;
				if (num > 90)
				{
					break;
				}
			}
			this.IsPairedTriangleCreated = false;
			this.IsChunkCreated = true;
		}

		// Token: 0x06000C74 RID: 3188 RVA: 0x00026404 File Offset: 0x00024604
		public void CreateMeshes()
		{
			this.IsVertexCreated = false;
			this.IsEdgeCreated = false;
			this.IsTriangleCreated = false;
			this.IsPairedTriangleCreated = false;
			this.IsChunkCreated = false;
			this.rootObject = new GameObject();
			this.rootObject.name = "MirroBreak";
			this.rootObject.transform.SetParent(Map.Instance.transform);
			this.rootObject.SetActive(false);
			this.chunkObject = new List<GameObject>();
			for (int i = 0; i < this.chunks.Count; i++)
			{
				GameObject gameObject = new GameObject();
				gameObject.name = "chunks" + i.ToString();
				gameObject.transform.parent = this.rootObject.transform;
				Mesh mesh = gameObject.AddComponent<MeshFilter>().mesh = new Mesh();
				MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
				meshRenderer.material = this._material;
				meshRenderer.sortingLayerName = "UI";
				meshRenderer.sortingOrder = 11;
				Vector2 vector = default(Vector2);
				Vector3[] array = new Vector3[3 * this.chunks[i].chunkedTriangleIndexes.Count * 2 * 2];
				int[] array2 = new int[24 * this.chunks[i].chunkedTriangleIndexes.Count];
				Vector2[] array3 = new Vector2[3 * this.chunks[i].chunkedTriangleIndexes.Count * 2 * 2];
				Vector2[] array4 = new Vector2[3 * this.chunks[i].chunkedTriangleIndexes.Count * 2 * 2];
				Vector3[] array5 = new Vector3[3 * this.chunks[i].chunkedTriangleIndexes.Count * 2 * 2];
				for (int j = 0; j < this.chunks[i].chunkedTriangleIndexes.Count; j++)
				{
					vector += this.vertices[this.triangles[this.chunks[i].chunkedTriangleIndexes[j]].vertices.x];
					vector += this.vertices[this.triangles[this.chunks[i].chunkedTriangleIndexes[j]].vertices.y];
					vector += this.vertices[this.triangles[this.chunks[i].chunkedTriangleIndexes[j]].vertices.z];
				}
				vector /= (float)(3 * this.chunks[i].chunkedTriangleIndexes.Count);
				Vector2 a = vector;
				vector += new Vector2(base.transform.position.x, base.transform.position.y);
				gameObject.transform.position = vector;
				if (i < MirrorBreakEffect.chunkInsideIndex)
				{
					this.chunks[i].chunkBasePosition = vector;
					this.chunks[i].rotationSpeed = (float)UnityEngine.Random.Range(50, 200);
					this.chunks[i].rotationVector = (a - this._centerPoint).normalized + new Vector3(0f, 0f, base.transform.position.z);
					if (UnityEngine.Random.Range(0, 2) == 0)
					{
						this.chunks[i].rotationVector = -this.chunks[i].rotationVector;
					}
					this.chunks[i].InitialRotationDisplacement = UnityEngine.Random.Range(-this._displacement_rotation, this._displacement_rotation);
					this.chunks[i].moveSpeed = UnityEngine.Random.Range(0.8f, 1.5f);
					this.chunks[i].moveDirection = (a - this._centerPoint).normalized;
					this.chunks[i].moveDirection += new Vector3(UnityEngine.Random.Range(0f, this.chunks[i].moveDirection.x), UnityEngine.Random.Range(0f, this.chunks[i].moveDirection.y), UnityEngine.Random.Range(5f, 10f));
					this.chunks[i].scale = UnityEngine.Random.Range(0.01f, 0.5f);
				}
				else
				{
					this.chunks[i].chunkBasePosition = vector;
					this.chunks[i].rotationSpeed = (float)UnityEngine.Random.Range(50, 200);
					this.chunks[i].rotationVector = (a - this._centerPoint).normalized + new Vector3(0f, 0f, base.transform.position.z);
					if (UnityEngine.Random.Range(0, 2) == 0)
					{
						this.chunks[i].rotationVector = -this.chunks[i].rotationVector;
					}
					this.chunks[i].InitialRotationDisplacement = UnityEngine.Random.Range(-this._displacement_rotation, this._displacement_rotation);
					this.chunks[i].moveSpeed = UnityEngine.Random.Range(1f, 2f);
					this.chunks[i].moveDirection = (a - this._centerPoint).normalized * 1.5f;
					this.chunks[i].moveDirection += new Vector3(UnityEngine.Random.Range(0f, this.chunks[i].moveDirection.x), UnityEngine.Random.Range(0f, this.chunks[i].moveDirection.y), UnityEngine.Random.Range(5f, 10f));
					this.chunks[i].scale = UnityEngine.Random.Range(0.01f, 0.1f);
				}
				float d = UnityEngine.Random.Range(-this._displacement_uv, this._displacement_uv * 0.5f);
				Vector2 b = new Vector2(UnityEngine.Random.Range(-this._displacement_uv * 0.5f, this._displacement_uv * 0.5f), UnityEngine.Random.Range(-this._displacement_uv * 0.5f, this._displacement_uv * 0.5f));
				for (int k = 0; k < this.chunks[i].chunkedTriangleIndexes.Count; k++)
				{
					array[k * 12] = (array[k * 12 + 3] = (array[k * 12 + 6] = (array[k * 12 + 9] = this.vertices[this.triangles[this.chunks[i].chunkedTriangleIndexes[k]].vertices.x] - vector)));
					array[k * 12 + 1] = (array[k * 12 + 4] = (array[k * 12 + 7] = (array[k * 12 + 10] = this.vertices[this.triangles[this.chunks[i].chunkedTriangleIndexes[k]].vertices.y] - vector)));
					array[k * 12 + 2] = (array[k * 12 + 5] = (array[k * 12 + 8] = (array[k * 12 + 11] = this.vertices[this.triangles[this.chunks[i].chunkedTriangleIndexes[k]].vertices.z] - vector)));
					array[k * 12] += array[k * 12].normalized * this._gap;
					array[k * 12 + 1] += array[k * 12 + 1].normalized * this._gap;
					array[k * 12 + 2] += array[k * 12 + 2].normalized * this._gap;
					array[k * 12 + 3] -= array[k * 12 + 3].normalized * this._gap;
					array[k * 12 + 4] -= array[k * 12 + 4].normalized * this._gap;
					array[k * 12 + 5] -= array[k * 12 + 5].normalized * this._gap;
					array[k * 12].z = (array[k * 12 + 1].z = (array[k * 12 + 2].z = this._thickness));
					array[k * 12 + 3].z = (array[k * 12 + 4].z = (array[k * 12 + 5].z = -this._thickness));
					array[k * 12 + 6] = array[k * 12];
					array[k * 12 + 7] = array[k * 12 + 1];
					array[k * 12 + 8] = array[k * 12 + 2];
					array[k * 12 + 9] = array[k * 12 + 3];
					array[k * 12 + 10] = array[k * 12 + 4];
					array[k * 12 + 11] = array[k * 12 + 5];
					array2[k * 24] = k * 12;
					array2[k * 24 + 1] = k * 12 + 1;
					array2[k * 24 + 2] = k * 12 + 2;
					array2[k * 24 + 3] = k * 12 + 3;
					array2[k * 24 + 4] = k * 12 + 4;
					array2[k * 24 + 5] = k * 12 + 5;
					array2[k * 24 + 6] = k * 12 + 7;
					array2[k * 24 + 7] = k * 12 + 6;
					array2[k * 24 + 8] = k * 12 + 9;
					array2[k * 24 + 9] = k * 12 + 9;
					array2[k * 24 + 10] = k * 12 + 6;
					array2[k * 24 + 11] = k * 12 + 10;
					array2[k * 24 + 12] = k * 12 + 8;
					array2[k * 24 + 13] = k * 12 + 7;
					array2[k * 24 + 14] = k * 12 + 11;
					array2[k * 24 + 15] = k * 12 + 11;
					array2[k * 24 + 16] = k * 12 + 7;
					array2[k * 24 + 17] = k * 12 + 10;
					array2[k * 24 + 18] = k * 12 + 6;
					array2[k * 24 + 19] = k * 12 + 8;
					array2[k * 24 + 20] = k * 12 + 9;
					array2[k * 24 + 21] = k * 12 + 9;
					array2[k * 24 + 22] = k * 12 + 8;
					array2[k * 24 + 23] = k * 12 + 11;
					array3[k * 12] = (array3[k * 12 + 3] = this.verticesUV[this.triangles[this.chunks[i].chunkedTriangleIndexes[k]].vertices.x]);
					array3[k * 12 + 1] = (array3[k * 12 + 4] = this.verticesUV[this.triangles[this.chunks[i].chunkedTriangleIndexes[k]].vertices.y]);
					array3[k * 12 + 2] = (array3[k * 12 + 5] = this.verticesUV[this.triangles[this.chunks[i].chunkedTriangleIndexes[k]].vertices.z]);
					array3[k * 12] += array3[k * 12].normalized * d + b;
					array3[k * 12 + 1] += array3[k * 12 + 1].normalized * d + b;
					array3[k * 12 + 2] += array3[k * 12 + 2].normalized * d + b;
					array3[k * 12 + 3] = (array3[k * 12 + 6] = (array3[k * 12 + 9] = array3[k * 12]));
					array3[k * 12 + 4] = (array3[k * 12 + 7] = (array3[k * 12 + 10] = array3[k * 12 + 1]));
					array3[k * 12 + 5] = (array3[k * 12 + 8] = (array3[k * 12 + 11] = array3[k * 12 + 2]));
					array5[k * 12] = (array5[k * 12 + 1] = (array5[k * 12 + 2] = new Vector3(1f, 0f, 0f)));
					array5[k * 12 + 3] = (array5[k * 12 + 4] = (array5[k * 12 + 5] = new Vector3(-1f, 0f, 0f)));
					array5[k * 12 + 6] = (array5[k * 12 + 7] = (array5[k * 12 + 8] = new Vector3(0f, 0f, 0f)));
					array5[k * 12 + 9] = (array5[k * 12 + 10] = (array5[k * 12 + 11] = new Vector3(0f, 0f, 0f)));
					float magnitude = array[k * 12].magnitude;
					float magnitude2 = array[k * 12 + 1].magnitude;
					float magnitude3 = array[k * 12 + 2].magnitude;
					array4[k * 12] = (array4[k * 12 + 1] = (array4[k * 12 + 2] = (array4[k * 12 + 3] = (array4[k * 12 + 4] = (array4[k * 12 + 5] = new Vector2(0f, 0f))))));
					array4[k * 12 + 6] = (array4[k * 12 + 7] = (array4[k * 12 + 8] = new Vector2(1f, 1f)));
					array4[k * 12 + 9] = (array4[k * 12 + 10] = (array4[k * 12 + 11] = new Vector2(1f, 0f)));
				}
				mesh.vertices = array;
				mesh.triangles = array2;
				mesh.uv = array3;
				mesh.uv2 = array4;
				mesh.normals = array5;
				this.chunkObject.Add(gameObject);
			}
			this.IsMeshCreated = true;
			this.rootObject.transform.localScale = new Vector3(this._scaleModifier, this._scaleModifier, 1f);
			if (this._addRootPosition)
			{
				this.rootObject.transform.position = base.transform.position;
			}
		}

		// Token: 0x06000C75 RID: 3189 RVA: 0x00027672 File Offset: 0x00025872
		public void CreateAll()
		{
			this.CreateVertex();
			this.SetEdges();
			this.SetTriangles();
			this.CreatePairTriangles();
			this.SettingMeshData();
			this.CreateChunk();
			this.CreateMeshes();
		}

		// Token: 0x06000C76 RID: 3190 RVA: 0x0002769E File Offset: 0x0002589E
		public void CreateAllSomeCracked()
		{
			this.CreateVertex();
			this.SetEdges();
			this.SetTriangles();
			this.CreatePairTriangles();
			this.SettingMeshData();
			this.CreateChunk();
			this.CreateMeshes();
			this.CrackedSome(4);
		}

		// Token: 0x06000C77 RID: 3191 RVA: 0x000276D4 File Offset: 0x000258D4
		public void Update()
		{
			if (!this.IsInitialized)
			{
				return;
			}
			if (this.IsMeshCreated)
			{
				for (int i = 0; i < this.chunks.Count; i++)
				{
					if (this._time > 5f)
					{
						this._accelator = this._time - 4.99f;
						this._accelator *= this._accelator * this._accelator;
						this._accelator += 1f;
					}
					else
					{
						this._accelator = 1f;
					}
					if (this._addRootPosition)
					{
						this.chunkObject[i].transform.position = this.chunks[i].chunkBasePosition + this.rootObject.transform.position;
					}
					else
					{
						this.chunkObject[i].transform.position = this.chunks[i].chunkBasePosition;
					}
					this.chunkObject[i].transform.position += this.chunks[i].moveDirection * this.chunks[i].moveSpeed * this._time * this._accelator;
					this.chunkObject[i].transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
					this.chunkObject[i].transform.RotateAround(this.chunks[i].chunkBasePosition + this.chunks[i].moveDirection * this.chunks[i].moveSpeed * this._time * this._accelator, this.chunks[i].rotationVector, this.chunks[i].InitialRotationDisplacement + this.chunks[i].rotationSpeed * this._time);
				}
			}
		}

		// Token: 0x06000C78 RID: 3192 RVA: 0x00027907 File Offset: 0x00025B07
		public void ChangeSprite()
		{
			this._material.SetTexture("_MainTex", this._resultSprite);
			this.RestoreMeshes();
		}

		// Token: 0x06000C79 RID: 3193 RVA: 0x00027928 File Offset: 0x00025B28
		public void RestoreMeshes()
		{
			for (int i = 0; i < this.chunks.Count; i++)
			{
				this.chunks[i].InitialRotationDisplacement = 0f;
				Vector2[] array = new Vector2[3 * this.chunks[i].chunkedTriangleIndexes.Count * 2 * 2];
				Vector3[] array2 = new Vector3[3 * this.chunks[i].chunkedTriangleIndexes.Count * 2 * 2];
				Vector2 vector = default(Vector2);
				for (int j = 0; j < this.chunks[i].chunkedTriangleIndexes.Count; j++)
				{
					vector += this.vertices[this.triangles[this.chunks[i].chunkedTriangleIndexes[j]].vertices.x];
					vector += this.vertices[this.triangles[this.chunks[i].chunkedTriangleIndexes[j]].vertices.y];
					vector += this.vertices[this.triangles[this.chunks[i].chunkedTriangleIndexes[j]].vertices.z];
				}
				vector /= (float)(3 * this.chunks[i].chunkedTriangleIndexes.Count);
				for (int k = 0; k < this.chunks[i].chunkedTriangleIndexes.Count; k++)
				{
					array[k * 12] = (array[k * 12 + 3] = this.verticesUV[this.triangles[this.chunks[i].chunkedTriangleIndexes[k]].vertices.x]);
					array[k * 12 + 1] = (array[k * 12 + 4] = this.verticesUV[this.triangles[this.chunks[i].chunkedTriangleIndexes[k]].vertices.y]);
					array[k * 12 + 2] = (array[k * 12 + 5] = this.verticesUV[this.triangles[this.chunks[i].chunkedTriangleIndexes[k]].vertices.z]);
					array[k * 12 + 3] = (array[k * 12 + 6] = (array[k * 12 + 9] = array[k * 12]));
					array[k * 12 + 4] = (array[k * 12 + 7] = (array[k * 12 + 10] = array[k * 12 + 1]));
					array[k * 12 + 5] = (array[k * 12 + 8] = (array[k * 12 + 11] = array[k * 12 + 2]));
					array2[k * 12] = (array2[k * 12 + 3] = (array2[k * 12 + 6] = (array2[k * 12 + 9] = this.vertices[this.triangles[this.chunks[i].chunkedTriangleIndexes[k]].vertices.x] - vector)));
					array2[k * 12 + 1] = (array2[k * 12 + 4] = (array2[k * 12 + 7] = (array2[k * 12 + 10] = this.vertices[this.triangles[this.chunks[i].chunkedTriangleIndexes[k]].vertices.y] - vector)));
					array2[k * 12 + 2] = (array2[k * 12 + 5] = (array2[k * 12 + 8] = (array2[k * 12 + 11] = this.vertices[this.triangles[this.chunks[i].chunkedTriangleIndexes[k]].vertices.z] - vector)));
					array2[k * 12] += array2[k * 12].normalized * this._gap;
					array2[k * 12 + 1] += array2[k * 12 + 1].normalized * this._gap;
					array2[k * 12 + 2] += array2[k * 12 + 2].normalized * this._gap;
					array2[k * 12 + 3] -= array2[k * 12 + 3].normalized * this._gap;
					array2[k * 12 + 4] -= array2[k * 12 + 4].normalized * this._gap;
					array2[k * 12 + 5] -= array2[k * 12 + 5].normalized * this._gap;
					array2[k * 12].z = (array2[k * 12 + 1].z = (array2[k * 12 + 2].z = this._thickness));
					array2[k * 12 + 3].z = (array2[k * 12 + 4].z = (array2[k * 12 + 5].z = -this._thickness));
					array2[k * 12 + 6] = array2[k * 12];
					array2[k * 12 + 7] = array2[k * 12 + 1];
					array2[k * 12 + 8] = array2[k * 12 + 2];
					array2[k * 12 + 9] = array2[k * 12 + 3];
					array2[k * 12 + 10] = array2[k * 12 + 4];
					array2[k * 12 + 11] = array2[k * 12 + 5];
				}
				Mesh mesh = this.chunkObject[i].GetComponent<MeshFilter>().mesh;
				mesh.vertices = array2;
				mesh.uv = array;
			}
		}

		// Token: 0x06000C7A RID: 3194 RVA: 0x000280EC File Offset: 0x000262EC
		public void CrackedSome(int num)
		{
			for (int i = 0; i < this.chunks.Count; i++)
			{
				this.chunks[i].InitialRotationDisplacement = 0f;
				Vector2[] array = new Vector2[3 * this.chunks[i].chunkedTriangleIndexes.Count * 2 * 2];
				Vector3[] array2 = new Vector3[3 * this.chunks[i].chunkedTriangleIndexes.Count * 2 * 2];
				Vector2 vector = default(Vector2);
				for (int j = 0; j < this.chunks[i].chunkedTriangleIndexes.Count; j++)
				{
					vector += this.vertices[this.triangles[this.chunks[i].chunkedTriangleIndexes[j]].vertices.x];
					vector += this.vertices[this.triangles[this.chunks[i].chunkedTriangleIndexes[j]].vertices.y];
					vector += this.vertices[this.triangles[this.chunks[i].chunkedTriangleIndexes[j]].vertices.z];
				}
				vector /= (float)(3 * this.chunks[i].chunkedTriangleIndexes.Count);
				for (int k = 0; k < this.chunks[i].chunkedTriangleIndexes.Count; k++)
				{
					array[k * 12] = (array[k * 12 + 3] = this.verticesUV[this.triangles[this.chunks[i].chunkedTriangleIndexes[k]].vertices.x]);
					array[k * 12 + 1] = (array[k * 12 + 4] = this.verticesUV[this.triangles[this.chunks[i].chunkedTriangleIndexes[k]].vertices.y]);
					array[k * 12 + 2] = (array[k * 12 + 5] = this.verticesUV[this.triangles[this.chunks[i].chunkedTriangleIndexes[k]].vertices.z]);
					array2[k * 12] = (array2[k * 12 + 3] = (array2[k * 12 + 6] = (array2[k * 12 + 9] = this.vertices[this.triangles[this.chunks[i].chunkedTriangleIndexes[k]].vertices.x] - vector)));
					array2[k * 12 + 1] = (array2[k * 12 + 4] = (array2[k * 12 + 7] = (array2[k * 12 + 10] = this.vertices[this.triangles[this.chunks[i].chunkedTriangleIndexes[k]].vertices.y] - vector)));
					array2[k * 12 + 2] = (array2[k * 12 + 5] = (array2[k * 12 + 8] = (array2[k * 12 + 11] = this.vertices[this.triangles[this.chunks[i].chunkedTriangleIndexes[k]].vertices.z] - vector)));
					if (i % num == 0)
					{
						array2[k * 12] += array2[k * 12].normalized * this._gap;
						array2[k * 12 + 1] += array2[k * 12 + 1].normalized * this._gap;
						array2[k * 12 + 2] += array2[k * 12 + 2].normalized * this._gap;
						array2[k * 12 + 3] -= array2[k * 12 + 3].normalized * this._gap;
						array2[k * 12 + 4] -= array2[k * 12 + 4].normalized * this._gap;
						array2[k * 12 + 5] -= array2[k * 12 + 5].normalized * this._gap;
					}
					array2[k * 12].z = (array2[k * 12 + 1].z = (array2[k * 12 + 2].z = this._thickness));
					array2[k * 12 + 3].z = (array2[k * 12 + 4].z = (array2[k * 12 + 5].z = -this._thickness));
					array2[k * 12 + 6] = array2[k * 12];
					array2[k * 12 + 7] = array2[k * 12 + 1];
					array2[k * 12 + 8] = array2[k * 12 + 2];
					array2[k * 12 + 9] = array2[k * 12 + 3];
					array2[k * 12 + 10] = array2[k * 12 + 4];
					array2[k * 12 + 11] = array2[k * 12 + 5];
				}
				Mesh mesh = this.chunkObject[i].GetComponent<MeshFilter>().mesh;
				mesh.vertices = array2;
				mesh.uv = array;
			}
		}

		// Token: 0x06000C7B RID: 3195 RVA: 0x000287FC File Offset: 0x000269FC
		public void CrackedAll()
		{
			for (int i = 0; i < this.chunks.Count; i++)
			{
				this.chunks[i].InitialRotationDisplacement = 0f;
				Vector2[] array = new Vector2[3 * this.chunks[i].chunkedTriangleIndexes.Count * 2 * 2];
				Vector3[] array2 = new Vector3[3 * this.chunks[i].chunkedTriangleIndexes.Count * 2 * 2];
				Vector2 vector = default(Vector2);
				for (int j = 0; j < this.chunks[i].chunkedTriangleIndexes.Count; j++)
				{
					vector += this.vertices[this.triangles[this.chunks[i].chunkedTriangleIndexes[j]].vertices.x];
					vector += this.vertices[this.triangles[this.chunks[i].chunkedTriangleIndexes[j]].vertices.y];
					vector += this.vertices[this.triangles[this.chunks[i].chunkedTriangleIndexes[j]].vertices.z];
				}
				vector /= (float)(3 * this.chunks[i].chunkedTriangleIndexes.Count);
				for (int k = 0; k < this.chunks[i].chunkedTriangleIndexes.Count; k++)
				{
					array[k * 12] = (array[k * 12 + 3] = this.verticesUV[this.triangles[this.chunks[i].chunkedTriangleIndexes[k]].vertices.x]);
					array[k * 12 + 1] = (array[k * 12 + 4] = this.verticesUV[this.triangles[this.chunks[i].chunkedTriangleIndexes[k]].vertices.y]);
					array[k * 12 + 2] = (array[k * 12 + 5] = this.verticesUV[this.triangles[this.chunks[i].chunkedTriangleIndexes[k]].vertices.z]);
					array2[k * 12] = (array2[k * 12 + 3] = (array2[k * 12 + 6] = (array2[k * 12 + 9] = this.vertices[this.triangles[this.chunks[i].chunkedTriangleIndexes[k]].vertices.x] - vector)));
					array2[k * 12 + 1] = (array2[k * 12 + 4] = (array2[k * 12 + 7] = (array2[k * 12 + 10] = this.vertices[this.triangles[this.chunks[i].chunkedTriangleIndexes[k]].vertices.y] - vector)));
					array2[k * 12 + 2] = (array2[k * 12 + 5] = (array2[k * 12 + 8] = (array2[k * 12 + 11] = this.vertices[this.triangles[this.chunks[i].chunkedTriangleIndexes[k]].vertices.z] - vector)));
					array2[k * 12] += array2[k * 12].normalized * this._gap;
					array2[k * 12 + 1] += array2[k * 12 + 1].normalized * this._gap;
					array2[k * 12 + 2] += array2[k * 12 + 2].normalized * this._gap;
					array2[k * 12 + 3] -= array2[k * 12 + 3].normalized * this._gap;
					array2[k * 12 + 4] -= array2[k * 12 + 4].normalized * this._gap;
					array2[k * 12 + 5] -= array2[k * 12 + 5].normalized * this._gap;
					array2[k * 12].z = (array2[k * 12 + 1].z = (array2[k * 12 + 2].z = this._thickness));
					array2[k * 12 + 3].z = (array2[k * 12 + 4].z = (array2[k * 12 + 5].z = -this._thickness));
					array2[k * 12 + 6] = array2[k * 12];
					array2[k * 12 + 7] = array2[k * 12 + 1];
					array2[k * 12 + 8] = array2[k * 12 + 2];
					array2[k * 12 + 9] = array2[k * 12 + 3];
					array2[k * 12 + 10] = array2[k * 12 + 4];
					array2[k * 12 + 11] = array2[k * 12 + 5];
				}
				Mesh mesh = this.chunkObject[i].GetComponent<MeshFilter>().mesh;
				mesh.vertices = array2;
				mesh.uv = array;
			}
		}

		// Token: 0x06000C7C RID: 3196 RVA: 0x00028F02 File Offset: 0x00027102
		public void RootObjectSetActive()
		{
			this.rootObject.SetActive(true);
		}

		// Token: 0x06000C7D RID: 3197 RVA: 0x00028F10 File Offset: 0x00027110
		public void RootObjectSetDeActive()
		{
			this.rootObject.SetActive(false);
		}

		// Token: 0x06000C7E RID: 3198 RVA: 0x00028F1E File Offset: 0x0002711E
		[ContextMenu("StopRenderTexture")]
		public void StopRenderTexture()
		{
			this._renderTextureCamera.enabled = false;
		}

		// Token: 0x06000C7F RID: 3199 RVA: 0x00028F2C File Offset: 0x0002712C
		[ContextMenu("StartRenderTexture")]
		public void StartRenderTexture()
		{
			this._renderTextureCamera.enabled = true;
		}

		// Token: 0x06000C80 RID: 3200 RVA: 0x00028F3A File Offset: 0x0002713A
		public void StartAnimation()
		{
			GameObject.Find("UI Canvas").SetActive(false);
		}

		// Token: 0x06000C81 RID: 3201 RVA: 0x00028F4C File Offset: 0x0002714C
		public void SetTime(float time)
		{
			this._time = time;
		}

		// Token: 0x04000A64 RID: 2660
		private static int chunkInsideIndex = -1;

		// Token: 0x04000A65 RID: 2661
		[SerializeField]
		private Vector2 _screenSize;

		// Token: 0x04000A66 RID: 2662
		[SerializeField]
		private int verticesNum = 100;

		// Token: 0x04000A67 RID: 2663
		[Header("미러 애니메이션")]
		[SerializeField]
		private float _displacement_uv = 0.1f;

		// Token: 0x04000A68 RID: 2664
		[SerializeField]
		private float _displacement_rotation = 0.1f;

		// Token: 0x04000A69 RID: 2665
		[SerializeField]
		private bool _forceControlcamera;

		// Token: 0x04000A6A RID: 2666
		[SerializeField]
		private bool _addRootPosition;

		// Token: 0x04000A6B RID: 2667
		private float _cameraSize = 5.625f;

		// Token: 0x04000A6C RID: 2668
		[Range(-10f, 10f)]
		[SerializeField]
		private float _time;

		// Token: 0x04000A6D RID: 2669
		[Range(0f, 10f)]
		[SerializeField]
		private float _delay;

		// Token: 0x04000A6E RID: 2670
		private float _accelator;

		// Token: 0x04000A6F RID: 2671
		[SerializeField]
		private Vector3 _normal3;

		// Token: 0x04000A70 RID: 2672
		[SerializeField]
		[Header("메쉬 세팅")]
		private float _thickness = 0.2f;

		// Token: 0x04000A71 RID: 2673
		[SerializeField]
		private float _gap = 0.1f;

		// Token: 0x04000A72 RID: 2674
		[SerializeField]
		private float _scaleModifier = 1.04f;

		// Token: 0x04000A73 RID: 2675
		[SerializeField]
		private Material _material;

		// Token: 0x04000A74 RID: 2676
		[SerializeField]
		private Texture2D _endingSprite;

		// Token: 0x04000A75 RID: 2677
		[SerializeField]
		private Texture2D _resultSprite;

		// Token: 0x04000A76 RID: 2678
		[Header("정점 세팅")]
		[Space(10f)]
		[SerializeField]
		[Tooltip("내부,원,외부,최외곽")]
		private Vector4 verticesPositionRatio = new Vector4(40f, 20f, 20f, 20f);

		// Token: 0x04000A77 RID: 2679
		[SerializeField]
		private Vector2 _centerPoint;

		// Token: 0x04000A78 RID: 2680
		[Tooltip("도넛 내외부 반지름.")]
		[SerializeField]
		private Vector2 _innerCircleRadius;

		// Token: 0x04000A79 RID: 2681
		[SerializeField]
		[Tooltip("최외곽 두께.")]
		private float _edgeThickness = 0.5f;

		// Token: 0x04000A7A RID: 2682
		[Header("청크 메이킹 데이터")]
		[SerializeField]
		private float _innerCircleTriangleAreaLevel = 0.1f;

		// Token: 0x04000A7B RID: 2683
		[SerializeField]
		[Range(0f, 1f)]
		private float _InnerCircleTriangleRandomProbabilities = 1f;

		// Token: 0x04000A7C RID: 2684
		[SerializeField]
		[Range(0f, 1f)]
		private float _InnerCircleTriangleRPDecreaseAmount = 1f;

		// Token: 0x04000A7D RID: 2685
		[SerializeField]
		private float _outterCircleTriangleAreaLevel = 5f;

		// Token: 0x04000A7E RID: 2686
		[SerializeField]
		[Range(0f, 1f)]
		private float _outterCircleTriangleRandomProbabilities = 1f;

		// Token: 0x04000A7F RID: 2687
		[SerializeField]
		[Range(0f, 1f)]
		private float _outterCircleTriangleRPDecreaseAmount = 1f;

		// Token: 0x04000A80 RID: 2688
		[Header("기본 화면 세팅")]
		[SerializeField]
		private Camera _renderTextureCamera;

		// Token: 0x04000A81 RID: 2689
		[SerializeField]
		private Vector2[] vertices;

		// Token: 0x04000A82 RID: 2690
		[SerializeField]
		private float2[] verticesUV;

		// Token: 0x04000A83 RID: 2691
		[SerializeField]
		private List<int2> edges;

		// Token: 0x04000A84 RID: 2692
		[SerializeField]
		private List<MirrorBreakEffect.Triangle> triangles;

		// Token: 0x04000A85 RID: 2693
		[SerializeField]
		private List<MirrorBreakEffect.ChunkData> chunks;

		// Token: 0x04000A86 RID: 2694
		[SerializeField]
		private List<GameObject> chunkObject;

		// Token: 0x04000A87 RID: 2695
		private GameObject rootObject;

		// Token: 0x04000A88 RID: 2696
		private List<int> deadVertices;

		// Token: 0x04000A89 RID: 2697
		private List<int2> deadEdges;

		// Token: 0x04000A8A RID: 2698
		private List<int> convexHullPointsIndex;

		// Token: 0x04000A8B RID: 2699
		private int closestCenterPointIndex;

		// Token: 0x04000A8C RID: 2700
		private float halfScreenSizeX;

		// Token: 0x04000A8D RID: 2701
		private float halfScreenSizeY;

		// Token: 0x04000A8E RID: 2702
		private Vector2 topLeft;

		// Token: 0x04000A8F RID: 2703
		private Vector2 topRight;

		// Token: 0x04000A90 RID: 2704
		private Vector2 downLeft;

		// Token: 0x04000A91 RID: 2705
		private Vector2 downRight;

		// Token: 0x04000A92 RID: 2706
		private float topLeftDegree;

		// Token: 0x04000A93 RID: 2707
		private float topRightDegree;

		// Token: 0x04000A94 RID: 2708
		private float downLeftDegree;

		// Token: 0x04000A95 RID: 2709
		private float downRightDegree;

		// Token: 0x04000A96 RID: 2710
		private float innerVerticesRatio;

		// Token: 0x04000A97 RID: 2711
		private float circleVerticesRatio;

		// Token: 0x04000A98 RID: 2712
		private float outerVerticesRatio;

		// Token: 0x04000A99 RID: 2713
		private float edgeVerticesRatio;

		// Token: 0x04000A9A RID: 2714
		private bool IsInitialized;

		// Token: 0x04000A9B RID: 2715
		private bool IsVertexCreated;

		// Token: 0x04000A9C RID: 2716
		private bool IsEdgeCreated;

		// Token: 0x04000A9D RID: 2717
		private bool IsTriangleCreated;

		// Token: 0x04000A9E RID: 2718
		private bool IsPairedTriangleCreated;

		// Token: 0x04000A9F RID: 2719
		private bool IsChunkCreated;

		// Token: 0x04000AA0 RID: 2720
		private bool IsMeshCreated;

		// Token: 0x04000AA1 RID: 2721
		[Header("Debug")]
		[SerializeField]
		private bool IsMeshCreatedVertexCheck;

		// Token: 0x04000AA2 RID: 2722
		[SerializeField]
		private bool IsMeshCreatedEdgeCheck;

		// Token: 0x04000AA3 RID: 2723
		[SerializeField]
		private bool IsMeshCreatedTriangleCheck;

		// Token: 0x04000AA4 RID: 2724
		[SerializeField]
		private bool IsMeshCreatedTrianglePairedCheck;

		// Token: 0x04000AA5 RID: 2725
		[SerializeField]
		private bool IsMeshCreatedChunkCheck = true;

		// Token: 0x04000AA6 RID: 2726
		private int firstTriangles;

		// Token: 0x04000AA7 RID: 2727
		private int convexHullTriangles;

		// Token: 0x04000AA8 RID: 2728
		private int convexHullToDeadVerticeTriangles;

		// Token: 0x04000AA9 RID: 2729
		private int outterCircleLinkVerticeTriangles;

		// Token: 0x04000AAA RID: 2730
		private int outterCircleLeftTriangles;

		// Token: 0x04000AAB RID: 2731
		private int insideConvexHullEdgeNum = 100;

		// Token: 0x04000AAC RID: 2732
		private int convexHullEdgeToOutterCircleVertice = 1000;

		// Token: 0x04000AAD RID: 2733
		private int outterCircleBranchVerticeLink = 1000;

		// Token: 0x04000AAE RID: 2734
		private Camera camera;

		// Token: 0x04000AAF RID: 2735
		private List<int> deadEdgeOutterCircle = new List<int>();

		// Token: 0x0200027C RID: 636
		[Serializable]
		public class Triangle
		{
			// Token: 0x06000C84 RID: 3204 RVA: 0x0002905C File Offset: 0x0002725C
			public void MakeChunk(int chunkIndex, bool inside, float innerCircleIndex, float inAreaLevel, float outAreaLevel, List<MirrorBreakEffect.Triangle> triangles, ref List<MirrorBreakEffect.Triangle> tempTriangleList, ref List<int> triangleStoreList)
			{
				tempTriangleList.Remove(this);
				this.IncludeTriangleToChunk(chunkIndex, this, inside, this.edgePairedTriangle.x, innerCircleIndex, inAreaLevel, outAreaLevel, triangles, ref tempTriangleList, ref triangleStoreList);
				this.IncludeTriangleToChunk(chunkIndex, this, inside, this.edgePairedTriangle.y, innerCircleIndex, inAreaLevel, outAreaLevel, triangles, ref tempTriangleList, ref triangleStoreList);
				this.IncludeTriangleToChunk(chunkIndex, this, inside, this.edgePairedTriangle.z, innerCircleIndex, inAreaLevel, outAreaLevel, triangles, ref tempTriangleList, ref triangleStoreList);
			}

			// Token: 0x06000C85 RID: 3205 RVA: 0x000290D0 File Offset: 0x000272D0
			public void IncludeTriangleToChunk(int chunkIndex, MirrorBreakEffect.Triangle triangle, bool inside, int targetTriangle, float innerCircleIndex, float inAreaLevel, float outAreaLevel, List<MirrorBreakEffect.Triangle> triangles, ref List<MirrorBreakEffect.Triangle> tempTriangleList, ref List<int> triangleStoreList)
			{
				if (inside)
				{
					if (triangle.crossProduct < inAreaLevel && (float)targetTriangle <= innerCircleIndex && UnityEngine.Random.Range(0f, 1f) < triangle.randomProbabilities)
					{
						for (int i = 0; i < tempTriangleList.Count; i++)
						{
							if (tempTriangleList[i].triangleIndex == targetTriangle)
							{
								MirrorBreakEffect.Triangle triangle2 = tempTriangleList[i];
								triangleStoreList.Add(targetTriangle);
								triangle2.crossProduct += triangle.crossProduct;
								triangle2.randomProbabilities -= triangle.decreaseAmount;
								triangle2.digLevel++;
								triangle2.MakeChunk(chunkIndex, inside, innerCircleIndex, inAreaLevel, outAreaLevel, triangles, ref tempTriangleList, ref triangleStoreList);
								return;
							}
						}
						return;
					}
				}
				else
				{
					if (MirrorBreakEffect.chunkInsideIndex == -1)
					{
						MirrorBreakEffect.chunkInsideIndex = chunkIndex;
					}
					if (this.crossProduct < outAreaLevel && (float)targetTriangle > innerCircleIndex && UnityEngine.Random.Range(0f, 1f) < triangle.randomProbabilities)
					{
						for (int j = 0; j < tempTriangleList.Count; j++)
						{
							if (tempTriangleList[j].triangleIndex == targetTriangle)
							{
								MirrorBreakEffect.Triangle triangle3 = tempTriangleList[j];
								triangleStoreList.Add(targetTriangle);
								triangle3.crossProduct += triangle.crossProduct;
								triangle3.randomProbabilities -= triangle.decreaseAmount;
								triangle3.digLevel++;
								triangle3.MakeChunk(chunkIndex, inside, innerCircleIndex, inAreaLevel, outAreaLevel, triangles, ref tempTriangleList, ref triangleStoreList);
								return;
							}
						}
					}
				}
			}

			// Token: 0x04000AB0 RID: 2736
			public int triangleIndex;

			// Token: 0x04000AB1 RID: 2737
			public int3 vertices;

			// Token: 0x04000AB2 RID: 2738
			public int3 edges;

			// Token: 0x04000AB3 RID: 2739
			public int3 edgePairedTriangle;

			// Token: 0x04000AB4 RID: 2740
			public Vector3 normal;

			// Token: 0x04000AB5 RID: 2741
			public float crossProduct;

			// Token: 0x04000AB6 RID: 2742
			public float randomProbabilities = 1f;

			// Token: 0x04000AB7 RID: 2743
			public float decreaseAmount = 0.3f;

			// Token: 0x04000AB8 RID: 2744
			public int digLevel = 1;
		}

		// Token: 0x0200027D RID: 637
		public class FinalMesh
		{
			// Token: 0x04000AB9 RID: 2745
			public List<MirrorBreakEffect.Triangle> meshTriangles;
		}

		// Token: 0x0200027E RID: 638
		[Serializable]
		public class ChunkData
		{
			// Token: 0x04000ABA RID: 2746
			public int baseTriangleIndex;

			// Token: 0x04000ABB RID: 2747
			public Vector3 chunkBasePosition;

			// Token: 0x04000ABC RID: 2748
			public Vector3 moveDirection;

			// Token: 0x04000ABD RID: 2749
			public float moveSpeed;

			// Token: 0x04000ABE RID: 2750
			public float InitialRotationDisplacement;

			// Token: 0x04000ABF RID: 2751
			public Vector3 rotationVector;

			// Token: 0x04000AC0 RID: 2752
			public float rotationSpeed;

			// Token: 0x04000AC1 RID: 2753
			public float scale;

			// Token: 0x04000AC2 RID: 2754
			public List<int> chunkedTriangleIndexes;
		}

		// Token: 0x0200027F RID: 639
		public struct VerticesWithAngle
		{
			// Token: 0x04000AC3 RID: 2755
			public int verticeIndex;

			// Token: 0x04000AC4 RID: 2756
			public float verticeAngle;
		}

		// Token: 0x02000280 RID: 640
		public class ConvexHullLinkedVertices
		{
			// Token: 0x04000AC5 RID: 2757
			public int convexHullverticeIndex;

			// Token: 0x04000AC6 RID: 2758
			public List<MirrorBreakEffect.VerticesWithAngle> linkedVerticesWithAngles;
		}

		// Token: 0x02000281 RID: 641
		public struct OutterVerticesCandidate
		{
			// Token: 0x04000AC7 RID: 2759
			public int verticeIndex;

			// Token: 0x04000AC8 RID: 2760
			public List<int> relateEdge;
		}

		// Token: 0x02000282 RID: 642
		public enum EdgeDirection
		{
			// Token: 0x04000ACA RID: 2762
			Up,
			// Token: 0x04000ACB RID: 2763
			Left,
			// Token: 0x04000ACC RID: 2764
			Down,
			// Token: 0x04000ACD RID: 2765
			Right,
			// Token: 0x04000ACE RID: 2766
			ShortTwo
		}
	}
}
