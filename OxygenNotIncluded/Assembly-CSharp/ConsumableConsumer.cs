using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KSerialization;
using UnityEngine;

// Token: 0x020003D9 RID: 985
[AddComponentMenu("KMonoBehaviour/scripts/ConsumableConsumer")]
public class ConsumableConsumer : KMonoBehaviour
{
	// Token: 0x060014B0 RID: 5296 RVA: 0x0006D82C File Offset: 0x0006BA2C
	[OnDeserialized]
	[Obsolete]
	private void OnDeserialized()
	{
		if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 29))
		{
			this.forbiddenTagSet = new HashSet<Tag>(this.forbiddenTags);
			this.forbiddenTags = null;
		}
	}

	// Token: 0x060014B1 RID: 5297 RVA: 0x0006D868 File Offset: 0x0006BA68
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (ConsumerManager.instance != null)
		{
			this.forbiddenTagSet = new HashSet<Tag>(ConsumerManager.instance.DefaultForbiddenTagsList);
			return;
		}
		this.forbiddenTagSet = new HashSet<Tag>();
	}

	// Token: 0x060014B2 RID: 5298 RVA: 0x0006D8A0 File Offset: 0x0006BAA0
	public bool IsPermitted(string consumable_id)
	{
		Tag item = new Tag(consumable_id);
		return !this.forbiddenTagSet.Contains(item);
	}

	// Token: 0x060014B3 RID: 5299 RVA: 0x0006D8C4 File Offset: 0x0006BAC4
	public void SetPermitted(string consumable_id, bool is_allowed)
	{
		Tag item = new Tag(consumable_id);
		if (is_allowed)
		{
			this.forbiddenTagSet.Remove(item);
		}
		else
		{
			this.forbiddenTagSet.Add(item);
		}
		this.consumableRulesChanged.Signal();
	}

	// Token: 0x04000B2F RID: 2863
	[Obsolete("Deprecated, use forbiddenTagSet")]
	[Serialize]
	public Tag[] forbiddenTags;

	// Token: 0x04000B30 RID: 2864
	[Serialize]
	public HashSet<Tag> forbiddenTagSet;

	// Token: 0x04000B31 RID: 2865
	public System.Action consumableRulesChanged;
}
