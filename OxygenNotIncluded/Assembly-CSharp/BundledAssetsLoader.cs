using System;
using System.IO;
using UnityEngine;

// Token: 0x020006C0 RID: 1728
public class BundledAssetsLoader : KMonoBehaviour
{
	// Token: 0x1700033D RID: 829
	// (get) Token: 0x06002F09 RID: 12041 RVA: 0x000F81F5 File Offset: 0x000F63F5
	// (set) Token: 0x06002F0A RID: 12042 RVA: 0x000F81FD File Offset: 0x000F63FD
	public BundledAssets Expansion1Assets { get; private set; }

	// Token: 0x06002F0B RID: 12043 RVA: 0x000F8208 File Offset: 0x000F6408
	protected override void OnPrefabInit()
	{
		BundledAssetsLoader.instance = this;
		global::Debug.Log("Expansion1: " + DlcManager.IsExpansion1Active().ToString());
		if (DlcManager.IsExpansion1Active())
		{
			global::Debug.Log("Loading Expansion1 assets from bundle");
			AssetBundle assetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, DlcManager.GetContentBundleName("EXPANSION1_ID")));
			global::Debug.Assert(assetBundle != null, "Expansion1 is Active but its asset bundle failed to load");
			GameObject gameObject = assetBundle.LoadAsset<GameObject>("Expansion1Assets");
			global::Debug.Assert(gameObject != null, "Could not load the Expansion1Assets prefab");
			this.Expansion1Assets = Util.KInstantiate(gameObject, base.gameObject, null).GetComponent<BundledAssets>();
		}
	}

	// Token: 0x04001BDC RID: 7132
	public static BundledAssetsLoader instance;
}
