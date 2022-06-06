using UnityEngine;

public class Menu : MonoBehaviour
{
    public bool open;
    public string menuName;

    public void Open()
    {
        open = true;
        gameObject.SetActive(true);
    }

    public void Close()
    {
        open = false;
        gameObject.SetActive(false);
    }

    public void PopUpOpen()
    {
        gameObject.SetActive(true);
    }

    public void PopUpClose()
    {
        gameObject.SetActive(false);
    }
}
