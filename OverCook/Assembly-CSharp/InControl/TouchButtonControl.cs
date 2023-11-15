using System;
using UnityEngine;

namespace InControl
{
	// Token: 0x020002C1 RID: 705
	public class TouchButtonControl : TouchControl
	{
		// Token: 0x06000E34 RID: 3636 RVA: 0x00045A45 File Offset: 0x00043E45
		public override void CreateControl()
		{
			this.button.Create("Button", base.transform, 1000);
		}

		// Token: 0x06000E35 RID: 3637 RVA: 0x00045A62 File Offset: 0x00043E62
		public override void DestroyControl()
		{
			this.button.Delete();
			if (this.currentTouch != null)
			{
				this.TouchEnded(this.currentTouch);
				this.currentTouch = null;
			}
		}

		// Token: 0x06000E36 RID: 3638 RVA: 0x00045A8D File Offset: 0x00043E8D
		public override void ConfigureControl()
		{
			base.transform.position = base.OffsetToWorldPosition(this.anchor, this.offset, this.offsetUnitType, this.lockAspectRatio);
			this.button.Update(true);
		}

		// Token: 0x06000E37 RID: 3639 RVA: 0x00045AC4 File Offset: 0x00043EC4
		public override void DrawGizmos()
		{
			this.button.DrawGizmos(this.ButtonPosition, Color.yellow);
		}

		// Token: 0x06000E38 RID: 3640 RVA: 0x00045ADC File Offset: 0x00043EDC
		private void Update()
		{
			if (this.dirty)
			{
				this.ConfigureControl();
				this.dirty = false;
			}
			else
			{
				this.button.Update();
			}
		}

		// Token: 0x06000E39 RID: 3641 RVA: 0x00045B08 File Offset: 0x00043F08
		public override void SubmitControlState(ulong updateTick, float deltaTime)
		{
			if (this.currentTouch == null && this.allowSlideToggle)
			{
				this.ButtonState = false;
				int touchCount = TouchManager.TouchCount;
				for (int i = 0; i < touchCount; i++)
				{
					this.ButtonState = (this.ButtonState || this.button.Contains(TouchManager.GetTouch(i)));
				}
			}
			base.SubmitButtonState(this.target, this.ButtonState, updateTick, deltaTime);
		}

		// Token: 0x06000E3A RID: 3642 RVA: 0x00045B83 File Offset: 0x00043F83
		public override void CommitControlState(ulong updateTick, float deltaTime)
		{
			base.CommitButton(this.target);
		}

		// Token: 0x06000E3B RID: 3643 RVA: 0x00045B91 File Offset: 0x00043F91
		public override void TouchBegan(Touch touch)
		{
			if (this.currentTouch != null)
			{
				return;
			}
			if (this.button.Contains(touch))
			{
				this.ButtonState = true;
				this.currentTouch = touch;
			}
		}

		// Token: 0x06000E3C RID: 3644 RVA: 0x00045BBE File Offset: 0x00043FBE
		public override void TouchMoved(Touch touch)
		{
			if (this.currentTouch != touch)
			{
				return;
			}
			if (this.toggleOnLeave && !this.button.Contains(touch))
			{
				this.ButtonState = false;
				this.currentTouch = null;
			}
		}

		// Token: 0x06000E3D RID: 3645 RVA: 0x00045BF7 File Offset: 0x00043FF7
		public override void TouchEnded(Touch touch)
		{
			if (this.currentTouch != touch)
			{
				return;
			}
			this.ButtonState = false;
			this.currentTouch = null;
		}

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x06000E3E RID: 3646 RVA: 0x00045C14 File Offset: 0x00044014
		// (set) Token: 0x06000E3F RID: 3647 RVA: 0x00045C1C File Offset: 0x0004401C
		private bool ButtonState
		{
			get
			{
				return this.buttonState;
			}
			set
			{
				if (this.buttonState != value)
				{
					this.buttonState = value;
					this.button.State = value;
				}
			}
		}

		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x06000E40 RID: 3648 RVA: 0x00045C3D File Offset: 0x0004403D
		// (set) Token: 0x06000E41 RID: 3649 RVA: 0x00045C6A File Offset: 0x0004406A
		public Vector3 ButtonPosition
		{
			get
			{
				return (!this.button.Ready) ? base.transform.position : this.button.Position;
			}
			set
			{
				if (this.button.Ready)
				{
					this.button.Position = value;
				}
			}
		}

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x06000E42 RID: 3650 RVA: 0x00045C88 File Offset: 0x00044088
		// (set) Token: 0x06000E43 RID: 3651 RVA: 0x00045C90 File Offset: 0x00044090
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

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x06000E44 RID: 3652 RVA: 0x00045CAC File Offset: 0x000440AC
		// (set) Token: 0x06000E45 RID: 3653 RVA: 0x00045CB4 File Offset: 0x000440B4
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

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x06000E46 RID: 3654 RVA: 0x00045CD5 File Offset: 0x000440D5
		// (set) Token: 0x06000E47 RID: 3655 RVA: 0x00045CDD File Offset: 0x000440DD
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

		// Token: 0x04000B03 RID: 2819
		[Header("Position")]
		[SerializeField]
		private TouchControlAnchor anchor = TouchControlAnchor.BottomRight;

		// Token: 0x04000B04 RID: 2820
		[SerializeField]
		private TouchUnitType offsetUnitType;

		// Token: 0x04000B05 RID: 2821
		[SerializeField]
		private Vector2 offset = new Vector2(-10f, 10f);

		// Token: 0x04000B06 RID: 2822
		[SerializeField]
		private bool lockAspectRatio = true;

		// Token: 0x04000B07 RID: 2823
		[Header("Options")]
		public TouchControl.ButtonTarget target = TouchControl.ButtonTarget.Action1;

		// Token: 0x04000B08 RID: 2824
		public bool allowSlideToggle = true;

		// Token: 0x04000B09 RID: 2825
		public bool toggleOnLeave;

		// Token: 0x04000B0A RID: 2826
		[Header("Sprites")]
		public TouchSprite button = new TouchSprite(15f);

		// Token: 0x04000B0B RID: 2827
		private bool buttonState;

		// Token: 0x04000B0C RID: 2828
		private Touch currentTouch;

		// Token: 0x04000B0D RID: 2829
		private bool dirty;
	}
}
