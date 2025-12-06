using System.ComponentModel;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using ImpactGrenade.Components;
using InventorySystem.Items.ThrowableProjectiles;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.Arguments.ServerEvents;
using RueI.API;
using RueI.API.Elements;
using UnityEngine;
using Utils;
using Pickup = Exiled.API.Features.Pickups.Pickup;
using exVents = Exiled.Events.Handlers;
using labVents = LabApi.Events.Handlers;
using Map = LabApi.Features.Wrappers.Map;

namespace ImpactGrenade
{
    public class Grenade : Components.ImpactGrenade
    {
        public override uint Id { get; set; } = 1;
        public override string Name { get; set; } = "Ударная граната";
        public override string Description { get; set; } = "<b><color=#d18f00>[</color><color=#9c0000>Ударная граната</color><color=#d18f00>]</color>\n" +
                                                           "<color=#d18f00>~ Моментально взрывается при контакте с поверхностью ~</color></b>";
        public override float Weight { get; set; } = 1;
        public override ProjectileType ProjectileType { get; } = ProjectileType.FragGrenade;

        public override bool EnableHighlight { get; set; } = true;
        public override string HighlightColor { get; set; } = "#FF0000";
        public override float HighlightRange { get; set; } = 0.4f;
        public override float HighlightIntensity { get; set; } = 0.5f;
        
        public override bool EnableParticles { get; set; } = true;
        public override Vector3 SpawnRange { get; set; } = new(0.35f, 0.35f, 0.35f);
        public override float ParticleSize { get; set; } = 0.1f;
        public override ushort Intensity { get; set; } = 5;

        [Description("Это сообщение появляется когда игрок подбирает предмет с земли.\n" +
                     "Сообщение при взятии его в руку береться из поля Description.")]
        public override string PickupMessage { get; set; } = "<color=#d18f00><b>(Ты подобрал ударную гранату!)</b></color>";
        public override ushort CustomItemPickupMessageDuration { get; set; } = 3;
        public override ushort CustomItemSelectMessageDuration { get; set; } = 5;
        public override ushort PickupMessageVerticalPosition { get; set; } = 150;
        public override ushort SelectMessageVerticalPosition { get; set; } = 115;

        protected override void SubscribeEvents()
        {
            exVents.Player.ThrownProjectile += OnThrownProjectile;
            exVents.Player.DroppedItem += OnDroppedItem;
            exVents.Player.Spawned += OnSpawned;
            exVents.Player.ItemAdded += OnGetItem;
            exVents.Player.ChangedItem += OnChangedItem;
            labVents.ServerEvents.PickupCreated += OnPickupCreated;
        }
        
        protected override void UnsubscribeEvents()
        {
            exVents.Player.ThrownProjectile -= OnThrownProjectile;
            exVents.Player.DroppedItem -= OnDroppedItem;
            exVents.Player.Spawned -= OnSpawned;
            exVents.Player.ItemAdded -= OnGetItem;
            exVents.Player.ChangedItem -= OnChangedItem;
            labVents.ServerEvents.PickupCreated -= OnPickupCreated;
        }

        private void OnGetItem(ItemAddedEventArgs ev)
        {
            if (!Check(ev.Pickup))
                return;
            
            RueDisplay.Get(ev.Player).Show(
                new Tag(),
                new BasicElement(PickupMessageVerticalPosition, PickupMessage),
                CustomItemPickupMessageDuration);
        }

        private void OnChangedItem(ChangedItemEventArgs ev)
        {
            if (!Check(ev.Item))
                return;
            
            RueDisplay.Get(ev.Player).Show(
                new Tag(),
                new BasicElement(SelectMessageVerticalPosition, Description),
                CustomItemSelectMessageDuration);
        }

        private void OnSpawned(SpawnedEventArgs ev)
        {
            if (!GiveOnSpawnRoles.Contains(ev.Player.Role))
                return;
            
            if (!ev.Player.IsInventoryFull)
                CustomItem.TryGive(ev.Player, Id, false);
            else
                CustomItem.Get(Id)?.Spawn(ev.Player.Position);
        }
        
        private void OnThrownProjectile(ThrownProjectileEventArgs ev)
        {
            if (Check(ev.Projectile))
            {
                var listener = ev.Projectile.GameObject.AddComponent<ImpactListener>();
                listener.ev = ev;
            }
        }

        private void OnPickupCreated(PickupCreatedEventArgs ev) => HighlightItem(Pickup.Get(ev.Pickup.GameObject));
        private void OnDroppedItem(DroppedItemEventArgs ev) => HighlightItem(ev.Pickup);
        
        private void HighlightItem(Pickup pickup)
        {
            if (Check(pickup))
            {
                if (!EnableHighlight)
                    return;
                
                if (ColorUtility.TryParseHtmlString(HighlightColor, out var color))
                {
                    var anchor = HighlightManager.MakeLight(pickup.Position, color,
                        LightShadows.None, HighlightRange, HighlightIntensity);
                
                    if (EnableParticles)
                        HighlightManager.ProceduralParticles(anchor.GameObject, color, 0, 0.05f,
                            SpawnRange, ParticleSize, Intensity);
                
                    anchor.Transform.SetParent(pickup.Transform);
                    anchor.Spawn();
                }
                else
                {
                    var anchor = HighlightManager.MakeLight(pickup.Position, Color.white,
                        LightShadows.None, HighlightRange, HighlightIntensity);
                
                    if (EnableParticles)
                        HighlightManager.ProceduralParticles(anchor.GameObject, Color.white, 0, 0.05f,
                            SpawnRange, ParticleSize, Intensity);
                
                    anchor.Transform.SetParent(pickup.Transform);
                    anchor.Spawn();
                    
                    Log.Warn("Установлен некорректный цвет подсветки, выбор значения по умолчанию..."); 
                }
            }
        }
    }
}