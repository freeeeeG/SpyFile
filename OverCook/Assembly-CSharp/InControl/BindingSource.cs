using System;
using System.IO;

namespace InControl
{
	// Token: 0x02000295 RID: 661
	public abstract class BindingSource : InputControlSource, IEquatable<BindingSource>
	{
		// Token: 0x06000C27 RID: 3111
		public abstract float GetValue(InputDevice inputDevice);

		// Token: 0x06000C28 RID: 3112
		public abstract bool GetState(InputDevice inputDevice);

		// Token: 0x06000C29 RID: 3113
		public abstract bool Equals(BindingSource other);

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x06000C2A RID: 3114
		public abstract string Name { get; }

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x06000C2B RID: 3115
		public abstract string DeviceName { get; }

		// Token: 0x06000C2C RID: 3116 RVA: 0x0003EEA9 File Offset: 0x0003D2A9
		public static bool operator ==(BindingSource a, BindingSource b)
		{
			return object.ReferenceEquals(a, b) || (a != null && b != null && a.BindingSourceType == b.BindingSourceType && a.Equals(b));
		}

		// Token: 0x06000C2D RID: 3117 RVA: 0x0003EEE1 File Offset: 0x0003D2E1
		public static bool operator !=(BindingSource a, BindingSource b)
		{
			return !(a == b);
		}

		// Token: 0x06000C2E RID: 3118 RVA: 0x0003EEED File Offset: 0x0003D2ED
		public override bool Equals(object other)
		{
			return this.Equals((BindingSource)other);
		}

		// Token: 0x06000C2F RID: 3119 RVA: 0x0003EEFB File Offset: 0x0003D2FB
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x06000C30 RID: 3120
		internal abstract BindingSourceType BindingSourceType { get; }

		// Token: 0x06000C31 RID: 3121
		internal abstract void Save(BinaryWriter writer);

		// Token: 0x06000C32 RID: 3122
		internal abstract void Load(BinaryReader reader);

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x06000C33 RID: 3123 RVA: 0x0003EF03 File Offset: 0x0003D303
		// (set) Token: 0x06000C34 RID: 3124 RVA: 0x0003EF0B File Offset: 0x0003D30B
		internal PlayerAction BoundTo { get; set; }

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x06000C35 RID: 3125 RVA: 0x0003EF14 File Offset: 0x0003D314
		internal virtual bool IsValid
		{
			get
			{
				return true;
			}
		}
	}
}
