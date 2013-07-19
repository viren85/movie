using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStoreLib.Models
{
    public class ReviewEntity : TableEntity
    {
        #region table emembers
        private static readonly string PARTITION_KEY = "CloudMovie";

        public string ReviewId { get; set; }
        public string ReviewerName { get; set; }
        public string Review { get; set; }
        public int ReviewerRating { get; set; }
        public int SystemRating { get; set; }

        #endregion

        public ReviewEntity(string rowKey)
            : base(PARTITION_KEY, rowKey)
        {
            ReviewId = rowKey;
        }

        public ReviewEntity(ReviewEntity review)
            :base(review.PartitionKey, review.RowKey)
        {
            ReviewId = review.ReviewId;
            ReviewerName = review.ReviewerName;
            Review = review.Review;
            ReviewerRating = review.ReviewerRating;
            SystemRating = review.SystemRating;
        }

        public static ReviewEntity CreateReviewEntity(string reviewrName, string review, int reviewerRating = 0, int systemRating = 0)
        {
            var reviewId = Guid.NewGuid().ToString();
            var reviewEntity = new ReviewEntity(reviewId);
            reviewEntity.ReviewId = reviewId;
            reviewEntity.ReviewerName = reviewrName;
            reviewEntity.Review = review;
            reviewEntity.ReviewerRating = reviewerRating;
            reviewEntity.SystemRating = systemRating;

            return reviewEntity;
        }
    }
}
