using System;
using System.Collections.Generic;

namespace Klei.AI
{
	// Token: 0x02000E02 RID: 3586
	public class Modifier : Resource
	{
		// Token: 0x06006E07 RID: 28167 RVA: 0x002B5D20 File Offset: 0x002B3F20
		public Modifier(string id, string name, string description) : base(id, name)
		{
			this.description = description;
		}

		// Token: 0x06006E08 RID: 28168 RVA: 0x002B5D3C File Offset: 0x002B3F3C
		public void Add(AttributeModifier modifier)
		{
			if (modifier.AttributeId != "")
			{
				this.SelfModifiers.Add(modifier);
			}
		}

		// Token: 0x06006E09 RID: 28169 RVA: 0x002B5D5C File Offset: 0x002B3F5C
		public virtual void AddTo(Attributes attributes)
		{
			foreach (AttributeModifier modifier in this.SelfModifiers)
			{
				attributes.Add(modifier);
			}
		}

		// Token: 0x06006E0A RID: 28170 RVA: 0x002B5DB0 File Offset: 0x002B3FB0
		public virtual void RemoveFrom(Attributes attributes)
		{
			foreach (AttributeModifier modifier in this.SelfModifiers)
			{
				attributes.Remove(modifier);
			}
		}

		// Token: 0x04005285 RID: 21125
		public string description;

		// Token: 0x04005286 RID: 21126
		public List<AttributeModifier> SelfModifiers = new List<AttributeModifier>();
	}
}
