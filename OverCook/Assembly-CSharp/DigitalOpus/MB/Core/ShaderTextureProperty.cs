using System;
using System.Collections.Generic;

namespace DigitalOpus.MB.Core
{
	// Token: 0x02000042 RID: 66
	[Serializable]
	public class ShaderTextureProperty
	{
		// Token: 0x060001BA RID: 442 RVA: 0x000112B4 File Offset: 0x0000F6B4
		public ShaderTextureProperty(string n, bool norm)
		{
			this.name = n;
			this.isNormalMap = norm;
		}

		// Token: 0x060001BB RID: 443 RVA: 0x000112CC File Offset: 0x0000F6CC
		public static string[] GetNames(List<ShaderTextureProperty> props)
		{
			string[] array = new string[props.Count];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = props[i].name;
			}
			return array;
		}

		// Token: 0x04000133 RID: 307
		public string name;

		// Token: 0x04000134 RID: 308
		public bool isNormalMap;
	}
}
