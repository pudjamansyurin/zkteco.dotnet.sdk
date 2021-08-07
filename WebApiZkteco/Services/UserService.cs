using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiZkteco.Models;

namespace WebApiZkteco.Services
{
    public interface IUserService
    {
        User Get(string sUserID);
        List<User> GetAll();
        void Add(User user);
        void Update(User u);
        void AddOrUpdate(User u);
        void Delete(User user);
        void Enable(User user);
        void Disable(User user, DateTime activeAt);
        bool HasPending();
        List<User> GetPending();
    }

    public class UserService : IUserService
    {
        private readonly ZkContext ctx;

        public UserService(ZkContext context)
        {
            ctx = context;
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
            user.disabled = false;
            user.activeAt = new DateTime();

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
            var user = Get(u.sUserID);

            if (user.sUserID == null)
            {
                Add(user);
            }
            else
            {
                Update(user);
            }

        }

        public void Delete(User user)
        {
            throw new NotImplementedException();
        }

        public void Enable(User user)
        {
            if (user.disabled)
            {
                //throw new Exception("User " + user.sUserID + " already active");
                user.disabled = false;
                user.activeAt = new DateTime();
                ctx.SaveChanges();
            }
        }

        public void Disable(User user, DateTime activeAt)
        {
            if (!user.disabled)
            {
                //throw new Exception("User " + user.sUserID + " already disabled");
                user.disabled = true;
                user.activeAt = activeAt;
                ctx.SaveChanges();
            }
        }

        public bool HasPending()
        {
            return ctx.Users.Any(shouldActive);
        }

        public List<User> GetPending()
        {
            return ctx.Users.Where(shouldActive).ToList();
        }

        private bool shouldActive(User u)
        {
            return u.disabled == true && DateTime.Now >= u.activeAt;
        }
    }
}
