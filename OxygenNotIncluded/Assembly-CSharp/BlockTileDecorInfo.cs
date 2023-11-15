using System;
using Rendering;
using UnityEngine;

// Token: 0x02000A42 RID: 2626
public class BlockTileDecorInfo : ScriptableObject
{
	// Token: 0x06004F16 RID: 20246 RVA: 0x001BFBDC File Offset: 0x001BDDDC
	public void PostProcess()
	{
		if (this.decor != null && this.atlas != null && this.atlas.items != null)
		{
			for (int i = 0; i < this.decor.Length; i++)
			{
				if (this.decor[i].variants != null && this.decor[i].variants.Length != 0)
				{
					for (int j = 0; j < this.decor[i].variants.Length; j++)
					{
						bool flag = false;
						foreach (TextureAtlas.Item item in this.atlas.items)
						{
							string text = item.name;
							int num = text.IndexOf("/");
							if (num != -1)
							{
								text = text.Substring(num + 1);
							}
							if (this.decor[i].variants[j].name == text)
							{
								this.decor[i].variants[j].atlasItem = item;
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							DebugUtil.LogErrorArgs(new object[]
							{
								base.name,
								"/",
								this.decor[i].name,
								"could not find ",
								this.decor[i].variants[j].name,
								"in",
								this.atlas.name
							});
						}
					}
				}
			}
		}
	}

	// Token: 0x04003378 RID: 13176
	public TextureAtlas atlas;

	// Token: 0x04003379 RID: 13177
	public TextureAtlas atlasSpec;

	// Token: 0x0400337A RID: 13178
	public int sortOrder;

	// Token: 0x0400337B RID: 13179
	public BlockTileDecorInfo.Decor[] decor;

	// Token: 0x020018D7 RID: 6359
	[Serializable]
	public struct ImageInfo
	{
		// Token: 0x0400731B RID: 29467
		public string name;

		// Token: 0x0400731C RID: 29468
		public Vector3 offset;

		// Token: 0x0400731D RID: 29469
		[NonSerialized]
		public TextureAtlas.Item atlasItem;
	}

	// Token: 0x020018D8 RID: 6360
	[Serializable]
	public struct Decor
	{
		// Token: 0x0400731E RID: 29470
		public string name;

		// Token: 0x0400731F RID: 29471
		[EnumFlags]
		public BlockTileRenderer.Bits requiredConnections;

		// Token: 0x04007320 RID: 29472
		[EnumFlags]
		public BlockTileRenderer.Bits forbiddenConnections;

		// Token: 0x04007321 RID: 29473
		public float probabilityCutoff;

		// Token: 0x04007322 RID: 29474
		public BlockTileDecorInfo.ImageInfo[] variants;

		// Token: 0x04007323 RID: 29475
		public int sortOrder;
	}
}
