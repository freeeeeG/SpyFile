using System;
using Characters;
using UnityEditor;
using UnityEngine;

namespace FX.CastAttackVisualEffect
{
	// Token: 0x0200028A RID: 650
	public abstract class CastAttackVisualEffect : VisualEffect
	{
		// Token: 0x06000CAB RID: 3243
		public abstract void Spawn(Vector3 position);

		// Token: 0x06000CAC RID: 3244
		public abstract void Spawn(Character owner, Collider2D collider, Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit);

		// Token: 0x06000CAD RID: 3245
		public abstract void Spawn(Character owner, Collider2D collider, Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit, Damage damage, ITarget target);

		// Token: 0x04000AE4 RID: 2788
		public static readonly Type[] types = new Type[]
		{
			typeof(SpawnOnHitPoint)
		};

		// Token: 0x0200028B RID: 651
		public class SubcomponentAttribute : UnityEditor.SubcomponentAttribute
		{
			// Token: 0x06000CB0 RID: 3248 RVA: 0x000295D0 File Offset: 0x000277D0
			public SubcomponentAttribute() : base(true, CastAttackVisualEffect.types)
			{
			}
		}

		// Token: 0x0200028C RID: 652
		[Serializable]
		public class Subcomponents : SubcomponentArray<CastAttackVisualEffect>
		{
			// Token: 0x06000CB1 RID: 3249 RVA: 0x000295E0 File Offset: 0x000277E0
			public void Spawn(Vector3 position)
			{
				for (int i = 0; i < this._components.Length; i++)
				{
					this._components[i].Spawn(position);
				}
			}

			// Token: 0x06000CB2 RID: 3250 RVA: 0x00029610 File Offset: 0x00027810
			public void Spawn(Character owner, Collider2D collider, Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit)
			{
				for (int i = 0; i < this._components.Length; i++)
				{
					this._components[i].Spawn(owner, collider, origin, direction, distance, raycastHit);
				}
			}

			// Token: 0x06000CB3 RID: 3251 RVA: 0x00029648 File Offset: 0x00027848
			public void Spawn(Character owner, Collider2D collider, Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit, Damage damage, ITarget target)
			{
				for (int i = 0; i < this._components.Length; i++)
				{
					this._components[i].Spawn(owner, collider, origin, direction, distance, raycastHit, damage, target);
				}
			}
		}
	}
}
