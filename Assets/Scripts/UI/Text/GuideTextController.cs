using UnityEngine;
using UnityEngine.UI;

public class GuideTextController : MonoBehaviour
{
    float moveTime = 0;
    float alphaNum = 0;
    Color myColor;
    private void Start()
    {
        myColor = GetComponent<Text>().color; 
    }
    private void Update()
    {
        moveTime += Time.deltaTime;
        alphaNum = Mathf.Sin(moveTime);
        if (alphaNum < 0)
        {
            alphaNum = -alphaNum;
        }
        myColor.a = alphaNum;
        GetComponent<Text>().color = myColor;
    }
}