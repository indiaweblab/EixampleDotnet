﻿using EixampleDotnet.Entities;
using EixampleDotnet.EntityFramework;
using System.Linq;

namespace EixampleDotnet.Application
{
    public class MembershipService : IMembershipService
    {
        private AppDbContext _dbContext { get; set; }
        private Session _session;

        public MembershipService(
            AppDbContext dbContext,
            Session session
            )
        {
            _dbContext = dbContext;
            _session = session;
        }

        public bool IsMember(string userId)
        {
            if (_session.TenantId.HasValue)
            {
                bool isMember = _dbContext.Memberships.Any(x => x.TenantId == _session.TenantId && x.UserId == userId);

                if (!isMember)
                {
                    return false;
                }
            }

            return true;
        }

        public void CreateMembership(ApplicationUser user)
        {
            bool exists = _dbContext.Memberships.Any(x => x.TenantId == _session.TenantId.Value && x.UserId == user.Id);

            if (!exists)
            {
                _dbContext.Memberships.Add(new Membership() { TenantId = _session.TenantId.Value, UserId = user.Id });
                _dbContext.SaveChanges();
            }            
        }
    }
}
