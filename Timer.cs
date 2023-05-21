using UnityEngine;
#if NAUGHTY_ATTRIBUTES
using NaughtyAttributes;
#endif

public class Timer : MonoBehaviour
{
    [SerializeField]
    private float speed = 1;
    [SerializeField]
    private float duration;

#if NAUGHTY_ATTRIBUTES
    [ShowNonSerializedField]
#else
    [SerializeField]
#endif
    private float time;
    public float CurrentTime
    {
        get => time;
        set
        {
            time = value;
        }
    }

    private void Update()
    {
        
    }




}
