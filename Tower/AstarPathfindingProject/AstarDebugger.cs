﻿using System;
using System.Text;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000036 RID: 54
	[AddComponentMenu("Pathfinding/Pathfinding Debugger")]
	[ExecuteInEditMode]
	[HelpURL("http://arongranberg.com/astar/documentation/stable/class_pathfinding_1_1_astar_debugger.php")]
	public class AstarDebugger : VersionedMonoBehaviour
	{
		// Token: 0x06000285 RID: 645 RVA: 0x0000D290 File Offset: 0x0000B490
		public void Start()
		{
			base.useGUILayout = false;
			this.fpsDrops = new float[this.fpsDropCounterSize];
			this.cam = base.GetComponent<Camera>();
			if (this.cam == null)
			{
				this.cam = Camera.main;
			}
			this.graph = new AstarDebugger.GraphPoint[this.graphBufferSize];
			if (Time.unscaledDeltaTime > 0f)
			{
				for (int i = 0; i < this.fpsDrops.Length; i++)
				{
					this.fpsDrops[i] = 1f / Time.unscaledDeltaTime;
				}
			}
		}

		// Token: 0x06000286 RID: 646 RVA: 0x0000D320 File Offset: 0x0000B520
		public void LateUpdate()
		{
			if (!this.show || (!Application.isPlaying && !this.showInEditor))
			{
				return;
			}
			if (Time.unscaledDeltaTime <= 0.0001f)
			{
				return;
			}
			int num = GC.CollectionCount(0);
			if (this.lastCollectNum != (float)num)
			{
				this.lastCollectNum = (float)num;
				this.delta = Time.realtimeSinceStartup - this.lastCollect;
				this.lastCollect = Time.realtimeSinceStartup;
				this.lastDeltaTime = Time.unscaledDeltaTime;
				this.collectAlloc = this.allocMem;
			}
			this.allocMem = (int)GC.GetTotalMemory(false);
			bool flag = this.allocMem < this.peakAlloc;
			this.peakAlloc = ((!flag) ? this.allocMem : this.peakAlloc);
			if (Time.realtimeSinceStartup - this.lastAllocSet > 0.3f || !Application.isPlaying)
			{
				int num2 = this.allocMem - this.lastAllocMemory;
				this.lastAllocMemory = this.allocMem;
				this.lastAllocSet = Time.realtimeSinceStartup;
				this.delayedDeltaTime = Time.unscaledDeltaTime;
				if (num2 >= 0)
				{
					this.allocRate = num2;
				}
			}
			if (Application.isPlaying)
			{
				this.fpsDrops[Time.frameCount % this.fpsDrops.Length] = ((Time.unscaledDeltaTime > 1E-05f) ? (1f / Time.unscaledDeltaTime) : 0f);
				int num3 = Time.frameCount % this.graph.Length;
				this.graph[num3].fps = ((Time.unscaledDeltaTime < 1E-05f) ? (1f / Time.unscaledDeltaTime) : 0f);
				this.graph[num3].collectEvent = flag;
				this.graph[num3].memory = (float)this.allocMem;
			}
			if (Application.isPlaying && this.cam != null && this.showGraph)
			{
				this.graphWidth = (float)this.cam.pixelWidth * 0.8f;
				float num4 = float.PositiveInfinity;
				float b = 0f;
				float num5 = float.PositiveInfinity;
				float b2 = 0f;
				for (int i = 0; i < this.graph.Length; i++)
				{
					num4 = Mathf.Min(this.graph[i].memory, num4);
					b = Mathf.Max(this.graph[i].memory, b);
					num5 = Mathf.Min(this.graph[i].fps, num5);
					b2 = Mathf.Max(this.graph[i].fps, b2);
				}
				int num6 = Time.frameCount % this.graph.Length;
				Matrix4x4 m = Matrix4x4.TRS(new Vector3(((float)this.cam.pixelWidth - this.graphWidth) / 2f, this.graphOffset, 1f), Quaternion.identity, new Vector3(this.graphWidth, this.graphHeight, 1f));
				for (int j = 0; j < this.graph.Length - 1; j++)
				{
					if (j != num6)
					{
						this.DrawGraphLine(j, m, (float)j / (float)this.graph.Length, (float)(j + 1) / (float)this.graph.Length, Mathf.InverseLerp(num4, b, this.graph[j].memory), Mathf.InverseLerp(num4, b, this.graph[j + 1].memory), Color.blue);
						this.DrawGraphLine(j, m, (float)j / (float)this.graph.Length, (float)(j + 1) / (float)this.graph.Length, Mathf.InverseLerp(num5, b2, this.graph[j].fps), Mathf.InverseLerp(num5, b2, this.graph[j + 1].fps), Color.green);
					}
				}
			}
		}

		// Token: 0x06000287 RID: 647 RVA: 0x0000D6F2 File Offset: 0x0000B8F2
		private void DrawGraphLine(int index, Matrix4x4 m, float x1, float x2, float y1, float y2, Color color)
		{
			Debug.DrawLine(this.cam.ScreenToWorldPoint(m.MultiplyPoint3x4(new Vector3(x1, y1))), this.cam.ScreenToWorldPoint(m.MultiplyPoint3x4(new Vector3(x2, y2))), color);
		}

		// Token: 0x06000288 RID: 648 RVA: 0x0000D730 File Offset: 0x0000B930
		public void OnGUI()
		{
			if (!this.show || (!Application.isPlaying && !this.showInEditor))
			{
				return;
			}
			if (this.style == null)
			{
				this.style = new GUIStyle();
				this.style.normal.textColor = Color.white;
				this.style.padding = new RectOffset(5, 5, 5, 5);
			}
			if (Time.realtimeSinceStartup - this.lastUpdate > 0.5f || this.cachedText == null || !Application.isPlaying)
			{
				this.lastUpdate = Time.realtimeSinceStartup;
				this.boxRect = new Rect(5f, (float)this.yOffset, 310f, 40f);
				this.text.Length = 0;
				this.text.AppendLine("A* Pathfinding Project Debugger");
				this.text.Append("A* Version: ").Append(AstarPath.Version.ToString());
				if (this.showMemProfile)
				{
					this.boxRect.height = this.boxRect.height + 200f;
					this.text.AppendLine();
					this.text.AppendLine();
					this.text.Append("Currently allocated".PadRight(25));
					this.text.Append(((float)this.allocMem / 1000000f).ToString("0.0 MB"));
					this.text.AppendLine();
					this.text.Append("Peak allocated".PadRight(25));
					this.text.Append(((float)this.peakAlloc / 1000000f).ToString("0.0 MB")).AppendLine();
					this.text.Append("Last collect peak".PadRight(25));
					this.text.Append(((float)this.collectAlloc / 1000000f).ToString("0.0 MB")).AppendLine();
					this.text.Append("Allocation rate".PadRight(25));
					this.text.Append(((float)this.allocRate / 1000000f).ToString("0.0 MB")).AppendLine();
					this.text.Append("Collection frequency".PadRight(25));
					this.text.Append(this.delta.ToString("0.00"));
					this.text.Append("s\n");
					this.text.Append("Last collect fps".PadRight(25));
					this.text.Append((1f / this.lastDeltaTime).ToString("0.0 fps"));
					this.text.Append(" (");
					this.text.Append(this.lastDeltaTime.ToString("0.000 s"));
					this.text.Append(")");
				}
				if (this.showFPS)
				{
					this.text.AppendLine();
					this.text.AppendLine();
					float num = (this.delayedDeltaTime > 1E-05f) ? (1f / this.delayedDeltaTime) : 0f;
					this.text.Append("FPS".PadRight(25)).Append(num.ToString("0.0 fps"));
					float num2 = float.PositiveInfinity;
					for (int i = 0; i < this.fpsDrops.Length; i++)
					{
						if (this.fpsDrops[i] < num2)
						{
							num2 = this.fpsDrops[i];
						}
					}
					this.text.AppendLine();
					this.text.Append(("Lowest fps (last " + this.fpsDrops.Length.ToString() + ")").PadRight(25)).Append(num2.ToString("0.0"));
				}
				if (this.showPathProfile)
				{
					Object active = AstarPath.active;
					this.text.AppendLine();
					if (active == null)
					{
						this.text.Append("\nNo AstarPath Object In The Scene");
					}
					else
					{
						if (ListPool<Vector3>.GetSize() > this.maxVecPool)
						{
							this.maxVecPool = ListPool<Vector3>.GetSize();
						}
						if (ListPool<GraphNode>.GetSize() > this.maxNodePool)
						{
							this.maxNodePool = ListPool<GraphNode>.GetSize();
						}
						this.text.Append("\nPool Sizes (size/total created)");
						for (int j = 0; j < this.debugTypes.Length; j++)
						{
							this.debugTypes[j].Print(this.text);
						}
					}
				}
				this.cachedText = this.text.ToString();
			}
			if (this.font != null)
			{
				this.style.font = this.font;
				this.style.fontSize = this.fontSize;
			}
			this.boxRect.height = this.style.CalcHeight(new GUIContent(this.cachedText), this.boxRect.width);
			GUI.Box(this.boxRect, "");
			GUI.Label(this.boxRect, this.cachedText, this.style);
			if (this.showGraph)
			{
				float num3 = float.PositiveInfinity;
				float num4 = 0f;
				float num5 = float.PositiveInfinity;
				float num6 = 0f;
				for (int k = 0; k < this.graph.Length; k++)
				{
					num3 = Mathf.Min(this.graph[k].memory, num3);
					num4 = Mathf.Max(this.graph[k].memory, num4);
					num5 = Mathf.Min(this.graph[k].fps, num5);
					num6 = Mathf.Max(this.graph[k].fps, num6);
				}
				GUI.color = Color.blue;
				float num7 = (float)Mathf.RoundToInt(num4 / 100000f);
				GUI.Label(new Rect(5f, (float)Screen.height - AstarMath.MapTo(num3, num4, 0f + this.graphOffset, this.graphHeight + this.graphOffset, num7 * 1000f * 100f) - 10f, 100f, 20f), (num7 / 10f).ToString("0.0 MB"));
				num7 = Mathf.Round(num3 / 100000f);
				GUI.Label(new Rect(5f, (float)Screen.height - AstarMath.MapTo(num3, num4, 0f + this.graphOffset, this.graphHeight + this.graphOffset, num7 * 1000f * 100f) - 10f, 100f, 20f), (num7 / 10f).ToString("0.0 MB"));
				GUI.color = Color.green;
				num7 = Mathf.Round(num6);
				GUI.Label(new Rect(55f, (float)Screen.height - AstarMath.MapTo(num5, num6, 0f + this.graphOffset, this.graphHeight + this.graphOffset, num7) - 10f, 100f, 20f), num7.ToString("0 FPS"));
				num7 = Mathf.Round(num5);
				GUI.Label(new Rect(55f, (float)Screen.height - AstarMath.MapTo(num5, num6, 0f + this.graphOffset, this.graphHeight + this.graphOffset, num7) - 10f, 100f, 20f), num7.ToString("0 FPS"));
			}
		}

		// Token: 0x06000289 RID: 649 RVA: 0x0000DEC4 File Offset: 0x0000C0C4
		public AstarDebugger()
		{
			AstarDebugger.PathTypeDebug[] array = new AstarDebugger.PathTypeDebug[7];
			array[0] = new AstarDebugger.PathTypeDebug("ABPath", () => PathPool.GetSize(typeof(ABPath)), () => PathPool.GetTotalCreated(typeof(ABPath)));
			array[1] = new AstarDebugger.PathTypeDebug("MultiTargetPath", () => PathPool.GetSize(typeof(MultiTargetPath)), () => PathPool.GetTotalCreated(typeof(MultiTargetPath)));
			array[2] = new AstarDebugger.PathTypeDebug("RandomPath", () => PathPool.GetSize(typeof(RandomPath)), () => PathPool.GetTotalCreated(typeof(RandomPath)));
			array[3] = new AstarDebugger.PathTypeDebug("FleePath", () => PathPool.GetSize(typeof(FleePath)), () => PathPool.GetTotalCreated(typeof(FleePath)));
			array[4] = new AstarDebugger.PathTypeDebug("ConstantPath", () => PathPool.GetSize(typeof(ConstantPath)), () => PathPool.GetTotalCreated(typeof(ConstantPath)));
			array[5] = new AstarDebugger.PathTypeDebug("FloodPath", () => PathPool.GetSize(typeof(FloodPath)), () => PathPool.GetTotalCreated(typeof(FloodPath)));
			array[6] = new AstarDebugger.PathTypeDebug("FloodPathTracer", () => PathPool.GetSize(typeof(FloodPathTracer)), () => PathPool.GetTotalCreated(typeof(FloodPathTracer)));
			this.debugTypes = array;
			base..ctor();
		}

		// Token: 0x04000189 RID: 393
		public int yOffset = 5;

		// Token: 0x0400018A RID: 394
		public bool show = true;

		// Token: 0x0400018B RID: 395
		public bool showInEditor;

		// Token: 0x0400018C RID: 396
		public bool showFPS;

		// Token: 0x0400018D RID: 397
		public bool showPathProfile;

		// Token: 0x0400018E RID: 398
		public bool showMemProfile;

		// Token: 0x0400018F RID: 399
		public bool showGraph;

		// Token: 0x04000190 RID: 400
		public int graphBufferSize = 200;

		// Token: 0x04000191 RID: 401
		public Font font;

		// Token: 0x04000192 RID: 402
		public int fontSize = 12;

		// Token: 0x04000193 RID: 403
		private StringBuilder text = new StringBuilder();

		// Token: 0x04000194 RID: 404
		private string cachedText;

		// Token: 0x04000195 RID: 405
		private float lastUpdate = -999f;

		// Token: 0x04000196 RID: 406
		private AstarDebugger.GraphPoint[] graph;

		// Token: 0x04000197 RID: 407
		private float delayedDeltaTime = 1f;

		// Token: 0x04000198 RID: 408
		private float lastCollect;

		// Token: 0x04000199 RID: 409
		private float lastCollectNum;

		// Token: 0x0400019A RID: 410
		private float delta;

		// Token: 0x0400019B RID: 411
		private float lastDeltaTime;

		// Token: 0x0400019C RID: 412
		private int allocRate;

		// Token: 0x0400019D RID: 413
		private int lastAllocMemory;

		// Token: 0x0400019E RID: 414
		private float lastAllocSet = -9999f;

		// Token: 0x0400019F RID: 415
		private int allocMem;

		// Token: 0x040001A0 RID: 416
		private int collectAlloc;

		// Token: 0x040001A1 RID: 417
		private int peakAlloc;

		// Token: 0x040001A2 RID: 418
		private int fpsDropCounterSize = 200;

		// Token: 0x040001A3 RID: 419
		private float[] fpsDrops;

		// Token: 0x040001A4 RID: 420
		private Rect boxRect;

		// Token: 0x040001A5 RID: 421
		private GUIStyle style;

		// Token: 0x040001A6 RID: 422
		private Camera cam;

		// Token: 0x040001A7 RID: 423
		private float graphWidth = 100f;

		// Token: 0x040001A8 RID: 424
		private float graphHeight = 100f;

		// Token: 0x040001A9 RID: 425
		private float graphOffset = 50f;

		// Token: 0x040001AA RID: 426
		private int maxVecPool;

		// Token: 0x040001AB RID: 427
		private int maxNodePool;

		// Token: 0x040001AC RID: 428
		private AstarDebugger.PathTypeDebug[] debugTypes;

		// Token: 0x0200010F RID: 271
		private struct GraphPoint
		{
			// Token: 0x040006A3 RID: 1699
			public float fps;

			// Token: 0x040006A4 RID: 1700
			public float memory;

			// Token: 0x040006A5 RID: 1701
			public bool collectEvent;
		}

		// Token: 0x02000110 RID: 272
		private struct PathTypeDebug
		{
			// Token: 0x06000A76 RID: 2678 RVA: 0x00043687 File Offset: 0x00041887
			public PathTypeDebug(string name, Func<int> getSize, Func<int> getTotalCreated)
			{
				this.name = name;
				this.getSize = getSize;
				this.getTotalCreated = getTotalCreated;
			}

			// Token: 0x06000A77 RID: 2679 RVA: 0x000436A0 File Offset: 0x000418A0
			public void Print(StringBuilder text)
			{
				int num = this.getTotalCreated();
				if (num > 0)
				{
					text.Append("\n").Append(("  " + this.name).PadRight(25)).Append(this.getSize()).Append("/").Append(num);
				}
			}

			// Token: 0x040006A6 RID: 1702
			private string name;

			// Token: 0x040006A7 RID: 1703
			private Func<int> getSize;

			// Token: 0x040006A8 RID: 1704
			private Func<int> getTotalCreated;
		}
	}
}
