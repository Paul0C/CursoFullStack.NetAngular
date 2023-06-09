using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CursoWebApi.Domain.Identity;
using CursoWebApi.Persistence.Contextos;
using CursoWebApi.Persistence.Contratos;
using Microsoft.EntityFrameworkCore;

namespace CursoWebApi.Persistence
{
    public class UserPersist : GeralPersist, IUserPersist
    {
        private readonly DataContext _context;
        
        public UserPersist(DataContext context) : base(context)
        {
            _context = context;

        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User> GetUserByUserNameAsync(string userName)
        {
            return await _context.Users
                                 .SingleOrDefaultAsync(user => user.UserName == userName.ToLower());
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

    }
}