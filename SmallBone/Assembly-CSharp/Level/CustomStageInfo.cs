using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Level
{
	// Token: 0x020004AC RID: 1196
	[CreateAssetMenu]
	public class CustomStageInfo : IStageInfo
	{
		// Token: 0x17000485 RID: 1157
		// (get) Token: 0x060016FC RID: 5884 RVA: 0x0004850D File Offset: 0x0004670D
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

		// Token: 0x17000486 RID: 1158
		// (get) Token: 0x060016FD RID: 5885 RVA: 0x0004851B File Offset: 0x0004671B
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

		// Token: 0x17000487 RID: 1159
		// (get) Token: 0x060016FE RID: 5886 RVA: 0x0004852B File Offset: 0x0004672B
		public override ParallaxBackground background
		{
			get
			{
				return this._background;
			}
		}

		// Token: 0x060016FF RID: 5887 RVA: 0x00048534 File Offset: 0x00046734
		[return: TupleElementNames(new string[]
		{
			"node1",
			"node2"
		})]
		private ValueTuple<PathNode, PathNode> GetPathAt(int pathIndex)
		{
			if (pathIndex >= this._maps.values.Length)
			{
				return new ValueTuple<PathNode, PathNode>(new PathNode(null, MapReward.Type.None, this._lastGate), PathNode.none);
			}
			return new ValueTuple<PathNode, PathNode>(this._maps.values[pathIndex], PathNode.none);
		}

		// Token: 0x06001700 RID: 5888 RVA: 0x00048580 File Offset: 0x00046780
		public override void Reset()
		{
			this.pathIndex = 0;
		}

		// Token: 0x06001701 RID: 5889 RVA: 0x00002191 File Offset: 0x00000391
		public override void Initialize()
		{
		}

		// Token: 0x06001702 RID: 5890 RVA: 0x00048589 File Offset: 0x00046789
		public override PathNode Next(NodeIndex nodeIndex)
		{
			this.nodeIndex = nodeIndex;
			this.pathIndex++;
			if (this.pathIndex >= this._maps.values.Length)
			{
				return null;
			}
			return this._maps.values[this.pathIndex];
		}

		// Token: 0x06001703 RID: 5891 RVA: 0x00002191 File Offset: 0x00000391
		public override void UpdateReferences()
		{
		}

		// Token: 0x04001422 RID: 5154
		[SerializeField]
		private ParallaxBackground _background;

		// Token: 0x04001423 RID: 5155
		[SerializeField]
		private Gate.Type _lastGate;

		// Token: 0x04001424 RID: 5156
		[SerializeField]
		private SerializablePathNode.Reorderable _maps;
	}
}
