using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000156 RID: 342
public class PersistentObject : MonoBehaviour
{
	// Token: 0x06000600 RID: 1536 RVA: 0x0002BBF8 File Offset: 0x00029FF8
	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		this.m_destroyOnLoad = this.ShouldDestroyOnNextLoad(SceneManager.GetActiveScene().name);
		SceneManager.sceneLoaded += this.OnSceneLoaded;
	}

	// Token: 0x06000601 RID: 1537 RVA: 0x0002BC3A File Offset: 0x0002A03A
	private void OnDestroy()
	{
		SceneManager.sceneLoaded -= this.OnSceneLoaded;
	}

	// Token: 0x06000602 RID: 1538 RVA: 0x0002BC4D File Offset: 0x0002A04D
	public void AddPersistingLevel(string _levelName)
	{
		if (!this.m_persistingLevels.Contains(_levelName))
		{
			ArrayUtils.PushBack<string>(ref this.m_persistingLevels, _levelName);
		}
	}

	// Token: 0x06000603 RID: 1539 RVA: 0x0002BC6C File Offset: 0x0002A06C
	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		if (this.m_destroyOnLoad)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		this.m_destroyOnLoad = this.ShouldDestroyOnNextLoad(SceneManager.GetActiveScene().name);
	}

	// Token: 0x06000604 RID: 1540 RVA: 0x0002BCA8 File Offset: 0x0002A0A8
	private bool ShouldDestroyOnNextLoad(string _currentLevel)
	{
		return !this.m_persistingLevels.Contains(_currentLevel) && (this.m_unpersistingLevels.Contains(_currentLevel) || this.m_defaultBehaviour == PersistentObject.PersistType.DontPersist);
	}

	// Token: 0x04000507 RID: 1287
	[SerializeField]
	public PersistentObject.PersistType m_defaultBehaviour;

	// Token: 0x04000508 RID: 1288
	[SerializeField]
	[SceneName]
	private string[] m_persistingLevels = new string[0];

	// Token: 0x04000509 RID: 1289
	[SerializeField]
	[SceneName]
	private string[] m_unpersistingLevels = new string[0];

	// Token: 0x0400050A RID: 1290
	private bool m_destroyOnLoad;

	// Token: 0x02000157 RID: 343
	public enum PersistType
	{
		// Token: 0x0400050C RID: 1292
		Persist,
		// Token: 0x0400050D RID: 1293
		DontPersist
	}
}
