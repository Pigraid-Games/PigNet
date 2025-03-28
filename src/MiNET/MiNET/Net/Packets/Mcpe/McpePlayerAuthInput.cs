using System.Collections.Generic;
using System.Numerics;
using MiNET.Utils;
using MiNET.Utils.Vectors;

namespace MiNET.Net.Packets.Mcpe;

public class McpePlayerAuthInput : Packet<McpePlayerAuthInput>
{
	public enum PlayerInputMode
	{
		Mouse = 1,
		Touch = 2,
		GamePad = 3,
		MotionController = 4
	}

	public enum PlayerInteractionModel
	{
		Touch = 0,
		Crosshair = 1,
		Classic = 2
	}

	public enum PlayerPlayMode
	{
		Normal = 0,
		Teaser = 1,
		Screen = 2,
		Viewer = 3,
		VR = 4,
		Placement = 5,
		LivingRoom = 6,
		ExitLevel = 7,
		ExitLevelLivingRoom = 8
	}

	public PlayerBlockActions Actions;
	public Vector2 AnalogMoveVector;
	public Vector3 CameraOrientation;
	public Vector3 Delta;
	public AuthInputFlags InputFlags;
	public PlayerInputMode InputMode;
	public PlayerInteractionModel InteractionModel;
	public Vector2 InteractRotation;
	public ItemStackRequests ItemStack;
	public Vector2 MoveVector;
	public PlayerPlayMode PlayMode;
	public PlayerLocation Position;
	public long Tick;
	
	public McpePlayerAuthInput()
	{
		Id = 0x90;
		IsMcpe = true;
	}


	protected override void DecodePacket()
	{
		base.DecodePacket();
		
		Vector2 Rot = ReadVector2();
		Vector3 Pos = ReadVector3();
		MoveVector = ReadVector2();
		float HeadYaw = ReadFloat();
		Position = new PlayerLocation(Pos.X, Pos.Y, Pos.Z, HeadYaw, Rot.Y, Rot.X);
		InputFlags = (AuthInputFlags) ReadUnsignedVarLong();
		InputMode = (PlayerInputMode) ReadUnsignedVarInt();
		PlayMode = (PlayerPlayMode) ReadUnsignedVarInt();
		InteractionModel = (PlayerInteractionModel) ReadUnsignedVarInt();
		InteractRotation = ReadVector2();
		Tick = ReadUnsignedVarLong();
		Delta = ReadVector3();

		if ((InputFlags & AuthInputFlags.PerformItemStackRequest) != 0) ItemStack = ReadItemStackRequests(true);

		if ((InputFlags & AuthInputFlags.PerformBlockActions) != 0) Actions = ReadPlayerBlockActions();

		AnalogMoveVector = ReadVector2();
		CameraOrientation = ReadVector3();
		ReadVector2(); // motion todo
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();
		
		WriteUnsignedVarLong((long) InputFlags);
		WriteUnsignedVarInt((uint) InputMode);
		WriteUnsignedVarInt((uint) PlayMode);
		WriteUnsignedVarInt((uint) InteractionModel);
		Write(InteractRotation);
		WriteUnsignedVarLong(Tick);
		Write(Delta);
		Write(AnalogMoveVector);
		Write(CameraOrientation);
	}

	public override void Reset()
	{
		base.Reset();
		Position = default;
		MoveVector = default;
		InputFlags = 0;
		InputMode = PlayerInputMode.Mouse;
		PlayMode = PlayerPlayMode.Normal;
		InteractionModel = PlayerInteractionModel.Touch;
		InteractRotation = default;
		Tick = 0;
		Delta = Vector3.Zero;
		AnalogMoveVector = Vector2.Zero;
		Actions = default;
		ItemStack = default;
	}

	public class PlayerBlockActions
	{
		public List<PlayerBlockActionData> PlayerBlockAction = new();
	}

	public class PlayerBlockActionData
	{
		public BlockCoordinates BlockCoordinates;
		public int Facing;
		public PlayerAction PlayerActionType;
	}
}