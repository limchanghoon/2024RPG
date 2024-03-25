using UnityEngine;

public interface IHit
{
    void Hit(int dmg, AttackAttribute attackAttribute, Transform ownerTr, bool isCri);
}
