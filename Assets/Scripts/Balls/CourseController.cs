using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class CourseController : MonoBehaviour
{
    [SerializeField] private Transform ballShooterTransform;
    [SerializeField] private Transform ballParentTransform;

    public GameObject baseBallReference;
    public List<ThrowProfile> throwProfiles = new List<ThrowProfile>();

    public int currentThrowIndex = 0;

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

            ThrowProfile profile = throwProfiles[currentThrowIndex];
            foreach(BallModifier mod in profile.modifiers)
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

    public void ShootNextBall()
    {

    }
}
