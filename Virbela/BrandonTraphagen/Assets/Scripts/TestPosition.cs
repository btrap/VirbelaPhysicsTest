using UnityEngine;
using UnityEngine.UI;
using SystemAssert = System.Diagnostics.Debug;

    /// <summary>
    /// Testing setup to attach unity UI hooks and Input hooks
    /// </summary>
    public class TestPosition : MonoBehaviour
{
    public Level LevelToTest = null;
    public Interactable ItemPrefab = null;
    public Interactable BotPrefab = null;

    public Text TestedCount;

    /// <summary>
    /// Assert that objects are setup.
    /// </summary>
    private void Start()
    {
        SystemAssert.Assert(LevelToTest != null);
        SystemAssert.Assert(ItemPrefab != null);
        SystemAssert.Assert(BotPrefab != null);
        SystemAssert.Assert(TestedCount != null);
    }

    /// <summary>
    /// Handle input for adding Interactable objects to test.
    /// </summary>
    private void Update()
    {
        TestedCount.text = "Tested - " + LevelToTest.TestedCount;

        if (Input.GetKey(KeyCode.Alpha1))
        {
            RandomlyAddItem();
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            RandomlyAddBot();
        }
    }

    /// <summary>
    /// Toggles the if Debug rays will be on.
    /// </summary>
    public void ToggleRays()
    {
        LevelToTest.ViewRays = !LevelToTest.ViewRays;
    }

    /// <summary>
    /// Moves all Interactable items in the LevelToTest randomly.
    /// </summary>
    public void RandomlyMoveInteractablesInScene()
    {
        LevelToTest.RandomlyMoveInteractablesInScene();
    }

    /// <summary>
    /// Adds an Interactable randomly into the LevelToTest.
    /// </summary>
    public void RandomlyAddInteractable(Interactable newInteractable)
    {
        newInteractable.MoveRandomlyAroundTransform(LevelToTest.Player.transform, LevelToTest.InteractablesMin, LevelToTest.InteractablesMax);
        LevelToTest.Interactables.Add(newInteractable);
    }

    /// <summary>
    /// Adds a Bot randomly into the LevelToTest.
    /// </summary>
    public void RandomlyAddBot()
    {
        RandomlyAddInteractable(Instantiate(BotPrefab, LevelToTest.InteractableParent.transform));
    }

    /// <summary>
    /// Adds an Item randomly into the LevelToTest.
    /// </summary>
    public void RandomlyAddItem()
    {
        RandomlyAddInteractable(Instantiate(ItemPrefab, LevelToTest.InteractableParent.transform));
    }
}
