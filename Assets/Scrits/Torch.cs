using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{
    [Header("References")]
    public GameObject panel;
    public AudioSource warningAudio;
    public AudioSource reloadedAudio;
    public TMPro.TextMeshProUGUI statusText;
    public UnityEngine.UI.Image warningImage;
    public UnityEngine.UI.Image reloadedImage;
    [Header("Settings")]
    [Range(0f, 1f)] public float transparentAlpha = 0.3f;
    public float maxHoldDuration = 10f;
    public float cooldownDuration = 20f;
    public float warningThreshold = 4f;

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
            enabled = false;
            return;
        }

        originalAlpha = panelImage.color.a;
        remainingHoldTime = maxHoldDuration;

        if (statusText != null) statusText.enabled = false;
        if (warningImage != null) warningImage.enabled = false;
        if (reloadedImage != null) reloadedImage.enabled = false;
    }

    void Update()
    {

        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                remainingHoldTime = maxHoldDuration;
                warningPlayed = false;
                StartCoroutine(ShowStatusWithImage("Reloaded", 2f, reloadedImage));
            }
        }


        if (!isActive &&
            cooldownTimer <= 0f &&
            remainingHoldTime > 0f &&
            Input.GetKeyDown(KeyCode.Alpha1))
        {
            ActivateTransparency();
        }

        if (isActive)
        {
            remainingHoldTime -= Time.deltaTime;

            if (remainingHoldTime <= warningThreshold && !warningPlayed)
            {
                StartCoroutine(PlayWarningWithMessage());
                warningPlayed = true;
            }

            if (!Input.GetKey(KeyCode.Alpha1))
            {
                DeactivateTransparency(false);
            }
            else if (remainingHoldTime <= 0f)
            {
                remainingHoldTime = 0f;
                DeactivateTransparency(true);
            }
        }
    }

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

    private void SetPanelAlpha(float alpha)
    {
        var c = panelImage.color;
        c.a = alpha;
        panelImage.color = c;
    }

    private IEnumerator PlayWarningWithMessage()
    {
        if (statusText != null)
        {
            statusText.text = "Battery Low";
            statusText.enabled = true;
        }

        if (warningImage != null)
            warningImage.enabled = true;

        if (warningAudio != null)
            warningAudio.Play();

        yield return new WaitForSeconds(2f);

        if (warningAudio != null)
            warningAudio.Stop();

        if (statusText != null)
            statusText.enabled = false;

        if (warningImage != null)
            warningImage.enabled = false;
    }

    private IEnumerator ShowStatusWithImage(string message, float seconds, UnityEngine.UI.Image imageToShow)
    {
        if (statusText != null)
        {
            statusText.text = message;
            statusText.enabled = true;
        }

        if (imageToShow != null)
            imageToShow.enabled = true;

        if (message == "Reloaded" && reloadedAudio != null)
            reloadedAudio.Play();

        yield return new WaitForSeconds(seconds);

        if (statusText != null)
            statusText.enabled = false;

        if (imageToShow != null)
            imageToShow.enabled = false;
    }
}