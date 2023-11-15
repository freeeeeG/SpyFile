using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Level
{
	// Token: 0x0200051C RID: 1308
	[CreateAssetMenu]
	public class SequentialStageInfo : IStageInfo
	{
		// Token: 0x1700052F RID: 1327
		// (get) Token: 0x060019C5 RID: 6597 RVA: 0x00050D62 File Offset: 0x0004EF62
		[TupleElementNames(new string[]
		{
			"node1",
			"node2"
		})]
		public override ValueTuple<PathNode, PathNode> currentMapPath
		{
			[return: TupleElementNames(new string[]
			{
				"node1",
				"node2"
			})]
			get
			{
				return this.GetPathAt(this.pathIndex);
			}
		}

		// Token: 0x17000530 RID: 1328
		// (get) Token: 0x060019C6 RID: 6598 RVA: 0x00050D70 File Offset: 0x0004EF70
		[TupleElementNames(new string[]
		{
			"node1",
			"node2"
		})]
		public override ValueTuple<PathNode, PathNode> nextMapPath
		{
			[return: TupleElementNames(new string[]
			{
				"node1",
				"node2"
			})]
			get
			{
				return this.GetPathAt(this.pathIndex + 1);
			}
		}

		// Token: 0x17000531 RID: 1329
		// (get) Token: 0x060019C7 RID: 6599 RVA: 0x00050D80 File Offset: 0x0004EF80
		public override ParallaxBackground background
		{
			get
			{
				return this._background;
			}
		}

		// Token: 0x060019C8 RID: 6600 RVA: 0x00050D88 File Offset: 0x0004EF88
		[return: TupleElementNames(new string[]
		{
			"node1",
			"node2"
		})]
		private ValueTuple<PathNode, PathNode> GetPathAt(int pathIndex)
		{
			return new ValueTuple<PathNode, PathNode>(this._path[pathIndex], PathNode.none);
		}

		// Token: 0x060019C9 RID: 6601 RVA: 0x00048580 File Offset: 0x00046780
		public override void Reset()
		{
			this.pathIndex = 0;
		}

		// Token: 0x060019CA RID: 6602 RVA: 0x00050D9C File Offset: 0x0004EF9C
		public override void Initialize()
		{
			this._path = new PathNode[this.maps.Length + 1];
			for (int i = 0; i < this.maps.Length; i++)
			{
				this._path[i] = new PathNode(this.maps[i], MapReward.Type.None, Gate.Type.Normal);
			}
			this._path[this.maps.Length] = PathNode.none;
		}

		// Token: 0x060019CB RID: 6603 RVA: 0x00050DFC File Offset: 0x0004EFFC
		public override PathNode Next(NodeIndex nodeIndex)
		{
			this.nodeIndex = nodeIndex;
			this.pathIndex++;
			if (this.pathIndex >= this._path.Length)
			{
				return null;
			}
			return this._path[this.pathIndex];
		}

		// Token: 0x060019CC RID: 6604 RVA: 0x00002191 File Offset: 0x00000391
		public override void UpdateReferences()
		{
		}

		// Token: 0x04001695 RID: 5781
		[SerializeField]
		private ParallaxBackground _background;

		// Token: 0x04001696 RID: 5782
		private PathNode[] _path;
	}
}
