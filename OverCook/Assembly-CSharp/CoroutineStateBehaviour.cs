using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200012E RID: 302
public class CoroutineStateBehaviour : StateMachineBehaviourEx
{
	// Token: 0x0600057F RID: 1407 RVA: 0x0002A50D File Offset: 0x0002890D
	protected virtual void OnEnter(Animator _animator, AnimatorStateInfo _stateInfo, int _layerIndex)
	{
	}

	// Token: 0x06000580 RID: 1408 RVA: 0x0002A50F File Offset: 0x0002890F
	protected virtual void OnUpdate(Animator _animator, AnimatorStateInfo _animatorStateInfo, int _layerIndex)
	{
	}

	// Token: 0x06000581 RID: 1409 RVA: 0x0002A511 File Offset: 0x00028911
	protected virtual void OnExit(Animator _animator, AnimatorStateInfo _stateInfo, int _layerIndex)
	{
	}

	// Token: 0x06000582 RID: 1410 RVA: 0x0002A514 File Offset: 0x00028914
	protected virtual IEnumerator Run(Animator _animator, AnimatorStateInfo _stateInfo, int _layerIndex)
	{
		yield return null;
		yield break;
	}

	// Token: 0x06000583 RID: 1411 RVA: 0x0002A528 File Offset: 0x00028928
	protected virtual void OnRunComplete()
	{
	}

	// Token: 0x06000584 RID: 1412 RVA: 0x0002A52A File Offset: 0x0002892A
	public override void OnStateEnter(Animator _animator, AnimatorStateInfo _stateInfo, int _layerIndex)
	{
		this.OnEnter(_animator, _stateInfo, _layerIndex);
		this.m_runCo = this.Run(_animator, _stateInfo, _layerIndex);
	}

	// Token: 0x06000585 RID: 1413 RVA: 0x0002A544 File Offset: 0x00028944
	public override void OnStateUpdate(Animator _animator, AnimatorStateInfo _animatorStateInfo, int _layerIndex)
	{
		if (this.m_runCo != null && !this.m_runCo.MoveNext())
		{
			this.m_runCo = null;
			this.OnRunComplete();
		}
		this.OnUpdate(_animator, _animatorStateInfo, _layerIndex);
	}

	// Token: 0x06000586 RID: 1414 RVA: 0x0002A577 File Offset: 0x00028977
	public override void OnStateExit(Animator _animator, AnimatorStateInfo _stateInfo, int _layerIndex)
	{
		this.OnExit(_animator, _stateInfo, _layerIndex);
	}

	// Token: 0x040004AC RID: 1196
	private IEnumerator m_runCo;
}
