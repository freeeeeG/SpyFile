using System;
using System.IO;
using UnityEngine;

namespace InControl
{
	// Token: 0x020002A7 RID: 679
	public class UnknownDeviceBindingSource : BindingSource
	{
		// Token: 0x06000CCD RID: 3277 RVA: 0x000423BB File Offset: 0x000407BB
		internal UnknownDeviceBindingSource()
		{
			this.Control = UnknownDeviceControl.None;
		}

		// Token: 0x06000CCE RID: 3278 RVA: 0x000423CE File Offset: 0x000407CE
		public UnknownDeviceBindingSource(UnknownDeviceControl control)
		{
			this.Control = control;
		}

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x06000CCF RID: 3279 RVA: 0x000423DD File Offset: 0x000407DD
		// (set) Token: 0x06000CD0 RID: 3280 RVA: 0x000423E5 File Offset: 0x000407E5
		public UnknownDeviceControl Control { get; protected set; }

		// Token: 0x06000CD1 RID: 3281 RVA: 0x000423F0 File Offset: 0x000407F0
		public override float GetValue(InputDevice device)
		{
			return this.Control.GetValue(device);
		}

		// Token: 0x06000CD2 RID: 3282 RVA: 0x0004240C File Offset: 0x0004080C
		public override bool GetState(InputDevice device)
		{
			return device != null && Utility.IsNotZero(this.GetValue(device));
		}

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x06000CD3 RID: 3283 RVA: 0x00042424 File Offset: 0x00040824
		public override string Name
		{
			get
			{
				if (base.BoundTo == null)
				{
					return string.Empty;
				}
				string str = string.Empty;
				if (this.Control.SourceRange == InputRangeType.ZeroToMinusOne)
				{
					str = "Negative ";
				}
				else if (this.Control.SourceRange == InputRangeType.ZeroToOne)
				{
					str = "Positive ";
				}
				InputDevice device = base.BoundTo.Device;
				if (device == InputDevice.Null)
				{
					return str + this.Control.Control.ToString();
				}
				InputControl control = device.GetControl(this.Control.Control);
				if (control == InputControl.Null)
				{
					return str + this.Control.Control.ToString();
				}
				return str + control.Handle;
			}
		}

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x06000CD4 RID: 3284 RVA: 0x0004250C File Offset: 0x0004090C
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
					return "Unknown Controller";
				}
				return device.Name;
			}
		}

		// Token: 0x06000CD5 RID: 3285 RVA: 0x00042550 File Offset: 0x00040950
		public override bool Equals(BindingSource other)
		{
			if (other == null)
			{
				return false;
			}
			UnknownDeviceBindingSource unknownDeviceBindingSource = other as UnknownDeviceBindingSource;
			return unknownDeviceBindingSource != null && this.Control == unknownDeviceBindingSource.Control;
		}

		// Token: 0x06000CD6 RID: 3286 RVA: 0x00042594 File Offset: 0x00040994
		public override bool Equals(object other)
		{
			if (other == null)
			{
				return false;
			}
			UnknownDeviceBindingSource unknownDeviceBindingSource = other as UnknownDeviceBindingSource;
			return unknownDeviceBindingSource != null && this.Control == unknownDeviceBindingSource.Control;
		}

		// Token: 0x06000CD7 RID: 3287 RVA: 0x000425D0 File Offset: 0x000409D0
		public override int GetHashCode()
		{
			return this.Control.GetHashCode();
		}

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x06000CD8 RID: 3288 RVA: 0x000425F1 File Offset: 0x000409F1
		internal override BindingSourceType BindingSourceType
		{
			get
			{
				return BindingSourceType.UnknownDeviceBindingSource;
			}
		}

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x06000CD9 RID: 3289 RVA: 0x000425F4 File Offset: 0x000409F4
		internal override bool IsValid
		{
			get
			{
				if (base.BoundTo == null)
				{
					Debug.LogError("Cannot query property 'IsValid' for unbound BindingSource.");
					return false;
				}
				InputDevice device = base.BoundTo.Device;
				return device == InputDevice.Null || device.HasControl(this.Control.Control);
			}
		}

		// Token: 0x06000CDA RID: 3290 RVA: 0x00042648 File Offset: 0x00040A48
		internal override void Load(BinaryReader reader)
		{
			UnknownDeviceControl control = default(UnknownDeviceControl);
			control.Load(reader);
			this.Control = control;
		}

		// Token: 0x06000CDB RID: 3291 RVA: 0x0004266C File Offset: 0x00040A6C
		internal override void Save(BinaryWriter writer)
		{
			this.Control.Save(writer);
		}
	}
}
