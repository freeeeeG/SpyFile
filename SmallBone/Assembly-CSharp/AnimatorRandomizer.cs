using System;
using UnityEngine;

// Token: 0x02000031 RID: 49
public class AnimatorRandomizer : MonoBehaviour
{
	// Token: 0x060000B9 RID: 185 RVA: 0x00004656 File Offset: 0x00002856
	private void OnEnable()
	{
		this._animator.Play(0, 0, UnityEngine.Random.value);
	}

	// Token: 0x040000A4 RID: 164
	[GetComponent]
	[SerializeField]
	private Animator _animator;
}
