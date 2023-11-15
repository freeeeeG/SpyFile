using System;
using Characters;
using FX.SpriteEffects;
using UnityEngine;

// Token: 0x0200000A RID: 10
public class AlphaThresholdSetter : MonoBehaviour
{
	// Token: 0x06000020 RID: 32 RVA: 0x00002CBF File Offset: 0x00000EBF
	private void Start()
	{
		this._character.spriteEffectStack.Add(new AlphaThreshold(this._priority, this._value, 0f));
	}

	// Token: 0x0400001B RID: 27
	[SerializeField]
	private Character _character;

	// Token: 0x0400001C RID: 28
	[SerializeField]
	private float _value = 0.8f;

	// Token: 0x0400001D RID: 29
	[SerializeField]
	private int _priority = -2147483647;
}
