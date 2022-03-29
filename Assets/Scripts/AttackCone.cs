using UnityEngine;

public class AttackCone : MonoBehaviour {

    public TurretScript turretAI;

    public bool isLeft = false;

    void Start()
    {
        turretAI = gameObject.GetComponentInParent<TurretScript>();
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if(col.CompareTag("Player"))
        {
            if (isLeft)
                turretAI.Attack(false);
            else
                turretAI.Attack(true);
        }       
    }

}
