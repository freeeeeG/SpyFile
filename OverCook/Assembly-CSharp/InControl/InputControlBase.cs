using System;
using UnityEngine;

namespace InControl
{
	// Token: 0x020002AE RID: 686
	public abstract class InputControlBase : IInputControl
	{
		// Token: 0x1700015D RID: 349
		// (get) Token: 0x06000D0B RID: 3339 RVA: 0x00040D99 File Offset: 0x0003F199
		// (set) Token: 0x06000D0C RID: 3340 RVA: 0x00040DA1 File Offset: 0x0003F1A1
		public ulong UpdateTick { get; protected set; }

		// Token: 0x06000D0D RID: 3341 RVA: 0x00040DAC File Offset: 0x0003F1AC
		private void PrepareForUpdate(ulong updateTick)
		{
			if (updateTick < this.pendingTick)
			{
				throw new InvalidOperationException("Cannot be updated with an earlier tick.");
			}
			if (this.pendingCommit && updateTick != this.pendingTick)
			{
				throw new InvalidOperationException("Cannot be updated for a new tick until pending tick is committed.");
			}
			if (updateTick > this.pendingTick)
			{
				this.lastState = this.thisState;
				this.nextState.Reset();
				this.pendingTick = updateTick;
				this.pendingCommit = true;
			}
		}

		// Token: 0x06000D0E RID: 3342 RVA: 0x00040E23 File Offset: 0x0003F223
		public bool UpdateWithState(bool state, ulong updateTick, float deltaTime)
		{
			this.PrepareForUpdate(updateTick);
			this.nextState.Set(state || this.nextState.State);
			return state;
		}

		// Token: 0x06000D0F RID: 3343 RVA: 0x00040E4C File Offset: 0x0003F24C
		public bool UpdateWithValue(float value, ulong updateTick, float deltaTime)
		{
			this.PrepareForUpdate(updateTick);
			if (Utility.Abs(value) > Utility.Abs(this.nextState.RawValue))
			{
				this.nextState.RawValue = value;
				if (!this.Raw)
				{
					value = Utility.ApplyDeadZone(value, this.lowerDeadZone, this.upperDeadZone);
				}
				this.nextState.Set(value, this.stateThreshold);
				return true;
			}
			return false;
		}

		// Token: 0x06000D10 RID: 3344 RVA: 0x00040EBC File Offset: 0x0003F2BC
		internal bool UpdateWithRawValue(float value, ulong updateTick, float deltaTime)
		{
			this.PrepareForUpdate(updateTick);
			if (Utility.Abs(value) > Utility.Abs(this.nextState.RawValue))
			{
				this.nextState.RawValue = value;
				this.nextState.Set(value, this.stateThreshold);
				return true;
			}
			return false;
		}

		// Token: 0x06000D11 RID: 3345 RVA: 0x00040F0C File Offset: 0x0003F30C
		internal void SetValue(float value, ulong updateTick)
		{
			if (updateTick > this.pendingTick)
			{
				this.lastState = this.thisState;
				this.nextState.Reset();
				this.pendingTick = updateTick;
				this.pendingCommit = true;
			}
			this.nextState.RawValue = value;
			this.nextState.Set(value, this.StateThreshold);
		}

		// Token: 0x06000D12 RID: 3346 RVA: 0x00040F68 File Offset: 0x0003F368
		public void ClearInputState()
		{
			this.lastState.Reset();
			this.thisState.Reset();
			this.nextState.Reset();
			this.wasRepeated = false;
			this.clearInputState = true;
		}

		// Token: 0x06000D13 RID: 3347 RVA: 0x00040F9C File Offset: 0x0003F39C
		public void Commit()
		{
			this.pendingCommit = false;
			this.thisState = this.nextState;
			if (this.clearInputState)
			{
				this.lastState = this.nextState;
				this.UpdateTick = this.pendingTick;
				this.clearInputState = false;
				return;
			}
			bool state = this.lastState.State;
			bool state2 = this.thisState.State;
			this.wasRepeated = false;
			if (state && !state2)
			{
				this.nextRepeatTime = 0f;
			}
			else if (state2)
			{
				if (state != state2)
				{
					this.nextRepeatTime = Time.realtimeSinceStartup + this.FirstRepeatDelay;
				}
				else if (Time.realtimeSinceStartup >= this.nextRepeatTime)
				{
					this.wasRepeated = true;
					this.nextRepeatTime = Time.realtimeSinceStartup + this.RepeatDelay;
				}
			}
			if (this.thisState != this.lastState)
			{
				this.UpdateTick = this.pendingTick;
			}
		}

		// Token: 0x06000D14 RID: 3348 RVA: 0x00041091 File Offset: 0x0003F491
		public void CommitWithState(bool state, ulong updateTick, float deltaTime)
		{
			this.UpdateWithState(state, updateTick, deltaTime);
			this.Commit();
		}

		// Token: 0x06000D15 RID: 3349 RVA: 0x000410A3 File Offset: 0x0003F4A3
		public void CommitWithValue(float value, ulong updateTick, float deltaTime)
		{
			this.UpdateWithValue(value, updateTick, deltaTime);
			this.Commit();
		}

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x06000D16 RID: 3350 RVA: 0x000410B5 File Offset: 0x0003F4B5
		public bool State
		{
			get
			{
				return this.Enabled && this.thisState.State;
			}
		}

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x06000D17 RID: 3351 RVA: 0x000410D0 File Offset: 0x0003F4D0
		public bool LastState
		{
			get
			{
				return this.Enabled && this.lastState.State;
			}
		}

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x06000D18 RID: 3352 RVA: 0x000410EB File Offset: 0x0003F4EB
		public float Value
		{
			get
			{
				return (!this.Enabled) ? 0f : this.thisState.Value;
			}
		}

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x06000D19 RID: 3353 RVA: 0x0004110D File Offset: 0x0003F50D
		public float LastValue
		{
			get
			{
				return (!this.Enabled) ? 0f : this.lastState.Value;
			}
		}

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x06000D1A RID: 3354 RVA: 0x0004112F File Offset: 0x0003F52F
		public float RawValue
		{
			get
			{
				return (!this.Enabled) ? 0f : this.thisState.RawValue;
			}
		}

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x06000D1B RID: 3355 RVA: 0x00041151 File Offset: 0x0003F551
		internal float NextRawValue
		{
			get
			{
				return (!this.Enabled) ? 0f : this.nextState.RawValue;
			}
		}

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x06000D1C RID: 3356 RVA: 0x00041173 File Offset: 0x0003F573
		public bool HasChanged
		{
			get
			{
				return this.Enabled && this.thisState != this.lastState;
			}
		}

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x06000D1D RID: 3357 RVA: 0x00041194 File Offset: 0x0003F594
		public bool IsPressed
		{
			get
			{
				return this.Enabled && this.thisState.State;
			}
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x06000D1E RID: 3358 RVA: 0x000411AF File Offset: 0x0003F5AF
		public bool WasPressed
		{
			get
			{
				return this.Enabled && this.thisState && !this.lastState;
			}
		}

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x06000D1F RID: 3359 RVA: 0x000411DD File Offset: 0x0003F5DD
		public bool WasReleased
		{
			get
			{
				return this.Enabled && !this.thisState && this.lastState;
			}
		}

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x06000D20 RID: 3360 RVA: 0x00041208 File Offset: 0x0003F608
		public bool WasRepeated
		{
			get
			{
				return this.Enabled && this.wasRepeated;
			}
		}

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x06000D21 RID: 3361 RVA: 0x0004121E File Offset: 0x0003F61E
		// (set) Token: 0x06000D22 RID: 3362 RVA: 0x00041226 File Offset: 0x0003F626
		public float Sensitivity
		{
			get
			{
				return this.sensitivity;
			}
			set
			{
				this.sensitivity = Mathf.Clamp01(value);
			}
		}

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x06000D23 RID: 3363 RVA: 0x00041234 File Offset: 0x0003F634
		// (set) Token: 0x06000D24 RID: 3364 RVA: 0x0004123C File Offset: 0x0003F63C
		public float LowerDeadZone
		{
			get
			{
				return this.lowerDeadZone;
			}
			set
			{
				this.lowerDeadZone = Mathf.Clamp01(value);
			}
		}

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x06000D25 RID: 3365 RVA: 0x0004124A File Offset: 0x0003F64A
		// (set) Token: 0x06000D26 RID: 3366 RVA: 0x00041252 File Offset: 0x0003F652
		public float UpperDeadZone
		{
			get
			{
				return this.upperDeadZone;
			}
			set
			{
				this.upperDeadZone = Mathf.Clamp01(value);
			}
		}

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x06000D27 RID: 3367 RVA: 0x00041260 File Offset: 0x0003F660
		// (set) Token: 0x06000D28 RID: 3368 RVA: 0x00041268 File Offset: 0x0003F668
		public float StateThreshold
		{
			get
			{
				return this.stateThreshold;
			}
			set
			{
				this.stateThreshold = Mathf.Clamp01(value);
			}
		}

		// Token: 0x06000D29 RID: 3369 RVA: 0x00041276 File Offset: 0x0003F676
		public static implicit operator bool(InputControlBase instance)
		{
			return instance.State;
		}

		// Token: 0x06000D2A RID: 3370 RVA: 0x0004127E File Offset: 0x0003F67E
		public static implicit operator float(InputControlBase instance)
		{
			return instance.Value;
		}

		// Token: 0x04000A18 RID: 2584
		private float sensitivity = 1f;

		// Token: 0x04000A19 RID: 2585
		private float lowerDeadZone;

		// Token: 0x04000A1A RID: 2586
		private float upperDeadZone = 1f;

		// Token: 0x04000A1B RID: 2587
		private float stateThreshold;

		// Token: 0x04000A1C RID: 2588
		public float FirstRepeatDelay = 0.8f;

		// Token: 0x04000A1D RID: 2589
		public float RepeatDelay = 0.1f;

		// Token: 0x04000A1E RID: 2590
		public bool Raw;

		// Token: 0x04000A1F RID: 2591
		internal bool Enabled = true;

		// Token: 0x04000A20 RID: 2592
		private ulong pendingTick;

		// Token: 0x04000A21 RID: 2593
		private bool pendingCommit;

		// Token: 0x04000A22 RID: 2594
		private float nextRepeatTime;

		// Token: 0x04000A23 RID: 2595
		private float lastPressedTime;

		// Token: 0x04000A24 RID: 2596
		private bool wasRepeated;

		// Token: 0x04000A25 RID: 2597
		private bool clearInputState;

		// Token: 0x04000A26 RID: 2598
		private InputControlState lastState;

		// Token: 0x04000A27 RID: 2599
		private InputControlState nextState;

		// Token: 0x04000A28 RID: 2600
		private InputControlState thisState;
	}
}
