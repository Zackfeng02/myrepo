using Amazon.DynamoDBv2.DataModel;
using System.ComponentModel.DataAnnotations;

namespace StreamingServiceApp.Models
{
    [DynamoDBTable("Reviews")]
    public class Review
    {
        public string ReviewID { get; set; }
        [Required(ErrorMessage = "Please enter Title!")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Please enter Review!")]
        public string ReviewDescription { get; set; }

        public int MovieId { get; set; }
        public int MovieRating { get; set; }
        public Movie Movie { get; set; }
        public string UserEmail { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
