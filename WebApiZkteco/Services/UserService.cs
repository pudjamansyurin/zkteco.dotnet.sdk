//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using WebApiZkteco.Models;

//namespace WebApiZkteco.Services
//{
//    public class UserService
//    {
//        static List<User> Users { get; }
//        static int nextId = 6;

//        static UserService()
//        {
//            Users = new List<User>
//            {
//                new User { Id = 1, Name = "Adjie", FingerIdx = 1, Priviledge = 1, Password = "abcdef", Enabled = true },
//                new User { Id = 2, Name = "Ananta", FingerIdx = 3, Priviledge = 1, Password = "ywer", Enabled = true },
//                new User { Id = 3, Name = "Pambudi", FingerIdx = 6, Priviledge = 1, Password = "asdew", Enabled = true },
//                new User { Id = 4, Name = "Rizal", FingerIdx = 8, Priviledge = 1, Password = "lskk", Enabled = false },
//                new User { Id = 5, Name = "Lia", FingerIdx = 4, Priviledge = 1, Password = "j12jj2", Enabled = true },
//            };
//        }

//        public static List<User> GetAll() => Users;

//        public static User Get(int id) => Users.FirstOrDefault(p => p.Id == id);

//        public static void Add(User User)
//        {
//            User.Id = nextId++;
//            Users.Add(User);
//        }

//        public static void Delete(int id)
//        {
//            var User = Get(id);
//            if (User is null)
//                return;

//            Users.Remove(User);
//        }

//        public static void Update(User User)
//        {
//            var index = Users.FindIndex(p => p.Id == User.Id);
//            if (index == -1)
//                return;

//            Users[index] = User;
//        }
//    }
//}
