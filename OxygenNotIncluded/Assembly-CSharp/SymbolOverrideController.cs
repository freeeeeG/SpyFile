using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200045E RID: 1118
[AddComponentMenu("KMonoBehaviour/scripts/SymbolOverrideController")]
public class SymbolOverrideController : KMonoBehaviour
{
	// Token: 0x170000C8 RID: 200
	// (get) Token: 0x0600187C RID: 6268 RVA: 0x0007F098 File Offset: 0x0007D298
	public SymbolOverrideController.SymbolEntry[] GetSymbolOverrides
	{
		get
		{
			return this.symbolOverrides.ToArray();
		}
	}

	// Token: 0x170000C9 RID: 201
	// (get) Token: 0x0600187D RID: 6269 RVA: 0x0007F0A5 File Offset: 0x0007D2A5
	// (set) Token: 0x0600187E RID: 6270 RVA: 0x0007F0AD File Offset: 0x0007D2AD
	public int version { get; private set; }

	// Token: 0x0600187F RID: 6271 RVA: 0x0007F0B8 File Offset: 0x0007D2B8
	protected override void OnPrefabInit()
	{
		this.animController = base.GetComponent<KBatchedAnimController>();
		DebugUtil.Assert(base.GetComponent<KBatchedAnimController>() != null, "SymbolOverrideController requires KBatchedAnimController");
		DebugUtil.Assert(base.GetComponent<KBatchedAnimController>().usingNewSymbolOverrideSystem, "SymbolOverrideController requires usingNewSymbolOverrideSystem to be set to true. Try adding the component by calling: SymbolOverrideControllerUtil.AddToPrefab");
		for (int i = 0; i < this.symbolOverrides.Count; i++)
		{
			SymbolOverrideController.SymbolEntry symbolEntry = this.symbolOverrides[i];
			symbolEntry.sourceSymbol = KAnimBatchManager.Instance().GetBatchGroupData(symbolEntry.sourceSymbolBatchTag).GetSymbol(symbolEntry.sourceSymbolId);
			this.symbolOverrides[i] = symbolEntry;
		}
		this.atlases = new KAnimBatch.AtlasList(0, KAnimBatchManager.MaxAtlasesByMaterialType[(int)this.animController.materialType]);
		this.faceGraph = base.GetComponent<FaceGraph>();
	}

	// Token: 0x06001880 RID: 6272 RVA: 0x0007F17C File Offset: 0x0007D37C
	public int AddSymbolOverride(HashedString target_symbol, KAnim.Build.Symbol source_symbol, int priority = 0)
	{
		if (source_symbol == null)
		{
			throw new Exception("NULL source symbol when overriding: " + target_symbol.ToString());
		}
		SymbolOverrideController.SymbolEntry symbolEntry = new SymbolOverrideController.SymbolEntry
		{
			targetSymbol = target_symbol,
			sourceSymbol = source_symbol,
			sourceSymbolId = new HashedString(source_symbol.hash.HashValue),
			sourceSymbolBatchTag = source_symbol.build.batchTag,
			priority = priority
		};
		int num = this.GetSymbolOverrideIdx(target_symbol, priority);
		if (num >= 0)
		{
			this.symbolOverrides[num] = symbolEntry;
		}
		else
		{
			num = this.symbolOverrides.Count;
			this.symbolOverrides.Add(symbolEntry);
		}
		this.MarkDirty();
		return num;
	}

	// Token: 0x06001881 RID: 6273 RVA: 0x0007F230 File Offset: 0x0007D430
	public bool RemoveSymbolOverride(HashedString target_symbol, int priority = 0)
	{
		for (int i = 0; i < this.symbolOverrides.Count; i++)
		{
			SymbolOverrideController.SymbolEntry symbolEntry = this.symbolOverrides[i];
			if (symbolEntry.targetSymbol == target_symbol && symbolEntry.priority == priority)
			{
				this.symbolOverrides.RemoveAt(i);
				this.MarkDirty();
				return true;
			}
		}
		return false;
	}

	// Token: 0x06001882 RID: 6274 RVA: 0x0007F28C File Offset: 0x0007D48C
	public void RemoveAllSymbolOverrides(int priority = 0)
	{
		this.symbolOverrides.RemoveAll((SymbolOverrideController.SymbolEntry x) => x.priority >= priority);
		this.MarkDirty();
	}

	// Token: 0x06001883 RID: 6275 RVA: 0x0007F2C4 File Offset: 0x0007D4C4
	public int GetSymbolOverrideIdx(HashedString target_symbol, int priority = 0)
	{
		for (int i = 0; i < this.symbolOverrides.Count; i++)
		{
			SymbolOverrideController.SymbolEntry symbolEntry = this.symbolOverrides[i];
			if (symbolEntry.targetSymbol == target_symbol && symbolEntry.priority == priority)
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x06001884 RID: 6276 RVA: 0x0007F30E File Offset: 0x0007D50E
	public int GetAtlasIdx(Texture2D atlas)
	{
		return this.atlases.GetAtlasIdx(atlas);
	}

	// Token: 0x06001885 RID: 6277 RVA: 0x0007F31C File Offset: 0x0007D51C
	public void ApplyOverrides()
	{
		if (this.requiresSorting)
		{
			this.symbolOverrides.Sort((SymbolOverrideController.SymbolEntry x, SymbolOverrideController.SymbolEntry y) => x.priority - y.priority);
			this.requiresSorting = false;
		}
		KAnimBatch batch = this.animController.GetBatch();
		DebugUtil.Assert(batch != null);
		KBatchGroupData batchGroupData = KAnimBatchManager.Instance().GetBatchGroupData(this.animController.batchGroupID);
		int count = batch.atlases.Count;
		this.atlases.Clear(count);
		DictionaryPool<HashedString, Pair<int, int>, SymbolOverrideController>.PooledDictionary pooledDictionary = DictionaryPool<HashedString, Pair<int, int>, SymbolOverrideController>.Allocate();
		ListPool<SymbolOverrideController.SymbolEntry, SymbolOverrideController>.PooledList pooledList = ListPool<SymbolOverrideController.SymbolEntry, SymbolOverrideController>.Allocate();
		for (int i = 0; i < this.symbolOverrides.Count; i++)
		{
			SymbolOverrideController.SymbolEntry symbolEntry = this.symbolOverrides[i];
			Pair<int, int> pair;
			if (pooledDictionary.TryGetValue(symbolEntry.targetSymbol, out pair))
			{
				int first = pair.first;
				if (symbolEntry.priority > first)
				{
					int second = pair.second;
					pooledDictionary[symbolEntry.targetSymbol] = new Pair<int, int>(symbolEntry.priority, second);
					pooledList[second] = symbolEntry;
				}
			}
			else
			{
				pooledDictionary[symbolEntry.targetSymbol] = new Pair<int, int>(symbolEntry.priority, pooledList.Count);
				pooledList.Add(symbolEntry);
			}
		}
		DictionaryPool<KAnim.Build, SymbolOverrideController.BatchGroupInfo, SymbolOverrideController>.PooledDictionary pooledDictionary2 = DictionaryPool<KAnim.Build, SymbolOverrideController.BatchGroupInfo, SymbolOverrideController>.Allocate();
		for (int j = 0; j < pooledList.Count; j++)
		{
			SymbolOverrideController.SymbolEntry symbolEntry2 = pooledList[j];
			SymbolOverrideController.BatchGroupInfo batchGroupInfo;
			if (!pooledDictionary2.TryGetValue(symbolEntry2.sourceSymbol.build, out batchGroupInfo))
			{
				batchGroupInfo = new SymbolOverrideController.BatchGroupInfo
				{
					build = symbolEntry2.sourceSymbol.build,
					data = KAnimBatchManager.Instance().GetBatchGroupData(symbolEntry2.sourceSymbol.build.batchTag)
				};
				Texture2D texture = symbolEntry2.sourceSymbol.build.GetTexture(0);
				int num = batch.atlases.GetAtlasIdx(texture);
				if (num < 0)
				{
					num = this.atlases.Add(texture);
				}
				batchGroupInfo.atlasIdx = num;
				pooledDictionary2[batchGroupInfo.build] = batchGroupInfo;
			}
			KAnim.Build.Symbol symbol = batchGroupData.GetSymbol(symbolEntry2.targetSymbol);
			if (symbol != null)
			{
				this.animController.SetSymbolOverrides(symbol.firstFrameIdx, symbol.numFrames, batchGroupInfo.atlasIdx, batchGroupInfo.data, symbolEntry2.sourceSymbol.firstFrameIdx, symbolEntry2.sourceSymbol.numFrames);
			}
		}
		pooledDictionary2.Recycle();
		pooledList.Recycle();
		pooledDictionary.Recycle();
		if (this.faceGraph != null)
		{
			this.faceGraph.ApplyShape();
		}
	}

	// Token: 0x06001886 RID: 6278 RVA: 0x0007F5B8 File Offset: 0x0007D7B8
	public void ApplyAtlases()
	{
		KAnimBatch batch = this.animController.GetBatch();
		this.atlases.Apply(batch.matProperties);
	}

	// Token: 0x06001887 RID: 6279 RVA: 0x0007F5E2 File Offset: 0x0007D7E2
	public KAnimBatch.AtlasList GetAtlasList()
	{
		return this.atlases;
	}

	// Token: 0x06001888 RID: 6280 RVA: 0x0007F5EC File Offset: 0x0007D7EC
	public void MarkDirty()
	{
		if (this.animController != null)
		{
			this.animController.SetDirty();
		}
		int version = this.version + 1;
		this.version = version;
		this.requiresSorting = true;
	}

	// Token: 0x04000D80 RID: 3456
	public bool applySymbolOverridesEveryFrame;

	// Token: 0x04000D81 RID: 3457
	[SerializeField]
	private List<SymbolOverrideController.SymbolEntry> symbolOverrides = new List<SymbolOverrideController.SymbolEntry>();

	// Token: 0x04000D82 RID: 3458
	private KAnimBatch.AtlasList atlases;

	// Token: 0x04000D83 RID: 3459
	private KBatchedAnimController animController;

	// Token: 0x04000D84 RID: 3460
	private FaceGraph faceGraph;

	// Token: 0x04000D86 RID: 3462
	private bool requiresSorting;

	// Token: 0x020010DE RID: 4318
	[Serializable]
	public struct SymbolEntry
	{
		// Token: 0x04005A52 RID: 23122
		public HashedString targetSymbol;

		// Token: 0x04005A53 RID: 23123
		[NonSerialized]
		public KAnim.Build.Symbol sourceSymbol;

		// Token: 0x04005A54 RID: 23124
		public HashedString sourceSymbolId;

		// Token: 0x04005A55 RID: 23125
		public HashedString sourceSymbolBatchTag;

		// Token: 0x04005A56 RID: 23126
		public int priority;
	}

	// Token: 0x020010DF RID: 4319
	private struct SymbolToOverride
	{
		// Token: 0x04005A57 RID: 23127
		public KAnim.Build.Symbol sourceSymbol;

		// Token: 0x04005A58 RID: 23128
		public HashedString targetSymbol;

		// Token: 0x04005A59 RID: 23129
		public KBatchGroupData data;

		// Token: 0x04005A5A RID: 23130
		public int atlasIdx;
	}

	// Token: 0x020010E0 RID: 4320
	private class BatchGroupInfo
	{
		// Token: 0x04005A5B RID: 23131
		public KAnim.Build build;

		// Token: 0x04005A5C RID: 23132
		public int atlasIdx;

		// Token: 0x04005A5D RID: 23133
		public KBatchGroupData data;
	}
}
