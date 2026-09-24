using UnityEngine;
using CrazyWeasol.Core;
namespace CrazyWeasol.Combat
{
    public sealed class CombatController : MonoBehaviour
    {
        [SerializeField] float attackCooldown=.28f; float nextAttack;
        void Update(){if(Time.time<nextAttack)return;if(Input.GetButtonDown("Fire1")){nextAttack=Time.time+attackCooldown;EventBus.Publish(new CombatAttackEvent(gameObject,1));}if(Input.GetButtonDown("Fire2")){nextAttack=Time.time+attackCooldown;EventBus.Publish(new CombatAttackEvent(gameObject,2));}}
    }
    public readonly struct CombatAttackEvent{public readonly GameObject Actor;public readonly int Tier;public CombatAttackEvent(GameObject actor,int tier){Actor=actor;Tier=tier;}}
}
