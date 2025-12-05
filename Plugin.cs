using System;
using Exiled.API.Features;
using Exiled.CustomItems.API;

namespace ImpactGrenade
{
    public class Plugin : Plugin<Config>
    {
        public static Plugin Instance;

        public override string Author => "Morkamo";
        public override string Name => "ImpactGrenade";
        public override string Prefix => Name;
        public override Version Version => new Version(1, 0, 0);
        public override Version RequiredExiledVersion { get; } = new Version(9, 1, 0);

        public Grenade ImpactGrenade;

        public override void OnEnabled()
        {
            Instance = this;
            ImpactGrenade = new Grenade();
            Config.ImpactGrenade.Register();
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Config.ImpactGrenade.Unregister();
            ImpactGrenade = null;
            Instance = null;
            base.OnDisabled();
        }
    }
}