using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000DF RID: 223
public class SoundAssetDataLoader : MonoBehaviour
{
	// Token: 0x06000542 RID: 1346 RVA: 0x0001522B File Offset: 0x0001342B
	private void Awake()
	{
		SoundManager.RegisterSoundAssetData(this.list_SoundAssetDatas);
	}

	// Token: 0x06000543 RID: 1347 RVA: 0x00015238 File Offset: 0x00013438
	private void OnDestroy()
	{
		SoundManager.UnregisterSoundAssetData(this.list_SoundAssetDatas);
	}

	// Token: 0x040004F1 RID: 1265
	[SerializeField]
	[Header("把要載入的聲音設定檔放在這邊")]
	private List<SoundAssetData> list_SoundAssetDatas;
}
