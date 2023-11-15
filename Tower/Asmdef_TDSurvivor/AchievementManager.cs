using System;
using UnityEngine;

// Token: 0x0200000A RID: 10
public class AchievementManager : Singleton<AchievementManager>
{
	// Token: 0x06000008 RID: 8 RVA: 0x0000208E File Offset: 0x0000028E
	private void OnEnable()
	{
		EventMgr.Register<eAchievementType>(eGameEvents.AchievementUnlock, new Action<eAchievementType>(this.OnAchievementUnlock));
	}

	// Token: 0x06000009 RID: 9 RVA: 0x000020A8 File Offset: 0x000002A8
	private void OnDisable()
	{
		EventMgr.Remove<eAchievementType>(eGameEvents.AchievementUnlock, new Action<eAchievementType>(this.OnAchievementUnlock));
	}

	// Token: 0x0600000A RID: 10 RVA: 0x000020C2 File Offset: 0x000002C2
	private void OnAchievementUnlock(eAchievementType type)
	{
		Debug.Log(string.Format("接收到成就解鎖: {0}", type));
		this.SetAchievementStatus(type, true);
	}

	// Token: 0x0600000B RID: 11 RVA: 0x000020E4 File Offset: 0x000002E4
	public void SetAchievementStatus(eAchievementType type, bool isDone)
	{
		int num = this.IsAchievementDone(type) ? 1 : 0;
		PlayerPrefs.SetInt(type.ToString(), isDone ? 1 : 0);
		if (num == 0 && isDone)
		{
			EventMgr.SendEvent<eAchievementType, string>(eGameEvents.UI_ShowAchievementUnlock, type, this.data.GetDescription(type));
		}
	}

	// Token: 0x0600000C RID: 12 RVA: 0x00002132 File Offset: 0x00000332
	public string GetAchievementDescription(eAchievementType type)
	{
		return this.data.GetDescription(type);
	}

	// Token: 0x0600000D RID: 13 RVA: 0x00002140 File Offset: 0x00000340
	public bool IsAchievementDone(eAchievementType type)
	{
		return 1 == PlayerPrefs.GetInt(type.ToString(), 0);
	}

	// Token: 0x0600000E RID: 14 RVA: 0x00002158 File Offset: 0x00000358
	[ContextMenu("重置所有成就")]
	private void ResetAchievements()
	{
		PlayerPrefs.DeleteAll();
	}

	// Token: 0x04000013 RID: 19
	[SerializeField]
	[Header("成就資料的Scriptable Object檔案")]
	private AchievementData data;
}
