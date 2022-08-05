using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] private Vector2 parallaxEffectMultiplier;

    private Transform cameraTransform;
    private Vector3 lastCamPosition;
    private float textureUnitSizeX;

    // Start is called before the first frame update
    private void Start()
    {
        cameraTransform = Camera.main.transform;
        lastCamPosition = cameraTransform.position;
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;
        textureUnitSizeX = texture.width / sprite.pixelsPerUnit;
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        Vector3 deltaMovement = cameraTransform.position - lastCamPosition;
        transform.position += new Vector3(deltaMovement.x * parallaxEffectMultiplier.x, deltaMovement.y * parallaxEffectMultiplier.y);
        lastCamPosition = cameraTransform.position;

        if(cameraTransform.position.x - transform.position.x >= textureUnitSizeX)
        {
            float offsetPositionX = (cameraTransform.position.x - transform.position.x) % textureUnitSizeX;
            transform.position = new Vector3(cameraTransform.position.x + offsetPositionX, transform.position.y);
        }
    }
}
