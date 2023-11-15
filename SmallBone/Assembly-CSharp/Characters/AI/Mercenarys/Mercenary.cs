using System;
using System.Collections;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.AI.Mercenarys
{
	// Token: 0x020011E0 RID: 4576
	public class Mercenary : MonoBehaviour
	{
		// Token: 0x170011E5 RID: 4581
		// (get) Token: 0x060059D4 RID: 22996 RVA: 0x0010B139 File Offset: 0x00109339
		// (set) Token: 0x060059D5 RID: 22997 RVA: 0x0010B141 File Offset: 0x00109341
		public bool follow
		{
			get
			{
				return this._follow;
			}
			set
			{
				this._follow = value;
			}
		}

		// Token: 0x060059D6 RID: 22998 RVA: 0x0010B14A File Offset: 0x0010934A
		private void Start()
		{
			this._owner = Singleton<Service>.Instance.levelManager.player;
			base.StartCoroutine(this.HorizontalFollow());
		}

		// Token: 0x060059D7 RID: 22999 RVA: 0x0010B16E File Offset: 0x0010936E
		private IEnumerator HorizontalFollow()
		{
			for (;;)
			{
				yield return null;
				if (this._follow)
				{
					float num = this._owner.transform.position.x - this._character.transform.position.x;
					if (Mathf.Abs(num) >= this._term)
					{
						Vector2 move = (num > 0f) ? Vector2.right : Vector2.left;
						this._character.movement.move = move;
					}
				}
			}
			yield break;
		}

		// Token: 0x0400488C RID: 18572
		[SerializeField]
		private Character _character;

		// Token: 0x0400488D RID: 18573
		private Character _owner;

		// Token: 0x0400488E RID: 18574
		[SerializeField]
		private float _term = 1.5f;

		// Token: 0x0400488F RID: 18575
		[SerializeField]
		private bool _follow = true;
	}
}
