
namespace PigNet.Net.EnumerationsTable;

public enum CameraAimAssistTargetMode
{
	Angle = 0,
	Distance = 1
}

public enum CameraAimAssistPresetPacketOperation
{
	Aaa = 1
}

public enum CameraPresetAudioListener
{
	Camera = 0,
	Player = 1
}

public enum CameraShakeAction
{
	Add = 0,
	Stop = 1
}

public enum CameraShakeType
{
	Positional = 0,
	Rotational = 1
}

public enum ClientCameraAimAssistPacketAction
{
	SetFromCameraPreset = 0,
	Clear = 1,
	Count = 2
}