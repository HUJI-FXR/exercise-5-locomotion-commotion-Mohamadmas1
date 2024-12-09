using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportManager : MonoBehaviour
{
    // Start is called before the first frame update
    public TeleportationProvider teleportationProvider;
    public static int teleportCounter = 0;
    public int allowedTeleports = 3;
    private void OnEnable()
    {
        teleportationProvider.endLocomotion += OnEndLocomotion;
    }

    private void OnDisable()
    {
        teleportationProvider.endLocomotion -= OnEndLocomotion;
    }

    void OnEndLocomotion(LocomotionSystem locomotionSystem)
    {
        teleportCounter++;
        Debug.Log("Teleportation #" + teleportCounter + " has ended");

        if (teleportCounter >= allowedTeleports)
        {
            teleportationProvider.enabled = false;
        }
    }
}
