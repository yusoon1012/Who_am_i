using UnityEngine;

public class DropItem : MonoBehaviour
{
    private float timeElapsed = default;

    private float TRUN_FORCE = 10.0f;
    private float DESTRUCTION_TIME = 60.0f;

    private void Start()
    {
        timeElapsed = 0.0f;
    }

    private void Update()
    {
        timeElapsed += Time.deltaTime;

        transform.eulerAngles = new Vector3(0.0f, TRUN_FORCE * timeElapsed, 0.0f);

        if (DESTRUCTION_TIME < timeElapsed)
        {
            Destroy(gameObject);
        }
    }
}
