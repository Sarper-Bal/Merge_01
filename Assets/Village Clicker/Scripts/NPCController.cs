using UnityEngine;
using DG.Tweening;

// NPC'nin davranışlarını yöneten script.
public class NPCController : MonoBehaviour
{
    private enum NPCState { Idle, MovingToCoin, MovingToWell }
    private NPCState currentState = NPCState.Idle;

    [SerializeField] private Transform wellTransform; // Kuyu objesinin transform'u
    [SerializeField] private float moveSpeed = 2f;

    private Transform targetCoin = null; // Hedefteki coin
    private Transform carriedCoin = null; // Taşıdığı coin

    void Update()
    {
        // Sadece Idle (boşta) durumundayken yeni bir coin ara.
        if (currentState == NPCState.Idle)
        {
            FindClosestCoin();
        }
    }

    private void FindClosestCoin()
    {
        // Performans için bu method her frame çağrılmamalı, ama basitlik için şimdilik böyle.
        // Daha optimize bir yol, coin spawn olduğunda event ile NPC'yi haberdar etmektir.
        Coin[] allCoins = FindObjectsOfType<Coin>();

        float closestDistance = Mathf.Infinity;
        Transform closestCoin = null;

        foreach (Coin coin in allCoins)
        {
            float distance = Vector3.Distance(transform.position, coin.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestCoin = coin.transform;
            }
        }

        if (closestCoin != null)
        {
            targetCoin = closestCoin;
            currentState = NPCState.MovingToCoin;
            MoveToTarget(targetCoin.position);
        }
    }

    // Hedefe doğru DOTween ile yumuşak hareket
    private void MoveToTarget(Vector3 targetPosition)
    {
        transform.DOMove(targetPosition, Vector3.Distance(transform.position, targetPosition) / moveSpeed)
            .SetEase(Ease.Linear)
            .OnComplete(() => OnTargetReached()); // Hareket bitince çağrılacak method
    }

    // Hedefe varıldığında ne olacağını yöneten method
    private void OnTargetReached()
    {
        if (currentState == NPCState.MovingToCoin)
        {
            // Coin'e ulaşıldı, şimdi coin'i "al" ve kuyuya doğru yola çık.
            PickUpCoin();
        }
        else if (currentState == NPCState.MovingToWell)
        {
            // Kuyuya ulaşıldı, coin'i "bırak".
            DropCoin();
        }
    }

    private void PickUpCoin()
    {
        if (targetCoin == null) return;

        carriedCoin = targetCoin;
        // Coin'i NPC'nin çocuğu yaparak onunla birlikte hareket etmesini sağla.
        carriedCoin.SetParent(this.transform);
        carriedCoin.DOLocalMove(Vector3.up * 0.5f, 0.2f); // Kafasının üstüne gibi bir pozisyona taşı

        targetCoin = null;
        currentState = NPCState.MovingToWell;
        MoveToTarget(wellTransform.position);
    }

    private void DropCoin()
    {
        if (carriedCoin == null) return;

        // Coin'i yok et ve GameManager'e coin kazanıldığını bildir.
        Destroy(carriedCoin.gameObject);
        GameManagerVillage.Instance.AddCoin(1);

        carriedCoin = null;
        currentState = NPCState.Idle; // Tekrar boşta durumuna geç.
    }
}