using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow : MonoBehaviour
{
    //[SerializeField] private Transform shadowParent;
    [SerializeField] private GameObject shadowPrefab;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] [Range(0f, 1f)] private float shadowSize = 0.5f;

    public GameObject shadow;
    private MeshRenderer mesh;
    private Material mat;

    void Start()
    {
        //shadow = Instantiate(shadowPrefab, shadowParent);
        shadow = Instantiate(shadowPrefab); //, transform
        mesh = shadow.GetComponent<MeshRenderer>();
        mat = Instantiate(mesh.material);
        mesh.material = mat;
    }

    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(gameObject.transform.position, Vector3.down, out hit, 5f, groundLayer))
            shadow.transform.position = new Vector3(hit.point.x, hit.point.y + 0.01f, hit.point.z);
        mat.SetFloat("_Scale", shadowSize);
    }
}
