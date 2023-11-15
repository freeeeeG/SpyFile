using System;
using System.IO;

namespace InControl
{
	// Token: 0x020002AA RID: 682
	public struct UnknownDeviceControl : IEquatable<UnknownDeviceControl>
	{
		// Token: 0x06000CE2 RID: 3298 RVA: 0x00042826 File Offset: 0x00040C26
		public UnknownDeviceControl(InputControlType control, InputRangeType sourceRange)
		{
			this.Control = control;
			this.SourceRange = sourceRange;
		}

		// Token: 0x06000CE3 RID: 3299 RVA: 0x00042838 File Offset: 0x00040C38
		internal float GetValue(InputDevice device)
		{
			if (device == null)
			{
				return 0f;
			}
			float value = device.GetControl(this.Control).Value;
			return InputRange.Remap(value, this.SourceRange, InputRangeType.ZeroToOne);
		}

		// Token: 0x06000CE4 RID: 3300 RVA: 0x00042870 File Offset: 0x00040C70
		public static bool operator ==(UnknownDeviceControl a, UnknownDeviceControl b)
		{
			if (object.ReferenceEquals(null, a))
			{
				return object.ReferenceEquals(null, b);
			}
			return a.Equals(b);
		}

		// Token: 0x06000CE5 RID: 3301 RVA: 0x00042898 File Offset: 0x00040C98
		public static bool operator !=(UnknownDeviceControl a, UnknownDeviceControl b)
		{
			return !(a == b);
		}

		// Token: 0x06000CE6 RID: 3302 RVA: 0x000428A4 File Offset: 0x00040CA4
		public bool Equals(UnknownDeviceControl other)
		{
			return this.Control == other.Control && this.SourceRange == other.SourceRange;
		}

		// Token: 0x06000CE7 RID: 3303 RVA: 0x000428CA File Offset: 0x00040CCA
		public override bool Equals(object other)
		{
			return this.Equals((UnknownDeviceControl)other);
		}

		// Token: 0x06000CE8 RID: 3304 RVA: 0x000428D8 File Offset: 0x00040CD8
		public override int GetHashCode()
		{
			return this.Control.GetHashCode() ^ this.SourceRange.GetHashCode();
		}

		// Token: 0x06000CE9 RID: 3305 RVA: 0x000428FD File Offset: 0x00040CFD
		public static implicit operator bool(UnknownDeviceControl control)
		{
			return control.Control != InputControlType.None;
		}

		// Token: 0x06000CEA RID: 3306 RVA: 0x0004290C File Offset: 0x00040D0C
		internal void Save(BinaryWriter writer)
		{
			writer.Write((int)this.Control);
			writer.Write((int)this.SourceRange);
		}

		// Token: 0x06000CEB RID: 3307 RVA: 0x00042926 File Offset: 0x00040D26
		internal void Load(BinaryReader reader)
		{
			this.Control = (InputControlType)reader.ReadInt32();
			this.SourceRange = (InputRangeType)reader.ReadInt32();
		}

		// Token: 0x04000A05 RID: 2565
		public static readonly UnknownDeviceControl None = new UnknownDeviceControl(InputControlType.None, InputRangeType.None);

		// Token: 0x04000A06 RID: 2566
		public InputControlType Control;

		// Token: 0x04000A07 RID: 2567
		public InputRangeType SourceRange;
	}
}
