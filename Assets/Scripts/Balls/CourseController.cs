using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class CourseController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform ballShooterTransform;
    [SerializeField] private Transform ballParentTransform;
    public GameObject baseBallReference;

    [Header("Throw List")]
    public List<ThrowProfile> throwProfiles = new List<ThrowProfile>();

    [Header("Current Course Values")]
    public int currentThrowIndex = 0;
    public float courseWidth = 7;
    public Vector2 courseHeightBounds = new Vector2();


    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (baseBallReference == null)
            {
                return;
            }

            if (currentThrowIndex >= throwProfiles.Count) {
                return;
            }

            var newBall = Instantiate(baseBallReference, ballParentTransform);
            newBall.transform.position = ballShooterTransform.position;
            newBall.GetComponent<BallController>().courseController = this;

            ThrowProfile profile = throwProfiles[currentThrowIndex];
            newBall.transform.position += new Vector3(profile.xOffset * courseWidth, 0, 0);
            foreach (BallModifier mod in profile.modifiers)
            {
                newBall.GetComponent<BallController>().AddModifier(mod);
            }

            currentThrowIndex++;
            

        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            currentThrowIndex = 0;
        }
    }

}
