using UnityEngine;
using SystemAssert =  System.Diagnostics.Debug;

/// <summary>
/// A base gameobjects the player can interacte with.
/// </summary>
public class Interactable : MonoBehaviour
{
    public Material BaseMaterial;
    public Material HighlightMaterial;

    /// <summary>
    /// This function randomly places this Interactable around a transform.  Only as far or as close as the min and max.
    /// </summary>
    /// <param name="originTransform">A valid transform to be generated around.</param>
    /// <param name="minPosition">The minimum position away.</param>
    /// <param name="maxPosition">The maximum position away.</param>
    /// <exception cref="SystemAssert.Assert" Assert minPosition is less than maxPosition.
    /// <exception cref="SystemAssert.Assert" Assert transform.transform is valid.
    public virtual void MoveRandomlyAroundTransform(Transform originTransform, float minPosition, float maxPosition)
    {
        SystemAssert.Assert(minPosition < maxPosition);
        float randomX = Random.Range(minPosition, maxPosition);
        float randomY = Random.Range(minPosition, maxPosition);
        float randomZ = Random.Range(minPosition, maxPosition);

        SystemAssert.Assert(originTransform != null);
        transform.position = originTransform.position + new Vector3(randomX, randomY, randomZ);

        // Reset the interacting state
        SetInteracting(false);
    }

    /// <summary>
    /// This is called when Unity firsts creates the Interacable and it sets the Material to Base.
    /// Materials must be attached before this call.
    /// </summary>
    /// <exception cref="SystemAssert.Assert" Assert BaseMaterial is valid.
    /// <exception cref="SystemAssert.Assert" Assert HighlightMaterial is valid.
    public virtual void Start()
    {
        SystemAssert.Assert(BaseMaterial != null);
        SystemAssert.Assert(HighlightMaterial != null);

        SetInteracting(false);
    }

    /// <summary>
    /// Sets the Material of the 
    /// </summary>
    /// <param name="interacting">If the object is being interacted with set the Material to Highlight</param>
    public virtual void SetInteracting(bool interacting)
    {
        if (interacting)
        {
            GetComponent<Renderer>().sharedMaterial = HighlightMaterial;
        }
        else
        {
            GetComponent<Renderer>().sharedMaterial = BaseMaterial;
        }
    }
}
