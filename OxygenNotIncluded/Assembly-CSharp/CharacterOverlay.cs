using System;
using UnityEngine;

// Token: 0x02000AB2 RID: 2738
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/CharacterOverlay")]
public class CharacterOverlay : KMonoBehaviour
{
	// Token: 0x060053B4 RID: 21428 RVA: 0x001E2925 File Offset: 0x001E0B25
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.Register();
	}

	// Token: 0x060053B5 RID: 21429 RVA: 0x001E2933 File Offset: 0x001E0B33
	public void Register()
	{
		if (this.registered)
		{
			return;
		}
		this.registered = true;
		NameDisplayScreen.Instance.AddNewEntry(base.gameObject);
	}

	// Token: 0x040037EF RID: 14319
	public bool shouldShowName;

	// Token: 0x040037F0 RID: 14320
	private bool registered;
}
