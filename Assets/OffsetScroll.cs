using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsetScroll : MonoBehaviour
{

    public float scrollSpeed;
    private Vector2 savedOffset;
    Material m_Material;
    void Start()
    {
        m_Material = GetComponent<Renderer>().material;

        savedOffset = m_Material.mainTextureOffset;
    }

    void Update()
    {
        float y = Mathf.Repeat(Time.time * scrollSpeed, 1);
        Vector2 offset = new Vector2(y, savedOffset.x);
        m_Material.mainTextureOffset = offset;
    }

    void OnDisable()
    {
        m_Material.mainTextureOffset = savedOffset;
    }
}