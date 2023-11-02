using StreamingServiceApp.Models;
using Microsoft.EntityFrameworkCore;


namespace StreamingServiceApp.DbData
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new MovieAppDbContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<MovieAppDbContext>>()))
            {

                if (context.Movies.Any())
                {
                    return;
                }

                context.Movies.AddRange(
                    new Movie
                    {
                        MovieName = "No Time To Die",
                        ReleaseDate = DateTime.Parse("2021-9-30"),
                        Genre = Genre.ACTION,
                        Rating = 3,
                        FilePath = "AWS Certified Solutions Architect Study Guide, 2nd Edition by Ben Piper, David Clinton.pdf",
                        Description = "James Bond is enjoying a tranquil life in Jamaica after leaving active service. However, his peace is short-lived as his old CIA friend, Felix Leiter, shows up and asks for help.",
                        ImageUrl = "https://m.media-amazon.com/images/I/616x9pOCRTL._AC_SY355_.jpg"
                    },

                    new Movie
                    {
                        MovieName = "Venom: Let There Be Carnage",
                        ReleaseDate = DateTime.Parse("2021-10-1"),
                        Genre = Genre.HORROR,
                        Rating = 4,
                        Description = "Eddie Brock is still struggling to coexist with the shape-shifting extraterrestrial Venom. ",
                        ImageUrl = "https://m.media-amazon.com/images/M/MV5BNTFiNzBlYmEtMTcxZS00ZTEyLWJmYmQtMjYzYjAxNGQwODAzXkEyXkFqcGdeQXVyMTEyMjM2NDc2._V1_.jpg"
                    },

                    new Movie
                    {
                        MovieName = "Greyhound",
                        ReleaseDate = DateTime.Parse("2020-7-10"),
                        Genre = Genre.ACTION,
                        Rating = 3,
                        Description = "U.S. Navy Cmdr. Ernest Krause is assigned to lead an Allied convoy across the Atlantic during World War II. His convoy, however, is pursued by German U-boats",
                        ImageUrl = "https://encrypted-tbn1.gstatic.com/images?q=tbn:ANd9GcRqGlJ1E_dsszf-lreRdhk3LiSe9gK1SBzNnw63UIxXiyveYR4I"
                    },

                    new Movie
                    {
                        MovieName = "Tenet",
                        ReleaseDate = DateTime.Parse("2020-8-12"),
                        Genre = Genre.THRILLER,
                        Rating = 4,
                        Description = "When a few objects that can be manipulated and used as weapons in the future fall into the wrong hands, a CIA operative, known as the Protagonist, must save the world.",
                        ImageUrl = "https://m.media-amazon.com/images/M/MV5BYzg0NGM2NjAtNmIxOC00MDJmLTg5ZmYtYzM0MTE4NWE2NzlhXkEyXkFqcGdeQXVyMTA4NjE0NjEy._V1_.jpg"
                    }
                ); ;
                context.SaveChanges();

                if (context.Users.Any())
                {
                    return;
                }

                context.Users.AddRange(
                    new User
                    {
                        Email = "prad@gmail.com",
                        Password = "12345",
                        ConfirmPassword = "12345"
                    },

                    new User
                    {
                        Email = "sample@gmail.com",
                        Password = "12345",
                        ConfirmPassword = "12345"
                    }
                );
                context.SaveChanges();
            }
        }

    }
}
