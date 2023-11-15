using System;
using UnityEngine;

namespace GameModes.Horde
{
	// Token: 0x020007CF RID: 1999
	public class HordeFlowController : FlowControllerBase
	{
		// Token: 0x04001E7C RID: 7804
		[AssignResource("horde_wave_number_ui", Editorbility.NonEditable)]
		[SerializeField]
		public GameObject m_waveNumberUIPrefab;

		// Token: 0x04001E7D RID: 7805
		[SerializeField]
		public float m_waveNumberUIDelay = 1f;

		// Token: 0x04001E7E RID: 7806
		[SerializeField]
		public string m_waveNumberUILocalisationTag = "Horde.Wave";

		// Token: 0x04001E7F RID: 7807
		[AssignResource("horde_mode_ui", Editorbility.Editable)]
		[SerializeField]
		public GameObject m_uiPrefab;
	}
}
