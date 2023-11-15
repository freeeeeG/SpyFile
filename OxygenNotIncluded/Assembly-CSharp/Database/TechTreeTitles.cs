using System;
using UnityEngine;

namespace Database
{
	// Token: 0x02000D28 RID: 3368
	public class TechTreeTitles : ResourceSet<TechTreeTitle>
	{
		// Token: 0x06006A2B RID: 27179 RVA: 0x00295870 File Offset: 0x00293A70
		public TechTreeTitles(ResourceSet parent) : base("TreeTitles", parent)
		{
		}

		// Token: 0x06006A2C RID: 27180 RVA: 0x00295880 File Offset: 0x00293A80
		public void Load(TextAsset tree_file)
		{
			foreach (ResourceTreeNode resourceTreeNode in new ResourceTreeLoader<ResourceTreeNode>(tree_file))
			{
				if (string.Equals(resourceTreeNode.Id.Substring(0, 1), "_"))
				{
					new TechTreeTitle(resourceTreeNode.Id, this, Strings.Get("STRINGS.RESEARCH.TREES.TITLE" + resourceTreeNode.Id.ToUpper()), resourceTreeNode);
				}
			}
		}
	}
}
