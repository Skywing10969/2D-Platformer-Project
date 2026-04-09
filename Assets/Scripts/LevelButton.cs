using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    public int level;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Button btn = GetComponent<Button>();

        if(PlayerPrefs.GetInt("LevelReached") < level)
        {
            btn.interactable = false;
        }
    }
}
