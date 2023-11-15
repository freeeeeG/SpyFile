using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x020006CD RID: 1741
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/ChoreGroupManager")]
public class ChoreGroupManager : KMonoBehaviour, ISaveLoadable
{
	// Token: 0x06002F52 RID: 12114 RVA: 0x000F9B35 File Offset: 0x000F7D35
	public static void DestroyInstance()
	{
		ChoreGroupManager.instance = null;
	}

	// Token: 0x17000340 RID: 832
	// (get) Token: 0x06002F53 RID: 12115 RVA: 0x000F9B3D File Offset: 0x000F7D3D
	public List<Tag> DefaultForbiddenTagsList
	{
		get
		{
			return this.defaultForbiddenTagsList;
		}
	}

	// Token: 0x17000341 RID: 833
	// (get) Token: 0x06002F54 RID: 12116 RVA: 0x000F9B45 File Offset: 0x000F7D45
	public Dictionary<Tag, int> DefaultChorePermission
	{
		get
		{
			return this.defaultChorePermissions;
		}
	}

	// Token: 0x06002F55 RID: 12117 RVA: 0x000F9B50 File Offset: 0x000F7D50
	protected override void OnSpawn()
	{
		base.OnSpawn();
		ChoreGroupManager.instance = this;
		this.ConvertOldVersion();
		foreach (ChoreGroup choreGroup in Db.Get().ChoreGroups.resources)
		{
			if (!this.defaultChorePermissions.ContainsKey(choreGroup.Id.ToTag()))
			{
				this.defaultChorePermissions.Add(choreGroup.Id.ToTag(), 2);
			}
		}
	}

	// Token: 0x06002F56 RID: 12118 RVA: 0x000F9BE8 File Offset: 0x000F7DE8
	private void ConvertOldVersion()
	{
		foreach (Tag key in this.defaultForbiddenTagsList)
		{
			if (!this.defaultChorePermissions.ContainsKey(key))
			{
				this.defaultChorePermissions.Add(key, -1);
			}
			this.defaultChorePermissions[key] = 0;
		}
		this.defaultForbiddenTagsList.Clear();
	}

	// Token: 0x04001C24 RID: 7204
	public static ChoreGroupManager instance;

	// Token: 0x04001C25 RID: 7205
	[Serialize]
	private List<Tag> defaultForbiddenTagsList = new List<Tag>();

	// Token: 0x04001C26 RID: 7206
	[Serialize]
	private Dictionary<Tag, int> defaultChorePermissions = new Dictionary<Tag, int>();
}
