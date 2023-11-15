using System;
using Characters;
using Services;
using Singletons;
using UnityEngine;

namespace Tutorial
{
	// Token: 0x020000C6 RID: 198
	public abstract class NPC : MonoBehaviour
	{
		// Token: 0x060003D4 RID: 980
		protected abstract void Activate();

		// Token: 0x060003D5 RID: 981
		protected abstract void Deactivate();

		// Token: 0x060003D6 RID: 982 RVA: 0x0000D580 File Offset: 0x0000B780
		private void Start()
		{
			this._player = Singleton<Service>.Instance.levelManager.player;
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x0000D598 File Offset: 0x0000B798
		private void OnTriggerEnter2D(Collider2D collision)
		{
			Character component = collision.GetComponent<Character>();
			if (component == null)
			{
				return;
			}
			if (component != this._player)
			{
				return;
			}
			this.Activate();
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x0000D5CC File Offset: 0x0000B7CC
		private void OnTriggerExit2D(Collider2D collision)
		{
			Character component = collision.GetComponent<Character>();
			if (component == null)
			{
				return;
			}
			if (component != this._player)
			{
				return;
			}
			this.Deactivate();
		}

		// Token: 0x040002FB RID: 763
		protected Character _player;
	}
}
