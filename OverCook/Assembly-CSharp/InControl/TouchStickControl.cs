using System;
using UnityEngine;

namespace InControl
{
	// Token: 0x020002C2 RID: 706
	public class TouchStickControl : TouchControl
	{
		// Token: 0x06000E49 RID: 3657 RVA: 0x00045DCA File Offset: 0x000441CA
		public override void CreateControl()
		{
			this.ring.Create("Ring", base.transform, 1000);
			this.knob.Create("Knob", base.transform, 1001);
		}

		// Token: 0x06000E4A RID: 3658 RVA: 0x00045E02 File Offset: 0x00044202
		public override void DestroyControl()
		{
			this.ring.Delete();
			this.knob.Delete();
			if (this.currentTouch != null)
			{
				this.TouchEnded(this.currentTouch);
				this.currentTouch = null;
			}
		}

		// Token: 0x06000E4B RID: 3659 RVA: 0x00045E38 File Offset: 0x00044238
		public override void ConfigureControl()
		{
			this.resetPosition = base.OffsetToWorldPosition(this.anchor, this.offset, this.offsetUnitType, true);
			base.transform.position = this.resetPosition;
			this.ring.Update(true);
			this.knob.Update(true);
			this.worldActiveArea = TouchManager.ConvertToWorld(this.activeArea, this.areaUnitType);
			this.worldKnobRange = TouchManager.ConvertToWorld(this.knobRange, this.knob.SizeUnitType);
		}

		// Token: 0x06000E4C RID: 3660 RVA: 0x00045EC0 File Offset: 0x000442C0
		public override void DrawGizmos()
		{
			this.ring.DrawGizmos(this.RingPosition, Color.yellow);
			this.knob.DrawGizmos(this.KnobPosition, Color.yellow);
			Utility.DrawCircleGizmo(this.RingPosition, this.worldKnobRange, Color.red);
			Utility.DrawRectGizmo(this.worldActiveArea, Color.green);
		}

		// Token: 0x06000E4D RID: 3661 RVA: 0x00045F24 File Offset: 0x00044324
		private void Update()
		{
			if (this.dirty)
			{
				this.ConfigureControl();
				this.dirty = false;
			}
			else
			{
				this.ring.Update();
				this.knob.Update();
			}
			if (this.IsNotActive)
			{
				if (this.resetWhenDone && this.KnobPosition != this.resetPosition)
				{
					Vector3 b = this.KnobPosition - this.RingPosition;
					this.RingPosition = Vector3.MoveTowards(this.RingPosition, this.resetPosition, this.ringResetSpeed * Time.deltaTime);
					this.KnobPosition = this.RingPosition + b;
				}
				if (this.KnobPosition != this.RingPosition)
				{
					this.KnobPosition = Vector3.MoveTowards(this.KnobPosition, this.RingPosition, this.knobResetSpeed * Time.deltaTime);
				}
			}
		}

		// Token: 0x06000E4E RID: 3662 RVA: 0x00046010 File Offset: 0x00044410
		public override void SubmitControlState(ulong updateTick, float deltaTime)
		{
			base.SubmitAnalogValue(this.target, this.snappedValue, this.lowerDeadZone, this.upperDeadZone, updateTick, deltaTime);
		}

		// Token: 0x06000E4F RID: 3663 RVA: 0x00046037 File Offset: 0x00044437
		public override void CommitControlState(ulong updateTick, float deltaTime)
		{
			base.CommitAnalog(this.target);
		}

		// Token: 0x06000E50 RID: 3664 RVA: 0x00046048 File Offset: 0x00044448
		public override void TouchBegan(Touch touch)
		{
			if (this.IsActive)
			{
				return;
			}
			this.beganPosition = TouchManager.ScreenToWorldPoint(touch.position);
			bool flag = this.worldActiveArea.Contains(this.beganPosition);
			bool flag2 = this.ring.Contains(this.beganPosition);
			if (this.snapToInitialTouch && (flag || flag2))
			{
				this.RingPosition = this.beganPosition;
				this.KnobPosition = this.beganPosition;
				this.currentTouch = touch;
			}
			else if (flag2)
			{
				this.KnobPosition = this.beganPosition;
				this.beganPosition = this.RingPosition;
				this.currentTouch = touch;
			}
			if (this.IsActive)
			{
				this.TouchMoved(touch);
				this.ring.State = true;
				this.knob.State = true;
			}
		}

		// Token: 0x06000E51 RID: 3665 RVA: 0x00046128 File Offset: 0x00044528
		public override void TouchMoved(Touch touch)
		{
			if (this.currentTouch != touch)
			{
				return;
			}
			this.movedPosition = TouchManager.ScreenToWorldPoint(touch.position);
			Vector3 vector = this.movedPosition - this.beganPosition;
			Vector3 normalized = vector.normalized;
			float magnitude = vector.magnitude;
			if (this.allowDragging)
			{
				float num = magnitude - this.worldKnobRange;
				if (num < 0f)
				{
					num = 0f;
				}
				this.beganPosition += num * normalized;
				this.RingPosition = this.beganPosition;
			}
			this.movedPosition = this.beganPosition + Mathf.Clamp(magnitude, 0f, this.worldKnobRange) * normalized;
			this.value = (this.movedPosition - this.beganPosition) / this.worldKnobRange;
			this.value.x = this.inputCurve.Evaluate(Utility.Abs(this.value.x)) * Mathf.Sign(this.value.x);
			this.value.y = this.inputCurve.Evaluate(Utility.Abs(this.value.y)) * Mathf.Sign(this.value.y);
			if (this.snapAngles == TouchControl.SnapAngles.None)
			{
				this.snappedValue = this.value;
				this.KnobPosition = this.movedPosition;
			}
			else
			{
				this.snappedValue = TouchControl.SnapTo(this.value, this.snapAngles);
				this.KnobPosition = this.beganPosition + this.snappedValue * this.worldKnobRange;
			}
			this.RingPosition = this.beganPosition;
		}

		// Token: 0x06000E52 RID: 3666 RVA: 0x000462F0 File Offset: 0x000446F0
		public override void TouchEnded(Touch touch)
		{
			if (this.currentTouch != touch)
			{
				return;
			}
			this.value = Vector3.zero;
			this.snappedValue = Vector3.zero;
			float magnitude = (this.resetPosition - this.RingPosition).magnitude;
			this.ringResetSpeed = ((!Utility.IsZero(this.resetDuration)) ? (magnitude / this.resetDuration) : magnitude);
			float magnitude2 = (this.RingPosition - this.KnobPosition).magnitude;
			this.knobResetSpeed = ((!Utility.IsZero(this.resetDuration)) ? (magnitude2 / this.resetDuration) : this.knobRange);
			this.currentTouch = null;
			this.ring.State = false;
			this.knob.State = false;
		}

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x06000E53 RID: 3667 RVA: 0x000463C0 File Offset: 0x000447C0
		public bool IsActive
		{
			get
			{
				return this.currentTouch != null;
			}
		}

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x06000E54 RID: 3668 RVA: 0x000463CE File Offset: 0x000447CE
		public bool IsNotActive
		{
			get
			{
				return this.currentTouch == null;
			}
		}

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x06000E55 RID: 3669 RVA: 0x000463D9 File Offset: 0x000447D9
		// (set) Token: 0x06000E56 RID: 3670 RVA: 0x00046406 File Offset: 0x00044806
		public Vector3 RingPosition
		{
			get
			{
				return (!this.ring.Ready) ? base.transform.position : this.ring.Position;
			}
			set
			{
				if (this.ring.Ready)
				{
					this.ring.Position = value;
				}
			}
		}

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x06000E57 RID: 3671 RVA: 0x00046424 File Offset: 0x00044824
		// (set) Token: 0x06000E58 RID: 3672 RVA: 0x00046451 File Offset: 0x00044851
		public Vector3 KnobPosition
		{
			get
			{
				return (!this.knob.Ready) ? base.transform.position : this.knob.Position;
			}
			set
			{
				if (this.knob.Ready)
				{
					this.knob.Position = value;
				}
			}
		}

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x06000E59 RID: 3673 RVA: 0x0004646F File Offset: 0x0004486F
		// (set) Token: 0x06000E5A RID: 3674 RVA: 0x00046477 File Offset: 0x00044877
		public TouchControlAnchor Anchor
		{
			get
			{
				return this.anchor;
			}
			set
			{
				if (this.anchor != value)
				{
					this.anchor = value;
					this.dirty = true;
				}
			}
		}

		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x06000E5B RID: 3675 RVA: 0x00046493 File Offset: 0x00044893
		// (set) Token: 0x06000E5C RID: 3676 RVA: 0x0004649B File Offset: 0x0004489B
		public Vector2 Offset
		{
			get
			{
				return this.offset;
			}
			set
			{
				if (this.offset != value)
				{
					this.offset = value;
					this.dirty = true;
				}
			}
		}

		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x06000E5D RID: 3677 RVA: 0x000464BC File Offset: 0x000448BC
		// (set) Token: 0x06000E5E RID: 3678 RVA: 0x000464C4 File Offset: 0x000448C4
		public TouchUnitType OffsetUnitType
		{
			get
			{
				return this.offsetUnitType;
			}
			set
			{
				if (this.offsetUnitType != value)
				{
					this.offsetUnitType = value;
					this.dirty = true;
				}
			}
		}

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x06000E5F RID: 3679 RVA: 0x000464E0 File Offset: 0x000448E0
		// (set) Token: 0x06000E60 RID: 3680 RVA: 0x000464E8 File Offset: 0x000448E8
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

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x06000E61 RID: 3681 RVA: 0x00046509 File Offset: 0x00044909
		// (set) Token: 0x06000E62 RID: 3682 RVA: 0x00046511 File Offset: 0x00044911
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

		// Token: 0x04000B0E RID: 2830
		[Header("Position")]
		[SerializeField]
		private TouchControlAnchor anchor = TouchControlAnchor.BottomLeft;

		// Token: 0x04000B0F RID: 2831
		[SerializeField]
		private TouchUnitType offsetUnitType;

		// Token: 0x04000B10 RID: 2832
		[SerializeField]
		private Vector2 offset = new Vector2(20f, 20f);

		// Token: 0x04000B11 RID: 2833
		[SerializeField]
		private TouchUnitType areaUnitType;

		// Token: 0x04000B12 RID: 2834
		[SerializeField]
		private Rect activeArea = new Rect(0f, 0f, 50f, 100f);

		// Token: 0x04000B13 RID: 2835
		[Header("Options")]
		public TouchControl.AnalogTarget target = TouchControl.AnalogTarget.LeftStick;

		// Token: 0x04000B14 RID: 2836
		public TouchControl.SnapAngles snapAngles;

		// Token: 0x04000B15 RID: 2837
		[Range(0f, 1f)]
		public float lowerDeadZone = 0.1f;

		// Token: 0x04000B16 RID: 2838
		[Range(0f, 1f)]
		public float upperDeadZone = 0.9f;

		// Token: 0x04000B17 RID: 2839
		public AnimationCurve inputCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x04000B18 RID: 2840
		public bool allowDragging;

		// Token: 0x04000B19 RID: 2841
		public bool snapToInitialTouch = true;

		// Token: 0x04000B1A RID: 2842
		public bool resetWhenDone = true;

		// Token: 0x04000B1B RID: 2843
		public float resetDuration = 0.1f;

		// Token: 0x04000B1C RID: 2844
		[Header("Sprites")]
		public TouchSprite ring = new TouchSprite(20f);

		// Token: 0x04000B1D RID: 2845
		public TouchSprite knob = new TouchSprite(10f);

		// Token: 0x04000B1E RID: 2846
		public float knobRange = 7.5f;

		// Token: 0x04000B1F RID: 2847
		private Vector3 resetPosition;

		// Token: 0x04000B20 RID: 2848
		private Vector3 beganPosition;

		// Token: 0x04000B21 RID: 2849
		private Vector3 movedPosition;

		// Token: 0x04000B22 RID: 2850
		private float ringResetSpeed;

		// Token: 0x04000B23 RID: 2851
		private float knobResetSpeed;

		// Token: 0x04000B24 RID: 2852
		private Rect worldActiveArea;

		// Token: 0x04000B25 RID: 2853
		private float worldKnobRange;

		// Token: 0x04000B26 RID: 2854
		private Vector3 value;

		// Token: 0x04000B27 RID: 2855
		private Vector3 snappedValue;

		// Token: 0x04000B28 RID: 2856
		private Touch currentTouch;

		// Token: 0x04000B29 RID: 2857
		private bool dirty;
	}
}
