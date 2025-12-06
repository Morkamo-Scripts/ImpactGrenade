using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.API.Features.Pickups.Projectiles;
using Exiled.Events.EventArgs.Player;
using InventorySystem.Items.ThrowableProjectiles;
using LabApi.Features.Wrappers;
using PlayerRoles.FirstPersonControl;
using UnityEngine;
using Utils;
using Item = Exiled.API.Features.Items.Item;
using Map = Exiled.API.Features.Map;
using Player = Exiled.API.Features.Player;
using Projectile = Exiled.API.Features.Pickups.Projectiles.Projectile;
using Server = LabApi.Features.Wrappers.Server;

namespace ImpactGrenade.Components
{
    public class ImpactListener : MonoBehaviour
    {
        public ThrownProjectileEventArgs ev;
        public ImpactGrenade ImpactGrenade { get; set; }
        public bool BlockProcessing { get; set; }

        private void Start()
        {
            GameObject colObj = new GameObject("CustomImpactTrigger");
            colObj.transform.SetParent(transform);
            colObj.transform.localPosition = Vector3.zero;
            colObj.transform.localRotation = Quaternion.identity;
            colObj.layer = LayerMask.NameToLayer("Ignore Raycast");

            SphereCollider col = colObj.AddComponent<SphereCollider>();
            col.isTrigger = true;
            col.radius = 0.1f;
            
            Collider original = GetComponent<Collider>();
            if (original != null)
                Physics.IgnoreCollision(original, col);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.name.Equals("DoorTriggerZone"))
                return;
            
            BlockProcessing = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.gameObject.name.Equals("DoorTriggerZone"))
                return;
            
            BlockProcessing = false;
        }
        
        bool hited  = false;
        
        private void OnCollisionEnter(Collision collision)
        {
            if (BlockProcessing)
                return;

            ev.Throwable.Destroy();
            ev.Projectile.Destroy();
            ev.Item.Destroy();
            ev.Pickup.Destroy();
            
            if (hited == false)
            {
                ExplosionUtils.ServerExplode(collision.contacts[0].point, ev.Player.Footprint, ExplosionType.Grenade);
                hited = true;
            }
        }
    }
}