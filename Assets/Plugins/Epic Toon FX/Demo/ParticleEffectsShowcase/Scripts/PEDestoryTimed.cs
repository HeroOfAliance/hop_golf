using UnityEngine;
using System.Collections;

public class PEDestoryTimed : MonoBehaviour {

    [SerializeField]
    private float _lifeTime;

	void Start ()
    {
        Destroy(gameObject, _lifeTime);
	}
	
}
