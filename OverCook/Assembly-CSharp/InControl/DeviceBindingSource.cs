using System;
using System.IO;
using UnityEngine;

namespace InControl
{
	// Token: 0x02000299 RID: 665
	public class DeviceBindingSource : BindingSource
	{
		// Token: 0x06000C38 RID: 3128 RVA: 0x0003EF17 File Offset: 0x0003D317
		internal DeviceBindingSource()
		{
			this.Control = InputControlType.None;
		}

		// Token: 0x06000C39 RID: 3129 RVA: 0x0003EF26 File Offset: 0x0003D326
		public DeviceBindingSource(InputControlType control)
		{
			this.Control = control;
		}

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x06000C3A RID: 3130 RVA: 0x0003EF35 File Offset: 0x0003D335
		// (set) Token: 0x06000C3B RID: 3131 RVA: 0x0003EF3D File Offset: 0x0003D33D
		public InputControlType Control { get; protected set; }

		// Token: 0x06000C3C RID: 3132 RVA: 0x0003EF46 File Offset: 0x0003D346
		public override float GetValue(InputDevice inputDevice)
		{
			if (inputDevice == null)
			{
				return 0f;
			}
			return inputDevice.GetControl(this.Control).Value;
		}

		// Token: 0x06000C3D RID: 3133 RVA: 0x0003EF65 File Offset: 0x0003D365
		public override bool GetState(InputDevice inputDevice)
		{
			return inputDevice != null && inputDevice.GetControl(this.Control).State;
		}

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x06000C3E RID: 3134 RVA: 0x0003EF80 File Offset: 0x0003D380
		public override string Name
		{
			get
			{
				if (base.BoundTo == null)
				{
					return string.Empty;
				}
				InputDevice device = base.BoundTo.Device;
				InputControl control = device.GetControl(this.Control);
				if (control == InputControl.Null)
				{
					return this.Control.ToString();
				}
				return device.GetControl(this.Control).Handle;
			}
		}

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x06000C3F RID: 3135 RVA: 0x0003EFE8 File Offset: 0x0003D3E8
		public override string DeviceName
		{
			get
			{
				if (base.BoundTo == null)
				{
					return string.Empty;
				}
				InputDevice device = base.BoundTo.Device;
				if (device == InputDevice.Null)
				{
					return "Controller";
				}
				return device.Name;
			}
		}

		// Token: 0x06000C40 RID: 3136 RVA: 0x0003F02C File Offset: 0x0003D42C
		public override bool Equals(BindingSource other)
		{
			if (other == null)
			{
				return false;
			}
			DeviceBindingSource deviceBindingSource = other as DeviceBindingSource;
			return deviceBindingSource != null && this.Control == deviceBindingSource.Control;
		}

		// Token: 0x06000C41 RID: 3137 RVA: 0x0003F06C File Offset: 0x0003D46C
		public override bool Equals(object other)
		{
			if (other == null)
			{
				return false;
			}
			DeviceBindingSource deviceBindingSource = other as DeviceBindingSource;
			return deviceBindingSource != null && this.Control == deviceBindingSource.Control;
		}

		// Token: 0x06000C42 RID: 3138 RVA: 0x0003F0A4 File Offset: 0x0003D4A4
		public override int GetHashCode()
		{
			return this.Control.GetHashCode();
		}

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x06000C43 RID: 3139 RVA: 0x0003F0C5 File Offset: 0x0003D4C5
		internal override BindingSourceType BindingSourceType
		{
			get
			{
				return BindingSourceType.DeviceBindingSource;
			}
		}

		// Token: 0x06000C44 RID: 3140 RVA: 0x0003F0C8 File Offset: 0x0003D4C8
		internal override void Save(BinaryWriter writer)
		{
			writer.Write((int)this.Control);
		}

		// Token: 0x06000C45 RID: 3141 RVA: 0x0003F0D6 File Offset: 0x0003D4D6
		internal override void Load(BinaryReader reader)
		{
			this.Control = (InputControlType)reader.ReadInt32();
		}

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x06000C46 RID: 3142 RVA: 0x0003F0E4 File Offset: 0x0003D4E4
		internal override bool IsValid
		{
			get
			{
				if (base.BoundTo == null)
				{
					Debug.LogError("Cannot query property 'IsValid' for unbound BindingSource.");
					return false;
				}
				return base.BoundTo.Device.HasControl(this.Control) || Utility.TargetIsStandard(this.Control);
			}
		}
	}
}
