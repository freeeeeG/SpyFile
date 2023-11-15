using System;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Characters.Movements;
using Runnables;
using UnityEngine;

namespace Characters.AI.DarkHero
{
	// Token: 0x02001226 RID: 4646
	public sealed class StateController : MonoBehaviour
	{
		// Token: 0x06005B17 RID: 23319 RVA: 0x0010DA7C File Offset: 0x0010BC7C
		private void Awake()
		{
			if (this._overrider == null)
			{
				this._overrider = new AnimationClipOverrider(this._baseAnimator);
				this._overrider.Override("EmptyIdle", this._idleClip);
				this._overrider.Override("EmptyWalk", this._walkClip);
				this._overrider.Override("EmptyJumpUp", this._jumpClip);
				this._overrider.Override("EmptyJumpDown", this._fallClip);
				this._overrider.Override("EmptyJumpDownLoop", this._fallRepeatClip);
			}
		}

		// Token: 0x06005B18 RID: 23320 RVA: 0x0010DB10 File Offset: 0x0010BD10
		private void Update()
		{
			this.UpdateStep();
		}

		// Token: 0x06005B19 RID: 23321 RVA: 0x0010DB18 File Offset: 0x0010BD18
		private void UpdateStep()
		{
			if (this._owner == null)
			{
				return;
			}
			if (this._nextStep >= this._stepHealthConditions.Length)
			{
				return;
			}
			if (this._tree.ExecutionStatus != TaskStatus.Running)
			{
				return;
			}
			if (this._owner.health.percent * 100.0 <= (double)this._stepHealthConditions[this._nextStep])
			{
				this._nextStep++;
				this._behaviorTreeCommunicator.SetVariable<SharedInt>(this.stepName, this._nextStep);
			}
		}

		// Token: 0x06005B1A RID: 23322 RVA: 0x0010DBA8 File Offset: 0x0010BDA8
		private void Start()
		{
			this._owner.movement.controller.lastStandingMask = Layers.footholdMask;
		}

		// Token: 0x06005B1B RID: 23323 RVA: 0x0010DBC4 File Offset: 0x0010BDC4
		private void OnDestroy()
		{
			this._overrider.Dispose();
			this._baseAnimator = null;
			this._idleClip = null;
			this._walkClip = null;
			this._jumpClip = null;
			this._fallClip = null;
			this._fallRepeatClip = null;
		}

		// Token: 0x06005B1C RID: 23324 RVA: 0x0010DBFC File Offset: 0x0010BDFC
		public void OnEnterGroundState()
		{
			if (this._lastState == StateController.State.Ground)
			{
				return;
			}
			this._lastState = StateController.State.Ground;
			this._characterAnimation.DetachOverrider(this._overrider);
			this._owner.movement.configs.Remove(this._inWallMovementConfig);
			this._onEnterGroundState.Run();
		}

		// Token: 0x06005B1D RID: 23325 RVA: 0x0010DC52 File Offset: 0x0010BE52
		public void OnExitGroundState()
		{
			this._onExitGroundState.Run();
		}

		// Token: 0x06005B1E RID: 23326 RVA: 0x0010DC60 File Offset: 0x0010BE60
		public void OnEnterWallState()
		{
			if (this._lastState == StateController.State.Wall)
			{
				return;
			}
			this._lastState = StateController.State.Wall;
			this._onEnterWallState.Run();
			this._characterAnimation.AttachOverrider(this._overrider);
			this._owner.movement.configs.Add(int.MaxValue, this._inWallMovementConfig);
		}

		// Token: 0x06005B1F RID: 23327 RVA: 0x0010DCBA File Offset: 0x0010BEBA
		public void OnExitWallState()
		{
			this._onExitWallState.Run();
		}

		// Token: 0x04004979 RID: 18809
		[SerializeField]
		private Character _owner;

		// Token: 0x0400497A RID: 18810
		[SerializeField]
		private BehaviorTree _tree;

		// Token: 0x0400497B RID: 18811
		[SerializeField]
		private BehaviorDesignerCommunicator _behaviorTreeCommunicator;

		// Token: 0x0400497C RID: 18812
		[SerializeField]
		private CharacterAnimation _characterAnimation;

		// Token: 0x0400497D RID: 18813
		[SerializeField]
		private RuntimeAnimatorController _baseAnimator;

		// Token: 0x0400497E RID: 18814
		[SerializeField]
		private AnimationClip _idleClip;

		// Token: 0x0400497F RID: 18815
		[SerializeField]
		private AnimationClip _walkClip;

		// Token: 0x04004980 RID: 18816
		[SerializeField]
		private AnimationClip _jumpClip;

		// Token: 0x04004981 RID: 18817
		[SerializeField]
		private AnimationClip _fallClip;

		// Token: 0x04004982 RID: 18818
		[SerializeField]
		private AnimationClip _fallRepeatClip;

		// Token: 0x04004983 RID: 18819
		[SerializeField]
		private Movement.Config _inWallMovementConfig;

		// Token: 0x04004984 RID: 18820
		[Runnable.SubcomponentAttribute]
		[SerializeField]
		private Runnable.Subcomponents _onEnterGroundState;

		// Token: 0x04004985 RID: 18821
		[Runnable.SubcomponentAttribute]
		[SerializeField]
		private Runnable.Subcomponents _onExitGroundState;

		// Token: 0x04004986 RID: 18822
		[Runnable.SubcomponentAttribute]
		[SerializeField]
		private Runnable.Subcomponents _onEnterWallState;

		// Token: 0x04004987 RID: 18823
		[Runnable.SubcomponentAttribute]
		[SerializeField]
		private Runnable.Subcomponents _onExitWallState;

		// Token: 0x04004988 RID: 18824
		private AnimationClipOverrider _overrider;

		// Token: 0x04004989 RID: 18825
		private StateController.State _lastState;

		// Token: 0x0400498A RID: 18826
		[SerializeField]
		private int[] _stepHealthConditions;

		// Token: 0x0400498B RID: 18827
		private int _nextStep;

		// Token: 0x0400498C RID: 18828
		private readonly string stepName = "Step";

		// Token: 0x02001227 RID: 4647
		public enum State
		{
			// Token: 0x0400498E RID: 18830
			None,
			// Token: 0x0400498F RID: 18831
			Ground,
			// Token: 0x04004990 RID: 18832
			Wall
		}
	}
}
