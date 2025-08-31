using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameoverManager : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private GameObject deadLine;
    [SerializeField] private Transform fruitsParent;

    [Header(" Timer ")]
    [SerializeField] private float durationThreshold;
    private float timer;
    private bool timerOn;
    private bool isGameover;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!isGameover)
            ManageGameover();
    }

    private void ManageGameover()
    {
        if (timerOn)
            ManageTimerOn();
        else
        {
            if (IsFruitAboveLine())
                StartTimer();
        }
    }

    private void ManageTimerOn()
    {
        timer += Time.deltaTime;

        if (!IsFruitAboveLine())
            StopTimer();

        if (timer >= durationThreshold)
            Gameover();
    }

    private bool IsFruitAboveLine()
    {
        for (int i = 0; i < fruitsParent.childCount; i++)
        {
            Fruit fruit = fruitsParent.GetChild(i).GetComponent<Fruit>();

            if (!fruit.HasCollided())
                continue;

            if (IsFruitAboveLine(fruitsParent.GetChild(i)))
                return true;
        }

        return false;
    }

    private bool IsFruitAboveLine(Transform fruit)
    {
        if (fruit.position.y > deadLine.transform.position.y)
            return true;

        return false;
    }

    private void StartTimer()
    {
        timer = 0;
        timerOn = true;
    }

    private void StopTimer()
    {
        timerOn = false;
    }

    private void Gameover()
    {
        Debug.LogError("Gameover");
        isGameover = true;

        GameManager.instance.SetGameoverState();
    }
}
