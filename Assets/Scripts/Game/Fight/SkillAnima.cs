using UnityEngine;

namespace Scripts.Game.Fight
{
    public class SkillAnima:MonoBehaviour
    {
        public void DestorySelf()
        {
            Destroy(this.gameObject);
        }
    }
}
