using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Memory.UI
{

public class MemoryScoreUI : MonoBehaviour
{
    [SerializeField]
    private Framework.MemoryGameManager manager = null;

    [SerializeField]
    private Text currentScoreText               = null;

    [SerializeField]
    private Text targetScoreText                = null;

    void Start()
    {
        targetScoreText.text = manager.GetTargetScore().ToString("00");
    }

    void Update()
    {
        currentScoreText.text = manager.GetCurrentScore().ToString("00");
    }
}

}