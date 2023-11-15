using System;
using Characters.Actions;
using UnityEngine;

namespace Characters.AI
{
	// Token: 0x0200111B RID: 4379
	public class StoneMonkeyBunsin : MonoBehaviour
	{
		// Token: 0x06005530 RID: 21808 RVA: 0x000FE719 File Offset: 0x000FC919
		private void Awake()
		{
			this._minion.onSummon += this.OnSummon;
			this._minion.onUnsummon += this.OnUnsummon;
		}

		// Token: 0x06005531 RID: 21809 RVA: 0x000FE749 File Offset: 0x000FC949
		private void OnSummon(Character owner, Character summoned)
		{
			this._minion.leader.player.onStartMotion += this.OnStartMotion;
		}

		// Token: 0x06005532 RID: 21810 RVA: 0x000FE76C File Offset: 0x000FC96C
		private void OnUnsummon(Character owner, Character summoned)
		{
			this._minion.leader.player.onStartMotion -= this.OnStartMotion;
		}

		// Token: 0x06005533 RID: 21811 RVA: 0x000FE790 File Offset: 0x000FC990
		private void OnStartMotion(Characters.Actions.Motion motion, float runSpeed)
		{
			if (this._minion.state != Minion.State.Summoned)
			{
				return;
			}
			StoneMonkeyBunsin.MotionMap motionMap = Array.Find<StoneMonkeyBunsin.MotionMap>(this._motionMap, (StoneMonkeyBunsin.MotionMap map) => map.motionKey.Equals(motion.key, StringComparison.OrdinalIgnoreCase));
			if (motionMap == null)
			{
				return;
			}
			this._minion.character.DoMotion(motionMap.targetMotion, runSpeed);
		}

		// Token: 0x04004448 RID: 17480
		[SerializeField]
		private Minion _minion;

		// Token: 0x04004449 RID: 17481
		[SerializeField]
		private StoneMonkeyBunsin.MotionMap[] _motionMap;

		// Token: 0x0200111C RID: 4380
		[Serializable]
		private class MotionMap
		{
			// Token: 0x170010EA RID: 4330
			// (get) Token: 0x06005535 RID: 21813 RVA: 0x000FE7EC File Offset: 0x000FC9EC
			public string motionKey
			{
				get
				{
					return this._motionKey;
				}
			}

			// Token: 0x170010EB RID: 4331
			// (get) Token: 0x06005536 RID: 21814 RVA: 0x000FE7F4 File Offset: 0x000FC9F4
			public Characters.Actions.Motion targetMotion
			{
				get
				{
					return this._targetMotion;
				}
			}

			// Token: 0x0400444A RID: 17482
			[SerializeField]
			private string _motionKey;

			// Token: 0x0400444B RID: 17483
			[SerializeField]
			private Characters.Actions.Motion _targetMotion;
		}
	}
}
