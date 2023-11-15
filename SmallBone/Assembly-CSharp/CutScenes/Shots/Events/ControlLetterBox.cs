using System;
using Scenes;
using Services;
using UI;
using UnityEngine;

namespace CutScenes.Shots.Events
{
	// Token: 0x02000200 RID: 512
	public class ControlLetterBox : Event
	{
		// Token: 0x06000A6D RID: 2669 RVA: 0x0001CA01 File Offset: 0x0001AC01
		public override void Run()
		{
			if (this._type == ControlLetterBox.Type.Activate)
			{
				this.Activate();
				return;
			}
			this.Deactivate();
		}

		// Token: 0x06000A6E RID: 2670 RVA: 0x0001CA18 File Offset: 0x0001AC18
		private void Start()
		{
			this._npcConversation = Scene<GameBase>.instance.uiManager.npcConversation;
		}

		// Token: 0x06000A6F RID: 2671 RVA: 0x0001CA2F File Offset: 0x0001AC2F
		private void Activate()
		{
			LetterBox.instance.Appear(0.4f);
		}

		// Token: 0x06000A70 RID: 2672 RVA: 0x0001CA40 File Offset: 0x0001AC40
		private void Deactivate()
		{
			this._npcConversation.Done();
			LetterBox.instance.Disappear(0.4f);
		}

		// Token: 0x06000A71 RID: 2673 RVA: 0x0001CA5C File Offset: 0x0001AC5C
		public void OnDisable()
		{
			if (Service.quitting)
			{
				return;
			}
			NpcConversation npcConversation = this._npcConversation;
			if (npcConversation != null)
			{
				npcConversation.Done();
			}
			LetterBox.instance.Disappear(0.4f);
		}

		// Token: 0x06000A72 RID: 2674 RVA: 0x0001CA5C File Offset: 0x0001AC5C
		private void OnDestroy()
		{
			if (Service.quitting)
			{
				return;
			}
			NpcConversation npcConversation = this._npcConversation;
			if (npcConversation != null)
			{
				npcConversation.Done();
			}
			LetterBox.instance.Disappear(0.4f);
		}

		// Token: 0x04000880 RID: 2176
		[SerializeField]
		private ControlLetterBox.Type _type;

		// Token: 0x04000881 RID: 2177
		private NpcConversation _npcConversation;

		// Token: 0x02000201 RID: 513
		private enum Type
		{
			// Token: 0x04000883 RID: 2179
			Activate,
			// Token: 0x04000884 RID: 2180
			Deactivate
		}
	}
}
