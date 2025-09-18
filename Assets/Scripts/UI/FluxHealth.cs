using UnityEngine;
using FluxFramework.UI;
using FluxFramework.Attributes;
using FluxFramework.Binding;
using UnityEngine.UI;
using FluxFramework.Core;
using TMPro;

namespace MyGame.UI
{
    public class FluxHealth : FluxUIComponent
    {

        [FluxBinding("Player.health")]
        [SerializeField] private Slider healthBarSlider;

        [FluxBinding("Player.health", ConverterType = typeof(HealthToTextConverter))]
        [SerializeField] private TextMeshProUGUI healthText;
        
        
    }

    public class HealthToTextConverter : IValueConverter<float, string>
    {
        public string Convert(float value) => (string)"PV : "+$"{value:F0}";
        public float ConvertBack(string value) => float.TryParse(value, out var f) ? f : 0;
        
        object IValueConverter.Convert(object value) => Convert((float)value);
        object IValueConverter.ConvertBack(object value) => ConvertBack((string)value);
    }

}