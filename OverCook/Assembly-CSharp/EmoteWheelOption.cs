using System;
using UnityEngine;

// Token: 0x02000A8F RID: 2703
[Serializable]
public class EmoteWheelOption
{
	// Token: 0x04002AF2 RID: 10994
	[SerializeField]
	public EmoteWheelOption.EmoteType m_type;

	// Token: 0x04002AF3 RID: 10995
	[SerializeField]
	public string m_emoteId;

	// Token: 0x04002AF4 RID: 10996
	[SerializeField]
	public EmoteWheelOption.Connection[] m_connections = new EmoteWheelOption.Connection[8];

	// Token: 0x04002AF5 RID: 10997
	[SerializeField]
	public GameObject m_wheelButtonPrefab;

	// Token: 0x04002AF6 RID: 10998
	[SerializeField]
	public GameObject m_wheelButtonHighlightPrefab;

	// Token: 0x04002AF7 RID: 10999
	[SerializeField]
	public GameObject m_dialogPrefab;

	// Token: 0x04002AF8 RID: 11000
	[SerializeField]
	public float m_duration = 3f;

	// Token: 0x04002AF9 RID: 11001
	[SerializeField]
	public Vector2 m_anchorOffset = new Vector2(0f, 0f);

	// Token: 0x04002AFA RID: 11002
	[SerializeField]
	public string m_animTrigger;

	// Token: 0x04002AFB RID: 11003
	[SerializeField]
	public bool m_triggerForCode;

	// Token: 0x04002AFC RID: 11004
	[NonSerialized]
	public int m_animTriggerHash;

	// Token: 0x02000A90 RID: 2704
	[Serializable]
	public enum EmoteType
	{
		// Token: 0x04002AFE RID: 11006
		Dialog,
		// Token: 0x04002AFF RID: 11007
		Animation,
		// Token: 0x04002B00 RID: 11008
		Both
	}

	// Token: 0x02000A91 RID: 2705
	[Serializable]
	public class Connection
	{
		// Token: 0x04002B01 RID: 11009
		public const int c_originConnection = -1;

		// Token: 0x04002B02 RID: 11010
		public const int c_noConnection = -2;

		// Token: 0x04002B03 RID: 11011
		[SerializeField]
		public int m_connectedTo = -2;

		// Token: 0x02000A92 RID: 2706
		public enum Direction
		{
			// Token: 0x04002B05 RID: 11013
			Right,
			// Token: 0x04002B06 RID: 11014
			DownRight,
			// Token: 0x04002B07 RID: 11015
			Down,
			// Token: 0x04002B08 RID: 11016
			DownLeft,
			// Token: 0x04002B09 RID: 11017
			Left,
			// Token: 0x04002B0A RID: 11018
			UpLeft,
			// Token: 0x04002B0B RID: 11019
			Up,
			// Token: 0x04002B0C RID: 11020
			UpRight,
			// Token: 0x04002B0D RID: 11021
			COUNT
		}
	}
}
