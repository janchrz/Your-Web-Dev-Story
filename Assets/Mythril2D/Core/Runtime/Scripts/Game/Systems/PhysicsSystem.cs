using UnityEngine;

namespace Gyvr.Mythril2D
{
    public class PhysicsSystem : AGameSystem
    {
        public override void OnSystemStart()
        {
            int projectileLayerID = LayerMask.NameToLayer(GameManager.Config.projectileLayer);

            Physics2D.IgnoreLayerCollision(projectileLayerID, projectileLayerID);

            foreach (string layer in GameManager.Config.layersIgnoredByProjectiles)
            {
                Physics2D.IgnoreLayerCollision(projectileLayerID, LayerMask.NameToLayer(layer));
            }
        }
    }
}


