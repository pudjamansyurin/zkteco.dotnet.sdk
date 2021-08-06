//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.DependencyInjection;
//using WebApiZkteco.Data;
//using System;
//using System.Linq;

//namespace WebApiZkteco.Models
//{
//    public static class SeedData
//    {
//        public static void Initialize(IServiceProvider serviceProvider)
//        {
//            using (var context = new ZkContext(
//                serviceProvider.GetRequiredService<
//                    DbContextOptions<ZkContext>>()))
//            {
//                // Look for any Users.
//                if (context.User.Any())
//                {
//                    return;   // DB has been seeded
//                }

//                context.User.AddRange(
//                    new User { Name = "Adjie", FingerIdx = 1, Priviledge = 1, Password = "abcdef", Enabled = true },
//                    new User { Name = "Ananta", FingerIdx = 3, Priviledge = 1, Password = "ywer", Enabled = true },
//                    new User { Name = "Pambudi", FingerIdx = 6, Priviledge = 1, Password = "asdew", Enabled = true },
//                    new User { Name = "Rizal", FingerIdx = 8, Priviledge = 1, Password = "lskk", Enabled = false },
//                    new User { Name = "Lia", FingerIdx = 4, Priviledge = 1, Password = "j12jj2", Enabled = true }
//                );
//                context.SaveChanges();
//            }
//        }
//    }
//}