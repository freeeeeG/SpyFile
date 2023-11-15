using System;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x020014B0 RID: 5296
	[TaskIcon("{SkinColor}HasReceivedEventIcon.png")]
	[TaskDescription("Returns success as soon as the event specified by eventName has been received.")]
	public class HasReceivedEvent : Conditional
	{
		// Token: 0x0600671F RID: 26399 RVA: 0x0012A6A4 File Offset: 0x001288A4
		public override void OnStart()
		{
			if (!this.registered)
			{
				base.Owner.RegisterEvent(this.eventName.Value, new Action(this.ReceivedEvent));
				base.Owner.RegisterEvent<object>(this.eventName.Value, new Action<object>(this.ReceivedEvent));
				base.Owner.RegisterEvent<object, object>(this.eventName.Value, new Action<object, object>(this.ReceivedEvent));
				base.Owner.RegisterEvent<object, object, object>(this.eventName.Value, new Action<object, object, object>(this.ReceivedEvent));
				this.registered = true;
			}
		}

		// Token: 0x06006720 RID: 26400 RVA: 0x0012A74B File Offset: 0x0012894B
		public override TaskStatus OnUpdate()
		{
			if (!this.eventReceived)
			{
				return TaskStatus.Failure;
			}
			return TaskStatus.Success;
		}

		// Token: 0x06006721 RID: 26401 RVA: 0x0012A758 File Offset: 0x00128958
		public override void OnEnd()
		{
			if (this.eventReceived)
			{
				base.Owner.UnregisterEvent(this.eventName.Value, new Action(this.ReceivedEvent));
				base.Owner.UnregisterEvent<object>(this.eventName.Value, new Action<object>(this.ReceivedEvent));
				base.Owner.UnregisterEvent<object, object>(this.eventName.Value, new Action<object, object>(this.ReceivedEvent));
				base.Owner.UnregisterEvent<object, object, object>(this.eventName.Value, new Action<object, object, object>(this.ReceivedEvent));
				this.registered = false;
			}
			this.eventReceived = false;
		}

		// Token: 0x06006722 RID: 26402 RVA: 0x0012A806 File Offset: 0x00128A06
		private void ReceivedEvent()
		{
			this.eventReceived = true;
		}

		// Token: 0x06006723 RID: 26403 RVA: 0x0012A80F File Offset: 0x00128A0F
		private void ReceivedEvent(object arg1)
		{
			this.ReceivedEvent();
			if (this.storedValue1 != null && !this.storedValue1.IsNone)
			{
				this.storedValue1.SetValue(arg1);
			}
		}

		// Token: 0x06006724 RID: 26404 RVA: 0x0012A838 File Offset: 0x00128A38
		private void ReceivedEvent(object arg1, object arg2)
		{
			this.ReceivedEvent();
			if (this.storedValue1 != null && !this.storedValue1.IsNone)
			{
				this.storedValue1.SetValue(arg1);
			}
			if (this.storedValue2 != null && !this.storedValue2.IsNone)
			{
				this.storedValue2.SetValue(arg2);
			}
		}

		// Token: 0x06006725 RID: 26405 RVA: 0x0012A890 File Offset: 0x00128A90
		private void ReceivedEvent(object arg1, object arg2, object arg3)
		{
			this.ReceivedEvent();
			if (this.storedValue1 != null && !this.storedValue1.IsNone)
			{
				this.storedValue1.SetValue(arg1);
			}
			if (this.storedValue2 != null && !this.storedValue2.IsNone)
			{
				this.storedValue2.SetValue(arg2);
			}
			if (this.storedValue3 != null && !this.storedValue3.IsNone)
			{
				this.storedValue3.SetValue(arg3);
			}
		}

		// Token: 0x06006726 RID: 26406 RVA: 0x0012A908 File Offset: 0x00128B08
		public override void OnBehaviorComplete()
		{
			base.Owner.UnregisterEvent(this.eventName.Value, new Action(this.ReceivedEvent));
			base.Owner.UnregisterEvent<object>(this.eventName.Value, new Action<object>(this.ReceivedEvent));
			base.Owner.UnregisterEvent<object, object>(this.eventName.Value, new Action<object, object>(this.ReceivedEvent));
			base.Owner.UnregisterEvent<object, object, object>(this.eventName.Value, new Action<object, object, object>(this.ReceivedEvent));
			this.eventReceived = false;
			this.registered = false;
		}

		// Token: 0x06006727 RID: 26407 RVA: 0x0012A9AB File Offset: 0x00128BAB
		public override void OnReset()
		{
			this.eventName = "";
		}

		// Token: 0x04005322 RID: 21282
		[Tooltip("The name of the event to receive")]
		public SharedString eventName = "";

		// Token: 0x04005323 RID: 21283
		[Tooltip("Optionally store the first sent argument")]
		[SharedRequired]
		public SharedVariable storedValue1;

		// Token: 0x04005324 RID: 21284
		[SharedRequired]
		[Tooltip("Optionally store the second sent argument")]
		public SharedVariable storedValue2;

		// Token: 0x04005325 RID: 21285
		[SharedRequired]
		[Tooltip("Optionally store the third sent argument")]
		public SharedVariable storedValue3;

		// Token: 0x04005326 RID: 21286
		private bool eventReceived;

		// Token: 0x04005327 RID: 21287
		private bool registered;
	}
}
