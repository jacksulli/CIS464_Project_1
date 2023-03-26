using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthCharge : MonoBehaviour
{
    [SerializeField] private GameObject explosion;

    [SerializeField] private FloatVariable depthChargeUses;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LifeCycle());
    }

    private IEnumerator LifeCycle()
    {
        yield return new WaitForSeconds(0.4f); //In theory this seconds would depend on the upward force value
        AudioManager.Instance.PlaySound("Splash");
        yield return new WaitForSeconds(1.5f);
        Instantiate(explosion, new Vector3(this.gameObject.transform.position.x, 0, this.gameObject.transform.position.z), transform.rotation);
        Die();
        
    }

    public void Die()
    {
        depthChargeUses.value += 1;
        Destroy(this.gameObject);
    }
}

    
