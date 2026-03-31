using UnityEngine;

public class MoleHitbox : MonoBehaviour
{

    private Mole mole;

    private void Start()
    {
        mole = GetComponentInParent<Mole>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (mole.IsUp)
        {
            mole.TaupeDown();
        }
    }
}
