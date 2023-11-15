using System;
using UnityEngine;

// Token: 0x02000B1F RID: 2847
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/InfoDescription")]
public class InfoDescription : KMonoBehaviour
{
	// Token: 0x1700066E RID: 1646
	// (get) Token: 0x0600579D RID: 22429 RVA: 0x00200FD8 File Offset: 0x001FF1D8
	// (set) Token: 0x0600579C RID: 22428 RVA: 0x00200FB1 File Offset: 0x001FF1B1
	public string DescriptionLocString
	{
		get
		{
			return this.descriptionLocString;
		}
		set
		{
			this.descriptionLocString = value;
			if (this.descriptionLocString != null)
			{
				this.description = Strings.Get(this.descriptionLocString);
			}
		}
	}

	// Token: 0x0600579E RID: 22430 RVA: 0x00200FE0 File Offset: 0x001FF1E0
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (!string.IsNullOrEmpty(this.nameLocString))
		{
			this.displayName = Strings.Get(this.nameLocString);
		}
		if (!string.IsNullOrEmpty(this.descriptionLocString))
		{
			this.description = Strings.Get(this.descriptionLocString);
		}
	}

	// Token: 0x04003B39 RID: 15161
	public string nameLocString = "";

	// Token: 0x04003B3A RID: 15162
	private string descriptionLocString = "";

	// Token: 0x04003B3B RID: 15163
	public string description;

	// Token: 0x04003B3C RID: 15164
	public string effect = "";

	// Token: 0x04003B3D RID: 15165
	public string displayName;
}
