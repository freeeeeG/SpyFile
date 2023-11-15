using System;
using System.Runtime.CompilerServices;
using GameResources;
using UnityEngine;

namespace Level
{
	// Token: 0x0200052D RID: 1325
	public class TestStageInfo : IStageInfo
	{
		// Token: 0x1700053E RID: 1342
		// (get) Token: 0x06001A07 RID: 6663 RVA: 0x00051BA3 File Offset: 0x0004FDA3
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
				return new ValueTuple<PathNode, PathNode>(this._path, PathNode.none);
			}
		}

		// Token: 0x1700053F RID: 1343
		// (get) Token: 0x06001A08 RID: 6664 RVA: 0x00051BA3 File Offset: 0x0004FDA3
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
				return new ValueTuple<PathNode, PathNode>(this._path, PathNode.none);
			}
		}

		// Token: 0x17000540 RID: 1344
		// (get) Token: 0x06001A09 RID: 6665 RVA: 0x00051BB5 File Offset: 0x0004FDB5
		public override ParallaxBackground background
		{
			get
			{
				return this._background;
			}
		}

		// Token: 0x06001A0A RID: 6666 RVA: 0x00002191 File Offset: 0x00000391
		public override void Reset()
		{
		}

		// Token: 0x06001A0B RID: 6667 RVA: 0x00051BBD File Offset: 0x0004FDBD
		public override void Initialize()
		{
			this._path = new PathNode(MapReference.FromPath("Assets/Level/Test/mapToTest.prefab"), MapReward.Type.Head, Gate.Type.Grave);
		}

		// Token: 0x06001A0C RID: 6668 RVA: 0x00051BD6 File Offset: 0x0004FDD6
		public override PathNode Next(NodeIndex nodeIndex)
		{
			return this._path;
		}

		// Token: 0x06001A0D RID: 6669 RVA: 0x00002191 File Offset: 0x00000391
		public override void UpdateReferences()
		{
		}

		// Token: 0x040016CC RID: 5836
		[SerializeField]
		private ParallaxBackground _background;

		// Token: 0x040016CD RID: 5837
		private PathNode _path;
	}
}
