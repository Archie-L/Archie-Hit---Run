using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damageMachiene : MonoBehaviour
{
    public GameObject box;
    public Vector3 boxsize = new Vector3(2, 0.5f, 1.7f);
    public bool slicing;
    public float damage;
    public string[] tags;
    public string[] tagscripts;
    public List<GameObject> sliced;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (slicing)
        {
            Collider[] hitColliders = Physics.OverlapBox(box.transform.position, boxsize, box.transform.rotation);
            foreach (Collider col in hitColliders)
            {
                bool nuts = false;
                for (int i = 0; i < sliced.Count; i++)
                {
                    if (sliced[i] == col.gameObject)
                    {
                        nuts = true;
                    }
                }
                if (!nuts)
                {
                    foreach (string tag in tags)
                    {
                        if (col.gameObject.tag == tag)
                        {
                            col.gameObject.GetComponent<health>().NPCHealth -= damage;
                        }
                        sliced.Add(col.gameObject);
                    }
                }
            }
        }
        else
        {
            sliced = new List<GameObject>();
        }
    }

    private void OnDrawGizmos()
    {
        if (slicing)
        {
            Gizmos.matrix = box.transform.localToWorldMatrix;
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(Vector3.zero, boxsize / box.transform.lossyScale.x);
        }
    }
}
