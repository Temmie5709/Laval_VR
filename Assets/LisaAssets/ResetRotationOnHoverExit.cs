using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ResetRotationOnHoverExit : MonoBehaviour
{
    public Vector3 resetRotation = new Vector3(0f, 0f, 0f);

    public void ResetRotation()
    {
        transform.rotation = Quaternion.Euler(resetRotation);
    }
}
