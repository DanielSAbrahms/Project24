using UnityEngine;

public class Game : MonoBehaviour
{
    public bool paused;

    // Start is called before the first frame update
    void Start()
    {
        paused = false;
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    { }
}
