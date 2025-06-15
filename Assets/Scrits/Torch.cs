using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{
    [Header("References")]
    public GameObject panel;           // UI panel whose alpha we change
    public AudioSource warningAudio;   // 1–2 s beep clip
    public TMPro.TextMeshProUGUI statusText;

    [Header("Settings")]
    [Range(0f, 1f)] public float transparentAlpha = 0.3f;
    public float maxHoldDuration = 10f;  // seconds of “budget”
    public float cooldownDuration = 20f;  // seconds to refill
    public float warningThreshold = 4f;   // play beep at ≤ this time

    // ─────────────────────────────────────────────────────────────
    private UnityEngine.UI.Image panelImage;
    private float originalAlpha;

    private bool isActive = false;
    private float remainingHoldTime = 0f;
    private float cooldownTimer = 0f;
    private bool warningPlayed = false;

    void Start()
    {
        panelImage = panel.GetComponent<UnityEngine.UI.Image>();
        if (panelImage == null)
        {
            Debug.LogWarning("No Image component found on panel!");
            enabled = false; return;
        }

        originalAlpha = panelImage.color.a;
        remainingHoldTime = maxHoldDuration;

        if (statusText != null) statusText.enabled = false;  // hide on start
    }

    void Update()
    {
        /* ---------- 1. Cooldown timer ---------- */
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                remainingHoldTime = maxHoldDuration;
                warningPlayed = false;
                StartCoroutine(ShowStatusForSeconds("Reloaded", 2f)); // show message
            }
        }

        /* ---------- 2. Try to activate ---------- */
        if (!isActive &&
            cooldownTimer <= 0f &&
            remainingHoldTime > 0f &&
            Input.GetKeyDown(KeyCode.Alpha1))
        {
            ActivateTransparency();
        }

        /* ---------- 3. While active ---------- */
        if (isActive)
        {
            remainingHoldTime -= Time.deltaTime;

            // Play warning at threshold
            if (remainingHoldTime <= warningThreshold && !warningPlayed)
            {
                StartCoroutine(PlayWarningWithMessage());
                warningPlayed = true;
            }

            // Re‑darken early if key released
            if (!Input.GetKey(KeyCode.Alpha1))
            {
                DeactivateTransparency(false);
            }
            // Or when budget spent
            else if (remainingHoldTime <= 0f)
            {
                remainingHoldTime = 0f;
                DeactivateTransparency(true);
            }
        }
    }

    /* ---------------- helper methods ---------------- */

    private void ActivateTransparency()
    {
        SetPanelAlpha(transparentAlpha);
        isActive = true;
    }

    private void DeactivateTransparency(bool startCooldown)
    {
        SetPanelAlpha(originalAlpha);
        isActive = false;

        if (startCooldown)
            cooldownTimer = cooldownDuration;
    }

    private void SetPanelAlpha(float a)
    {
        var c = panelImage.color;
        c.a = a;
        panelImage.color = c;
    }

    /* ---------- coroutines ---------- */

    // Plays the beep and shows “Battery Low” while it’s audible
    private IEnumerator PlayWarningWithMessage()
    {
        if (statusText != null)
        {
            statusText.text = "Battery Low";
            statusText.enabled = true;
        }

        if (warningAudio != null)
            warningAudio.Play();

        yield return new WaitForSeconds(2f); // keep for 2 s

        if (warningAudio != null)
            warningAudio.Stop();

        if (statusText != null)
            statusText.enabled = false;
    }

    // Reusable helper to display any short‑lived message
    private IEnumerator ShowStatusForSeconds(string msg, float seconds)
    {
        if (statusText == null) yield break;

        statusText.text = msg;
        statusText.enabled = true;
        yield return new WaitForSeconds(seconds);
        statusText.enabled = false;
    }
}