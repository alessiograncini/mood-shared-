using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleMesh : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    [ContextMenu("Toggle Mesh")]
    void Toggle()
    {
        foreach (var mesh in GetComponentsInChildren<MeshRenderer>())
        {
            mesh.enabled = !mesh.enabled;
        }
    }
}
