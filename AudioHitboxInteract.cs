using Unity.VisualScripting;
using UnityEngine;

public class AudioHitboxInteract : MonoBehaviour
{
    public GameObject PlayerModel;
    public UISystemMain UISystemMain;
    public bool GoodOrBad;
    public bool ShowDialougeAfterPickup;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GameObject() == PlayerModel.GameObject() )
        {
            print("player touched!");
            UISystemMain.UpdateNightGuessCount(GoodOrBad);
        }
        if (ShowDialougeAfterPickup)
        {
            UISystemMain.ShowDialougeAtNight();
        }
    }
    // ---- //
}
