using System;
using Characters.Operations;
using Hardmode;
using Singletons;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.YggdrasillElderEnt
{
	// Token: 0x0200113C RID: 4412
	public class SweepHand : MonoBehaviour
	{
		// Token: 0x060055DB RID: 21979 RVA: 0x000FFE4A File Offset: 0x000FE04A
		private void Awake()
		{
			if (Singleton<HardmodeManager>.Instance.hardmode)
			{
				this._onAttackInHardmode.Initialize();
			}
		}

		// Token: 0x060055DC RID: 21980 RVA: 0x000FFE64 File Offset: 0x000FE064
		public void Attack()
		{
			this._collisionDetector.Initialize(this._monterBody, this._collider);
			base.StartCoroutine(this._collisionDetector.CRun(base.transform));
			if (Singleton<HardmodeManager>.Instance.hardmode)
			{
				this._onAttackInHardmode.gameObject.SetActive(true);
				this._onAttackInHardmode.Run(this._owner);
			}
			this._effects.gameObject.SetActive(true);
		}

		// Token: 0x060055DD RID: 21981 RVA: 0x000FFEDF File Offset: 0x000FE0DF
		public void Stop()
		{
			this._collisionDetector.Stop();
			if (Singleton<HardmodeManager>.Instance.hardmode)
			{
				this._onAttackInHardmode.Stop();
			}
			this._effects.gameObject.SetActive(false);
		}

		// Token: 0x040044CE RID: 17614
		[SerializeField]
		private Character _owner;

		// Token: 0x040044CF RID: 17615
		[GetComponent]
		[SerializeField]
		private Collider2D _collider;

		// Token: 0x040044D0 RID: 17616
		[SerializeField]
		private GameObject _monterBody;

		// Token: 0x040044D1 RID: 17617
		[SerializeField]
		private GameObject _effects;

		// Token: 0x040044D2 RID: 17618
		[SerializeField]
		private YggdrasillElderEntCollisionDetector _collisionDetector;

		// Token: 0x040044D3 RID: 17619
		[SerializeField]
		[Subcomponent(typeof(OperationInfos))]
		private OperationInfos _onAttackInHardmode;
	}
}
