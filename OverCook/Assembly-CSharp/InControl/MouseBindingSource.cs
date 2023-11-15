using System;
using System.IO;
using UnityEngine;

namespace InControl
{
	// Token: 0x020002A1 RID: 673
	public class MouseBindingSource : BindingSource
	{
		// Token: 0x06000C76 RID: 3190 RVA: 0x000409CB File Offset: 0x0003EDCB
		internal MouseBindingSource()
		{
		}

		// Token: 0x06000C77 RID: 3191 RVA: 0x000409D3 File Offset: 0x0003EDD3
		public MouseBindingSource(Mouse mouseControl)
		{
			this.Control = mouseControl;
		}

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x06000C78 RID: 3192 RVA: 0x000409E2 File Offset: 0x0003EDE2
		// (set) Token: 0x06000C79 RID: 3193 RVA: 0x000409EA File Offset: 0x0003EDEA
		public Mouse Control { get; protected set; }

		// Token: 0x06000C7A RID: 3194 RVA: 0x000409F4 File Offset: 0x0003EDF4
		internal static bool SafeGetMouseButton(int button)
		{
			try
			{
				return Input.GetMouseButton(button);
			}
			catch (ArgumentException)
			{
			}
			return false;
		}

		// Token: 0x06000C7B RID: 3195 RVA: 0x00040A28 File Offset: 0x0003EE28
		internal static bool ButtonIsPressed(Mouse control)
		{
			int num = MouseBindingSource.buttonTable[(int)control];
			return num >= 0 && MouseBindingSource.SafeGetMouseButton(num);
		}

		// Token: 0x06000C7C RID: 3196 RVA: 0x00040A4C File Offset: 0x0003EE4C
		public override float GetValue(InputDevice inputDevice)
		{
			int num = MouseBindingSource.buttonTable[(int)this.Control];
			if (num >= 0)
			{
				return (!MouseBindingSource.SafeGetMouseButton(num)) ? 0f : 1f;
			}
			switch (this.Control)
			{
			case Mouse.NegativeX:
				return -Mathf.Min(Input.GetAxisRaw("mouse x") * MouseBindingSource.ScaleX, 0f);
			case Mouse.PositiveX:
				return Mathf.Max(0f, Input.GetAxisRaw("mouse x") * MouseBindingSource.ScaleX);
			case Mouse.NegativeY:
				return -Mathf.Min(Input.GetAxisRaw("mouse y") * MouseBindingSource.ScaleY, 0f);
			case Mouse.PositiveY:
				return Mathf.Max(0f, Input.GetAxisRaw("mouse y") * MouseBindingSource.ScaleY);
			case Mouse.PositiveScrollWheel:
				return Mathf.Max(0f, Input.GetAxisRaw("mouse z") * MouseBindingSource.ScaleZ);
			case Mouse.NegativeScrollWheel:
				return -Mathf.Min(Input.GetAxisRaw("mouse z") * MouseBindingSource.ScaleZ, 0f);
			default:
				return 0f;
			}
		}

		// Token: 0x06000C7D RID: 3197 RVA: 0x00040B5E File Offset: 0x0003EF5E
		public override bool GetState(InputDevice inputDevice)
		{
			return Utility.IsNotZero(this.GetValue(inputDevice));
		}

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x06000C7E RID: 3198 RVA: 0x00040B6C File Offset: 0x0003EF6C
		public override string Name
		{
			get
			{
				return this.Control.ToString();
			}
		}

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x06000C7F RID: 3199 RVA: 0x00040B8D File Offset: 0x0003EF8D
		public override string DeviceName
		{
			get
			{
				return "Mouse";
			}
		}

		// Token: 0x06000C80 RID: 3200 RVA: 0x00040B94 File Offset: 0x0003EF94
		public override bool Equals(BindingSource other)
		{
			if (other == null)
			{
				return false;
			}
			MouseBindingSource mouseBindingSource = other as MouseBindingSource;
			return mouseBindingSource != null && this.Control == mouseBindingSource.Control;
		}

		// Token: 0x06000C81 RID: 3201 RVA: 0x00040BD4 File Offset: 0x0003EFD4
		public override bool Equals(object other)
		{
			if (other == null)
			{
				return false;
			}
			MouseBindingSource mouseBindingSource = other as MouseBindingSource;
			return mouseBindingSource != null && this.Control == mouseBindingSource.Control;
		}

		// Token: 0x06000C82 RID: 3202 RVA: 0x00040C0C File Offset: 0x0003F00C
		public override int GetHashCode()
		{
			return this.Control.GetHashCode();
		}

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x06000C83 RID: 3203 RVA: 0x00040C2D File Offset: 0x0003F02D
		internal override BindingSourceType BindingSourceType
		{
			get
			{
				return BindingSourceType.MouseBindingSource;
			}
		}

		// Token: 0x06000C84 RID: 3204 RVA: 0x00040C30 File Offset: 0x0003F030
		internal override void Save(BinaryWriter writer)
		{
			writer.Write((int)this.Control);
		}

		// Token: 0x06000C85 RID: 3205 RVA: 0x00040C3E File Offset: 0x0003F03E
		internal override void Load(BinaryReader reader)
		{
			this.Control = (Mouse)reader.ReadInt32();
		}

		// Token: 0x040009DC RID: 2524
		public static float ScaleX = 0.2f;

		// Token: 0x040009DD RID: 2525
		public static float ScaleY = 0.2f;

		// Token: 0x040009DE RID: 2526
		public static float ScaleZ = 0.2f;

		// Token: 0x040009DF RID: 2527
		private static readonly int[] buttonTable = new int[]
		{
			-1,
			0,
			1,
			2,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			3,
			4,
			5,
			6,
			7,
			8
		};
	}
}
