using System;
using System.Collections;
using System.Linq;
using Characters.AI.Behaviours.Attacks;
using Level.Chapter4;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours.Pope
{
	// Token: 0x0200135C RID: 4956
	public sealed class Purification : Behaviour
	{
		// Token: 0x060061AE RID: 25006 RVA: 0x0011DA47 File Offset: 0x0011BC47
		private void Awake()
		{
			this._platforms = new Platform[this._count];
		}

		// Token: 0x060061AF RID: 25007 RVA: 0x0011DA5A File Offset: 0x0011BC5A
		public override IEnumerator CRun(AIController controller)
		{
			base.result = Behaviour.Result.Doing;
			yield return this._moveHandler.CMove(controller);
			this.Ready(controller.character);
			yield return this._ready.CRun(controller);
			this.Purifiy(controller.character);
			yield return this._attack.CRun(controller);
			base.result = Behaviour.Result.Success;
			yield break;
		}

		// Token: 0x060061B0 RID: 25008 RVA: 0x0011DA70 File Offset: 0x0011BC70
		public void Purifiy(Character owner)
		{
			(from platform in this._platforms.ToList<Platform>()
			where platform != null
			select platform).ToList<Platform>().ForEach(delegate(Platform platform)
			{
				platform.Purifiy(owner);
			});
		}

		// Token: 0x060061B1 RID: 25009 RVA: 0x0011DAD0 File Offset: 0x0011BCD0
		private void Ready(Character owner)
		{
			this._platformContainer.NoTentacleTakeTo(this._platforms);
			(from platform in this._platforms
			where platform != null
			select platform).ToList<Platform>().ForEach(delegate(Platform platform)
			{
				platform.ShowSign(owner);
			});
		}

		// Token: 0x04004ECD RID: 20173
		[UnityEditor.Subcomponent(typeof(ActionAttack))]
		[SerializeField]
		private ActionAttack _ready;

		// Token: 0x04004ECE RID: 20174
		[SerializeField]
		[UnityEditor.Subcomponent(typeof(ActionAttack))]
		private ActionAttack _attack;

		// Token: 0x04004ECF RID: 20175
		[UnityEditor.Subcomponent(typeof(MoveHandler))]
		[SerializeField]
		private MoveHandler _moveHandler;

		// Token: 0x04004ED0 RID: 20176
		[SerializeField]
		private PlatformContainer _platformContainer;

		// Token: 0x04004ED1 RID: 20177
		[SerializeField]
		private int _count;

		// Token: 0x04004ED2 RID: 20178
		private Platform[] _platforms;
	}
}
