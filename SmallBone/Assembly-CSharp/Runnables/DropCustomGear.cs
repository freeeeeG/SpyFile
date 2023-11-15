using System;
using System.Linq;
using Characters.Gear;
using Services;
using Singletons;
using UnityEngine;

namespace Runnables
{
	// Token: 0x02000318 RID: 792
	public sealed class DropCustomGear : Runnable
	{
		// Token: 0x06000F51 RID: 3921 RVA: 0x0002EC98 File Offset: 0x0002CE98
		public override void Run()
		{
			Gear gear = this.Load();
			Singleton<Service>.Instance.levelManager.DropGear(gear, base.transform.position);
		}

		// Token: 0x06000F52 RID: 3922 RVA: 0x0002ECC8 File Offset: 0x0002CEC8
		private Gear Load()
		{
			DropCustomGear.CustomGears.Property[] values = this._customGears.values;
			float num = UnityEngine.Random.Range(0f, values.Sum((DropCustomGear.CustomGears.Property a) => a.weight));
			for (int i = 0; i < values.Length; i++)
			{
				num -= values[i].weight;
				if (num <= 0f)
				{
					return values[i].gear;
				}
			}
			return values[0].gear;
		}

		// Token: 0x04000CA6 RID: 3238
		[SerializeField]
		private DropCustomGear.CustomGears _customGears;

		// Token: 0x02000319 RID: 793
		[Serializable]
		private class CustomGears : ReorderableArray<DropCustomGear.CustomGears.Property>
		{
			// Token: 0x0200031A RID: 794
			[Serializable]
			internal class Property
			{
				// Token: 0x17000339 RID: 825
				// (get) Token: 0x06000F55 RID: 3925 RVA: 0x0002ED4B File Offset: 0x0002CF4B
				public float weight
				{
					get
					{
						return this._weight;
					}
				}

				// Token: 0x1700033A RID: 826
				// (get) Token: 0x06000F56 RID: 3926 RVA: 0x0002ED53 File Offset: 0x0002CF53
				public Gear gear
				{
					get
					{
						return this._gear;
					}
				}

				// Token: 0x04000CA7 RID: 3239
				[SerializeField]
				private float _weight;

				// Token: 0x04000CA8 RID: 3240
				[SerializeField]
				private Gear _gear;
			}
		}
	}
}
