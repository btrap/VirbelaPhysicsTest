using System.Collections.Generic;
using UnityEngine;
using SystemAssert = System.Diagnostics.Debug;

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

//[System.Serializable]
//public class LevelStateData
//{
//    public Vector3 playerPosition;
//}

/// <summary>
/// The level manages a Player and multiple Interactable game objects.
/// </summary>
public class Level : MonoBehaviour
{
    public Player Player = null;

    public GameObject InteractableParent = null;
    public List<Interactable> Interactables = new List<Interactable>();

    private float currentShortestDist = Mathf.Infinity;
    public Interactable CurrentClosest = null;
    public int TestedCount = 0;

    public float InteractablesMin = -100.0f;
    public float InteractablesMax = 100.0f;

    public GameObject CullSphere = null;
    public bool ViewRays = true;
    private Ray testRay = new Ray();
    private RaycastHit rayHit;

    /// <summary>
    /// Start the level by randomly moving Interactable objects around and setting the first Interactable to test for closest.
    /// </summary>
    void Start()
    {
        SystemAssert.Assert(Player != null);
        SystemAssert.Assert(InteractableParent != null);

        RandomlyMoveInteractablesInScene();

        if (Interactables.Count > 0)
        {
            CurrentClosest = Interactables[0];
        }
    }

    /// <summary>
    /// Moves all Interactable objects in the scene randomly.
    /// </summary>
    public void RandomlyMoveInteractablesInScene()
    {
        foreach (var interactable in Interactables)
        {
            interactable.MoveRandomlyAroundTransform(Player.transform, InteractablesMin, InteractablesMax);
        }
    }

    /// <summary>
    /// Test for the closest Interactable to the Player
    /// </summary>
    void Update()
    {
        //TestAllInteractables();
        TestWithShere();
    }

    /// <summary>
    /// Tests if the distance between an Interactable Transform and a Player Transform is the shortest.
    /// </summary>
    /// <param name="interactableTransform"> The Interactable Transform </param>
    /// <param name="playerTransform">The Player Transform </param>
    /// <returns> Is this shorter than the previous shortest. </returns>
    public bool TestIfShortestDistance(Transform interactableTransform, Transform playerTransform)
    {
        testRay.origin = interactableTransform.position;
        testRay.direction = (playerTransform.position - interactableTransform.position);
        float rayDist = Vector3.Distance(playerTransform.position, interactableTransform.position);

        if (ViewRays)
        {
            Debug.DrawRay(interactableTransform.position, testRay.direction * rayDist, Color.red);
        }

        // Test for the closest point on the player and if anything is in the way.
        if (Physics.Raycast(testRay, out rayHit, rayDist) && (rayHit.transform == playerTransform))
        {
            Vector3 playerHit = rayHit.point;
            testRay.origin = playerTransform.position;
            testRay.direction = -testRay.direction;
            // Test for the closest point on the interactable and if anything is in the way.
            // The previous transform test probably makes this one redudant, possible optimization.
            if (Physics.Raycast(testRay, out rayHit) && (rayHit.transform == interactableTransform))
            {
                // Do we need distance? technically we could save a sqrt if we just keep the square distance.
                // Proportionally the shortest square distance will still be the shortest.
                // Possible optimization.
                float newDist = Vector3.Distance(playerHit, rayHit.point);
                if (newDist < currentShortestDist)
                {
                    currentShortestDist = newDist;
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Test for closest interactable inside of a culling sphere.
    /// </summary>
    public void TestWithShere()
    {
        // Nothing to test
        if (Interactables.Count <= 0)
        {
            TestedCount = Interactables.Count;
            return;
        }

        // We need a current closest to test
        if (CurrentClosest == null)
        {
            CurrentClosest = Interactables[0];
        }

        // Get first item to kick off tests
        CurrentClosest.SetInteracting(false);

        currentShortestDist = Vector3.Distance(Player.transform.position, CurrentClosest.transform.position);
        Collider[] hitColliders = Physics.OverlapSphere(Player.transform.position, currentShortestDist);
        CullSphere.transform.position = Player.transform.position;
        float diameter = 2 * currentShortestDist;
        CullSphere.transform.localScale = new Vector3(diameter, diameter, diameter);

        TestedCount = hitColliders.Length;

        foreach (var hitcollider in hitColliders)
        {
            if (hitcollider.tag != "Interactable")
                continue;

            if (TestIfShortestDistance(hitcollider.transform, Player.transform))
            {
                CurrentClosest = hitcollider.GetComponent<Interactable>();
            }
        }

        CurrentClosest.SetInteracting(true);
    }

    /// <summary>
    /// Test for the closest interactable in the scene.
    /// First solution / replaced with TestWithShere
    /// </summary>
    public void TestAllInteractables()
    {
        TestedCount = Interactables.Count;

        // Nothing to test
        if (Interactables.Count <= 0)
        {
            return;
        }

        // We need a current closest to test
        if (CurrentClosest == null)
        {
            CurrentClosest = Interactables[0];
        }

        CurrentClosest.SetInteracting(false);
        currentShortestDist = Vector3.Distance(Player.transform.position, CurrentClosest.transform.position);

        foreach (var interactable in Interactables)
        {
            if (TestIfShortestDistance(interactable.transform, Player.transform))
            {
                CurrentClosest = interactable.GetComponent<Interactable>();
            }
        }

        CurrentClosest.SetInteracting(true);
    }

    //private void Awake()
    //{
    //    string destination = Application.persistentDataPath + "/save.dat";
    //    FileStream file;

    //    if (File.Exists(destination))
    //    {
    //        file = File.OpenRead(destination);
    //    }
    //    else
    //    {
    //        Debug.LogError("File not found");
    //        return;
    //    }

    //    BinaryFormatter bf = new BinaryFormatter();
    //    LevelStateData levelStateData = (LevelStateData)bf.Deserialize(file);
    //    file.Close();

    //    Player.transform.localScale = levelStateData.playerPosition;
    //    //interactables = new List<Interactable>(levelData.interactables);
    //}

    //private void OnDestroy()
    //{
    //    string destination = Application.persistentDataPath + "/save.dat";
    //    FileStream file;

    //    if (File.Exists(destination))
    //    {
    //        file = File.OpenWrite(destination);
    //    }
    //    else
    //    {
    //        file = File.Create(destination);
    //    }

    //    LevelStateData levelStateData = new LevelStateData();
    //    levelStateData.playerPosition = Player.transform.localScale;
    //    //levelData.interactables = interactables.ToArray();
    //    BinaryFormatter bf = new BinaryFormatter();
    //    bf.Serialize(file, levelStateData);
    //    file.Close();
    //}
}
