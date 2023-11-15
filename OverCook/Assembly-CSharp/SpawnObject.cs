using System;
using UnityEngine;

// Token: 0x02000114 RID: 276
public class SpawnObject : StateMachineBehaviour
{
	// Token: 0x06000516 RID: 1302 RVA: 0x000292E3 File Offset: 0x000276E3
	public override void OnStateEnter(Animator _animator, AnimatorStateInfo _stateInfo, int _layerIndex)
	{
		this.m_spanwedObject.Instantiate(_animator.transform.position, Quaternion.identity);
	}

	// Token: 0x0400047B RID: 1147
	[SerializeField]
	private GameObject m_spanwedObject;
}
