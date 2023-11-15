using System;
using UnityEngine;

namespace InControl
{
	// Token: 0x020002C3 RID: 707
	public class TouchSwipeControl : TouchControl
	{
		// Token: 0x06000E64 RID: 3684 RVA: 0x0004655F File Offset: 0x0004495F
		public override void CreateControl()
		{
		}

		// Token: 0x06000E65 RID: 3685 RVA: 0x00046561 File Offset: 0x00044961
		public override void DestroyControl()
		{
			if (this.currentTouch != null)
			{
				this.TouchEnded(this.currentTouch);
				this.currentTouch = null;
			}
		}

		// Token: 0x06000E66 RID: 3686 RVA: 0x00046581 File Offset: 0x00044981
		public override void ConfigureControl()
		{
			this.worldActiveArea = TouchManager.ConvertToWorld(this.activeArea, this.areaUnitType);
		}

		// Token: 0x06000E67 RID: 3687 RVA: 0x0004659A File Offset: 0x0004499A
		public override void DrawGizmos()
		{
			Utility.DrawRectGizmo(this.worldActiveArea, Color.yellow);
		}

		// Token: 0x06000E68 RID: 3688 RVA: 0x000465AC File Offset: 0x000449AC
		private void Update()
		{
			if (this.dirty)
			{
				this.ConfigureControl();
				this.dirty = false;
			}
		}

		// Token: 0x06000E69 RID: 3689 RVA: 0x000465C8 File Offset: 0x000449C8
		public override void SubmitControlState(ulong updateTick, float deltaTime)
		{
			Vector2 value = TouchControl.SnapTo(this.currentVector, this.snapAngles);
			base.SubmitAnalogValue(this.target, value, 0f, 1f, updateTick, deltaTime);
			base.SubmitButtonState(this.upTarget, this.fireButtonTarget && this.nextButtonTarget == this.upTarget, updateTick, deltaTime);
			base.SubmitButtonState(this.downTarget, this.fireButtonTarget && this.nextButtonTarget == this.downTarget, updateTick, deltaTime);
			base.SubmitButtonState(this.leftTarget, this.fireButtonTarget && this.nextButtonTarget == this.leftTarget, updateTick, deltaTime);
			base.SubmitButtonState(this.rightTarget, this.fireButtonTarget && this.nextButtonTarget == this.rightTarget, updateTick, deltaTime);
			base.SubmitButtonState(this.tapTarget, this.fireButtonTarget && this.nextButtonTarget == this.tapTarget, updateTick, deltaTime);
			if (this.fireButtonTarget && this.nextButtonTarget != TouchControl.ButtonTarget.None)
			{
				this.fireButtonTarget = !this.oneSwipePerTouch;
				this.lastButtonTarget = this.nextButtonTarget;
				this.nextButtonTarget = TouchControl.ButtonTarget.None;
			}
		}

		// Token: 0x06000E6A RID: 3690 RVA: 0x00046710 File Offset: 0x00044B10
		public override void CommitControlState(ulong updateTick, float deltaTime)
		{
			base.CommitAnalog(this.target);
			base.CommitButton(this.upTarget);
			base.CommitButton(this.downTarget);
			base.CommitButton(this.leftTarget);
			base.CommitButton(this.rightTarget);
			base.CommitButton(this.tapTarget);
		}

		// Token: 0x06000E6B RID: 3691 RVA: 0x00046768 File Offset: 0x00044B68
		public override void TouchBegan(Touch touch)
		{
			if (this.currentTouch != null)
			{
				return;
			}
			this.beganPosition = TouchManager.ScreenToWorldPoint(touch.position);
			if (this.worldActiveArea.Contains(this.beganPosition))
			{
				this.lastPosition = this.beganPosition;
				this.currentTouch = touch;
				this.currentVector = Vector2.zero;
				this.fireButtonTarget = true;
				this.nextButtonTarget = TouchControl.ButtonTarget.None;
				this.lastButtonTarget = TouchControl.ButtonTarget.None;
			}
		}

		// Token: 0x06000E6C RID: 3692 RVA: 0x000467E0 File Offset: 0x00044BE0
		public override void TouchMoved(Touch touch)
		{
			if (this.currentTouch != touch)
			{
				return;
			}
			Vector3 a = TouchManager.ScreenToWorldPoint(touch.position);
			Vector3 vector = a - this.lastPosition;
			if (vector.magnitude >= this.sensitivity)
			{
				this.lastPosition = a;
				this.currentVector = vector.normalized;
				if (this.fireButtonTarget)
				{
					TouchControl.ButtonTarget buttonTargetForVector = this.GetButtonTargetForVector(this.currentVector);
					if (buttonTargetForVector != this.lastButtonTarget)
					{
						this.nextButtonTarget = buttonTargetForVector;
					}
				}
			}
		}

		// Token: 0x06000E6D RID: 3693 RVA: 0x0004686C File Offset: 0x00044C6C
		public override void TouchEnded(Touch touch)
		{
			if (this.currentTouch != touch)
			{
				return;
			}
			this.currentTouch = null;
			this.currentVector = Vector2.zero;
			Vector3 b = TouchManager.ScreenToWorldPoint(touch.position);
			if ((this.beganPosition - b).magnitude < this.sensitivity)
			{
				this.fireButtonTarget = true;
				this.nextButtonTarget = this.tapTarget;
				this.lastButtonTarget = TouchControl.ButtonTarget.None;
				return;
			}
			this.fireButtonTarget = false;
			this.nextButtonTarget = TouchControl.ButtonTarget.None;
			this.lastButtonTarget = TouchControl.ButtonTarget.None;
		}

		// Token: 0x06000E6E RID: 3694 RVA: 0x000468F8 File Offset: 0x00044CF8
		private TouchControl.ButtonTarget GetButtonTargetForVector(Vector2 vector)
		{
			Vector2 lhs = TouchControl.SnapTo(vector, TouchControl.SnapAngles.Four);
			if (lhs == Vector2.up)
			{
				return this.upTarget;
			}
			if (lhs == Vector2.right)
			{
				return this.rightTarget;
			}
			if (lhs == -Vector2.up)
			{
				return this.downTarget;
			}
			if (lhs == -Vector2.right)
			{
				return this.leftTarget;
			}
			return TouchControl.ButtonTarget.None;
		}

		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x06000E6F RID: 3695 RVA: 0x00046974 File Offset: 0x00044D74
		// (set) Token: 0x06000E70 RID: 3696 RVA: 0x0004697C File Offset: 0x00044D7C
		public Rect ActiveArea
		{
			get
			{
				return this.activeArea;
			}
			set
			{
				if (this.activeArea != value)
				{
					this.activeArea = value;
					this.dirty = true;
				}
			}
		}

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x06000E71 RID: 3697 RVA: 0x0004699D File Offset: 0x00044D9D
		// (set) Token: 0x06000E72 RID: 3698 RVA: 0x000469A5 File Offset: 0x00044DA5
		public TouchUnitType AreaUnitType
		{
			get
			{
				return this.areaUnitType;
			}
			set
			{
				if (this.areaUnitType != value)
				{
					this.areaUnitType = value;
					this.dirty = true;
				}
			}
		}

		// Token: 0x04000B2A RID: 2858
		[Header("Position")]
		[SerializeField]
		private TouchUnitType areaUnitType;

		// Token: 0x04000B2B RID: 2859
		[SerializeField]
		private Rect activeArea = new Rect(25f, 25f, 50f, 50f);

		// Token: 0x04000B2C RID: 2860
		[Range(0f, 1f)]
		public float sensitivity = 0.1f;

		// Token: 0x04000B2D RID: 2861
		[Header("Analog Target")]
		public TouchControl.AnalogTarget target;

		// Token: 0x04000B2E RID: 2862
		public TouchControl.SnapAngles snapAngles;

		// Token: 0x04000B2F RID: 2863
		[Header("Button Targets")]
		public TouchControl.ButtonTarget upTarget;

		// Token: 0x04000B30 RID: 2864
		public TouchControl.ButtonTarget downTarget;

		// Token: 0x04000B31 RID: 2865
		public TouchControl.ButtonTarget leftTarget;

		// Token: 0x04000B32 RID: 2866
		public TouchControl.ButtonTarget rightTarget;

		// Token: 0x04000B33 RID: 2867
		public TouchControl.ButtonTarget tapTarget;

		// Token: 0x04000B34 RID: 2868
		public bool oneSwipePerTouch;

		// Token: 0x04000B35 RID: 2869
		private Rect worldActiveArea;

		// Token: 0x04000B36 RID: 2870
		private Vector3 currentVector;

		// Token: 0x04000B37 RID: 2871
		private Vector3 beganPosition;

		// Token: 0x04000B38 RID: 2872
		private Vector3 lastPosition;

		// Token: 0x04000B39 RID: 2873
		private Touch currentTouch;

		// Token: 0x04000B3A RID: 2874
		private bool fireButtonTarget;

		// Token: 0x04000B3B RID: 2875
		private TouchControl.ButtonTarget nextButtonTarget;

		// Token: 0x04000B3C RID: 2876
		private TouchControl.ButtonTarget lastButtonTarget;

		// Token: 0x04000B3D RID: 2877
		private bool dirty;
	}
}
