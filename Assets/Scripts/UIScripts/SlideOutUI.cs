// Help from https://medium.com/developers-arena/creating-a-sliding-mobile-menu-in-unity-56940e44556e
using UnityEngine;
using UnityEngine.UI;
public class SlideOutUI : MonoBehaviour
{
    public Animator anim;
    public bool help;

    [Tooltip("Every button in the slide out menu")]
    public Button[] buttons;
    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateSlideOutUI(bool button)
    {
        help = button;
        anim.SetBool("button pressed", button);
        if (!button)
        {
            //Invoke("HideTheScreen", 1f);
        }
    }
    /*
    void HideTheScreen()
    {
        gameObject.SetActive(false);
    }
    */
    public void MakeButtonsWork()
    {
        foreach(Button button in buttons)
        {
            button.interactable = true;
        }
    }
}
