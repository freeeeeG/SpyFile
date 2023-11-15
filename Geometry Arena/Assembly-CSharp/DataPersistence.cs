using System;
using UnityEngine;

// Token: 0x02000020 RID: 32
public class DataPersistence : MonoBehaviour
{
	// Token: 0x06000198 RID: 408 RVA: 0x0000AAD8 File Offset: 0x00008CD8
	private void Awake()
	{
		if (this.currentSceneType == EnumSceneType.UNINITED)
		{
			Debug.LogError("DataPresistence_Scene_Uninited");
			return;
		}
		if (DataPersistence.inst != null)
		{
			Debug.LogWarning("Inst!=null");
			Object.Destroy(base.gameObject);
			return;
		}
		DataPersistence.inst = this;
		Object.DontDestroyOnLoad(base.gameObject);
		Object.Instantiate<GameObject>(this.prefab_TempData);
		TempData.inst.currentSceneType = this.currentSceneType;
		Object.Instantiate<GameObject>(this.prefab_GameData);
		Object.Instantiate<GameObject>(this.prefab_PPS);
		Object.Instantiate<GameObject>(this.prefab_Stars);
		Object.Instantiate<GameObject>(this.prefab_BGMControl);
		Object.Instantiate<GameObject>(this.prefab_SqlManager);
		Object.Instantiate<GameObject>(this.prefab_SteamManager);
		Object.Instantiate<GameObject>(this.prefab_AssetManager);
	}

	// Token: 0x06000199 RID: 409 RVA: 0x00009B24 File Offset: 0x00007D24
	private void Start()
	{
		Setting.Inst.ApplySettingSuddenly();
	}

	// Token: 0x0400019B RID: 411
	public static DataPersistence inst;

	// Token: 0x0400019C RID: 412
	[SerializeField]
	private EnumSceneType currentSceneType = EnumSceneType.UNINITED;

	// Token: 0x0400019D RID: 413
	[Header("Prefabs")]
	[SerializeField]
	private GameObject prefab_TempData;

	// Token: 0x0400019E RID: 414
	[SerializeField]
	private GameObject prefab_GameData;

	// Token: 0x0400019F RID: 415
	[SerializeField]
	private GameObject prefab_PPS;

	// Token: 0x040001A0 RID: 416
	[SerializeField]
	private GameObject prefab_Stars;

	// Token: 0x040001A1 RID: 417
	[SerializeField]
	private GameObject prefab_BGMControl;

	// Token: 0x040001A2 RID: 418
	[SerializeField]
	private GameObject prefab_SqlManager;

	// Token: 0x040001A3 RID: 419
	[SerializeField]
	private GameObject prefab_SteamManager;

	// Token: 0x040001A4 RID: 420
	[SerializeField]
	private GameObject prefab_AssetManager;
}
