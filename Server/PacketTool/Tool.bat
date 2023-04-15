START bin/Debug/netcoreapp3.1/PacketTool.exe

XCOPY /Y ServerPacketManager.cs "../Server/Packet"
XCOPY /Y Packets.cs "../Server/Packet"
XCOPY /Y PacketType.cs "../Server/Packet"

XCOPY /Y ClientPacketManager.cs "../../../2DMultiSimulator-Client/Assets/Script/Server/Packet"
XCOPY /Y Packets.cs "../../../2DMultiSimulator-Client/Assets/Script/Server/Packet"
XCOPY /Y PacketType.cs "../../../2DMultiSimulator-Client/Assets/Script/Server/Packet"

cmd/k