using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergePushEffect : MonoBehaviour
{
    [Header(" Settings ")]
    [SerializeField] private float pushRadius;
    [SerializeField] private Vector2 minMaxPushMagnitude;
    [SerializeField] private float pushMagnitude;
    private Vector2 pushPosition;

    [Header(" DEBUG ")]
    [SerializeField] private bool enableGizmos;

    private void Awake()
    {
        MergeManager.onMergeProcessed += MergeProcessedCallback;
        SettingsManager.onPushMagnitudeChanged += PushMagnitudeChangedCallback;
    }

    private void OnDestroy()
    {
        MergeManager.onMergeProcessed -= MergeProcessedCallback;
        SettingsManager.onPushMagnitudeChanged -= PushMagnitudeChangedCallback;
    }

    private void MergeProcessedCallback(FruitType fruitType, Vector2 mergePos)
    {
        pushPosition = mergePos;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(mergePos, pushRadius);

        foreach(Collider2D collider in colliders)
        {
            if(collider.TryGetComponent(out Fruit fruit))
            {
                Vector2 force = ((Vector2)fruit.transform.position - mergePos).normalized;
                force *= pushMagnitude;

                fruit.GetComponent<Rigidbody2D>().AddForce(force);
            }
        }
    }
    
    private void PushMagnitudeChangedCallback(float newPushMagnitude)
    {
        pushMagnitude = Mathf.Lerp(minMaxPushMagnitude.x, minMaxPushMagnitude.y, newPushMagnitude);
    }

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        if (!enableGizmos)
            return;

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(pushPosition, pushRadius);
    }

#endif
}
