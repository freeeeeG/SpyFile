using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000A53 RID: 2643
public class DialogueAnimationState : CoroutineStateBehaviour
{
	// Token: 0x06003430 RID: 13360 RVA: 0x000F52AF File Offset: 0x000F36AF
	protected virtual void Awake()
	{
		this.m_completedTriggerHash = Animator.StringToHash(this.m_completedTrigger);
	}

	// Token: 0x06003431 RID: 13361 RVA: 0x000F52C2 File Offset: 0x000F36C2
	private void OnValidate()
	{
		this.m_dialogueFlowroutine.OnValidate();
	}

	// Token: 0x06003432 RID: 13362 RVA: 0x000F52CF File Offset: 0x000F36CF
	private void OnAnimatorEnable()
	{
		this.m_dialogueFlowroutine.OnEnable();
	}

	// Token: 0x06003433 RID: 13363 RVA: 0x000F52DC File Offset: 0x000F36DC
	private void OnAnimatorDisable()
	{
		this.m_dialogueFlowroutine.OnDisable();
	}

	// Token: 0x06003434 RID: 13364 RVA: 0x000F52EC File Offset: 0x000F36EC
	protected override void OnEnter(Animator _animator, AnimatorStateInfo _stateInfo, int _layerIndex)
	{
		AnimatorCommunications animatorCommunications = _animator.gameObject.RequireComponent<AnimatorCommunications>();
		AnimatorCommunications animatorCommunications2 = animatorCommunications;
		animatorCommunications2.AnimatorEnabledCallback = (CallbackVoid)Delegate.Combine(animatorCommunications2.AnimatorEnabledCallback, new CallbackVoid(this.OnAnimatorEnable));
		AnimatorCommunications animatorCommunications3 = animatorCommunications;
		animatorCommunications3.AnimatorDisabledCallback = (CallbackVoid)Delegate.Combine(animatorCommunications3.AnimatorDisabledCallback, new CallbackVoid(this.OnAnimatorDisable));
	}

	// Token: 0x06003435 RID: 13365 RVA: 0x000F534C File Offset: 0x000F374C
	private static bool HasSplitPads()
	{
		GameInputConfig inputConfig = PlayerInputLookup.GetInputConfig();
		GameInputConfig.ConfigEntry[] playerConfigs = inputConfig.m_playerConfigs;
		for (int i = 0; i < playerConfigs.Length; i++)
		{
			if (playerConfigs[i].Side != PadSide.Both)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06003436 RID: 13366 RVA: 0x000F538C File Offset: 0x000F378C
	private static bool HasDifferentControllers()
	{
		PlayerManager playerManager = GameUtils.RequestManager<PlayerManager>();
		GamepadUser.ControlTypeEnum? controlTypeEnum = null;
		for (int i = 0; i < 4; i++)
		{
			GamepadUser user = playerManager.GetUser((EngagementSlot)i);
			if (user != null)
			{
				if (controlTypeEnum == null)
				{
					controlTypeEnum = new GamepadUser.ControlTypeEnum?(user.ControlType);
				}
				if (controlTypeEnum.Value != user.ControlType)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06003437 RID: 13367 RVA: 0x000F5400 File Offset: 0x000F3800
	protected override IEnumerator Run(Animator _animator, AnimatorStateInfo _stateInfo, int _layerIndex)
	{
		if (this.m_hasButtonlessAlternative && (DialogueAnimationState.HasDifferentControllers() || DialogueAnimationState.HasSplitPads()))
		{
			for (int i = 0; i < this.m_dialogueFlowroutine.DialogueScript.Length; i++)
			{
				string[] dialogueScript = this.m_dialogueFlowroutine.DialogueScript;
				if (!dialogueScript[i].Contains(".Buttonless"))
				{
					string[] array;
					int num;
					(array = dialogueScript)[num = i] = array[num] + ".Buttonless";
				}
			}
		}
		this.m_dialogueFlowroutine.Setup(this.m_anchor, this.m_pivot);
		IEnumerator enumerator = this.m_dialogueFlowroutine.Run();
		while (enumerator.MoveNext())
		{
			yield return null;
		}
		_animator.SetTrigger(this.m_completedTriggerHash);
		yield break;
	}

	// Token: 0x06003438 RID: 13368 RVA: 0x000F5424 File Offset: 0x000F3824
	protected override void OnExit(Animator _animator, AnimatorStateInfo _stateInfo, int _layerIndex)
	{
		this.m_dialogueFlowroutine.Shutdown();
		AnimatorCommunications animatorCommunications = _animator.gameObject.RequireComponent<AnimatorCommunications>();
		AnimatorCommunications animatorCommunications2 = animatorCommunications;
		animatorCommunications2.AnimatorEnabledCallback = (CallbackVoid)Delegate.Remove(animatorCommunications2.AnimatorEnabledCallback, new CallbackVoid(this.OnAnimatorEnable));
		AnimatorCommunications animatorCommunications3 = animatorCommunications;
		animatorCommunications3.AnimatorDisabledCallback = (CallbackVoid)Delegate.Remove(animatorCommunications3.AnimatorDisabledCallback, new CallbackVoid(this.OnAnimatorDisable));
	}

	// Token: 0x040029E3 RID: 10723
	[SerializeField]
	private string m_completedTrigger;

	// Token: 0x040029E4 RID: 10724
	[SerializeField]
	private DialogueFlowroutine m_dialogueFlowroutine = new DialogueFlowroutine();

	// Token: 0x040029E5 RID: 10725
	[SerializeField]
	private Vector2 m_anchor = 0.5f * Vector2.one;

	// Token: 0x040029E6 RID: 10726
	[SerializeField]
	private Vector2 m_pivot = 0.5f * Vector2.one;

	// Token: 0x040029E7 RID: 10727
	[SerializeField]
	private bool m_hasButtonlessAlternative;

	// Token: 0x040029E8 RID: 10728
	public int m_completedTriggerHash;

	// Token: 0x040029E9 RID: 10729
	private GameObject m_dialogueObject;
}
