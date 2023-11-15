using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding.Examples
{
	// Token: 0x020000E5 RID: 229
	[HelpURL("http://arongranberg.com/astar/documentation/stable/class_pathfinding_1_1_examples_1_1_procedural_world.php")]
	public class ProceduralWorld : MonoBehaviour
	{
		// Token: 0x060009CB RID: 2507 RVA: 0x000409A1 File Offset: 0x0003EBA1
		private void Start()
		{
			this.Update();
			AstarPath.active.Scan(null);
			base.StartCoroutine(this.GenerateTiles());
		}

		// Token: 0x060009CC RID: 2508 RVA: 0x000409C4 File Offset: 0x0003EBC4
		private void Update()
		{
			Int2 @int = new Int2(Mathf.RoundToInt((this.target.position.x - this.tileSize * 0.5f) / this.tileSize), Mathf.RoundToInt((this.target.position.z - this.tileSize * 0.5f) / this.tileSize));
			this.range = ((this.range < 1) ? 1 : this.range);
			bool flag = true;
			while (flag)
			{
				flag = false;
				foreach (KeyValuePair<Int2, ProceduralWorld.ProceduralTile> keyValuePair in this.tiles)
				{
					if (Mathf.Abs(keyValuePair.Key.x - @int.x) > this.range || Mathf.Abs(keyValuePair.Key.y - @int.y) > this.range)
					{
						keyValuePair.Value.Destroy();
						this.tiles.Remove(keyValuePair.Key);
						flag = true;
						break;
					}
				}
			}
			for (int i = @int.x - this.range; i <= @int.x + this.range; i++)
			{
				for (int j = @int.y - this.range; j <= @int.y + this.range; j++)
				{
					if (!this.tiles.ContainsKey(new Int2(i, j)))
					{
						ProceduralWorld.ProceduralTile proceduralTile = new ProceduralWorld.ProceduralTile(this, i, j);
						IEnumerator enumerator2 = proceduralTile.Generate();
						enumerator2.MoveNext();
						this.tileGenerationQueue.Enqueue(enumerator2);
						this.tiles.Add(new Int2(i, j), proceduralTile);
					}
				}
			}
			for (int k = @int.x - this.disableAsyncLoadWithinRange; k <= @int.x + this.disableAsyncLoadWithinRange; k++)
			{
				for (int l = @int.y - this.disableAsyncLoadWithinRange; l <= @int.y + this.disableAsyncLoadWithinRange; l++)
				{
					this.tiles[new Int2(k, l)].ForceFinish();
				}
			}
		}

		// Token: 0x060009CD RID: 2509 RVA: 0x00040C08 File Offset: 0x0003EE08
		private IEnumerator GenerateTiles()
		{
			for (;;)
			{
				if (this.tileGenerationQueue.Count > 0)
				{
					IEnumerator routine = this.tileGenerationQueue.Dequeue();
					yield return base.StartCoroutine(routine);
				}
				yield return null;
			}
			yield break;
		}

		// Token: 0x040005F1 RID: 1521
		public Transform target;

		// Token: 0x040005F2 RID: 1522
		public ProceduralWorld.ProceduralPrefab[] prefabs;

		// Token: 0x040005F3 RID: 1523
		public int range = 1;

		// Token: 0x040005F4 RID: 1524
		public int disableAsyncLoadWithinRange = 1;

		// Token: 0x040005F5 RID: 1525
		public float tileSize = 100f;

		// Token: 0x040005F6 RID: 1526
		public int subTiles = 20;

		// Token: 0x040005F7 RID: 1527
		public bool staticBatching;

		// Token: 0x040005F8 RID: 1528
		private Queue<IEnumerator> tileGenerationQueue = new Queue<IEnumerator>();

		// Token: 0x040005F9 RID: 1529
		private Dictionary<Int2, ProceduralWorld.ProceduralTile> tiles = new Dictionary<Int2, ProceduralWorld.ProceduralTile>();

		// Token: 0x0200017D RID: 381
		public enum RotationRandomness
		{
			// Token: 0x04000878 RID: 2168
			AllAxes,
			// Token: 0x04000879 RID: 2169
			Y
		}

		// Token: 0x0200017E RID: 382
		[Serializable]
		public class ProceduralPrefab
		{
			// Token: 0x0400087A RID: 2170
			public GameObject prefab;

			// Token: 0x0400087B RID: 2171
			public float density;

			// Token: 0x0400087C RID: 2172
			public float perlin;

			// Token: 0x0400087D RID: 2173
			public float perlinPower = 1f;

			// Token: 0x0400087E RID: 2174
			public Vector2 perlinOffset = Vector2.zero;

			// Token: 0x0400087F RID: 2175
			public float perlinScale = 1f;

			// Token: 0x04000880 RID: 2176
			public float random = 1f;

			// Token: 0x04000881 RID: 2177
			public ProceduralWorld.RotationRandomness randomRotation;

			// Token: 0x04000882 RID: 2178
			public bool singleFixed;
		}

		// Token: 0x0200017F RID: 383
		private class ProceduralTile
		{
			// Token: 0x17000197 RID: 407
			// (get) Token: 0x06000BB6 RID: 2998 RVA: 0x0004AAB6 File Offset: 0x00048CB6
			// (set) Token: 0x06000BB7 RID: 2999 RVA: 0x0004AABE File Offset: 0x00048CBE
			public bool destroyed { get; private set; }

			// Token: 0x06000BB8 RID: 3000 RVA: 0x0004AAC7 File Offset: 0x00048CC7
			public ProceduralTile(ProceduralWorld world, int x, int z)
			{
				this.x = x;
				this.z = z;
				this.world = world;
				this.rnd = new Random(x * 10007 ^ z * 36007);
			}

			// Token: 0x06000BB9 RID: 3001 RVA: 0x0004AAFE File Offset: 0x00048CFE
			public IEnumerator Generate()
			{
				this.ie = this.InternalGenerate();
				GameObject gameObject = new GameObject("Tile " + this.x.ToString() + " " + this.z.ToString());
				this.root = gameObject.transform;
				while (this.ie != null && this.root != null && this.ie.MoveNext())
				{
					yield return this.ie.Current;
				}
				this.ie = null;
				yield break;
			}

			// Token: 0x06000BBA RID: 3002 RVA: 0x0004AB0D File Offset: 0x00048D0D
			public void ForceFinish()
			{
				while (this.ie != null && this.root != null && this.ie.MoveNext())
				{
				}
				this.ie = null;
			}

			// Token: 0x06000BBB RID: 3003 RVA: 0x0004AB3C File Offset: 0x00048D3C
			private Vector3 RandomInside()
			{
				return new Vector3
				{
					x = ((float)this.x + (float)this.rnd.NextDouble()) * this.world.tileSize,
					z = ((float)this.z + (float)this.rnd.NextDouble()) * this.world.tileSize
				};
			}

			// Token: 0x06000BBC RID: 3004 RVA: 0x0004ABA0 File Offset: 0x00048DA0
			private Vector3 RandomInside(float px, float pz)
			{
				return new Vector3
				{
					x = (px + (float)this.rnd.NextDouble() / (float)this.world.subTiles) * this.world.tileSize,
					z = (pz + (float)this.rnd.NextDouble() / (float)this.world.subTiles) * this.world.tileSize
				};
			}

			// Token: 0x06000BBD RID: 3005 RVA: 0x0004AC14 File Offset: 0x00048E14
			private Quaternion RandomYRot(ProceduralWorld.ProceduralPrefab prefab)
			{
				if (prefab.randomRotation != ProceduralWorld.RotationRandomness.AllAxes)
				{
					return Quaternion.Euler(0f, 360f * (float)this.rnd.NextDouble(), 0f);
				}
				return Quaternion.Euler(360f * (float)this.rnd.NextDouble(), 360f * (float)this.rnd.NextDouble(), 360f * (float)this.rnd.NextDouble());
			}

			// Token: 0x06000BBE RID: 3006 RVA: 0x0004AC86 File Offset: 0x00048E86
			private IEnumerator InternalGenerate()
			{
				Debug.Log("Generating tile " + this.x.ToString() + ", " + this.z.ToString());
				int counter = 0;
				float[,] ditherMap = new float[this.world.subTiles + 2, this.world.subTiles + 2];
				int num3;
				for (int i = 0; i < this.world.prefabs.Length; i = num3 + 1)
				{
					ProceduralWorld.ProceduralPrefab pref = this.world.prefabs[i];
					if (pref.singleFixed)
					{
						Vector3 position = new Vector3(((float)this.x + 0.5f) * this.world.tileSize, 0f, ((float)this.z + 0.5f) * this.world.tileSize);
						Object.Instantiate<GameObject>(pref.prefab, position, Quaternion.identity).transform.parent = this.root;
					}
					else
					{
						float subSize = this.world.tileSize / (float)this.world.subTiles;
						for (int k = 0; k < this.world.subTiles; k++)
						{
							for (int l = 0; l < this.world.subTiles; l++)
							{
								ditherMap[k + 1, l + 1] = 0f;
							}
						}
						for (int sx = 0; sx < this.world.subTiles; sx = num3 + 1)
						{
							for (int sz = 0; sz < this.world.subTiles; sz = num3 + 1)
							{
								float px = (float)this.x + (float)sx / (float)this.world.subTiles;
								float pz = (float)this.z + (float)sz / (float)this.world.subTiles;
								float b = Mathf.Pow(Mathf.PerlinNoise((px + pref.perlinOffset.x) * pref.perlinScale, (pz + pref.perlinOffset.y) * pref.perlinScale), pref.perlinPower);
								float num = pref.density * Mathf.Lerp(1f, b, pref.perlin) * Mathf.Lerp(1f, (float)this.rnd.NextDouble(), pref.random);
								float num2 = subSize * subSize * num + ditherMap[sx + 1, sz + 1];
								int count = Mathf.RoundToInt(num2);
								ditherMap[sx + 1 + 1, sz + 1] += 0.4375f * (num2 - (float)count);
								ditherMap[sx + 1 - 1, sz + 1 + 1] += 0.1875f * (num2 - (float)count);
								ditherMap[sx + 1, sz + 1 + 1] += 0.3125f * (num2 - (float)count);
								ditherMap[sx + 1 + 1, sz + 1 + 1] += 0.0625f * (num2 - (float)count);
								for (int j = 0; j < count; j = num3 + 1)
								{
									Vector3 position2 = this.RandomInside(px, pz);
									Object.Instantiate<GameObject>(pref.prefab, position2, this.RandomYRot(pref)).transform.parent = this.root;
									num3 = counter;
									counter = num3 + 1;
									if (counter % 2 == 0)
									{
										yield return null;
									}
									num3 = j;
								}
								num3 = sz;
							}
							num3 = sx;
						}
					}
					pref = null;
					num3 = i;
				}
				ditherMap = null;
				yield return null;
				yield return null;
				if (Application.HasProLicense() && this.world.staticBatching)
				{
					StaticBatchingUtility.Combine(this.root.gameObject);
				}
				yield break;
			}

			// Token: 0x06000BBF RID: 3007 RVA: 0x0004AC98 File Offset: 0x00048E98
			public void Destroy()
			{
				if (this.root != null)
				{
					Debug.Log("Destroying tile " + this.x.ToString() + ", " + this.z.ToString());
					Object.Destroy(this.root.gameObject);
					this.root = null;
				}
				this.ie = null;
			}

			// Token: 0x04000883 RID: 2179
			private int x;

			// Token: 0x04000884 RID: 2180
			private int z;

			// Token: 0x04000885 RID: 2181
			private Random rnd;

			// Token: 0x04000886 RID: 2182
			private ProceduralWorld world;

			// Token: 0x04000888 RID: 2184
			private Transform root;

			// Token: 0x04000889 RID: 2185
			private IEnumerator ie;
		}
	}
}
