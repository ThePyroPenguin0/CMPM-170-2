using UnityEngine;
using UnityEngine.UI;

public class ResourceBar : MonoBehaviour // Credit to https://www.youtube.com/watch?v=BLfNP4Sc_iA
{
    public Slider ResourceSlider;
    public Gradient ResourceGradient;
    public Image ResourceFill;
    public void setResource(int Resourceremaining)
    {
        ResourceSlider.value = Resourceremaining;
        ResourceFill.color = ResourceGradient.Evaluate(ResourceSlider.normalizedValue);
    }
    public void setMaxResource(int maxResource)
    {
        ResourceSlider.maxValue = maxResource;
        ResourceSlider.value = maxResource;
        ResourceFill.color = ResourceGradient.Evaluate(1f);
    }
}
