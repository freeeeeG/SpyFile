using System;
using System.Collections.Generic;
using Pathfinding.Recast;
using Pathfinding.Serialization;
using Pathfinding.Util;
using Pathfinding.Voxels;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x0200006D RID: 109
	[JsonOptIn]
	[Preserve]
	public class RecastGraph : NavmeshBase, IUpdatableGraph
	{
		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x060005C4 RID: 1476 RVA: 0x00020B76 File Offset: 0x0001ED76
		protected override bool RecalculateNormals
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x060005C5 RID: 1477 RVA: 0x00020B79 File Offset: 0x0001ED79
		public override float TileWorldSizeX
		{
			get
			{
				return (float)this.tileSizeX * this.cellSize;
			}
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x060005C6 RID: 1478 RVA: 0x00020B89 File Offset: 0x0001ED89
		public override float TileWorldSizeZ
		{
			get
			{
				return (float)this.tileSizeZ * this.cellSize;
			}
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x060005C7 RID: 1479 RVA: 0x00020B99 File Offset: 0x0001ED99
		protected override float MaxTileConnectionEdgeDistance
		{
			get
			{
				return this.walkableClimb;
			}
		}

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x060005C8 RID: 1480 RVA: 0x00020BA1 File Offset: 0x0001EDA1
		[Obsolete("Obsolete since this is not accurate when the graph is rotated (rotation was not supported when this property was created)")]
		public Bounds forcedBounds
		{
			get
			{
				return new Bounds(this.forcedBoundsCenter, this.forcedBoundsSize);
			}
		}

		// Token: 0x060005C9 RID: 1481 RVA: 0x00020BB4 File Offset: 0x0001EDB4
		[Obsolete("Use node.ClosestPointOnNode instead")]
		public Vector3 ClosestPointOnNode(TriangleMeshNode node, Vector3 pos)
		{
			return node.ClosestPointOnNode(pos);
		}

		// Token: 0x060005CA RID: 1482 RVA: 0x00020BBD File Offset: 0x0001EDBD
		[Obsolete("Use node.ContainsPoint instead")]
		public bool ContainsPoint(TriangleMeshNode node, Vector3 pos)
		{
			return node.ContainsPoint((Int3)pos);
		}

		// Token: 0x060005CB RID: 1483 RVA: 0x00020BCC File Offset: 0x0001EDCC
		public void SnapForceBoundsToScene()
		{
			List<RasterizationMesh> list = this.CollectMeshes(new Bounds(Vector3.zero, new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity)));
			if (list.Count == 0)
			{
				return;
			}
			Bounds bounds = list[0].bounds;
			for (int i = 1; i < list.Count; i++)
			{
				bounds.Encapsulate(list[i].bounds);
				list[i].Pool();
			}
			this.forcedBoundsCenter = bounds.center;
			this.forcedBoundsSize = bounds.size;
		}

		// Token: 0x060005CC RID: 1484 RVA: 0x00020C5E File Offset: 0x0001EE5E
		GraphUpdateThreading IUpdatableGraph.CanUpdateAsync(GraphUpdateObject o)
		{
			if (!o.updatePhysics)
			{
				return GraphUpdateThreading.SeparateThread;
			}
			return (GraphUpdateThreading)7;
		}

		// Token: 0x060005CD RID: 1485 RVA: 0x00020C6C File Offset: 0x0001EE6C
		void IUpdatableGraph.UpdateAreaInit(GraphUpdateObject o)
		{
			if (!o.updatePhysics)
			{
				return;
			}
			RelevantGraphSurface.UpdateAllPositions();
			IntRect touchingTiles = base.GetTouchingTiles(o.bounds, this.TileBorderSizeInWorldUnits);
			Bounds tileBounds = base.GetTileBounds(touchingTiles);
			tileBounds.Expand(new Vector3(1f, 0f, 1f) * this.TileBorderSizeInWorldUnits * 2f);
			List<RasterizationMesh> inputMeshes = this.CollectMeshes(tileBounds);
			if (this.globalVox == null)
			{
				this.globalVox = new Voxelize(this.CellHeight, this.cellSize, this.walkableClimb, this.walkableHeight, this.maxSlope, this.maxEdgeLength);
			}
			this.globalVox.inputMeshes = inputMeshes;
		}

		// Token: 0x060005CE RID: 1486 RVA: 0x00020D20 File Offset: 0x0001EF20
		void IUpdatableGraph.UpdateArea(GraphUpdateObject guo)
		{
			IntRect touchingTiles = base.GetTouchingTiles(guo.bounds, this.TileBorderSizeInWorldUnits);
			if (!guo.updatePhysics)
			{
				for (int i = touchingTiles.ymin; i <= touchingTiles.ymax; i++)
				{
					for (int j = touchingTiles.xmin; j <= touchingTiles.xmax; j++)
					{
						NavmeshTile graph = this.tiles[i * this.tileXCount + j];
						NavMeshGraph.UpdateArea(guo, graph);
					}
				}
				return;
			}
			Voxelize voxelize = this.globalVox;
			if (voxelize == null)
			{
				throw new InvalidOperationException("No Voxelizer object. UpdateAreaInit should have been called before this function.");
			}
			for (int k = touchingTiles.xmin; k <= touchingTiles.xmax; k++)
			{
				for (int l = touchingTiles.ymin; l <= touchingTiles.ymax; l++)
				{
					this.stagingTiles.Add(this.BuildTileMesh(voxelize, k, l, 0));
				}
			}
			uint graphIndex = (uint)AstarPath.active.data.GetGraphIndex(this);
			for (int m = 0; m < this.stagingTiles.Count; m++)
			{
				GraphNode[] nodes = this.stagingTiles[m].nodes;
				GraphNode[] array = nodes;
				for (int n = 0; n < array.Length; n++)
				{
					array[n].GraphIndex = graphIndex;
				}
			}
			for (int num = 0; num < voxelize.inputMeshes.Count; num++)
			{
				voxelize.inputMeshes[num].Pool();
			}
			ListPool<RasterizationMesh>.Release(ref voxelize.inputMeshes);
		}

		// Token: 0x060005CF RID: 1487 RVA: 0x00020E90 File Offset: 0x0001F090
		void IUpdatableGraph.UpdateAreaPost(GraphUpdateObject guo)
		{
			for (int i = 0; i < this.stagingTiles.Count; i++)
			{
				NavmeshTile navmeshTile = this.stagingTiles[i];
				int num = navmeshTile.x + navmeshTile.z * this.tileXCount;
				NavmeshTile navmeshTile2 = this.tiles[num];
				for (int j = 0; j < navmeshTile2.nodes.Length; j++)
				{
					navmeshTile2.nodes[j].Destroy();
				}
				this.tiles[num] = navmeshTile;
			}
			for (int k = 0; k < this.stagingTiles.Count; k++)
			{
				NavmeshTile tile = this.stagingTiles[k];
				base.ConnectTileWithNeighbours(tile, false);
			}
			NavmeshTile[] array = this.stagingTiles.ToArray();
			this.navmeshUpdateData.OnRecalculatedTiles(array);
			if (this.OnRecalculatedTiles != null)
			{
				this.OnRecalculatedTiles(array);
			}
			this.stagingTiles.Clear();
		}

		// Token: 0x060005D0 RID: 1488 RVA: 0x00020F78 File Offset: 0x0001F178
		protected override IEnumerable<Progress> ScanInternal()
		{
			TriangleMeshNode.SetNavmeshHolder(AstarPath.active.data.GetGraphIndex(this), this);
			if (!Application.isPlaying)
			{
				RelevantGraphSurface.FindAllGraphSurfaces();
			}
			RelevantGraphSurface.UpdateAllPositions();
			foreach (Progress progress in this.ScanAllTiles())
			{
				yield return progress;
			}
			IEnumerator<Progress> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x060005D1 RID: 1489 RVA: 0x00020F88 File Offset: 0x0001F188
		public override GraphTransform CalculateTransform()
		{
			return new GraphTransform(Matrix4x4.TRS(this.forcedBoundsCenter, Quaternion.Euler(this.rotation), Vector3.one) * Matrix4x4.TRS(-this.forcedBoundsSize * 0.5f, Quaternion.identity, Vector3.one));
		}

		// Token: 0x060005D2 RID: 1490 RVA: 0x00020FE0 File Offset: 0x0001F1E0
		private void InitializeTileInfo()
		{
			int num = (int)(this.forcedBoundsSize.x / this.cellSize + 0.5f);
			int num2 = (int)(this.forcedBoundsSize.z / this.cellSize + 0.5f);
			if (!this.useTiles)
			{
				this.tileSizeX = num;
				this.tileSizeZ = num2;
			}
			else
			{
				this.tileSizeX = this.editorTileSize;
				this.tileSizeZ = this.editorTileSize;
			}
			this.tileXCount = (num + this.tileSizeX - 1) / this.tileSizeX;
			this.tileZCount = (num2 + this.tileSizeZ - 1) / this.tileSizeZ;
			if (this.tileXCount * this.tileZCount > 524288)
			{
				throw new Exception(string.Concat(new string[]
				{
					"Too many tiles (",
					(this.tileXCount * this.tileZCount).ToString(),
					") maximum is ",
					524288.ToString(),
					"\nTry disabling ASTAR_RECAST_LARGER_TILES under the 'Optimizations' tab in the A* inspector."
				}));
			}
			this.tiles = new NavmeshTile[this.tileXCount * this.tileZCount];
		}

		// Token: 0x060005D3 RID: 1491 RVA: 0x000210FC File Offset: 0x0001F2FC
		private List<RasterizationMesh>[] PutMeshesIntoTileBuckets(List<RasterizationMesh> meshes)
		{
			List<RasterizationMesh>[] array = new List<RasterizationMesh>[this.tiles.Length];
			Vector3 amount = new Vector3(1f, 0f, 1f) * this.TileBorderSizeInWorldUnits * 2f;
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = ListPool<RasterizationMesh>.Claim();
			}
			for (int j = 0; j < meshes.Count; j++)
			{
				RasterizationMesh rasterizationMesh = meshes[j];
				Bounds bounds = rasterizationMesh.bounds;
				bounds.Expand(amount);
				IntRect touchingTiles = base.GetTouchingTiles(bounds, 0f);
				for (int k = touchingTiles.ymin; k <= touchingTiles.ymax; k++)
				{
					for (int l = touchingTiles.xmin; l <= touchingTiles.xmax; l++)
					{
						array[l + k * this.tileXCount].Add(rasterizationMesh);
					}
				}
			}
			return array;
		}

		// Token: 0x060005D4 RID: 1492 RVA: 0x000211E2 File Offset: 0x0001F3E2
		protected IEnumerable<Progress> ScanAllTiles()
		{
			RecastGraph.<>c__DisplayClass50_0 CS$<>8__locals1 = new RecastGraph.<>c__DisplayClass50_0();
			CS$<>8__locals1.<>4__this = this;
			this.transform = this.CalculateTransform();
			this.InitializeTileInfo();
			if (this.scanEmptyGraph)
			{
				base.FillWithEmptyTiles();
				yield break;
			}
			this.walkableClimb = Mathf.Min(this.walkableClimb, this.walkableHeight);
			yield return new Progress(0f, "Finding Meshes");
			Bounds bounds = this.transform.Transform(new Bounds(this.forcedBoundsSize * 0.5f, this.forcedBoundsSize));
			List<RasterizationMesh> meshes = this.CollectMeshes(bounds);
			CS$<>8__locals1.buckets = this.PutMeshesIntoTileBuckets(meshes);
			Queue<Int2> tileQueue = new Queue<Int2>();
			for (int i = 0; i < this.tileZCount; i++)
			{
				for (int j = 0; j < this.tileXCount; j++)
				{
					tileQueue.Enqueue(new Int2(j, i));
				}
			}
			ParallelWorkQueue<Int2> parallelWorkQueue = new ParallelWorkQueue<Int2>(tileQueue);
			CS$<>8__locals1.voxelizers = new Voxelize[parallelWorkQueue.threadCount];
			for (int k = 0; k < CS$<>8__locals1.voxelizers.Length; k++)
			{
				CS$<>8__locals1.voxelizers[k] = new Voxelize(this.CellHeight, this.cellSize, this.walkableClimb, this.walkableHeight, this.maxSlope, this.maxEdgeLength);
			}
			parallelWorkQueue.action = delegate(Int2 tile, int threadIndex)
			{
				CS$<>8__locals1.voxelizers[threadIndex].inputMeshes = CS$<>8__locals1.buckets[tile.x + tile.y * CS$<>8__locals1.<>4__this.tileXCount];
				CS$<>8__locals1.<>4__this.tiles[tile.x + tile.y * CS$<>8__locals1.<>4__this.tileXCount] = CS$<>8__locals1.<>4__this.BuildTileMesh(CS$<>8__locals1.voxelizers[threadIndex], tile.x, tile.y, threadIndex);
			};
			int timeoutMillis = Application.isPlaying ? 1 : 200;
			foreach (int num in parallelWorkQueue.Run(timeoutMillis))
			{
				yield return new Progress(Mathf.Lerp(0.1f, 0.9f, (float)num / (float)this.tiles.Length), "Calculated Tiles: " + num.ToString() + "/" + this.tiles.Length.ToString());
			}
			IEnumerator<int> enumerator = null;
			yield return new Progress(0.9f, "Assigning Graph Indices");
			CS$<>8__locals1.graphIndex = (uint)AstarPath.active.data.GetGraphIndex(this);
			this.GetNodes(delegate(GraphNode node)
			{
				node.GraphIndex = CS$<>8__locals1.graphIndex;
			});
			int num3;
			for (int coordinateSum = 0; coordinateSum <= 1; coordinateSum = num3 + 1)
			{
				RecastGraph.<>c__DisplayClass50_1 CS$<>8__locals2 = new RecastGraph.<>c__DisplayClass50_1();
				CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
				CS$<>8__locals2.direction = 0;
				while (CS$<>8__locals2.direction <= 1)
				{
					for (int l = 0; l < this.tiles.Length; l++)
					{
						if ((this.tiles[l].x + this.tiles[l].z) % 2 == coordinateSum)
						{
							tileQueue.Enqueue(new Int2(this.tiles[l].x, this.tiles[l].z));
						}
					}
					parallelWorkQueue = new ParallelWorkQueue<Int2>(tileQueue);
					parallelWorkQueue.action = delegate(Int2 tile, int threadIndex)
					{
						if (CS$<>8__locals2.direction == 0 && tile.x < CS$<>8__locals2.CS$<>8__locals1.<>4__this.tileXCount - 1)
						{
							CS$<>8__locals2.CS$<>8__locals1.<>4__this.ConnectTiles(CS$<>8__locals2.CS$<>8__locals1.<>4__this.tiles[tile.x + tile.y * CS$<>8__locals2.CS$<>8__locals1.<>4__this.tileXCount], CS$<>8__locals2.CS$<>8__locals1.<>4__this.tiles[tile.x + 1 + tile.y * CS$<>8__locals2.CS$<>8__locals1.<>4__this.tileXCount]);
						}
						if (CS$<>8__locals2.direction == 1 && tile.y < CS$<>8__locals2.CS$<>8__locals1.<>4__this.tileZCount - 1)
						{
							CS$<>8__locals2.CS$<>8__locals1.<>4__this.ConnectTiles(CS$<>8__locals2.CS$<>8__locals1.<>4__this.tiles[tile.x + tile.y * CS$<>8__locals2.CS$<>8__locals1.<>4__this.tileXCount], CS$<>8__locals2.CS$<>8__locals1.<>4__this.tiles[tile.x + (tile.y + 1) * CS$<>8__locals2.CS$<>8__locals1.<>4__this.tileXCount]);
						}
					};
					int numTilesInQueue = tileQueue.Count;
					foreach (int num2 in parallelWorkQueue.Run(timeoutMillis))
					{
						yield return new Progress(0.95f, string.Concat(new string[]
						{
							"Connected Tiles ",
							(numTilesInQueue - num2).ToString(),
							"/",
							numTilesInQueue.ToString(),
							" (Phase ",
							(CS$<>8__locals2.direction + 1 + 2 * coordinateSum).ToString(),
							" of 4)"
						}));
					}
					enumerator = null;
					num3 = CS$<>8__locals2.direction;
					CS$<>8__locals2.direction = num3 + 1;
				}
				CS$<>8__locals2 = null;
				num3 = coordinateSum;
			}
			for (int m = 0; m < meshes.Count; m++)
			{
				meshes[m].Pool();
			}
			ListPool<RasterizationMesh>.Release(ref meshes);
			this.navmeshUpdateData.OnRecalculatedTiles(this.tiles);
			if (this.OnRecalculatedTiles != null)
			{
				this.OnRecalculatedTiles(this.tiles.Clone() as NavmeshTile[]);
			}
			yield break;
			yield break;
		}

		// Token: 0x060005D5 RID: 1493 RVA: 0x000211F4 File Offset: 0x0001F3F4
		private List<RasterizationMesh> CollectMeshes(Bounds bounds)
		{
			List<RasterizationMesh> list = ListPool<RasterizationMesh>.Claim();
			RecastMeshGatherer recastMeshGatherer = new RecastMeshGatherer(bounds, this.terrainSampleSize, this.mask, this.tagMask, this.colliderRasterizeDetail);
			if (this.rasterizeMeshes)
			{
				recastMeshGatherer.CollectSceneMeshes(list);
			}
			recastMeshGatherer.CollectRecastMeshObjs(list);
			if (this.rasterizeTerrain)
			{
				float desiredChunkSize = this.cellSize * (float)Math.Max(this.tileSizeX, this.tileSizeZ);
				recastMeshGatherer.CollectTerrainMeshes(this.rasterizeTrees, desiredChunkSize, list);
			}
			if (this.rasterizeColliders)
			{
				recastMeshGatherer.CollectColliderMeshes(list);
			}
			if (list.Count == 0)
			{
				Debug.LogWarning("No MeshFilters were found contained in the layers specified by the 'mask' variables");
			}
			return list;
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x060005D6 RID: 1494 RVA: 0x0002128E File Offset: 0x0001F48E
		private float CellHeight
		{
			get
			{
				return Mathf.Max(this.forcedBoundsSize.y / 64000f, 0.001f);
			}
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x060005D7 RID: 1495 RVA: 0x000212AB File Offset: 0x0001F4AB
		private int CharacterRadiusInVoxels
		{
			get
			{
				return Mathf.CeilToInt(this.characterRadius / this.cellSize - 0.1f);
			}
		}

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x060005D8 RID: 1496 RVA: 0x000212C5 File Offset: 0x0001F4C5
		private int TileBorderSizeInVoxels
		{
			get
			{
				return this.CharacterRadiusInVoxels + 3;
			}
		}

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x060005D9 RID: 1497 RVA: 0x000212CF File Offset: 0x0001F4CF
		private float TileBorderSizeInWorldUnits
		{
			get
			{
				return (float)this.TileBorderSizeInVoxels * this.cellSize;
			}
		}

		// Token: 0x060005DA RID: 1498 RVA: 0x000212E0 File Offset: 0x0001F4E0
		private Bounds CalculateTileBoundsWithBorder(int x, int z)
		{
			Bounds result = default(Bounds);
			result.SetMinMax(new Vector3((float)x * this.TileWorldSizeX, 0f, (float)z * this.TileWorldSizeZ), new Vector3((float)(x + 1) * this.TileWorldSizeX, this.forcedBoundsSize.y, (float)(z + 1) * this.TileWorldSizeZ));
			result.Expand(new Vector3(1f, 0f, 1f) * this.TileBorderSizeInWorldUnits * 2f);
			return result;
		}

		// Token: 0x060005DB RID: 1499 RVA: 0x00021370 File Offset: 0x0001F570
		protected NavmeshTile BuildTileMesh(Voxelize vox, int x, int z, int threadIndex = 0)
		{
			vox.borderSize = this.TileBorderSizeInVoxels;
			vox.forcedBounds = this.CalculateTileBoundsWithBorder(x, z);
			vox.width = this.tileSizeX + vox.borderSize * 2;
			vox.depth = this.tileSizeZ + vox.borderSize * 2;
			if (!this.useTiles && this.relevantGraphSurfaceMode == RecastGraph.RelevantGraphSurfaceMode.OnlyForCompletelyInsideTile)
			{
				vox.relevantGraphSurfaceMode = RecastGraph.RelevantGraphSurfaceMode.RequireForAll;
			}
			else
			{
				vox.relevantGraphSurfaceMode = this.relevantGraphSurfaceMode;
			}
			vox.minRegionSize = Mathf.RoundToInt(this.minRegionSize / (this.cellSize * this.cellSize));
			vox.Init();
			vox.VoxelizeInput(this.transform, this.CalculateTileBoundsWithBorder(x, z));
			vox.FilterLedges(vox.voxelWalkableHeight, vox.voxelWalkableClimb, vox.cellSize, vox.cellHeight);
			vox.FilterLowHeightSpans(vox.voxelWalkableHeight, vox.cellSize, vox.cellHeight);
			vox.BuildCompactField();
			vox.BuildVoxelConnections();
			vox.ErodeWalkableArea(this.CharacterRadiusInVoxels);
			vox.BuildDistanceField();
			vox.BuildRegions();
			VoxelContourSet cset = new VoxelContourSet();
			vox.BuildContours(this.contourMaxError, 1, cset, 5);
			VoxelMesh voxelMesh;
			vox.BuildPolyMesh(cset, 3, out voxelMesh);
			for (int i = 0; i < voxelMesh.verts.Length; i++)
			{
				voxelMesh.verts[i] *= 1000;
			}
			vox.transformVoxel2Graph.Transform(voxelMesh.verts);
			return this.CreateTile(vox, voxelMesh, x, z, threadIndex);
		}

		// Token: 0x060005DC RID: 1500 RVA: 0x000214EC File Offset: 0x0001F6EC
		private NavmeshTile CreateTile(Voxelize vox, VoxelMesh mesh, int x, int z, int threadIndex)
		{
			if (mesh.tris == null)
			{
				throw new ArgumentNullException("mesh.tris");
			}
			if (mesh.verts == null)
			{
				throw new ArgumentNullException("mesh.verts");
			}
			if (mesh.tris.Length % 3 != 0)
			{
				throw new ArgumentException("Indices array's length must be a multiple of 3 (mesh.tris)");
			}
			if (mesh.verts.Length >= 4095)
			{
				if (this.tileXCount * this.tileZCount == 1)
				{
					throw new ArgumentException("Too many vertices per tile (more than " + 4095.ToString() + ").\n<b>Try enabling tiling in the recast graph settings.</b>\n");
				}
				throw new ArgumentException("Too many vertices per tile (more than " + 4095.ToString() + ").\n<b>Try reducing tile size or enabling ASTAR_RECAST_LARGER_TILES under the 'Optimizations' tab in the A* Inspector</b>");
			}
			else
			{
				NavmeshTile navmeshTile = new NavmeshTile
				{
					x = x,
					z = z,
					w = 1,
					d = 1,
					tris = mesh.tris,
					bbTree = new BBTree(),
					graph = this
				};
				navmeshTile.vertsInGraphSpace = Utility.RemoveDuplicateVertices(mesh.verts, navmeshTile.tris);
				navmeshTile.verts = (Int3[])navmeshTile.vertsInGraphSpace.Clone();
				this.transform.Transform(navmeshTile.verts);
				uint num = (uint)(this.active.data.graphs.Length + threadIndex);
				if (num > 255U)
				{
					throw new Exception("Graph limit reached. Multithreaded recast calculations cannot be done because a few scratch graph indices are required.");
				}
				TriangleMeshNode.SetNavmeshHolder((int)num, navmeshTile);
				navmeshTile.nodes = new TriangleMeshNode[navmeshTile.tris.Length / 3];
				AstarPath active = this.active;
				lock (active)
				{
					base.CreateNodes(navmeshTile.nodes, navmeshTile.tris, x + z * this.tileXCount, num);
				}
				navmeshTile.bbTree.RebuildFrom(navmeshTile.nodes);
				NavmeshBase.CreateNodeConnections(navmeshTile.nodes);
				TriangleMeshNode.SetNavmeshHolder((int)num, null);
				return navmeshTile;
			}
		}

		// Token: 0x060005DD RID: 1501 RVA: 0x000216D0 File Offset: 0x0001F8D0
		protected override void DeserializeSettingsCompatibility(GraphSerializationContext ctx)
		{
			base.DeserializeSettingsCompatibility(ctx);
			this.characterRadius = ctx.reader.ReadSingle();
			this.contourMaxError = ctx.reader.ReadSingle();
			this.cellSize = ctx.reader.ReadSingle();
			ctx.reader.ReadSingle();
			this.walkableHeight = ctx.reader.ReadSingle();
			this.maxSlope = ctx.reader.ReadSingle();
			this.maxEdgeLength = ctx.reader.ReadSingle();
			this.editorTileSize = ctx.reader.ReadInt32();
			this.tileSizeX = ctx.reader.ReadInt32();
			this.nearestSearchOnlyXZ = ctx.reader.ReadBoolean();
			this.useTiles = ctx.reader.ReadBoolean();
			this.relevantGraphSurfaceMode = (RecastGraph.RelevantGraphSurfaceMode)ctx.reader.ReadInt32();
			this.rasterizeColliders = ctx.reader.ReadBoolean();
			this.rasterizeMeshes = ctx.reader.ReadBoolean();
			this.rasterizeTerrain = ctx.reader.ReadBoolean();
			this.rasterizeTrees = ctx.reader.ReadBoolean();
			this.colliderRasterizeDetail = ctx.reader.ReadSingle();
			this.forcedBoundsCenter = ctx.DeserializeVector3();
			this.forcedBoundsSize = ctx.DeserializeVector3();
			this.mask = ctx.reader.ReadInt32();
			int num = ctx.reader.ReadInt32();
			this.tagMask = new List<string>(num);
			for (int i = 0; i < num; i++)
			{
				this.tagMask.Add(ctx.reader.ReadString());
			}
			this.showMeshOutline = ctx.reader.ReadBoolean();
			this.showNodeConnections = ctx.reader.ReadBoolean();
			this.terrainSampleSize = ctx.reader.ReadInt32();
			this.walkableClimb = ctx.DeserializeFloat(this.walkableClimb);
			this.minRegionSize = ctx.DeserializeFloat(this.minRegionSize);
			this.tileSizeZ = ctx.DeserializeInt(this.tileSizeX);
			this.showMeshSurface = ctx.reader.ReadBoolean();
		}

		// Token: 0x04000330 RID: 816
		[JsonMember]
		public float characterRadius = 1.5f;

		// Token: 0x04000331 RID: 817
		[JsonMember]
		public float contourMaxError = 2f;

		// Token: 0x04000332 RID: 818
		[JsonMember]
		public float cellSize = 0.5f;

		// Token: 0x04000333 RID: 819
		[JsonMember]
		public float walkableHeight = 2f;

		// Token: 0x04000334 RID: 820
		[JsonMember]
		public float walkableClimb = 0.5f;

		// Token: 0x04000335 RID: 821
		[JsonMember]
		public float maxSlope = 30f;

		// Token: 0x04000336 RID: 822
		[JsonMember]
		public float maxEdgeLength = 20f;

		// Token: 0x04000337 RID: 823
		[JsonMember]
		public float minRegionSize = 3f;

		// Token: 0x04000338 RID: 824
		[JsonMember]
		public int editorTileSize = 128;

		// Token: 0x04000339 RID: 825
		[JsonMember]
		public int tileSizeX = 128;

		// Token: 0x0400033A RID: 826
		[JsonMember]
		public int tileSizeZ = 128;

		// Token: 0x0400033B RID: 827
		[JsonMember]
		public bool useTiles = true;

		// Token: 0x0400033C RID: 828
		public bool scanEmptyGraph;

		// Token: 0x0400033D RID: 829
		[JsonMember]
		public RecastGraph.RelevantGraphSurfaceMode relevantGraphSurfaceMode;

		// Token: 0x0400033E RID: 830
		[JsonMember]
		public bool rasterizeColliders;

		// Token: 0x0400033F RID: 831
		[JsonMember]
		public bool rasterizeMeshes = true;

		// Token: 0x04000340 RID: 832
		[JsonMember]
		public bool rasterizeTerrain = true;

		// Token: 0x04000341 RID: 833
		[JsonMember]
		public bool rasterizeTrees = true;

		// Token: 0x04000342 RID: 834
		[JsonMember]
		public float colliderRasterizeDetail = 10f;

		// Token: 0x04000343 RID: 835
		[JsonMember]
		public LayerMask mask = -1;

		// Token: 0x04000344 RID: 836
		[JsonMember]
		public List<string> tagMask = new List<string>();

		// Token: 0x04000345 RID: 837
		[JsonMember]
		public int terrainSampleSize = 3;

		// Token: 0x04000346 RID: 838
		[JsonMember]
		public Vector3 rotation;

		// Token: 0x04000347 RID: 839
		[JsonMember]
		public Vector3 forcedBoundsCenter;

		// Token: 0x04000348 RID: 840
		private Voxelize globalVox;

		// Token: 0x04000349 RID: 841
		public const int BorderVertexMask = 1;

		// Token: 0x0400034A RID: 842
		public const int BorderVertexOffset = 31;

		// Token: 0x0400034B RID: 843
		private List<NavmeshTile> stagingTiles = new List<NavmeshTile>();

		// Token: 0x02000137 RID: 311
		public enum RelevantGraphSurfaceMode
		{
			// Token: 0x04000739 RID: 1849
			DoNotRequire,
			// Token: 0x0400073A RID: 1850
			OnlyForCompletelyInsideTile,
			// Token: 0x0400073B RID: 1851
			RequireForAll
		}
	}
}
