using System;
using UnityEngine;

namespace InControl
{
	// Token: 0x020002B6 RID: 694
	public class TwoAxisInputControl : IInputControl
	{
		// Token: 0x06000D42 RID: 3394 RVA: 0x00041F1E File Offset: 0x0004031E
		public TwoAxisInputControl()
		{
			this.Left = new OneAxisInputControl();
			this.Right = new OneAxisInputControl();
			this.Up = new OneAxisInputControl();
			this.Down = new OneAxisInputControl();
		}

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x06000D43 RID: 3395 RVA: 0x00041F5D File Offset: 0x0004035D
		// (set) Token: 0x06000D44 RID: 3396 RVA: 0x00041F65 File Offset: 0x00040365
		public float X { get; protected set; }

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x06000D45 RID: 3397 RVA: 0x00041F6E File Offset: 0x0004036E
		// (set) Token: 0x06000D46 RID: 3398 RVA: 0x00041F76 File Offset: 0x00040376
		public float Y { get; protected set; }

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x06000D47 RID: 3399 RVA: 0x00041F7F File Offset: 0x0004037F
		// (set) Token: 0x06000D48 RID: 3400 RVA: 0x00041F87 File Offset: 0x00040387
		public OneAxisInputControl Left { get; protected set; }

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x06000D49 RID: 3401 RVA: 0x00041F90 File Offset: 0x00040390
		// (set) Token: 0x06000D4A RID: 3402 RVA: 0x00041F98 File Offset: 0x00040398
		public OneAxisInputControl Right { get; protected set; }

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x06000D4B RID: 3403 RVA: 0x00041FA1 File Offset: 0x000403A1
		// (set) Token: 0x06000D4C RID: 3404 RVA: 0x00041FA9 File Offset: 0x000403A9
		public OneAxisInputControl Up { get; protected set; }

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x06000D4D RID: 3405 RVA: 0x00041FB2 File Offset: 0x000403B2
		// (set) Token: 0x06000D4E RID: 3406 RVA: 0x00041FBA File Offset: 0x000403BA
		public OneAxisInputControl Down { get; protected set; }

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x06000D4F RID: 3407 RVA: 0x00041FC3 File Offset: 0x000403C3
		// (set) Token: 0x06000D50 RID: 3408 RVA: 0x00041FCB File Offset: 0x000403CB
		public ulong UpdateTick { get; protected set; }

		// Token: 0x06000D51 RID: 3409 RVA: 0x00041FD4 File Offset: 0x000403D4
		public void ClearInputState()
		{
			this.Left.ClearInputState();
			this.Right.ClearInputState();
			this.Up.ClearInputState();
			this.Down.ClearInputState();
			this.lastState = false;
			this.lastValue = Vector2.zero;
			this.thisState = false;
			this.thisValue = Vector2.zero;
			this.clearInputState = true;
		}

		// Token: 0x06000D52 RID: 3410 RVA: 0x00042038 File Offset: 0x00040438
		public void Filter(TwoAxisInputControl twoAxisInputControl, float deltaTime)
		{
			this.UpdateWithAxes(twoAxisInputControl.X, twoAxisInputControl.Y, InputManager.CurrentTick, deltaTime);
		}

		// Token: 0x06000D53 RID: 3411 RVA: 0x00042054 File Offset: 0x00040454
		internal void UpdateWithAxes(float x, float y, ulong updateTick, float deltaTime)
		{
			this.lastState = this.thisState;
			this.lastValue = this.thisValue;
			this.thisValue = ((!this.Raw) ? Utility.ApplyCircularDeadZone(x, y, this.LowerDeadZone, this.UpperDeadZone) : new Vector2(x, y));
			this.X = this.thisValue.x;
			this.Y = this.thisValue.y;
			this.Left.CommitWithValue(Mathf.Max(0f, -this.X), updateTick, deltaTime);
			this.Right.CommitWithValue(Mathf.Max(0f, this.X), updateTick, deltaTime);
			if (InputManager.InvertYAxis)
			{
				this.Up.CommitWithValue(Mathf.Max(0f, -this.Y), updateTick, deltaTime);
				this.Down.CommitWithValue(Mathf.Max(0f, this.Y), updateTick, deltaTime);
			}
			else
			{
				this.Up.CommitWithValue(Mathf.Max(0f, this.Y), updateTick, deltaTime);
				this.Down.CommitWithValue(Mathf.Max(0f, -this.Y), updateTick, deltaTime);
			}
			this.thisState = (this.Up.State || this.Down.State || this.Left.State || this.Right.State);
			if (this.clearInputState)
			{
				this.lastState = this.thisState;
				this.lastValue = this.thisValue;
				this.clearInputState = false;
				this.HasChanged = false;
				return;
			}
			if (this.thisValue != this.lastValue)
			{
				this.UpdateTick = updateTick;
				this.HasChanged = true;
			}
			else
			{
				this.HasChanged = false;
			}
		}

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x06000D55 RID: 3413 RVA: 0x00042270 File Offset: 0x00040670
		// (set) Token: 0x06000D54 RID: 3412 RVA: 0x00042237 File Offset: 0x00040637
		public float StateThreshold
		{
			get
			{
				return this.stateThreshold;
			}
			set
			{
				this.stateThreshold = value;
				this.Left.StateThreshold = value;
				this.Right.StateThreshold = value;
				this.Up.StateThreshold = value;
				this.Down.StateThreshold = value;
			}
		}

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x06000D56 RID: 3414 RVA: 0x00042278 File Offset: 0x00040678
		public bool State
		{
			get
			{
				return this.thisState;
			}
		}

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x06000D57 RID: 3415 RVA: 0x00042280 File Offset: 0x00040680
		public bool LastState
		{
			get
			{
				return this.lastState;
			}
		}

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x06000D58 RID: 3416 RVA: 0x00042288 File Offset: 0x00040688
		public Vector2 Value
		{
			get
			{
				return this.thisValue;
			}
		}

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x06000D59 RID: 3417 RVA: 0x00042290 File Offset: 0x00040690
		public Vector2 LastValue
		{
			get
			{
				return this.lastValue;
			}
		}

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x06000D5A RID: 3418 RVA: 0x00042298 File Offset: 0x00040698
		public Vector2 Vector
		{
			get
			{
				return this.thisValue;
			}
		}

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x06000D5B RID: 3419 RVA: 0x000422A0 File Offset: 0x000406A0
		// (set) Token: 0x06000D5C RID: 3420 RVA: 0x000422A8 File Offset: 0x000406A8
		public bool HasChanged { get; protected set; }

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x06000D5D RID: 3421 RVA: 0x000422B1 File Offset: 0x000406B1
		public bool IsPressed
		{
			get
			{
				return this.thisState;
			}
		}

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x06000D5E RID: 3422 RVA: 0x000422B9 File Offset: 0x000406B9
		public bool WasPressed
		{
			get
			{
				return this.thisState && !this.lastState;
			}
		}

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x06000D5F RID: 3423 RVA: 0x000422D2 File Offset: 0x000406D2
		public bool WasReleased
		{
			get
			{
				return !this.thisState && this.lastState;
			}
		}

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x06000D60 RID: 3424 RVA: 0x000422E8 File Offset: 0x000406E8
		public float Angle
		{
			get
			{
				return Utility.VectorToAngle(this.thisValue);
			}
		}

		// Token: 0x06000D61 RID: 3425 RVA: 0x000422F5 File Offset: 0x000406F5
		public static implicit operator bool(TwoAxisInputControl instance)
		{
			return instance.thisState;
		}

		// Token: 0x06000D62 RID: 3426 RVA: 0x000422FD File Offset: 0x000406FD
		public static implicit operator Vector2(TwoAxisInputControl instance)
		{
			return instance.thisValue;
		}

		// Token: 0x06000D63 RID: 3427 RVA: 0x00042305 File Offset: 0x00040705
		public static implicit operator Vector3(TwoAxisInputControl instance)
		{
			return instance.thisValue;
		}

		// Token: 0x04000AA6 RID: 2726
		public float LowerDeadZone;

		// Token: 0x04000AA7 RID: 2727
		public float UpperDeadZone = 1f;

		// Token: 0x04000AA8 RID: 2728
		public bool Raw;

		// Token: 0x04000AA9 RID: 2729
		private bool thisState;

		// Token: 0x04000AAA RID: 2730
		private bool lastState;

		// Token: 0x04000AAB RID: 2731
		private Vector2 thisValue;

		// Token: 0x04000AAC RID: 2732
		private Vector2 lastValue;

		// Token: 0x04000AAD RID: 2733
		private bool clearInputState;

		// Token: 0x04000AAE RID: 2734
		private float stateThreshold;
	}
}
