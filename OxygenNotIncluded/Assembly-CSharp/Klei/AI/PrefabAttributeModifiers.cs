using System;
using System.Collections.Generic;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02000E07 RID: 3591
	[AddComponentMenu("KMonoBehaviour/scripts/PrefabAttributeModifiers")]
	public class PrefabAttributeModifiers : KMonoBehaviour
	{
		// Token: 0x06006E2E RID: 28206 RVA: 0x002B6401 File Offset: 0x002B4601
		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
		}

		// Token: 0x06006E2F RID: 28207 RVA: 0x002B6409 File Offset: 0x002B4609
		public void AddAttributeDescriptor(AttributeModifier modifier)
		{
			this.descriptors.Add(modifier);
		}

		// Token: 0x06006E30 RID: 28208 RVA: 0x002B6417 File Offset: 0x002B4617
		public void RemovePrefabAttribute(AttributeModifier modifier)
		{
			this.descriptors.Remove(modifier);
		}

		// Token: 0x04005290 RID: 21136
		public List<AttributeModifier> descriptors = new List<AttributeModifier>();
	}
}
