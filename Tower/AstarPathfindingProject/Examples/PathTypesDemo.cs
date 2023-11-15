using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding.Examples
{
	// Token: 0x020000EE RID: 238
	[HelpURL("http://arongranberg.com/astar/documentation/stable/class_pathfinding_1_1_examples_1_1_path_types_demo.php")]
	public class PathTypesDemo : MonoBehaviour
	{
		// Token: 0x060009FB RID: 2555 RVA: 0x000417A4 File Offset: 0x0003F9A4
		private void Update()
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			Vector3 vector = ray.origin + ray.direction * (ray.origin.y / -ray.direction.y);
			this.end.position = vector;
			if (Input.GetMouseButtonUp(0))
			{
				if (Input.GetKey(KeyCode.LeftShift))
				{
					this.multipoints.Add(vector);
				}
				if (Input.GetKey(KeyCode.LeftControl))
				{
					this.multipoints.Clear();
				}
				if (Input.mousePosition.x > 225f)
				{
					this.DemoPath();
				}
			}
			if (Input.GetMouseButton(0) && Input.GetKey(KeyCode.LeftAlt) && (this.lastPath == null || this.lastPath.IsDone()))
			{
				this.DemoPath();
			}
		}

		// Token: 0x060009FC RID: 2556 RVA: 0x00041880 File Offset: 0x0003FA80
		public void OnGUI()
		{
			GUILayout.BeginArea(new Rect(5f, 5f, 220f, (float)(Screen.height - 10)), "", "Box");
			switch (this.activeDemo)
			{
			case PathTypesDemo.DemoMode.ABPath:
				GUILayout.Label("Basic path. Finds a path from point A to point B.", Array.Empty<GUILayoutOption>());
				break;
			case PathTypesDemo.DemoMode.MultiTargetPath:
				GUILayout.Label("Multi Target Path. Finds a path quickly from one point to many others in a single search.", Array.Empty<GUILayoutOption>());
				break;
			case PathTypesDemo.DemoMode.RandomPath:
				GUILayout.Label("Randomized Path. Finds a path with a specified length in a random direction or biased towards some point when using a larger aim strenggth.", Array.Empty<GUILayoutOption>());
				break;
			case PathTypesDemo.DemoMode.FleePath:
				GUILayout.Label("Flee Path. Tries to flee from a specified point. Remember to set Flee Strength!", Array.Empty<GUILayoutOption>());
				break;
			case PathTypesDemo.DemoMode.ConstantPath:
				GUILayout.Label("Finds all nodes which it costs less than some value to reach.", Array.Empty<GUILayoutOption>());
				break;
			case PathTypesDemo.DemoMode.FloodPath:
				GUILayout.Label("Searches the whole graph from a specific point. FloodPathTracer can then be used to quickly find a path to that point", Array.Empty<GUILayoutOption>());
				break;
			case PathTypesDemo.DemoMode.FloodPathTracer:
				GUILayout.Label("Traces a path to where the FloodPath started. Compare the calculation times for this path with ABPath!\nGreat for TD games", Array.Empty<GUILayoutOption>());
				break;
			}
			GUILayout.Space(5f);
			GUILayout.Label("Note that the paths are rendered without ANY post-processing applied, so they might look a bit jagged", Array.Empty<GUILayoutOption>());
			GUILayout.Space(5f);
			GUILayout.Label("Click anywhere to recalculate the path. Hold Alt to continuously recalculate the path while the mouse is pressed.", Array.Empty<GUILayoutOption>());
			if (this.activeDemo == PathTypesDemo.DemoMode.ConstantPath || this.activeDemo == PathTypesDemo.DemoMode.RandomPath || this.activeDemo == PathTypesDemo.DemoMode.FleePath)
			{
				GUILayout.Label("Search Distance (" + this.searchLength.ToString() + ")", Array.Empty<GUILayoutOption>());
				this.searchLength = Mathf.RoundToInt(GUILayout.HorizontalSlider((float)this.searchLength, 0f, 100000f, Array.Empty<GUILayoutOption>()));
			}
			if (this.activeDemo == PathTypesDemo.DemoMode.RandomPath || this.activeDemo == PathTypesDemo.DemoMode.FleePath)
			{
				GUILayout.Label("Spread (" + this.spread.ToString() + ")", Array.Empty<GUILayoutOption>());
				this.spread = Mathf.RoundToInt(GUILayout.HorizontalSlider((float)this.spread, 0f, 40000f, Array.Empty<GUILayoutOption>()));
				GUILayout.Label(((this.activeDemo == PathTypesDemo.DemoMode.RandomPath) ? "Aim strength" : "Flee strength") + " (" + this.aimStrength.ToString() + ")", Array.Empty<GUILayoutOption>());
				this.aimStrength = GUILayout.HorizontalSlider(this.aimStrength, 0f, 1f, Array.Empty<GUILayoutOption>());
			}
			if (this.activeDemo == PathTypesDemo.DemoMode.MultiTargetPath)
			{
				GUILayout.Label("Hold shift and click to add new target points. Hold ctr and click to remove all target points", Array.Empty<GUILayoutOption>());
			}
			if (GUILayout.Button("A to B path", Array.Empty<GUILayoutOption>()))
			{
				this.activeDemo = PathTypesDemo.DemoMode.ABPath;
			}
			if (GUILayout.Button("Multi Target Path", Array.Empty<GUILayoutOption>()))
			{
				this.activeDemo = PathTypesDemo.DemoMode.MultiTargetPath;
			}
			if (GUILayout.Button("Random Path", Array.Empty<GUILayoutOption>()))
			{
				this.activeDemo = PathTypesDemo.DemoMode.RandomPath;
			}
			if (GUILayout.Button("Flee path", Array.Empty<GUILayoutOption>()))
			{
				this.activeDemo = PathTypesDemo.DemoMode.FleePath;
			}
			if (GUILayout.Button("Constant Path", Array.Empty<GUILayoutOption>()))
			{
				this.activeDemo = PathTypesDemo.DemoMode.ConstantPath;
			}
			if (GUILayout.Button("Flood Path", Array.Empty<GUILayoutOption>()))
			{
				this.activeDemo = PathTypesDemo.DemoMode.FloodPath;
			}
			if (GUILayout.Button("Flood Path Tracer", Array.Empty<GUILayoutOption>()))
			{
				this.activeDemo = PathTypesDemo.DemoMode.FloodPathTracer;
			}
			GUILayout.EndArea();
		}

		// Token: 0x060009FD RID: 2557 RVA: 0x00041B74 File Offset: 0x0003FD74
		public void OnPathComplete(Path p)
		{
			if (this.lastRender == null)
			{
				return;
			}
			this.ClearPrevious();
			if (p.error)
			{
				return;
			}
			GameObject gameObject = new GameObject("LineRenderer", new Type[]
			{
				typeof(LineRenderer)
			});
			LineRenderer component = gameObject.GetComponent<LineRenderer>();
			component.sharedMaterial = this.lineMat;
			component.startWidth = this.lineWidth;
			component.endWidth = this.lineWidth;
			component.positionCount = p.vectorPath.Count;
			for (int i = 0; i < p.vectorPath.Count; i++)
			{
				component.SetPosition(i, p.vectorPath[i] + this.pathOffset);
			}
			this.lastRender.Add(gameObject);
		}

		// Token: 0x060009FE RID: 2558 RVA: 0x00041C34 File Offset: 0x0003FE34
		private void ClearPrevious()
		{
			for (int i = 0; i < this.lastRender.Count; i++)
			{
				Object.Destroy(this.lastRender[i]);
			}
			this.lastRender.Clear();
		}

		// Token: 0x060009FF RID: 2559 RVA: 0x00041C73 File Offset: 0x0003FE73
		private void OnDestroy()
		{
			this.ClearPrevious();
			this.lastRender = null;
		}

		// Token: 0x06000A00 RID: 2560 RVA: 0x00041C84 File Offset: 0x0003FE84
		private void DemoPath()
		{
			Path path = null;
			switch (this.activeDemo)
			{
			case PathTypesDemo.DemoMode.ABPath:
				path = ABPath.Construct(this.start.position, this.end.position, new OnPathDelegate(this.OnPathComplete));
				break;
			case PathTypesDemo.DemoMode.MultiTargetPath:
				base.StartCoroutine(this.DemoMultiTargetPath());
				break;
			case PathTypesDemo.DemoMode.RandomPath:
			{
				RandomPath randomPath = RandomPath.Construct(this.start.position, this.searchLength, new OnPathDelegate(this.OnPathComplete));
				randomPath.spread = this.spread;
				randomPath.aimStrength = this.aimStrength;
				randomPath.aim = this.end.position;
				path = randomPath;
				break;
			}
			case PathTypesDemo.DemoMode.FleePath:
			{
				FleePath fleePath = FleePath.Construct(this.start.position, this.end.position, this.searchLength, new OnPathDelegate(this.OnPathComplete));
				fleePath.aimStrength = this.aimStrength;
				fleePath.spread = this.spread;
				path = fleePath;
				break;
			}
			case PathTypesDemo.DemoMode.ConstantPath:
				base.StartCoroutine(this.DemoConstantPath());
				break;
			case PathTypesDemo.DemoMode.FloodPath:
				path = (this.lastFloodPath = FloodPath.Construct(this.end.position, null));
				break;
			case PathTypesDemo.DemoMode.FloodPathTracer:
				if (this.lastFloodPath != null)
				{
					path = FloodPathTracer.Construct(this.end.position, this.lastFloodPath, new OnPathDelegate(this.OnPathComplete));
				}
				break;
			}
			if (path != null)
			{
				AstarPath.StartPath(path, false);
				this.lastPath = path;
			}
		}

		// Token: 0x06000A01 RID: 2561 RVA: 0x00041E04 File Offset: 0x00040004
		private IEnumerator DemoMultiTargetPath()
		{
			MultiTargetPath mp = MultiTargetPath.Construct(this.multipoints.ToArray(), this.end.position, null, null);
			this.lastPath = mp;
			AstarPath.StartPath(mp, false);
			yield return base.StartCoroutine(mp.WaitForPath());
			List<GameObject> list = new List<GameObject>(this.lastRender);
			this.lastRender.Clear();
			for (int i = 0; i < mp.vectorPaths.Length; i++)
			{
				if (mp.vectorPaths[i] != null)
				{
					List<Vector3> list2 = mp.vectorPaths[i];
					GameObject gameObject;
					if (list.Count > i && list[i].GetComponent<LineRenderer>() != null)
					{
						gameObject = list[i];
						list.RemoveAt(i);
					}
					else
					{
						gameObject = new GameObject("LineRenderer_" + i.ToString(), new Type[]
						{
							typeof(LineRenderer)
						});
					}
					LineRenderer component = gameObject.GetComponent<LineRenderer>();
					component.sharedMaterial = this.lineMat;
					component.startWidth = this.lineWidth;
					component.endWidth = this.lineWidth;
					component.positionCount = list2.Count;
					for (int j = 0; j < list2.Count; j++)
					{
						component.SetPosition(j, list2[j] + this.pathOffset);
					}
					this.lastRender.Add(gameObject);
				}
			}
			for (int k = 0; k < list.Count; k++)
			{
				Object.Destroy(list[k]);
			}
			yield break;
		}

		// Token: 0x06000A02 RID: 2562 RVA: 0x00041E13 File Offset: 0x00040013
		public IEnumerator DemoConstantPath()
		{
			ConstantPath constPath = ConstantPath.Construct(this.end.position, this.searchLength, null);
			AstarPath.StartPath(constPath, false);
			this.lastPath = constPath;
			yield return base.StartCoroutine(constPath.WaitForPath());
			this.ClearPrevious();
			List<GraphNode> allNodes = constPath.allNodes;
			Mesh mesh = new Mesh();
			List<Vector3> list = new List<Vector3>();
			bool flag = false;
			for (int i = allNodes.Count - 1; i >= 0; i--)
			{
				Vector3 a = (Vector3)allNodes[i].position + this.pathOffset;
				if (list.Count == 65000 && !flag)
				{
					Debug.LogError("Too many nodes, rendering a mesh would throw 65K vertex error. Using Debug.DrawRay instead for the rest of the nodes");
					flag = true;
				}
				if (flag)
				{
					Debug.DrawRay(a, Vector3.up, Color.blue);
				}
				else
				{
					GridGraph gridGraph = AstarData.GetGraph(allNodes[i]) as GridGraph;
					float d = 1f;
					if (gridGraph != null)
					{
						d = gridGraph.nodeSize;
					}
					list.Add(a + new Vector3(-0.5f, 0f, -0.5f) * d);
					list.Add(a + new Vector3(0.5f, 0f, -0.5f) * d);
					list.Add(a + new Vector3(-0.5f, 0f, 0.5f) * d);
					list.Add(a + new Vector3(0.5f, 0f, 0.5f) * d);
				}
			}
			Vector3[] array = list.ToArray();
			int[] array2 = new int[3 * array.Length / 2];
			int j = 0;
			int num = 0;
			while (j < array.Length)
			{
				array2[num] = j;
				array2[num + 1] = j + 1;
				array2[num + 2] = j + 2;
				array2[num + 3] = j + 1;
				array2[num + 4] = j + 3;
				array2[num + 5] = j + 2;
				num += 6;
				j += 4;
			}
			Vector2[] array3 = new Vector2[array.Length];
			for (int k = 0; k < array3.Length; k += 4)
			{
				array3[k] = new Vector2(0f, 0f);
				array3[k + 1] = new Vector2(1f, 0f);
				array3[k + 2] = new Vector2(0f, 1f);
				array3[k + 3] = new Vector2(1f, 1f);
			}
			mesh.vertices = array;
			mesh.triangles = array2;
			mesh.uv = array3;
			mesh.RecalculateNormals();
			GameObject gameObject = new GameObject("Mesh", new Type[]
			{
				typeof(MeshRenderer),
				typeof(MeshFilter)
			});
			gameObject.GetComponent<MeshFilter>().mesh = mesh;
			gameObject.GetComponent<MeshRenderer>().material = this.squareMat;
			this.lastRender.Add(gameObject);
			yield break;
		}

		// Token: 0x04000616 RID: 1558
		public PathTypesDemo.DemoMode activeDemo;

		// Token: 0x04000617 RID: 1559
		public Transform start;

		// Token: 0x04000618 RID: 1560
		public Transform end;

		// Token: 0x04000619 RID: 1561
		public Vector3 pathOffset;

		// Token: 0x0400061A RID: 1562
		public Material lineMat;

		// Token: 0x0400061B RID: 1563
		public Material squareMat;

		// Token: 0x0400061C RID: 1564
		public float lineWidth;

		// Token: 0x0400061D RID: 1565
		public int searchLength = 1000;

		// Token: 0x0400061E RID: 1566
		public int spread = 100;

		// Token: 0x0400061F RID: 1567
		public float aimStrength;

		// Token: 0x04000620 RID: 1568
		private Path lastPath;

		// Token: 0x04000621 RID: 1569
		private FloodPath lastFloodPath;

		// Token: 0x04000622 RID: 1570
		private List<GameObject> lastRender = new List<GameObject>();

		// Token: 0x04000623 RID: 1571
		private List<Vector3> multipoints = new List<Vector3>();

		// Token: 0x02000185 RID: 389
		public enum DemoMode
		{
			// Token: 0x040008A9 RID: 2217
			ABPath,
			// Token: 0x040008AA RID: 2218
			MultiTargetPath,
			// Token: 0x040008AB RID: 2219
			RandomPath,
			// Token: 0x040008AC RID: 2220
			FleePath,
			// Token: 0x040008AD RID: 2221
			ConstantPath,
			// Token: 0x040008AE RID: 2222
			FloodPath,
			// Token: 0x040008AF RID: 2223
			FloodPathTracer
		}
	}
}
