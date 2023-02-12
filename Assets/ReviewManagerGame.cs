using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Google.Play.Review;

public class ReviewManagerGame : MonoBehaviour
{
    public void RequestReview()
    {
        /* // StartCoroutine(AndroidReview());
         var reviewManager = new ReviewManager();

         // start preloading the review prompt in the background
         var playReviewInfoAsyncOperation = reviewManager.RequestReviewFlow();

         // define a callback after the preloading is done
         playReviewInfoAsyncOperation.Completed += playReviewInfoAsync =>
         {
             if (playReviewInfoAsync.Error == ReviewErrorCode.NoError)
             {
                 // display the review prompt
                 var playReviewInfo = playReviewInfoAsync.GetResult();
                 reviewManager.LaunchReviewFlow(playReviewInfo);
             }
             else
             {
                 // handle error when loading review prompt
             }
         };
     }*/
    }
}
