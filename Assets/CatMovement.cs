using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CatMovement : MonoBehaviour
{
    public Vector2 shakeRange = new Vector2(1,2);

    public Vector2 shakeStrength = new Vector2(1,1);
    // Start is called before the first frame update
    private Vector3 startPosition;
    void Start()
    {
        // Set the object's initial position
        startPosition = transform.position;

        // Generate a random value for the x-axis position
        float xPosition = Random.Range(-1f, 1f)*shakeStrength.x;

        // Set the target position, which is slightly above and to the right (or left) of the start position
        Vector3 targetPosition = startPosition + new Vector3(xPosition, 1*shakeStrength.y, 0);

        // Use DoTween to animate the object's position from the start position to the target position
        transform.DOLocalMove(targetPosition, Random.Range(shakeRange.x, shakeRange.y)) // animate over 2 seconds
            .SetEase(Ease.InOutSine) // use a sine easing function
             // loop indefinitely, bouncing back and forth between the start and end positions
            .SetRelative(false) // the target position is relative to the start position
            .OnComplete(OnAnimationComplete); // call the OnAnimationComplete() function when the animation ends
    }
    void OnAnimationComplete()
    {
        // Generate a new random position for the object
    
        float xPosition = Random.Range(-1f, 1f)*shakeStrength.x;
        Vector3 targetPosition = startPosition + new Vector3(xPosition*shakeStrength.x, 1*shakeStrength.y, 0);
        transform.DOLocalMove(startPosition, Random.Range(shakeRange.x, shakeRange.y)).SetEase(Ease.InOutSine).SetRelative(false).OnComplete(
            ()=>
                // Start a new animation to the new position
                transform.DOLocalMove(targetPosition, Random.Range(shakeRange.x, shakeRange.y)) // animate over 2 seconds
                    .SetEase(Ease.InOutSine) // use a sine easing function
                    .SetLoops(1, LoopType.Yoyo) // loop indefinitely, bouncing back and forth between the start and end positions
                    .SetRelative(false) // the target position is relative to the start position
                    .OnComplete(OnAnimationComplete) // call the OnAnimationComplete() function when the animation ends
            );
   
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
