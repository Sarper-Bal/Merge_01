using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(FruitManager))]
public class FruitManagerUI : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private Image nextFruitImage;
    private FruitManager fruitManager;

    private void Awake()
    {
        FruitManager.onNextFruitIndexSet += UpdateNextFruitImage;
    }

    private void OnDestroy()
    {
        FruitManager.onNextFruitIndexSet -= UpdateNextFruitImage;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    private void UpdateNextFruitImage()
    {
        if(fruitManager == null)
            fruitManager = GetComponent<FruitManager>();

        nextFruitImage.sprite = fruitManager.GetNextFruitSprite();
    }
}
