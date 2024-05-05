using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Apply gradient to each line of text
/// </summary>
[RequireComponent(typeof(TMP_Text))]
public class TextGradientPerLine : MonoBehaviour
{
	[SerializeField] private bool isGradientStepsLikeInMaxLine;
	[SerializeField] private bool isCenteredGradient;

	private TMP_Text text;
	private Color[] colorSteps = Array.Empty<Color>();
	private VertexGradient[] gradients = Array.Empty<VertexGradient>();
	private TMP_Text Text => text ??= GetComponent<TMP_Text>();
	private UnityAction onLocalized;

	private void FixedUpdate()
	{
		ApplyColorTransitionsPerLines();
	}

#if UNITY_EDITOR
	private void OnValidate()
	{
		text = GetComponent<TMP_Text>();
		ApplyColorTransitionsPerLines();
	}
#endif

	/// <summary>
	/// Gradient method
	/// Get gradient by steps, and make vertex gradient array from colors
	/// Apply to each line by character
	/// </summary>
	public void ApplyColorTransitionsPerLines()
	{
		Text.ForceMeshUpdate();
		if (Text.textInfo == null)
			return;
		var textInfo = Text.textInfo;
		var lineCount = textInfo.lineCount;

		if (isGradientStepsLikeInMaxLine)
		{
			var maxLineLength = textInfo.lineInfo.Max(x => x.lastCharacterIndex - x.firstCharacterIndex) + 1;
			GetColorTransitions(ref colorSteps, Text.colorGradient.topLeft, Text.colorGradient.topRight, maxLineLength + 1);
			if (colorSteps.Length > gradients.Length)
				Array.Resize(ref gradients, colorSteps.Length);

			for (var i = 0; i < colorSteps.Length - 1; i++)
				gradients[i] = new VertexGradient(colorSteps[i], colorSteps[i + 1], colorSteps[i], colorSteps[i + 1]);
		}

		for (var lineIndex = 0; lineIndex < lineCount; lineIndex++)
		{
			var firstCharacterIndex = textInfo.lineInfo[lineIndex].firstCharacterIndex;
			var lastCharacterIndex = textInfo.lineInfo[lineIndex].lastCharacterIndex;

			if (isGradientStepsLikeInMaxLine)
				ProcessMaxLineGradient(firstCharacterIndex, lastCharacterIndex, textInfo);
			else
				ProcessPerLineGradient(lastCharacterIndex, firstCharacterIndex, textInfo);
		}
	}

	private void ProcessPerLineGradient(int lastCharacterIndex, int firstCharacterIndex, TMP_TextInfo textInfo)
	{
		var count = lastCharacterIndex - firstCharacterIndex + 1;
		GetColorTransitions(ref colorSteps, Text.colorGradient.topLeft, Text.colorGradient.topRight, count + 1);
		if (colorSteps.Length > gradients.Length)
			Array.Resize(ref gradients, colorSteps.Length);

		for (var i = 0; i < colorSteps.Length - 1; i++)
			gradients[i] = new VertexGradient(colorSteps[i], colorSteps[i + 1], colorSteps[i], colorSteps[i + 1]);

		for (var index = firstCharacterIndex; index <= lastCharacterIndex; index++)
		{
			var materialIndex = textInfo.characterInfo[index].materialReferenceIndex;
			var colors = textInfo.meshInfo[materialIndex].colors32;
			var vertexIndex = textInfo.characterInfo[index].vertexIndex;

			if (!textInfo.characterInfo[index].isVisible)
				continue;
			var gradientIndex = index - firstCharacterIndex;

			colors[vertexIndex + 0] = gradients[gradientIndex].bottomLeft;
			colors[vertexIndex + 1] = gradients[gradientIndex].topLeft;
			colors[vertexIndex + 2] = gradients[gradientIndex].bottomRight;
			colors[vertexIndex + 3] = gradients[gradientIndex].topRight;
			Text.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
		}
	}

	private void ProcessMaxLineGradient(int firstCharacterIndex, int lastCharacterIndex, TMP_TextInfo textInfo)
	{
		for (var index = firstCharacterIndex; index <= lastCharacterIndex; index++)
		{
			var materialIndex = textInfo.characterInfo[index].materialReferenceIndex;
			var colors = textInfo.meshInfo[materialIndex].colors32;
			var vertexIndex = textInfo.characterInfo[index].vertexIndex;

			if (!textInfo.characterInfo[index].isVisible)
				continue;
			var gradientIndex = index - firstCharacterIndex;

			colors[vertexIndex + 0] = gradients[gradientIndex].bottomLeft;
			colors[vertexIndex + 1] = gradients[gradientIndex].topLeft;
			colors[vertexIndex + 2] = gradients[gradientIndex].bottomRight;
			colors[vertexIndex + 3] = gradients[gradientIndex].topRight;
			Text.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
		}
	}

	/// <summary>
	/// Split gradient by steps from color to color
	/// </summary>
	/// <param name="start">Start color</param>
	/// <param name="end">End color</param>
	/// <param name="transitionSteps">Steps count</param>
	/// <returns>Array of colors</returns>
	private void GetColorTransitions(ref Color[] colorArray, Color start, Color end, int transitionSteps)
	{
		if (transitionSteps > colorArray.Length)
		{
			var steps = transitionSteps;
			if (transitionSteps % 4 != 0)
				steps = transitionSteps + transitionSteps % 4;
			Array.Resize(ref colorArray, steps);
		}

		var centerIndex = Mathf.Floor(transitionSteps / 2f);

		float r, g, b, a;
		var stepsCount = isCenteredGradient 
			? centerIndex 
			: transitionSteps;
		
		r = (end.r - start.r) / (stepsCount - 1);
		g = (end.g - start.g) / (stepsCount - 1);
		b = (end.b - start.b) / (stepsCount - 1);
		a = (end.a - start.a) / (stepsCount - 1);
		
		if (isCenteredGradient)
			for (var i = 0; i < centerIndex + 1; i++)
			{
				var color = new Color(start.r + r * i, start.g + g * i, start.b + b * i, start.a + a * i);
				colorArray[i] = color;
				colorArray[transitionSteps - 1 - i] = color;
			}
		else
			for (var i = 0; i < transitionSteps; i++)
				colorArray[i] = new Color(start.r + r * i, start.g + g * i, start.b + b * i, start.a + a * i);
	}
}