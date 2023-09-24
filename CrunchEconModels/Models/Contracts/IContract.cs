using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrunchEconModels.Models.Contracts
{
    public interface IContract
    {
        ContractTypes ContractType { get; set; }
        ulong PlayerSteamId { get; set; }
        string ContractSettingsJson { get; set; }
        Task<EventResult> StartContract();

    }
}
