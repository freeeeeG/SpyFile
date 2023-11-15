using System;
using FX;
using Level;
using UnityEngine;

// Token: 0x0200008A RID: 138
public class SpawnObjectAtMapCleared : MonoBehaviour
{
	// Token: 0x060002A3 RID: 675 RVA: 0x0000A861 File Offset: 0x00008A61
	private void Start()
	{
		this._enemyWave.onClear += this.Spawn;
	}

	// Token: 0x060002A4 RID: 676 RVA: 0x0000A87C File Offset: 0x00008A7C
	private void Spawn()
	{
		this._effect.Spawn(this._gameObject.transform.position, 0f, 1f);
		this._gameObject.SetActive(true);
		this._enemyWave.onClear -= this.Spawn;
	}

	// Token: 0x060002A5 RID: 677 RVA: 0x0000A8D2 File Offset: 0x00008AD2
	private void OnDestroy()
	{
		this._enemyWave.onClear -= this.Spawn;
	}

	// Token: 0x04000231 RID: 561
	[SerializeField]
	private EnemyWave _enemyWave;

	// Token: 0x04000232 RID: 562
	[SerializeField]
	private EffectInfo _effect;

	// Token: 0x04000233 RID: 563
	[SerializeField]
	private GameObject _gameObject;
}
