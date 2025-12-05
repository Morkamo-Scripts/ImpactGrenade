using Exiled.API.Interfaces;

namespace ImpactGrenade
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = false;

        public Grenade ImpactGrenade { get; set; } = new Grenade();
    }
}