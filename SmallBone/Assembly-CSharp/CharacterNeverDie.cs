using System;
using Characters;
using Services;
using Singletons;
using UnityEngine;

// Token: 0x02000091 RID: 145
public class CharacterNeverDie : MonoBehaviour
{
	// Token: 0x060002C6 RID: 710 RVA: 0x0000B110 File Offset: 0x00009310
	private void Start()
	{
		if (this._character == null)
		{
			this._character = Singleton<Service>.Instance.levelManager.player;
		}
		this._character.health.onDie += this.OnDie;
	}

	// Token: 0x060002C7 RID: 711 RVA: 0x0000B15C File Offset: 0x0000935C
	private void OnDie()
	{
		this._character.health.Heal(1.0, true);
	}

	// Token: 0x060002C8 RID: 712 RVA: 0x0000B179 File Offset: 0x00009379
	public void Remove()
	{
		this._character.health.onDie -= this.OnDie;
	}

	// Token: 0x04000253 RID: 595
	[SerializeField]
	private Character _character;
}
