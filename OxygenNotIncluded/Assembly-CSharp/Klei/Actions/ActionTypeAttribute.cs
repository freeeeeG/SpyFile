using System;

namespace Klei.Actions
{
	// Token: 0x02000E1F RID: 3615
	[AttributeUsage(AttributeTargets.Class, Inherited = true)]
	public class ActionTypeAttribute : Attribute
	{
		// Token: 0x06006EB7 RID: 28343 RVA: 0x002B863F File Offset: 0x002B683F
		public ActionTypeAttribute(string groupName, string typeName, bool generateConfig = true)
		{
			this.TypeName = typeName;
			this.GroupName = groupName;
			this.GenerateConfig = generateConfig;
		}

		// Token: 0x06006EB8 RID: 28344 RVA: 0x002B865C File Offset: 0x002B685C
		public static bool operator ==(ActionTypeAttribute lhs, ActionTypeAttribute rhs)
		{
			bool flag = object.Equals(lhs, null);
			bool flag2 = object.Equals(rhs, null);
			if (flag || flag2)
			{
				return flag == flag2;
			}
			return lhs.TypeName == rhs.TypeName && lhs.GroupName == rhs.GroupName;
		}

		// Token: 0x06006EB9 RID: 28345 RVA: 0x002B86A9 File Offset: 0x002B68A9
		public static bool operator !=(ActionTypeAttribute lhs, ActionTypeAttribute rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x06006EBA RID: 28346 RVA: 0x002B86B5 File Offset: 0x002B68B5
		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		// Token: 0x06006EBB RID: 28347 RVA: 0x002B86BE File Offset: 0x002B68BE
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x040052E2 RID: 21218
		public readonly string TypeName;

		// Token: 0x040052E3 RID: 21219
		public readonly string GroupName;

		// Token: 0x040052E4 RID: 21220
		public readonly bool GenerateConfig;
	}
}
