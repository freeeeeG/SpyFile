using System;
using Level;
using Services;
using Singletons;
using UnityEngine;

// Token: 0x0200006E RID: 110
public class PlayBackgroundMusicAtMapCleared : MonoBehaviour
{
	// Token: 0x06000203 RID: 515 RVA: 0x00008E56 File Offset: 0x00007056
	private void Start()
	{
		this._enemyWave.onClear += this.Spawn;
	}

	// Token: 0x06000204 RID: 516 RVA: 0x00008E70 File Offset: 0x00007070
	private void Spawn()
	{
		PersistentSingleton<SoundManager>.Instance.PlayBackgroundMusic(Singleton<Service>.Instance.levelManager.currentChapter.currentStage.music, 1f, true, true, false);
		this._enemyWave.onClear -= this.Spawn;
	}

	// Token: 0x06000205 RID: 517 RVA: 0x00008EBF File Offset: 0x000070BF
	private void OnDestroy()
	{
		this._enemyWave.onClear -= this.Spawn;
	}

	// Token: 0x040001BC RID: 444
	[SerializeField]
	private EnemyWave _enemyWave;
}
