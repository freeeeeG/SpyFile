using System;
using System.Collections;
using System.Collections.Generic;
using Characters;
using PhysicsUtils;
using UnityEngine;

namespace Runnables.Triggers
{
	// Token: 0x02000357 RID: 855
	public class EnterZone : Trigger
	{
		// Token: 0x06000FFA RID: 4090 RVA: 0x0002FC99 File Offset: 0x0002DE99
		private void Start()
		{
			base.StartCoroutine(this.CTriggerEnter());
		}

		// Token: 0x06000FFB RID: 4091 RVA: 0x0002FCA8 File Offset: 0x0002DEA8
		public IEnumerator CTriggerEnter()
		{
			for (;;)
			{
				yield return null;
				EnterZone._lapper.contactFilter.SetLayerMask(this._layer);
				ReadonlyBoundedList<Collider2D> results = EnterZone._lapper.OverlapCollider(this._collider).results;
				this._result = false;
				if (results.Count != 0)
				{
					if (this._checkCharacterType)
					{
						using (IEnumerator<Collider2D> enumerator = results.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								Collider2D collider2D = enumerator.Current;
								Target component = collider2D.GetComponent<Target>();
								if (!(component == null) && component.character.type == this._characterType)
								{
									this._result = true;
									break;
								}
							}
							continue;
						}
						yield break;
					}
					this._result = true;
				}
			}
		}

		// Token: 0x06000FFC RID: 4092 RVA: 0x0002FCB7 File Offset: 0x0002DEB7
		protected override bool Check()
		{
			return this._result;
		}

		// Token: 0x04000D11 RID: 3345
		[SerializeField]
		private Collider2D _collider;

		// Token: 0x04000D12 RID: 3346
		[SerializeField]
		private LayerMask _layer;

		// Token: 0x04000D13 RID: 3347
		[Space]
		[Header("Character Type")]
		[SerializeField]
		private bool _checkCharacterType;

		// Token: 0x04000D14 RID: 3348
		[SerializeField]
		private Character.Type _characterType;

		// Token: 0x04000D15 RID: 3349
		private bool _result;

		// Token: 0x04000D16 RID: 3350
		private static readonly NonAllocOverlapper _lapper = new NonAllocOverlapper(15);
	}
}
