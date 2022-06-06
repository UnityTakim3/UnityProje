using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    [SerializeField] public Menu[] menus;

    private void Awake()
    {
        Instance = this;
    }

    public void OpenMenu(string menuName)
    {
        for (int i = 0; i < menus.Length; i++)
            if (menus[i].menuName == menuName)
                menus[i].Open();
            else if (menus[i].open)
                CloseMenu(menus[i]);
    }

    public void OpenMenu(Menu menu)
    {
        for (int i = 0; i < menus.Length; i++)
            if (menus[i].open)
                CloseMenu(menus[i]);
        menu.Open();
    }

    public void CloseMenu(Menu menu)
    {
        menu.Close();
    }

    public void OpenPopUpMenu(string menuName)
    {
        for (int j = 0; j < menus.Length; j++)
            if (menus[j].menuName == menuName)
                OpenPopUpMenu(menus[j]);
    }

    public void OpenPopUpMenu(Menu menu)
    {
        menu.PopUpOpen();
    }

}
