using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

// Token: 0x02000088 RID: 136
[CreateAssetMenu]
public class SettingsList : ScriptableObject
{
	// Token: 0x0600029C RID: 668 RVA: 0x0000A750 File Offset: 0x00008950
	private void Awake()
	{
		if (!Application.isPlaying)
		{
			return;
		}
		Addressables.LoadAssetsAsync<UnityEngine.Object>(this._settings, new Action<UnityEngine.Object>(this.OnLoaded));
	}

	// Token: 0x0600029D RID: 669 RVA: 0x00002191 File Offset: 0x00000391
	private void OnLoaded(UnityEngine.Object @object)
	{
	}

	// Token: 0x0400022D RID: 557
	[SerializeField]
	private AssetReference[] _settings;
}
