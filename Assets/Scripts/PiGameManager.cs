using UnityEngine;
using TMPro;

public class PiGameManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI nextDigitText;

    [Header("Game Settings")]
    public int maxHealth = 3;
    
    // PI string (you can extend this as needed)
    private const string PI_DIGITS = "31415926535897932384626433832795028841971693993751058209749445923078164062862089986280348253421170679";
    
    private int currentScore = 0;
    private int currentHealth;
    private int currentDigitIndex = 0;
    
    private void Start()
    {
        // Initialize health and score
        currentHealth = maxHealth;
        UpdateUI();
    }
    
    public bool CheckDigit(char digit)
    {
        // Check if the collected digit matches the next digit in PI
        if (currentDigitIndex < PI_DIGITS.Length && digit == PI_DIGITS[currentDigitIndex])
        {
            currentScore++;
            currentDigitIndex++;
            UpdateUI();
            return true;
        }
        else
        {
            currentHealth--;
            UpdateUI();
            
            // Check if game over
            if (currentHealth <= 0)
            {
                GameOver();
            }
            
            return false;
        }
    }
    
    private void UpdateUI()
    {
        // Update all UI elements
        scoreText.text = "Score: " + currentScore;
        healthText.text = "Health: " + currentHealth;
        
        // Show next digit to collect
        if (currentDigitIndex < PI_DIGITS.Length)
        {
            nextDigitText.text = "Next Digit: " + PI_DIGITS[currentDigitIndex];
        }
        else
        {
            nextDigitText.text = "You completed PI!";
        }
    }
    
    private void GameOver()
    {
        Debug.Log("Game Over! Final Score: " + currentScore);
        // Add any game over logic here (restart, show game over screen, etc.)
        
        // Optional: Pause the game
        Time.timeScale = 0;
    }
    
    // Method to restart the game if needed
    public void RestartGame()
    {
        currentScore = 0;
        currentHealth = maxHealth;
        currentDigitIndex = 0;
        Time.timeScale = 1;
        UpdateUI();
    }
}