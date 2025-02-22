using System.Collections;
using models;
using UnityEngine;

public enum IngredientCookingState
{
    UNCOOKED, COOKING, COMPLETE, BURNED, DISPOSED
}

public class IngredientController : MonoBehaviour
{
    // Components
    private SpriteRenderer spriteRenderer;
    private ProgressBar progressBarScript;
    
    // Manage state variables
    public IngredientCookingState currentIngredientState = IngredientCookingState.UNCOOKED;
    private Ingredient ingredient;
    
    // Manage Children of this empty game object
    private GameObject ingredientGameObject;
    private GameObject progressBarUiCanvas;

    private bool isIngredientBurned;
    
    // Start is called before the first frame update
    void Start()
    {
        // get children game objects nested under this empty game object 
        ingredientGameObject = this.transform.GetChild(0).gameObject;
        progressBarUiCanvas = this.transform.GetChild(1).gameObject;
        
        // will be used to update color of sprite if it is burned
        spriteRenderer = ingredientGameObject.GetComponent<SpriteRenderer>();
        
        // create an instance of an Ingredient data object
        ingredient = IngredientMap.dict[ingredientGameObject.name];
        // set the timer in the progress bar
        progressBarScript = progressBarUiCanvas.transform.GetChild(0).gameObject.GetComponent<ProgressBar>();
        progressBarScript.SetTimer(ingredient.timeToCook);
        Debug.Log(ingredient.name);
        Debug.Log(ingredient.cookType);
    }

    // Update is called once per frame
    void Update()
    {
        if (progressBarUiCanvas.activeSelf)
        {
            if (isIngredientBurned)
            {
                Debug.Log("Ingredient burned");
                currentIngredientState = IngredientCookingState.BURNED;
                ingredientGameObject.GetComponent<SpriteRenderer>().color = new Color(0.41f, 0.15f, 0.15f);
                DisableProgressBar();
            }
            else if (currentIngredientState != IngredientCookingState.COMPLETE && progressBarScript.IsComplete())
            {
                Debug.Log("Timer complete");
                currentIngredientState = IngredientCookingState.COMPLETE;
                // start timer that will give some wiggle room before it burns
                StartCoroutine(StartIngredientCompleteTimer());
            } 
            else if (currentIngredientState != IngredientCookingState.COMPLETE)
            {
                currentIngredientState = IngredientCookingState.COOKING;
            }
        }
    }

    public void EnableProgressBar()
    {
        Debug.Log("Enable progress bar");
        progressBarUiCanvas.SetActive(true);
    }
    
    public void DisableProgressBar()
    {
        Debug.Log("Disable progress bar");
        progressBarUiCanvas.SetActive(false);
    }

    private IEnumerator StartIngredientCompleteTimer()
    {
        //yield return new WaitForSeconds(2.0f); 
        yield return new WaitForSeconds(5.0f);
        isIngredientBurned = true;
    }

    public void DestroyIngredientAndProgressBar()
    {
        Destroy(this.gameObject);
    }

    public void SetIngredientSprite(Sprite ingredientSprite)
    {
        spriteRenderer.sprite = ingredientSprite;
    }

    public bool CanCookIngredient(PlayerPowerState playerState)
    {
        return (playerState == PlayerPowerState.FIRE_ACTIVE && ingredient.cookType == CookType.FIRE)
               || (playerState == PlayerPowerState.WATER_ACTIVE && ingredient.cookType == CookType.WATER)
               || (playerState == PlayerPowerState.AIR_ACTIVE && ingredient.cookType == CookType.AIR);
    }
}
