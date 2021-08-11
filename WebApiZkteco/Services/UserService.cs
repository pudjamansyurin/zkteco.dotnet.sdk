using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiZkteco.Models;

namespace WebApiZkteco.Services
{
    public interface IUserService
    {
        bool Any(string sUserID);
        User Get(string sUserID);
        List<User> GetAll();
        void Add(User user);
        void Update(User u);
        void AddOrUpdate(User u);
        void Delete(User user);
        void SetActive(User user, bool enable);
        void Schedule(User user, DateTime start, DateTime stop);
        List<User> Active();
        List<User> ShouldActive();
        List<User> ShouldDeactive();
    }

    public class UserService : IUserService
    {
        private readonly ZkContext ctx;

        public UserService(ZkContext context)
        {
            ctx = context;
        }

        public bool Any(string sUserID)
        {
            return ctx.Users.Any(u => u.sUserID == sUserID);
        }

        public User Get(string sUserID)
        {
            try
            {
                return ctx.Users.First(u => u.sUserID == sUserID);
            }
            catch (Exception e)
            {
                throw new Exception("User not found: ", e);
            }
        }

        public List<User> GetAll()
        {
            return ctx.Users.ToList();
        }

        public void Add(User user)
        {
            user.active = true;
            user.activeStart = default;
            user.activeStop = default;

            ctx.Users.Add(user);
            ctx.SaveChanges();
        }

        public void Update(User u)
        {
            var user = Get(u.sUserID);

            // update basic data
            user.sName = u.sName;
            user.sPassword = u.sPassword;
            user.bEnabled = u.bEnabled;
            user.iPrivilege = u.iPrivilege;
            // update finger if any
            if (u.sFingerData != null)
            {
                user.idwFingerIndex = u.idwFingerIndex;
                user.iFingerFlag = u.iFingerFlag;
                user.sFingerData = u.sFingerData;
                user.iFingerLen = u.iFingerLen;
            }
            // update face if any
            if (u.sFaceData != null)
            {
                user.iFaceIndex = u.iFaceIndex;
                user.sFaceData = u.sFaceData;
                user.iFaceLen = u.iFaceLen;
            }

            ctx.Users.Update(user);
            ctx.SaveChanges();
        }

        public void AddOrUpdate(User u)
        {
            if (Any(u.sUserID))
            {
                Update(u);
            }
            else
            {
                Add(u);
            }
        }

        public void Delete(User user)
        {
            throw new NotImplementedException();
        }

        public void SetActive(User user, bool enable)
        {
            user.active = enable;
            ctx.SaveChanges();

            if (!enable)
            {
                Schedule(user, default, default);
            }
        }


        public void Schedule(User user, DateTime start, DateTime stop)
        {
            user.activeStart = start;
            user.activeStop = stop;
            ctx.SaveChanges();
        }

        public List<User> Active()
        {
            return ctx.Users.Where(u => (DateTime.Now >= u.activeStart && DateTime.Now <= u.activeStop) && u.active).ToList();
        }

        public List<User> ShouldActive()
        {
            return ctx.Users.Where(u => (DateTime.Now >= u.activeStart && DateTime.Now <= u.activeStop) && !u.active).ToList();
        }

        public List<User> ShouldDeactive()
        {
            return ctx.Users.Where(u => !(DateTime.Now >= u.activeStart && DateTime.Now <= u.activeStop) && u.active).ToList();
        }
    }
}
