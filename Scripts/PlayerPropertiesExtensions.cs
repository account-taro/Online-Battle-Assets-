using ExitGames.Client.Photon;
using Photon.Realtime;

public static class PlayerPropertiesExtensions
{
    private const string ScoreKey = "Score";
    private static readonly Hashtable propsToSet = new Hashtable();

    public static int GetScore(this Player player)
    {
        return (player.CustomProperties[ScoreKey] is int score) ? score : 0;
    }

    public static void AddScore(this Player player, int value)
    {
        propsToSet[ScoreKey]= player.GetScore() +value;
        player.SetCustomProperties(propsToSet);
        propsToSet.Clear();
    }
}
