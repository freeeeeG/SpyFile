using System;
using Characters.Abilities.Customs;
using Scenes;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x020009EC RID: 2540
	public class FighterPassiveAttacher : AbilityAttacher
	{
		// Token: 0x17000B9A RID: 2970
		// (get) Token: 0x06003608 RID: 13832 RVA: 0x000A0480 File Offset: 0x0009E680
		public bool rageReady
		{
			get
			{
				return this._fighterPassive.rageReady;
			}
		}

		// Token: 0x06003609 RID: 13833 RVA: 0x000A048D File Offset: 0x0009E68D
		public override void OnIntialize()
		{
			this._fighterPassive.Initialize();
		}

		// Token: 0x0600360A RID: 13834 RVA: 0x000A049A File Offset: 0x0009E69A
		public override void StartAttach()
		{
			base.owner.ability.Add(this._fighterPassive);
		}

		// Token: 0x0600360B RID: 13835 RVA: 0x000A04B3 File Offset: 0x0009E6B3
		public override void StopAttach()
		{
			if (base.owner == null)
			{
				return;
			}
			base.owner.ability.Remove(this._fighterPassive);
		}

		// Token: 0x0600360C RID: 13836 RVA: 0x000A04DC File Offset: 0x0009E6DC
		private void Update()
		{
			if (this._fighterPassive.buffAttached && Scene<GameBase>.instance.uiManager.letterBox.visible)
			{
				if (this._storingTime)
				{
					return;
				}
				this._storingTime = true;
				Chronometer.global.DetachTimeScale(this._fighterPassive);
				base.owner.chronometer.master.DetachTimeScale(this._fighterPassive);
				return;
			}
			else
			{
				if (!this._storingTime)
				{
					return;
				}
				this._storingTime = false;
				if (this._fighterPassive.buffAttached)
				{
					Chronometer.global.AttachTimeScale(this._fighterPassive, this._fighterPassive.timeScale);
					base.owner.chronometer.master.AttachTimeScale(this._fighterPassive, 1f / this._fighterPassive.timeScale);
				}
				return;
			}
		}

		// Token: 0x0600360D RID: 13837 RVA: 0x000A05AE File Offset: 0x0009E7AE
		public void Rage()
		{
			this._fighterPassive.AttachRage();
		}

		// Token: 0x0600360E RID: 13838 RVA: 0x0001B058 File Offset: 0x00019258
		public override string ToString()
		{
			return this.GetAutoName();
		}

		// Token: 0x04002B67 RID: 11111
		[SerializeField]
		private FighterPassive _fighterPassive;

		// Token: 0x04002B68 RID: 11112
		private bool _storingTime;
	}
}
