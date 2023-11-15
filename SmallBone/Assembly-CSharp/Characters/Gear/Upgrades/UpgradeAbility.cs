using System;
using UnityEditor;
using UnityEngine;

namespace Characters.Gear.Upgrades
{
	// Token: 0x02000850 RID: 2128
	public abstract class UpgradeAbility : MonoBehaviour
	{
		// Token: 0x06002C49 RID: 11337
		public abstract void Attach(Character target);

		// Token: 0x06002C4A RID: 11338
		public abstract void Detach();

		// Token: 0x0400256B RID: 9579
		public static Type[] types = new Type[]
		{
			typeof(AttachAbility),
			typeof(AttachSavableAbility),
			typeof(RunOperations),
			typeof(NegotiatorsCoin),
			typeof(AssetManagement),
			typeof(AdamantiumSkeleton),
			typeof(RebornRecovery)
		};

		// Token: 0x02000851 RID: 2129
		public class SubcomponentAttribute : UnityEditor.SubcomponentAttribute
		{
			// Token: 0x06002C4D RID: 11341 RVA: 0x00087973 File Offset: 0x00085B73
			public SubcomponentAttribute() : base(true, UpgradeAbility.types)
			{
			}
		}

		// Token: 0x02000852 RID: 2130
		[Serializable]
		public class Subcomponents : SubcomponentArray<UpgradeAbility>
		{
			// Token: 0x1700092F RID: 2351
			public UpgradeAbility this[int i]
			{
				get
				{
					return base.components[i];
				}
			}

			// Token: 0x06002C4F RID: 11343 RVA: 0x0008798C File Offset: 0x00085B8C
			public void Attach(Character target)
			{
				for (int i = 0; i < base.components.Length; i++)
				{
					base.components[i].Attach(target);
				}
			}

			// Token: 0x06002C50 RID: 11344 RVA: 0x000879BC File Offset: 0x00085BBC
			public void DetachAll()
			{
				for (int i = 0; i < base.components.Length; i++)
				{
					base.components[i].Detach();
				}
			}
		}
	}
}
