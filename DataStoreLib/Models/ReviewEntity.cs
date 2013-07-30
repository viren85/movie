using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace DataStoreLib.Models
{
    public class ReviewEntity : TableEntity
    {
        #region table emembers
        public static readonly string PARTITION_KEY = "CloudMovie";

        public string ReviewId { get; set; }
        public string ReviewerName { get; set; }
        public string Review { get; set; }
        public int ReviewerRating { get; set; }
        public int SystemRating { get; set; }

        public override void ReadEntity(IDictionary<string, EntityProperty> properties,
                                        Microsoft.WindowsAzure.Storage.OperationContext operationContext)
        {
            base.ReadEntity(properties, operationContext);

            ReviewId = ReadString(properties, "ReviewId");
            ReviewerName = ReadString(properties, "ReviewerName");
            Review = ReadString(properties, "Review");
            ReviewerRating = ReadInt(properties, "ReviewerRating");
            SystemRating = ReadInt(properties, "SystemRating");
        }

        public override IDictionary<string, EntityProperty> WriteEntity(Microsoft.WindowsAzure.Storage.OperationContext operationContext)
        {
            var dict = MergeDicts(base.WriteEntity(operationContext));

            WriteString(dict, "ReviewId", ReviewId);
            WriteString(dict, "ReviewerName", ReviewerName);
            WriteString(dict, "Review", Review);
            WriteInt(dict, "ReviewerRating", ReviewerRating);
            WriteInt(dict, "SystemRating", SystemRating);

            return dict;
        }

        #endregion

        public ReviewEntity()
            : base(PARTITION_KEY, "")
        {
            
        }

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
