using UnityEngine;
using Map = Exiled.API.Features.Map;
using Player = Exiled.API.Features.Player;
using Projectile = Exiled.API.Features.Pickups.Projectiles.Projectile;

namespace ImpactGrenade.Components
{
    public class ImpactListener : MonoBehaviour
    {
        public Projectile Projectile;
        public ImpactGrenade ImpactGrenade;
        public Player Player;
        public bool BlockProcessing { get; set; }

        private void Start()
        {
            GameObject colObj = new GameObject("CustomImpactTrigger");
            colObj.transform.SetParent(transform);
            colObj.transform.localPosition = Vector3.zero;
            colObj.transform.localRotation = Quaternion.identity;

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

        private void OnCollisionEnter(Collision collision)
        {
            if (!BlockProcessing)
            {
                Map.Explode(Projectile.Position, ImpactGrenade.ProjectileType, Player);
                Projectile.Destroy();
            }
        }
    }
}