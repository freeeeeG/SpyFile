using System;
using Data;
using UnityEngine;

namespace Runnables
{
	// Token: 0x0200030C RID: 780
	public sealed class ControlHardmodeLevel : Runnable
	{
		// Token: 0x06000F3B RID: 3899 RVA: 0x0002E958 File Offset: 0x0002CB58
		public override void Run()
		{
			if (!this.CheckCondition())
			{
				return;
			}
			switch (this._method)
			{
			case ControlHardmodeLevel.Method.Add:
				this.Add();
				return;
			case ControlHardmodeLevel.Method.Remove:
				this.Remove();
				return;
			case ControlHardmodeLevel.Method.Set:
				this.Set();
				return;
			default:
				return;
			}
		}

		// Token: 0x06000F3C RID: 3900 RVA: 0x0002E99C File Offset: 0x0002CB9C
		private bool CheckCondition()
		{
			return GameData.HardmodeProgress.hardmodeLevel > GameData.HardmodeProgress.clearedLevel;
		}

		// Token: 0x06000F3D RID: 3901 RVA: 0x0002E9B0 File Offset: 0x0002CBB0
		private void Add()
		{
			ControlHardmodeLevel.ValueType valueType = this._valueType;
			if (valueType == ControlHardmodeLevel.ValueType.Current)
			{
				GameData.HardmodeProgress.hardmodeLevel = Mathf.Min(GameData.HardmodeProgress.hardmodeLevel + this._value, GameData.HardmodeProgress.maxLevel);
				return;
			}
			if (valueType != ControlHardmodeLevel.ValueType.Cleared)
			{
				return;
			}
			GameData.HardmodeProgress.clearedLevel = Mathf.Min(GameData.HardmodeProgress.clearedLevel + this._value, GameData.HardmodeProgress.maxLevel);
			GameData.HardmodeProgress.SaveAll();
		}

		// Token: 0x06000F3E RID: 3902 RVA: 0x0002EA08 File Offset: 0x0002CC08
		private void Remove()
		{
			ControlHardmodeLevel.ValueType valueType = this._valueType;
			if (valueType == ControlHardmodeLevel.ValueType.Current)
			{
				GameData.HardmodeProgress.hardmodeLevel = Mathf.Max(GameData.HardmodeProgress.hardmodeLevel - this._value, 0);
				return;
			}
			if (valueType != ControlHardmodeLevel.ValueType.Cleared)
			{
				return;
			}
			GameData.HardmodeProgress.clearedLevel = Mathf.Max(GameData.HardmodeProgress.clearedLevel - this._value, 0);
			GameData.HardmodeProgress.SaveAll();
		}

		// Token: 0x06000F3F RID: 3903 RVA: 0x0002EA58 File Offset: 0x0002CC58
		private void Set()
		{
			ControlHardmodeLevel.ValueType valueType = this._valueType;
			if (valueType == ControlHardmodeLevel.ValueType.Current)
			{
				GameData.HardmodeProgress.hardmodeLevel = Mathf.Clamp(this._value, 0, GameData.HardmodeProgress.maxLevel);
				return;
			}
			if (valueType != ControlHardmodeLevel.ValueType.Cleared)
			{
				return;
			}
			GameData.HardmodeProgress.clearedLevel = Mathf.Clamp(this._value, 0, GameData.HardmodeProgress.maxLevel);
			GameData.HardmodeProgress.SaveAll();
		}

		// Token: 0x04000C8E RID: 3214
		[SerializeField]
		private ControlHardmodeLevel.ValueType _valueType;

		// Token: 0x04000C8F RID: 3215
		[SerializeField]
		private ControlHardmodeLevel.Method _method;

		// Token: 0x04000C90 RID: 3216
		[SerializeField]
		private int _value;

		// Token: 0x0200030D RID: 781
		private enum ValueType
		{
			// Token: 0x04000C92 RID: 3218
			Current,
			// Token: 0x04000C93 RID: 3219
			Cleared
		}

		// Token: 0x0200030E RID: 782
		private enum Method
		{
			// Token: 0x04000C95 RID: 3221
			Add,
			// Token: 0x04000C96 RID: 3222
			Remove,
			// Token: 0x04000C97 RID: 3223
			Set
		}
	}
}
