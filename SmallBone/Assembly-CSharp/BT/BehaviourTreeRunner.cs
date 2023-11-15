using System;
using System.Collections;
using Characters;
using UnityEditor;
using UnityEngine;

namespace BT
{
	// Token: 0x020013FF RID: 5119
	public class BehaviourTreeRunner : MonoBehaviour
	{
		// Token: 0x1700141C RID: 5148
		// (get) Token: 0x060064CA RID: 25802 RVA: 0x0012427F File Offset: 0x0012247F
		public Context context
		{
			get
			{
				return this._context;
			}
		}

		// Token: 0x060064CB RID: 25803 RVA: 0x00124287 File Offset: 0x00122487
		private void OnEnable()
		{
			if (this._runOnEnable)
			{
				this.Run();
			}
		}

		// Token: 0x060064CC RID: 25804 RVA: 0x00124297 File Offset: 0x00122497
		public void Run()
		{
			this._context = Context.Create();
			this._setting.ApplyTo(this._context);
			base.StartCoroutine(this.CRun());
		}

		// Token: 0x060064CD RID: 25805 RVA: 0x001242C2 File Offset: 0x001224C2
		public void Run(Context context)
		{
			this._context = context;
			base.StartCoroutine(this.CRun());
		}

		// Token: 0x060064CE RID: 25806 RVA: 0x001242D8 File Offset: 0x001224D8
		private IEnumerator CRun()
		{
			Character character = this._context.Get<Character>(Key.OwnerCharacter);
			this._root.ResetState();
			NodeState result;
			do
			{
				this._context.deltaTime = ((character == null) ? Chronometer.global.deltaTime : character.chronometer.master.deltaTime);
				result = this._root.Tick(this._context);
				yield return null;
			}
			while (result == NodeState.Running);
			yield break;
		}

		// Token: 0x04005141 RID: 20801
		[SerializeField]
		private bool _runOnEnable = true;

		// Token: 0x04005142 RID: 20802
		[SerializeField]
		private ContextSetting _setting;

		// Token: 0x04005143 RID: 20803
		[SerializeField]
		[Subcomponent(typeof(BehaviourTree))]
		private BehaviourTree _root;

		// Token: 0x04005144 RID: 20804
		private Context _context = new Context();
	}
}
