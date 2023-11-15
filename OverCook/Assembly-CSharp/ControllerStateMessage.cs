using System;
using BitStream;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x020008AC RID: 2220
public class ControllerStateMessage : Serialisable
{
	// Token: 0x06002B44 RID: 11076 RVA: 0x000CA994 File Offset: 0x000C8D94
	public bool Deserialise(BitStreamReader reader)
	{
		this.m_ButtonStates = reader.ReadByte(4);
		this.m_AxisX = reader.ReadFloat32();
		this.m_AxisY = reader.ReadFloat32();
		this.m_uChefEntityID = (uint)reader.ReadUInt16(10);
		this.m_Time = reader.ReadFloat32();
		reader.ReadQuaternion(ref this.rotation);
		this.m_underControl = reader.ReadBit();
		return true;
	}

	// Token: 0x06002B45 RID: 11077 RVA: 0x000CA9FC File Offset: 0x000C8DFC
	public void Serialise(BitStreamWriter writer)
	{
		writer.Write(this.m_ButtonStates, 4);
		writer.Write(this.m_AxisX);
		writer.Write(this.m_AxisY);
		writer.Write(this.m_uChefEntityID, 10);
		writer.Write(this.m_Time);
		writer.Write(ref this.rotation);
		writer.Write(this.m_underControl);
	}

	// Token: 0x06002B46 RID: 11078 RVA: 0x000CAA60 File Offset: 0x000C8E60
	public bool IsButtonDown(PlayerInputLookup.LogicalButtonID _button)
	{
		switch (_button)
		{
		case PlayerInputLookup.LogicalButtonID.PickupAndDrop:
			return (this.m_ButtonStates & 4) != 0;
		case PlayerInputLookup.LogicalButtonID.Dash:
			return (this.m_ButtonStates & 1) != 0;
		case PlayerInputLookup.LogicalButtonID.WorkstationInteract:
			return (this.m_ButtonStates & 8) != 0;
		default:
			return _button == PlayerInputLookup.LogicalButtonID.Curse && (this.m_ButtonStates & 2) != 0;
		}
	}

	// Token: 0x06002B47 RID: 11079 RVA: 0x000CAACC File Offset: 0x000C8ECC
	public static byte GetButtonState(bool _dash, bool _curse, bool _pickup, bool _interact)
	{
		byte b = 0;
		if (_dash)
		{
			b |= 1;
		}
		if (_curse)
		{
			b |= 2;
		}
		if (_pickup)
		{
			b |= 4;
		}
		if (_interact)
		{
			b |= 8;
		}
		return b;
	}

	// Token: 0x06002B48 RID: 11080 RVA: 0x000CAB08 File Offset: 0x000C8F08
	public void SetButtonPressed(PlayerInputLookup.LogicalButtonID _button, bool _pressed)
	{
		if (_pressed)
		{
			switch (_button)
			{
			case PlayerInputLookup.LogicalButtonID.PickupAndDrop:
				this.m_ButtonStates |= 4;
				break;
			case PlayerInputLookup.LogicalButtonID.Dash:
				this.m_ButtonStates |= 1;
				break;
			case PlayerInputLookup.LogicalButtonID.WorkstationInteract:
				this.m_ButtonStates |= 8;
				break;
			default:
				if (_button == PlayerInputLookup.LogicalButtonID.Curse)
				{
					this.m_ButtonStates |= 2;
				}
				break;
			}
		}
		else
		{
			switch (_button)
			{
			case PlayerInputLookup.LogicalButtonID.PickupAndDrop:
				this.m_ButtonStates = (byte)((int)this.m_ButtonStates & -5);
				break;
			case PlayerInputLookup.LogicalButtonID.Dash:
				this.m_ButtonStates = (byte)((int)this.m_ButtonStates & -2);
				break;
			case PlayerInputLookup.LogicalButtonID.WorkstationInteract:
				this.m_ButtonStates = (byte)((int)this.m_ButtonStates & -9);
				break;
			default:
				if (_button == PlayerInputLookup.LogicalButtonID.Curse)
				{
					this.m_ButtonStates = (byte)((int)this.m_ButtonStates & -3);
				}
				break;
			}
		}
	}

	// Token: 0x06002B49 RID: 11081 RVA: 0x000CAC10 File Offset: 0x000C9010
	public void Copy(ControllerStateMessage _other)
	{
		this.m_ButtonStates = _other.m_ButtonStates;
		this.m_AxisX = _other.m_AxisX;
		this.m_AxisY = _other.m_AxisY;
		this.m_uChefEntityID = _other.m_uChefEntityID;
		this.m_Time = _other.m_Time;
	}

	// Token: 0x06002B4A RID: 11082 RVA: 0x000CAC50 File Offset: 0x000C9050
	public bool IsDifferent(ControllerStateMessage other)
	{
		return (other == null && this != null) || (this == null && other != null) || ((other != null || this != null) && (other.m_AxisX != this.m_AxisX || other.m_AxisY != this.m_AxisY || other.m_ButtonStates != this.m_ButtonStates || other.m_uChefEntityID != this.m_uChefEntityID || other.m_Time != this.m_Time || other.m_underControl != this.m_underControl));
	}

	// Token: 0x04002242 RID: 8770
	public uint m_uChefEntityID;

	// Token: 0x04002243 RID: 8771
	public byte m_ButtonStates;

	// Token: 0x04002244 RID: 8772
	public float m_AxisX;

	// Token: 0x04002245 RID: 8773
	public float m_AxisY;

	// Token: 0x04002246 RID: 8774
	public float m_Time;

	// Token: 0x04002247 RID: 8775
	public bool m_underControl = true;

	// Token: 0x04002248 RID: 8776
	public Quaternion rotation;
}
