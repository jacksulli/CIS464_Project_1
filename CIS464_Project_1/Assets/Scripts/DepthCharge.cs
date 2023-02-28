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
        yield return new WaitForSeconds(3f);
        Instantiate(explosion, new Vector3(this.gameObject.transform.position.x, 0, this.gameObject.transform.position.z), transform.rotation);
        depthChargeUses.value += 1;
        Destroy(this.gameObject);
    }
}

    
