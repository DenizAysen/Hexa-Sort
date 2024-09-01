using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Hexagon : MonoBehaviour
{
    [SerializeField] private Renderer renderer;
    private Collider _meshCollider;

    public HexStack HexStack {  get; private set; }
    public Color Color 
    {  
        get => renderer.material.color ;
        set => renderer.material.color = value; 
    }
    private void Awake()
    {
        _meshCollider = GetComponent<Collider>();
    }
    public void Configure(HexStack hexStack)
    {
        HexStack = hexStack;
    }
    public void DisableCollider()=> _meshCollider.enabled = false;
    public void SetParent(Transform parent) => transform.SetParent(parent);
    public void MoveToLocal(Vector3 targetLocalPos)
    {
        LeanTween.cancel(gameObject);

        float delay = transform.GetSiblingIndex() * .01f;

        LeanTween.moveLocal(gameObject, targetLocalPos, .2f).setEase(LeanTweenType.easeInOutSine)
            .setDelay(delay);

        Vector3 direction = (targetLocalPos - transform.localPosition).With(y: 0).normalized;
        Vector3 rotationAxis = Vector3.Cross(Vector3.up, direction);

        LeanTween.rotateAround(gameObject, rotationAxis, 180, .2f)
            .setEase(LeanTweenType.easeInOutSine)
            .setDelay(delay);
    }
    public void Vanish(float delay)
    {
        LeanTween.cancel(gameObject);
        LeanTween.scale(gameObject, Vector3.zero, .2f).
            setEase(LeanTweenType.easeInBack)
            .setDelay(delay)
            .setOnComplete(() => Destroy(gameObject));
    }
}
