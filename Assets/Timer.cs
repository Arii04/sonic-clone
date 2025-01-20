using UnityEngine;
using TMPro;

public class Timer : PlayerAction
{
    [SerializeField] TextMeshProUGUI timerText;
    float elapsedTime;
    const float minSoftness = -0.5f; // Minimum softness value
    const float maxSoftness = 0.5f; // Maximum softness value
    float currentSoftness = minSoftness; // Current softness value
    const float softnessLerpSpeed = 0.5f; // Slower speed of the lerp transition

    void Update()
    {
        // Update the elapsed time
        elapsedTime += Time.deltaTime;
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        int milliseconds = Mathf.FloorToInt((elapsedTime * 1000) % 1000);
        timerText.text = string.Format("TIME:\n{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);

        // Get the player's speed
        float playerSpeed = playerPhysics.RB.linearVelocity.magnitude;

        // Ensure playerPhysics.speed is not zero to avoid division by zero
        float normalizedSpeed = playerPhysics.speed > 0 ? playerSpeed / playerPhysics.speed : 0;

        // Calculate the target softness value based on the player's speed
        float targetSoftness = Mathf.Lerp(minSoftness, maxSoftness, normalizedSpeed);

        // Smoothly interpolate the current softness value towards the target softness value
        currentSoftness = Mathf.Lerp(currentSoftness, targetSoftness, softnessLerpSpeed * Time.deltaTime);

        // Set the text softness with the new softness value
        timerText.fontMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, currentSoftness);
    }
}

