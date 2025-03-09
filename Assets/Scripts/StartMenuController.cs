using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public GameObject transCircle;
    float circleTime = 1;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(StartGame());
        }

        if (GetComponent<Animator>().GetBool("StartGame"))
        {
            transform.position += Vector3.right / 80;
            transCircle.transform.position = transform.position;
            transCircle.transform.localScale = new Vector3(Mathf.Lerp(0, 70, circleTime), Mathf.Lerp(0, 70, circleTime), 0);
            circleTime -= Time.deltaTime / 3f;
        }
    }

    IEnumerator StartGame()
    {
        GetComponent<Animator>().SetBool("StartGame", true);
        yield return new WaitForSeconds(3f);

        SceneManager.LoadScene("SampleScene");
    }
}
