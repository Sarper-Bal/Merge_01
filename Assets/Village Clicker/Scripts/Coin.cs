using UnityEngine;
using DG.Tweening;

// Bu script, Coin prefab'ına eklenecek.
// Oluşturulduğunda kendini bir animasyonla gösterir.
public class Coin : MonoBehaviour
{
    [SerializeField] private float appearDuration = 0.5f;
    [SerializeField] private Ease appearEase = Ease.OutBack; // Animasyonun yumuşaklık tipi

    void Start()
    {
        // Coin'in başlangıç ölçeğini sıfır yap.
        transform.localScale = Vector3.zero;

        // DOTween ile ölçeği yavaşça 1'e getirerek "fırlama/belirme" efekti yap.
        transform.DOScale(Vector3.one, appearDuration).SetEase(appearEase);
    }
}