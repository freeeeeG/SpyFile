using System;
using System.Collections;
using Characters.Abilities.Triggers;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x020009FE RID: 2558
	public class TriggerAbilityAttacher : AbilityAttacher
	{
		// Token: 0x17000BA1 RID: 2977
		// (get) Token: 0x06003663 RID: 13923 RVA: 0x000A110C File Offset: 0x0009F30C
		// (set) Token: 0x06003664 RID: 13924 RVA: 0x000A1114 File Offset: 0x0009F314
		public bool attached { get; private set; }

		// Token: 0x06003665 RID: 13925 RVA: 0x000A111D File Offset: 0x0009F31D
		private void Awake()
		{
			this._trigger.onTriggered += this.OnTriggered;
		}

		// Token: 0x06003666 RID: 13926 RVA: 0x000A1136 File Offset: 0x0009F336
		private void OnTriggered()
		{
			base.owner.ability.Add(this._abilityComponent.ability);
		}

		// Token: 0x06003667 RID: 13927 RVA: 0x000A1154 File Offset: 0x0009F354
		public override void OnIntialize()
		{
			this._abilityComponent.Initialize();
		}

		// Token: 0x06003668 RID: 13928 RVA: 0x000A1161 File Offset: 0x0009F361
		public override void StartAttach()
		{
			this.attached = true;
			this._trigger.Attach(base.owner);
			this._cUpdateReference = this.StartCoroutineWithReference(this.CUpdate());
		}

		// Token: 0x06003669 RID: 13929 RVA: 0x000A1190 File Offset: 0x0009F390
		public override void StopAttach()
		{
			this.attached = false;
			if (base.owner == null)
			{
				return;
			}
			this._trigger.Detach();
			this._cUpdateReference.Stop();
			base.owner.ability.Remove(this._abilityComponent.ability);
		}

		// Token: 0x0600366A RID: 13930 RVA: 0x000A11E5 File Offset: 0x0009F3E5
		private IEnumerator CUpdate()
		{
			for (;;)
			{
				this._trigger.UpdateTime(Chronometer.global.deltaTime);
				yield return null;
			}
			yield break;
		}

		// Token: 0x0600366B RID: 13931 RVA: 0x0001B058 File Offset: 0x00019258
		public override string ToString()
		{
			return this.GetAutoName();
		}

		// Token: 0x04002B9D RID: 11165
		[SerializeField]
		[TriggerComponent.SubcomponentAttribute]
		private TriggerComponent _trigger;

		// Token: 0x04002B9E RID: 11166
		[SerializeField]
		[AbilityComponent.SubcomponentAttribute]
		private AbilityComponent _abilityComponent;

		// Token: 0x04002B9F RID: 11167
		private CoroutineReference _cUpdateReference;
	}
}
