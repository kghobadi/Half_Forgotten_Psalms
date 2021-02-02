using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(PixelateRenderer), PostProcessEvent.AfterStack, "Custom/Pixelate")]
public sealed class PixelateEffect : PostProcessEffectSettings
{
    [Range(0f, 200f), Tooltip("Pixelate Effect Fidelity")]
    public FloatParameter blend = new FloatParameter { value = 50f };
}

public sealed class PixelateRenderer : PostProcessEffectRenderer<PixelateEffect>
{
    public override void Render(PostProcessRenderContext context)
    {
        var sheet = context.propertySheets.Get(Shader.Find("Hidden/Winning"));
        sheet.properties.SetFloat("_Blend", settings.blend);
        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}
