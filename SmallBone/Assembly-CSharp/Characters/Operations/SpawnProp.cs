using System;
using Level;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000E38 RID: 3640
	public sealed class SpawnProp : CharacterOperation
	{
		// Token: 0x06004887 RID: 18567 RVA: 0x000D2E4C File Offset: 0x000D104C
		public override void Run(Character owner)
		{
			Prop prop = UnityEngine.Object.Instantiate<Prop>(this._prop, this._spawnPoint.transform.position, Quaternion.identity, Map.Instance.transform);
			if (this.relatedLookingDirection && owner.lookingDirection == Character.LookingDirection.Left)
			{
				prop.transform.localScale = new Vector3(prop.transform.localScale.x * -1f, prop.transform.localScale.y, prop.transform.localScale.z);
			}
		}

		// Token: 0x0400379E RID: 14238
		[SerializeField]
		private Prop _prop;

		// Token: 0x0400379F RID: 14239
		[SerializeField]
		private Transform _spawnPoint;

		// Token: 0x040037A0 RID: 14240
		[SerializeField]
		private bool relatedLookingDirection;
	}
}
