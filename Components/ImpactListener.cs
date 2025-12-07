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
        public ThrownProjectileEventArgs Ev;
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

        private bool _hited;
        
        private void OnCollisionEnter(Collision collision)
        {
            if (BlockProcessing)
                return;

            Ev.Throwable.Destroy();
            Ev.Projectile.Destroy();
            Ev.Item.Destroy();
            Ev.Pickup.Destroy();
            
            if (_hited == false)
            {
                ExplosionUtils.ServerExplode(collision.contacts[0].point, Ev.Player.Footprint, ExplosionType.Grenade);
                _hited = true;
            }
        }
    }
}