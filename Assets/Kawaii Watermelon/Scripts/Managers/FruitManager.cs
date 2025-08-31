using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using Random = UnityEngine.Random;

public class FruitManager : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private Fruit[] fruitPrefabs;
    [SerializeField] private Fruit[] spawnableFruits;
    [SerializeField] private Transform fruitsParent;
    [SerializeField] private LineRenderer fruitSpawnLine;
    private Fruit currentFruit;

    [Header(" Settings ")]
    [SerializeField] private float fruitsYSpawnPos;
    [SerializeField] private float spawnDelay;
    private bool canControl;
    private bool isControlling;

    // YENİ EKLENDİ BAŞLANGIÇ
    [Header(" Game Over by Fruit Limit ")]
    [Tooltip("Bu kadar meyve atıldıktan sonra oyun biter.")]
    [SerializeField] private int fruitDropLimit = 50; // Inspector'dan ayarlanabilir meyve limiti.
    [Tooltip("Limit dolduktan kaç saniye sonra oyunun biteceği.")]
    [SerializeField] private float gameOverDelay = 3f;  // Oyunun bitmesi için bekleme süresi.
    private int fruitsDroppedCount = 0;                 // Atılan meyveleri saymak için sayaç.
    private bool isLimitReached = false;                // Limite ulaşılıp ulaşılmadığını kontrol eder, tekrar tekrar tetiklenmesini önler.
    // YENİ EKLENDİ SON

    [Header(" Next Fruit Settings ")]
    private int nextFruitIndex;

    [Header(" Debug ")]
    [SerializeField] private bool enableGizmos;

    [Header(" Actions ")]
    public static Action onNextFruitIndexSet;

    private void Awake()
    {
        MergeManager.onMergeProcessed += MergeProcessedCallback;
    }

    private void OnDestroy()
    {
        MergeManager.onMergeProcessed -= MergeProcessedCallback;
    }

    void Start()
    {
        SetNextFruitIndex();
        canControl = true;
        HideLine();
    }

    void Update()
    {
        if (!GameManager.instance.IsGameState())
            return;

        if (canControl)
            ManagePlayerInput();
    }

    private void ManagePlayerInput()
    {
        if (Input.GetMouseButtonDown(0))
            MouseDownCallback();
        else if (Input.GetMouseButton(0))
        {
            if (isControlling)
                MouseDragCallback();
            else
                MouseDownCallback();
        }
        else if (Input.GetMouseButtonUp(0) && isControlling)
            MouseUpCallback();
    }

    private void MouseDownCallback()
    {
        DisplayLine();
        PlaceLineAtClickedPosition();
        SpawnFruit();
        isControlling = true;
    }

    private void MouseDragCallback()
    {
        PlaceLineAtClickedPosition();
        currentFruit.MoveTo(new Vector2(GetSpawnPosition().x, fruitsYSpawnPos));
    }

    private void MouseUpCallback()
    {
        HideLine();

        if (currentFruit != null)
        {
            currentFruit.EnablePhysics();

            // YENİ EKLENDİ BAŞLANGIÇ
            // Oyuncu meyveyi bıraktığında sayacı artır ve limiti kontrol et.
            fruitsDroppedCount++;
            CheckFruitLimit();
            // YENİ EKLENDİ SON
        }

        // DEĞİŞTİRİLDİ: Zamanlayıcı sadece limit dolmadıysa başlamalı.
        if (!isLimitReached)
        {
            canControl = false;
            StartControlTimer();
        }

        isControlling = false;
    }

    private void SpawnFruit()
    {
        Vector2 spawnPosition = GetSpawnPosition();
        Fruit fruitToInstantiate = spawnableFruits[nextFruitIndex];
        currentFruit = Instantiate(
            fruitToInstantiate,
            spawnPosition,
            Quaternion.identity,
            fruitsParent);
        SetNextFruitIndex();
    }

    // YENİ EKLENDİ BAŞLANGIÇ
    /// <summary>
    /// Atılan meyve sayısının limite ulaşıp ulaşmadığını kontrol eder.
    /// </summary>
    private void CheckFruitLimit()
    {
        // Eğer limite daha önce ulaşıldıysa veya oyun "Game" durumunda değilse, tekrar kontrol etme.
        if (isLimitReached || !GameManager.instance.IsGameState())
            return;

        if (fruitsDroppedCount >= fruitDropLimit)
        {
            Debug.Log("Meyve atma limiti doldu! Oyun " + gameOverDelay + " saniye içinde bitecek.");
            isLimitReached = true;
            canControl = false; // Oyuncunun yeni meyve atmasını engelle.

            // GameManager'a belirli bir süre sonra oyunu bitirmesi için komut gönder.
            GameManager.instance.EndGameAfterDelay(gameOverDelay);
        }
    }
    // YENİ EKLENDİ SON

    private void SetNextFruitIndex()
    {
        nextFruitIndex = Random.Range(0, spawnableFruits.Length);
        onNextFruitIndexSet?.Invoke();
    }

    public string GetNextFruitName()
    {
        return spawnableFruits[nextFruitIndex].name;
    }

    public Sprite GetNextFruitSprite()
    {
        return spawnableFruits[nextFruitIndex].GetSprite();
    }

    private Vector2 GetClickedWorldPosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private Vector2 GetSpawnPosition()
    {
        Vector2 worldClickedPosition = GetClickedWorldPosition();
        worldClickedPosition.y = fruitsYSpawnPos;
        return worldClickedPosition;
    }

    private void PlaceLineAtClickedPosition()
    {
        fruitSpawnLine.SetPosition(0, GetSpawnPosition());
        fruitSpawnLine.SetPosition(1, GetSpawnPosition() + Vector2.down * 15);
    }

    private void HideLine()
    {
        fruitSpawnLine.enabled = false;
    }

    private void DisplayLine()
    {
        fruitSpawnLine.enabled = true;
    }

    private void StartControlTimer()
    {
        Invoke("StopControlTimer", spawnDelay);
    }

    private void StopControlTimer()
    {
        canControl = true;
    }

    private void MergeProcessedCallback(FruitType fruitType, Vector2 spawnPosition)
    {
        for (int i = 0; i < fruitPrefabs.Length; i++)
        {
            if (fruitPrefabs[i].GetFruitType() == fruitType)
            {
                SpawnMergedFruit(fruitPrefabs[i], spawnPosition);
                break;
            }
        }
    }

    private void SpawnMergedFruit(Fruit fruit, Vector2 spawnPosition)
    {
        Fruit fruitInstance = Instantiate(fruit, spawnPosition, Quaternion.identity, fruitsParent);
        fruitInstance.EnablePhysics();
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (!enableGizmos)
            return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(new Vector3(-50, fruitsYSpawnPos, 0), new Vector3(50, fruitsYSpawnPos, 0));
    }
#endif
}