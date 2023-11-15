using System;

namespace UnityEngine.Rendering.Universal.PostProcessing
{
	// Token: 0x020000BE RID: 190
	[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
	public sealed class CompoundRendererFeatureAttribute : Attribute
	{
		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060002A7 RID: 679 RVA: 0x0000AC6C File Offset: 0x00008E6C
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060002A8 RID: 680 RVA: 0x0000AC74 File Offset: 0x00008E74
		public InjectionPoint InjectionPoint
		{
			get
			{
				return this.injectionPoint;
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060002A9 RID: 681 RVA: 0x0000AC7C File Offset: 0x00008E7C
		public bool ShareInstance
		{
			get
			{
				return this.shareInstance;
			}
		}

		// Token: 0x060002AA RID: 682 RVA: 0x0000AC84 File Offset: 0x00008E84
		public CompoundRendererFeatureAttribute(string name, InjectionPoint injectionPoint, bool shareInstance = false)
		{
			this.name = name;
			this.injectionPoint = injectionPoint;
			this.shareInstance = shareInstance;
		}

		// Token: 0x060002AB RID: 683 RVA: 0x0000ACA4 File Offset: 0x00008EA4
		public static CompoundRendererFeatureAttribute GetAttribute(Type type)
		{
			if (type == null)
			{
				return null;
			}
			object[] customAttributes = type.GetCustomAttributes(typeof(CompoundRendererFeatureAttribute), false);
			if (customAttributes.Length == 0)
			{
				return null;
			}
			return customAttributes[0] as CompoundRendererFeatureAttribute;
		}

		// Token: 0x04000234 RID: 564
		private readonly string name;

		// Token: 0x04000235 RID: 565
		private readonly InjectionPoint injectionPoint;

		// Token: 0x04000236 RID: 566
		private readonly bool shareInstance;
	}
}
