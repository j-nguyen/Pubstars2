using HQMEditorDedicated;

namespace PubstarsGameServer.Model
{
    public class RankedPlayer
    {
        public readonly string Name;
        public readonly byte[] IP;
        public readonly Player PlayerStruct;
        public readonly double Rating;

        public RankedPlayer(string name, byte[] ip, Player player, double rating)
        {
            Name = name;
            IP = ip;
            PlayerStruct = player;
            Rating = rating;
        }
    }
}
