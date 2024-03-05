using Newtonsoft.Json;
using Visma.Timelogger.Domain.Entities;

namespace Visma.Timelogger.Persistence
{
    public class DevTestData
    {
        public static Tuple<Project[], List<TimeRecord>> TestData = GenerateProjects();
        private static Tuple<Project[], List<TimeRecord>> GenerateProjects()
        {
            Random rnd = new Random();

            var freelancerId1 = Guid.Parse("486E3E8F-0DC3-4E00-8711-BE3A6CB1399E");
            var freelancerId2 = Guid.Parse("DD330056-EE5A-451B-AC2C-AF0CB20EB213");

            var customerId1 = Guid.Parse("671758F5-320D-4D1C-8C1A-54CDC55F2F75");
            var customerId2 = Guid.Parse("AE1E5C8E-7106-401A-A51F-FBEC70972DEE");

            int projectQuantity = 10;

            const string chars = "AB CD EFGH IJKLMN  OPQ RSTUV WX YZ  ";

            Project[] projects = new Project[projectQuantity];

            List<TimeRecord> records = new List<TimeRecord>();
            for (int i = 0; i < projectQuantity; i++)
            {
                DateTime startDate = DateTime.Now.AddDays(rnd.Next(-50, 2)).Date;
                Project project = new Project()
                {
                    Id = Guid.NewGuid(),
                    CustomerId = rnd.Next(1, 3) % 2 == 0 ? customerId1 : customerId2,
                    FreelancerId = rnd.Next(1, 3) % 2 == 0 ? freelancerId1 : freelancerId2,
                    StartTime = startDate,
                    Deadline = startDate.AddDays(rnd.Next(5, 100)),
                    IsActive = rnd.Next(1, 3) % 2 == 0 ? false : true,
                    Name = new string(Enumerable.Repeat(chars, 10)
                    .Select(s => s[rnd.Next(s.Length)]).ToArray())
                };

                for (int j = 0; j < rnd.Next(0, 10); j++)
                {
                    TimeRecord record = new TimeRecord()
                    {
                        Id = Guid.NewGuid(),
                        ProjectId = project.Id,
                        FreelancerId = project.FreelancerId,
                        DurationMinutes = rnd.Next(30, 8 * 24 + 1),
                        StartTime = GenerateRandomDate(project.StartTime, project.Deadline)
                    };
                    records.Add(record);
                }
                projects[i] = project;
            }
            Console.WriteLine(JsonConvert.SerializeObject(projects));

            return Tuple.Create(projects, records);
        }
        private static DateTime GenerateRandomDate(DateTime startDate, DateTime endDate)
        {
            Random random = new Random();
            long range = endDate.Ticks - startDate.Ticks;
            long randomTicks = (long)(random.NextDouble() * range);

            return new DateTime(startDate.Ticks + randomTicks).Date;
        }
    }
}
