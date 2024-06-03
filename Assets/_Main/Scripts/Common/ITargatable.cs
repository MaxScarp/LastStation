using UnityEngine;

public interface ITargatable
{
    public Transform GetTarget();
    public void Hit(Gun gun = null);
}
