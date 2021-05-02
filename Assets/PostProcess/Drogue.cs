using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(DrogueRenderer), PostProcessEvent.AfterStack, "Custom/Drogue")]
public sealed class Drogue : PostProcessEffectSettings
{
    [Header("Features")]
    public BoolParameter distortion = new BoolParameter { value = true };
    public BoolParameter lsd = new BoolParameter { value = true };
    public BoolParameter kale = new BoolParameter { value = true };

    [Header("Settings")]
    [Range(0f, 1f), Tooltip("Drogue effect intensity.")]
    public FloatParameter blend = new FloatParameter { value = 0.5f };
}

public sealed class DrogueRenderer : PostProcessEffectRenderer<Drogue>
{
    public override void Render(PostProcessRenderContext context)
    {
        var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/Drogue"));
        sheet.properties.SetFloat("_Blend", settings.blend);
        sheet.properties.SetFloat("_Disto", settings.distortion ? 1 : 0);
        sheet.properties.SetFloat("_LSD", settings.lsd ? 1 : 0);
        sheet.properties.SetFloat("_Kale", settings.kale ? 1 : 0);
        sheet.properties.SetFloat("_RealTime", Time.realtimeSinceStartup);
        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}
