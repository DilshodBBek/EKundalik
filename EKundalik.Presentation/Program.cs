using EKundalik.Infrastructure.Persistence;

namespace EKundalik.Presentation
{
    internal class Program
    {
        static void Main(string[] args)
        {
            EKundalikDbContext.CreateDb();
        }
    }
}