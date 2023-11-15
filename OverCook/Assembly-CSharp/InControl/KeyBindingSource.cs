using System;
using System.IO;

namespace InControl
{
	// Token: 0x0200029C RID: 668
	public class KeyBindingSource : BindingSource
	{
		// Token: 0x06000C4D RID: 3149 RVA: 0x0003F296 File Offset: 0x0003D696
		internal KeyBindingSource()
		{
		}

		// Token: 0x06000C4E RID: 3150 RVA: 0x0003F29E File Offset: 0x0003D69E
		public KeyBindingSource(KeyCombo keyCombo)
		{
			this.Control = keyCombo;
		}

		// Token: 0x06000C4F RID: 3151 RVA: 0x0003F2AD File Offset: 0x0003D6AD
		public KeyBindingSource(params Key[] keys)
		{
			this.Control = new KeyCombo(keys);
		}

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x06000C50 RID: 3152 RVA: 0x0003F2C1 File Offset: 0x0003D6C1
		// (set) Token: 0x06000C51 RID: 3153 RVA: 0x0003F2C9 File Offset: 0x0003D6C9
		public KeyCombo Control { get; protected set; }

		// Token: 0x06000C52 RID: 3154 RVA: 0x0003F2D2 File Offset: 0x0003D6D2
		public override float GetValue(InputDevice inputDevice)
		{
			return (!this.GetState(inputDevice)) ? 0f : 1f;
		}

		// Token: 0x06000C53 RID: 3155 RVA: 0x0003F2F0 File Offset: 0x0003D6F0
		public override bool GetState(InputDevice inputDevice)
		{
			return this.Control.IsPressed;
		}

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x06000C54 RID: 3156 RVA: 0x0003F30C File Offset: 0x0003D70C
		public override string Name
		{
			get
			{
				return this.Control.ToString();
			}
		}

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x06000C55 RID: 3157 RVA: 0x0003F32D File Offset: 0x0003D72D
		public override string DeviceName
		{
			get
			{
				return "Keyboard";
			}
		}

		// Token: 0x06000C56 RID: 3158 RVA: 0x0003F334 File Offset: 0x0003D734
		public override bool Equals(BindingSource other)
		{
			if (other == null)
			{
				return false;
			}
			KeyBindingSource keyBindingSource = other as KeyBindingSource;
			return keyBindingSource != null && this.Control == keyBindingSource.Control;
		}

		// Token: 0x06000C57 RID: 3159 RVA: 0x0003F378 File Offset: 0x0003D778
		public override bool Equals(object other)
		{
			if (other == null)
			{
				return false;
			}
			KeyBindingSource keyBindingSource = other as KeyBindingSource;
			return keyBindingSource != null && this.Control == keyBindingSource.Control;
		}

		// Token: 0x06000C58 RID: 3160 RVA: 0x0003F3B4 File Offset: 0x0003D7B4
		public override int GetHashCode()
		{
			return this.Control.GetHashCode();
		}

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x06000C59 RID: 3161 RVA: 0x0003F3D5 File Offset: 0x0003D7D5
		internal override BindingSourceType BindingSourceType
		{
			get
			{
				return BindingSourceType.KeyBindingSource;
			}
		}

		// Token: 0x06000C5A RID: 3162 RVA: 0x0003F3D8 File Offset: 0x0003D7D8
		internal override void Load(BinaryReader reader)
		{
			KeyCombo control = default(KeyCombo);
			control.Load(reader);
			this.Control = control;
		}

		// Token: 0x06000C5B RID: 3163 RVA: 0x0003F3FC File Offset: 0x0003D7FC
		internal override void Save(BinaryWriter writer)
		{
			this.Control.Save(writer);
		}
	}
}
