using ExitGames.Client.Photon;
using Photon.Realtime;

public static class GameRoomProperty
{
    private const string KeyStartTime = "StartTime";
    private static readonly Hashtable propsToSet = new Hashtable();

    public static bool TryGetStartTime(this Room room, out int timestamp)
    {
        if (room.CustomProperties[KeyStartTime] is int value)
        {
            timestamp = value;
            return true;
        }
        else
        {
            timestamp = 0;
            return false;
        }
    }

    public static void SetStartTime(this Room room, int timestamp)
    {
        propsToSet[KeyStartTime] = timestamp;
        room.SetCustomProperties(propsToSet);
        propsToSet.Clear();
    }
}
