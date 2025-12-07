using System.Collections.Generic;
using System.ComponentModel;
using Exiled.API.Enums;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using PlayerRoles;
using UnityEngine;

namespace ImpactGrenade.Components
{
    public abstract class ImpactGrenade : CustomItem
    {
        public override ItemType Type { get; set; } = ItemType.GrenadeHE;
        public override SpawnProperties SpawnProperties { get; set; } = null;
        public abstract ProjectileType ProjectileType { get; }
        
        public abstract bool EnableHighlight { get; set; }
        public abstract string HighlightColor { get; set; }
        public abstract float HighlightRange { get; set; }
        public abstract float HighlightIntensity { get; set; }
        
        public abstract bool EnableParticles { get; set; }
        public abstract Vector3 SpawnRange { get; set; }
        public abstract float ParticleSize { get; set; }
        public abstract ushort Intensity { get; set; }

        public abstract string PickupMessage { get; set; }
        public abstract ushort CustomItemPickupMessageDuration { get; set; }
        public abstract ushort CustomItemSelectMessageDuration { get; set; }
        public abstract ushort PickupMessageVerticalPosition { get; set; }
        public abstract ushort SelectMessageVerticalPosition { get; set; }

        [Description("{Role} : {Chance}")]
        public Dictionary<RoleTypeId, byte> GiveOnSpawnRoles { get; set; } = new()
        {
            [RoleTypeId.NtfSpecialist] = 100,
            [RoleTypeId.ChaosMarauder] = 30,
        };
    }
}