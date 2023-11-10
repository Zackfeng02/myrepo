namespace StreamingServiceApp.Models
{
    public class MovieReview
    {
        public int MovieId { get; set; }
        public Review Review { get; set; }

        public Movie Movie { get; set; }
    }
}
