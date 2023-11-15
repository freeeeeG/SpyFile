using System;
using Level;
using Scenes;
using UI.GearPopup;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x02000995 RID: 2453
	public class AbilityBuffDisplay : DroppedGear
	{
		// Token: 0x060034C5 RID: 13509 RVA: 0x0009B756 File Offset: 0x00099956
		public void Initialize(AbilityBuff dish)
		{
			this._dish = dish;
		}

		// Token: 0x060034C6 RID: 13510 RVA: 0x0009B760 File Offset: 0x00099960
		public override void OpenPopupBy(Character character)
		{
			Vector3 position = base.transform.position;
			Vector3 position2 = character.transform.position;
			position.x = position2.x + ((position.x > position2.x) ? InteractiveObject._popupUIOffset.x : (-InteractiveObject._popupUIOffset.x));
			position.y += InteractiveObject._popupUIOffset.y;
			GearPopupCanvas gearPopupCanvas = Scene<GameBase>.instance.uiManager.gearPopupCanvas;
			gearPopupCanvas.gearPopup.Set(this._dish.displayName, this._dish.description);
			gearPopupCanvas.gearPopup.SetInteractionLabel(this);
			gearPopupCanvas.Open(position);
		}

		// Token: 0x060034C7 RID: 13511 RVA: 0x0009B80F File Offset: 0x00099A0F
		public override void ClosePopup()
		{
			base.ClosePopup();
			Scene<GameBase>.instance.uiManager.gearPopupCanvas.Close();
		}

		// Token: 0x060034C8 RID: 13512 RVA: 0x00002191 File Offset: 0x00000391
		public void Initialize()
		{
		}

		// Token: 0x04002A84 RID: 10884
		private AbilityBuff _dish;
	}
}
