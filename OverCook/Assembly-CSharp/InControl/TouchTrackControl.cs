using System;
using UnityEngine;

namespace InControl
{
	// Token: 0x020002C4 RID: 708
	public class TouchTrackControl : TouchControl
	{
		// Token: 0x06000E74 RID: 3700 RVA: 0x000469FA File Offset: 0x00044DFA
		public override void CreateControl()
		{
			this.ConfigureControl();
		}

		// Token: 0x06000E75 RID: 3701 RVA: 0x00046A02 File Offset: 0x00044E02
		public override void DestroyControl()
		{
			if (this.currentTouch != null)
			{
				this.TouchEnded(this.currentTouch);
				this.currentTouch = null;
			}
		}

		// Token: 0x06000E76 RID: 3702 RVA: 0x00046A22 File Offset: 0x00044E22
		public override void ConfigureControl()
		{
			this.worldActiveArea = TouchManager.ConvertToWorld(this.activeArea, this.areaUnitType);
		}

		// Token: 0x06000E77 RID: 3703 RVA: 0x00046A3B File Offset: 0x00044E3B
		public override void DrawGizmos()
		{
			Utility.DrawRectGizmo(this.worldActiveArea, Color.yellow);
		}

		// Token: 0x06000E78 RID: 3704 RVA: 0x00046A4D File Offset: 0x00044E4D
		private void Update()
		{
			if (this.dirty)
			{
				this.ConfigureControl();
				this.dirty = false;
			}
		}

		// Token: 0x06000E79 RID: 3705 RVA: 0x00046A68 File Offset: 0x00044E68
		public override void SubmitControlState(ulong updateTick, float deltaTime)
		{
			Vector3 a = this.thisPosition - this.lastPosition;
			base.SubmitRawAnalogValue(this.target, a * this.scale, updateTick, deltaTime);
			this.lastPosition = this.thisPosition;
		}

		// Token: 0x06000E7A RID: 3706 RVA: 0x00046AB2 File Offset: 0x00044EB2
		public override void CommitControlState(ulong updateTick, float deltaTime)
		{
			base.CommitAnalog(this.target);
		}

		// Token: 0x06000E7B RID: 3707 RVA: 0x00046AC0 File Offset: 0x00044EC0
		public override void TouchBegan(Touch touch)
		{
			if (this.currentTouch != null)
			{
				return;
			}
			Vector3 point = TouchManager.ScreenToWorldPoint(touch.position);
			if (this.worldActiveArea.Contains(point))
			{
				this.thisPosition = TouchManager.ScreenToViewPoint(touch.position * 100f);
				this.lastPosition = this.thisPosition;
				this.currentTouch = touch;
			}
		}

		// Token: 0x06000E7C RID: 3708 RVA: 0x00046B24 File Offset: 0x00044F24
		public override void TouchMoved(Touch touch)
		{
			if (this.currentTouch != touch)
			{
				return;
			}
			this.thisPosition = TouchManager.ScreenToViewPoint(touch.position * 100f);
		}

		// Token: 0x06000E7D RID: 3709 RVA: 0x00046B4E File Offset: 0x00044F4E
		public override void TouchEnded(Touch touch)
		{
			if (this.currentTouch != touch)
			{
				return;
			}
			this.thisPosition = Vector3.zero;
			this.lastPosition = Vector3.zero;
			this.currentTouch = null;
		}

		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x06000E7E RID: 3710 RVA: 0x00046B7A File Offset: 0x00044F7A
		// (set) Token: 0x06000E7F RID: 3711 RVA: 0x00046B82 File Offset: 0x00044F82
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

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x06000E80 RID: 3712 RVA: 0x00046BA3 File Offset: 0x00044FA3
		// (set) Token: 0x06000E81 RID: 3713 RVA: 0x00046BAB File Offset: 0x00044FAB
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

		// Token: 0x04000B3E RID: 2878
		[Header("Dimensions")]
		[SerializeField]
		private TouchUnitType areaUnitType;

		// Token: 0x04000B3F RID: 2879
		[SerializeField]
		private Rect activeArea = new Rect(25f, 25f, 50f, 50f);

		// Token: 0x04000B40 RID: 2880
		[Header("Analog Target")]
		public TouchControl.AnalogTarget target = TouchControl.AnalogTarget.LeftStick;

		// Token: 0x04000B41 RID: 2881
		public float scale = 1f;

		// Token: 0x04000B42 RID: 2882
		private Rect worldActiveArea;

		// Token: 0x04000B43 RID: 2883
		private Vector3 lastPosition;

		// Token: 0x04000B44 RID: 2884
		private Vector3 thisPosition;

		// Token: 0x04000B45 RID: 2885
		private Touch currentTouch;

		// Token: 0x04000B46 RID: 2886
		private bool dirty;
	}
}
