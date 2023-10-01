using CrunchEconModels.Models.Upgrades;

namespace CrunchEconUI.Services
{
    public class ProductionUpgradeService
    {
        public Dictionary<int,Upgrade> Assembler { get; set; } = new();
        public Dictionary<int,Upgrade> RefinerySpeed { get; set; } = new();
        public Dictionary<int,Upgrade> RefineryYield { get; set; } = new();

        public Dictionary<ulong, List<Upgrade>> PlayersUpgrades { get; set; }

        public List<Upgrade> GetRefineryYields()
        {
            return RefineryYield.Values.ToList();
        }

        public List<Upgrade> GetRefinerySpeeds()
        {
            return RefinerySpeed.Values.ToList();
        }

        public List<Upgrade> GetAssembler()
        {
            return Assembler.Values.ToList();
        }


        public List<Upgrade> GetPlayersUpgrade(ulong player)
        {
            return PlayersUpgrades[player];
        }

    }
}
