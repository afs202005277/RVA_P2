using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingAreaWithCubeUI : MonoBehaviour
{
    [System.Serializable]
    public class Recipe
    {
        public string ingredientName; // Name or tag of the ingredient
        public int requiredAmount;    // Number of this ingredient required
    }

    public List<Recipe> recipes;           // List of required ingredients and their amounts
    public GameObject prefabToEnable;     // Prefab to enable after crafting

    private Dictionary<string, int> ingredientCounter = new();
    private bool isRecipeCompleted = false;
    private List<GameObject> ingredientsPlaced = new();

    public TextMeshProUGUI floatingText;

    private void Start()
    {
        UpdateRecipeUI();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isRecipeCompleted) return;

        string ingredientName = other.gameObject.tag;

        foreach (var recipe in recipes)
        {
            if (ingredientName == recipe.ingredientName)
            {
                if (!ingredientCounter.ContainsKey(ingredientName))
                {
                    ingredientCounter[ingredientName] = 0;
                }
                ingredientCounter[ingredientName]++;
                ingredientsPlaced.Add(other.gameObject);
                Rigidbody ingredientRigidbody = other.gameObject.GetComponent<Rigidbody>();
                if (ingredientRigidbody != null)
                {
                    ingredientRigidbody.velocity = Vector3.zero;
                    ingredientRigidbody.angularVelocity = Vector3.zero;
                    ingredientRigidbody.useGravity = false;
                    ingredientRigidbody.constraints = RigidbodyConstraints.FreezeAll;

                    Debug.Log($"{other.name} has been placed in the drop area and frozen.");
                }
                UpdateRecipeUI();

                if (CheckRecipeCompletion())
                {
                    DestroyIngredients();
                    CompleteRecipe();
                }
                return;
            }
        }
    }

    private void DestroyIngredients()
    {
        foreach (var ingredient in ingredientsPlaced)
        {
            Destroy(ingredient);
        }
    }

    private bool CheckRecipeCompletion()
    {
        foreach (var recipe in recipes)
        {
            if (!ingredientCounter.ContainsKey(recipe.ingredientName) || ingredientCounter[recipe.ingredientName] < recipe.requiredAmount)
            {
                return false;
            }
        }
        return true;
    }

    private void CompleteRecipe()
    {
        isRecipeCompleted = true;
        prefabToEnable.SetActive(true);
        Destroy(gameObject);
    }

    private void UpdateRecipeUI()
    {
        if (floatingText == null)
        {
            Debug.LogError("No Text component found on the recipe UI prefab!");
            return;
        }

        string recipeText = "Materials Needed:\n";
        foreach (var recipe in recipes)
        {
            int currentAmount = ingredientCounter.ContainsKey(recipe.ingredientName) ? ingredientCounter[recipe.ingredientName] : 0;
            recipeText += $"{recipe.ingredientName}: {currentAmount}/{recipe.requiredAmount}\n";
        }
        floatingText.text = recipeText;
    }
}
