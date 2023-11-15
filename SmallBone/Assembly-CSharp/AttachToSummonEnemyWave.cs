using System;
using Characters;
using Characters.Operations;
using Level;
using UnityEngine;

// Token: 0x0200000C RID: 12
public class AttachToSummonEnemyWave : CharacterOperation
{
	// Token: 0x06000024 RID: 36 RVA: 0x00002D17 File Offset: 0x00000F17
	public override void Run(Character owner)
	{
		Map.Instance.waveContainer.AttachToSummonEnemyWave(this._character);
	}

	// Token: 0x0400001F RID: 31
	[SerializeField]
	private Character _character;
}
