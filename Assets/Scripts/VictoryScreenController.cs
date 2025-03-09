using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryScreenController : MonoBehaviour
{

    public TextMeshProUGUI deathCount;

    // Start is called before the first frame update
    void Start()
    {
        deathCount.text = "You died " + GameController.DeathCount.ToString() + " times!";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameController.DeathCount = 0;
            GameController.CurrentLevel = 0;
            SceneManager.LoadScene("Title");
        }
    }
}
