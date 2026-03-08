using UnityEngine;
using UnityEngine.SceneManagement;

public class InterfaceButton : MonoBehaviour
{
    
    [SerializeField] private GameObject panelMenu;
    [SerializeField] private GameObject panelHTP;

    private void Start()
    {
        panelMenu.SetActive(true);
        panelHTP.SetActive(false);
    }
    
    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void HTP()
    {
        panelMenu.SetActive(false);
        panelHTP.SetActive(true);
    }

    public void Back()
    {
        panelMenu.SetActive(true);
        panelHTP.SetActive(false);
    }
    
    public void Exit()
    {
        Application.Quit();
    }
}
