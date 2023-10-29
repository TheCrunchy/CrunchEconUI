using CrunchEconModels;
using CrunchEconModels.Models.EntityFramework;

namespace CrunchEconUI.Services
{
    public static class DBService
    {
        public static EconContext Context { get; set; }
        public static void Setup() 
        {
            Context = new EconContext(Program.DBString);
        }
    }
}
