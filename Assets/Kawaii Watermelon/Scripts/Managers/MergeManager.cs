using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MergeManager : MonoBehaviour
{
    [Header(" Actions ")]
    public static Action<FruitType, Vector2> onMergeProcessed;

    [Header(" Settings ")]
    Fruit lastSender;

    // Start is called before the first frame update
    void Awake()
    {
        Fruit.onCollisionWithFruit += CollisionBetweenFruitsCallback;
    }

    private void OnDestroy()
    {
        Fruit.onCollisionWithFruit -= CollisionBetweenFruitsCallback;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CollisionBetweenFruitsCallback(Fruit sender, Fruit otherFruit)
    {
        if (lastSender != null)
            return;

        lastSender = sender;

        ProcessMerge(sender, otherFruit);

        Debug.Log("Collision detected by " + sender.name);
    }

    private void ProcessMerge(Fruit sender, Fruit otherFruit)
    {
        FruitType mergeFruitType = sender.GetFruitType();
        mergeFruitType += 1;

        Vector2 fruitSpawnPos = (sender.transform.position + otherFruit.transform.position) / 2;

        sender.Merge();
        otherFruit.Merge();

        StartCoroutine(ResetLastSenderCoroutine());

        onMergeProcessed?.Invoke(mergeFruitType, fruitSpawnPos);
    }

    IEnumerator ResetLastSenderCoroutine()
    {
        yield return new WaitForEndOfFrame();
        lastSender = null;
    }
}
