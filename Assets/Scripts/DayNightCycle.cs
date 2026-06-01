using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [SerializeField]
    private Light sun;
    [SerializeField]
    private float dayDuration = 300f;

    public void Update()
    {
        sun.transform.Rotate(Vector3.right, (360f / dayDuration) * Time.deltaTime);

        float height = sun.transform.forward.y;

        if (height < 0.2f && height > -0.2f)
        {
            sun.color = Color.Lerp(Color.red, Color.yellow, (height + 0.2f) / 0.4f);
            sun.intensity = 0.5f;
        }
        else if (height < 0)
        {
            sun.color = Color.black;
            sun.intensity = 0.05f;
        }
        else
        {
            sun.color = Color.yellow;
            sun.intensity = 2f;
        }
    }
}