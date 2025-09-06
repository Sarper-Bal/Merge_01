using UnityEngine;
using DG.Tweening; // DOTween kütüphanesini kullanabilmek için

// Bu script, kurbağa objesine eklenecek.
// Tıklanma olayını dinler, coin fırlatır ve tıklama animasyonunu oynatır.
public class FrogController : MonoBehaviour
{
    [Tooltip("Fırlatılacak Coin Prefab'ı")]
    [SerializeField] private GameObject coinPrefab;

    [Tooltip("Coin'in fırlayacağı maksimum yarıçap")]
    [SerializeField] private float spawnRadius = 3f;

    [Tooltip("Animasyon süresi")]
    [SerializeField] private float punchDuration = 0.3f;

    [Tooltip("Animasyonun gücü")]
    [SerializeField] private float punchStrength = 0.2f;

    private void OnMouseDown()
    {
        // Tıklama animasyonu: Kurbağayı anlık olarak büyütüp küçülterek "tıklanma hissi" verir.
        transform.DOPunchScale(Vector3.one * punchStrength, punchDuration, 10, 1);

        // Coin fırlatma işlemini çağır.
        SpawnCoin();
    }

    private void SpawnCoin()
    {
        if (coinPrefab == null)
        {
            Debug.LogError("Coin Prefab'ı atanmamış!");
            return;
        }

        // Rastgele bir yön ve mesafe belirle (bir daire içinde).
        Vector2 randomCirclePoint = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPosition = transform.position + new Vector3(randomCirclePoint.x, randomCirclePoint.y, 0);

        // Coin'i oluştur.
        Instantiate(coinPrefab, spawnPosition, Quaternion.identity);
    }
}