using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentVariations : MonoBehaviour
{

    public int NumberOfParticles = 0;
    MeshRenderer[] _meshRenderer;
    GameObject[] _text;
    GameObject[] _sides;


       private int currentValue = 0;

    void Start()
    {
        // get all of the children of this object 
        // separate mesh - sides and more 
        _meshRenderer = new MeshRenderer[NumberOfParticles];
        _text = new GameObject[NumberOfParticles];
        _sides = new GameObject[NumberOfParticles*6];
        // store them in an array

    }
    // Update is called once per frame
    [ContextMenu("Toggle Mesh")]
    /// <summary>
    ///
    /// </summary>
    void Toggle()
    {
        foreach (var mesh in GetComponentsInChildren<MeshRenderer>())
        {
            mesh.enabled = !mesh.enabled;
        }
    }
}
